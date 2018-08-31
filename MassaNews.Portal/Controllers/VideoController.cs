using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using Entities.Classes;
using MassaNews.Service.Enum;
using MassaNews.Service.Models;
using MassaNews.Service.Services;
using MassaNews.Portal.Functions;
using MassaNews.Portal.Models;
using MassaNews.Portal.ViewModels;
using System.Net;
using System.Web.UI;
using MassaNews.Service.Util;

namespace MassaNews.Portal.Controllers
{
  public class VideoController : BaseController
  {
    #region Properties
    private NoticiaService NoticiaSrv { get; }
    private LocalService LocalSrv { get; }
    private WeatherService WeatherSrv { get; }
    #endregion

    #region Constructor
    public VideoController()
    {
      NoticiaSrv = new NoticiaService();
      LocalSrv = new LocalService();
      WeatherSrv = new WeatherService();
    }

    #endregion

    #region Actions
    [Route("videos")]
    public ActionResult Index(int p = 1)
    {
      var objCidade = Cidade.Load(GetMyLocationId);

      var objModel = new VideoIndex
      {
        Title = "Vídeos - Massa News",
        Description = "Assista todos os Vídeos no Massa News.",
        Robots = "index, follow",
        Canonical = $"{Constants.UrlWeb}/videos",
        ImgOpenGraph = $"{Constants.UrlWeb}/content/images/avatar/editorial/avatar-videos.jpg",
        Highlights = Noticia.GetBySection(objCidade.Microregion.Home.Id, Section.Videos.Id).Take(3).Select(VideoModel.Map),
        RegionHighlights = GetRegionHighlights(objCidade.MicroregiaoId),
        SportsHighlights = GetSportsHighlights(),
        EntertainmentHighlights = GetEntertainmentHighlights(),
        NegociosDaTerraHighlights = GetNegociosDaTerraHighlights(),
        BlogsHighlights = GetBlogHighlights(),
        LiveOnHighlights = GetLiveOnHighlights(),
        DescobrindoCuritibaHighlights = GetDescobrindoCuritibaHighlights()
      };

      //ViewBag's
      ViewBag.IsVideo = true;
      ViewBag.ActiveNav = "Vídeos";
      ViewBag.EditoriaUrl = "videos";

      return View(objModel);
    }

    [Route("videos/{categoria:regex(^(?!.*[.]html$).*$)}")]
    public ActionResult Categoria(string categoria)
    {
      //Set the initial period
      var endDate = DateTime.Now;
      var startDate = new DateTime(endDate.AddMonths(-2).Year, endDate.AddMonths(-2).Month, 01);

      var lstNoticias = GetListOfNews(categoria, startDate, endDate);

      if (lstNoticias.Count() < 4)
      {
        startDate = new DateTime(2016, 01, 01);
        lstNoticias = GetListOfNews(categoria, startDate, endDate);
      }

      var activeNav = string.Empty;

      switch (categoria)
      {
        case "curitiba-regiao":
          activeNav = "Curitiba e região";
          break;
        case "maringa-regiao":
          activeNav = "Maringá e região";
          break;
        case "foz-do-iguacu-regiao":
          activeNav = "Foz do Iguaçu e região";
          break;
        case "londrina-regiao":
          activeNav = "Londrina e região";
          break;
        case "ponta-grossa-regiao":
          activeNav = "Ponta Grossa e região";
          break;
        case "noticias":
          activeNav = "Notícias";
          break;
        case "esportes":
          activeNav = "Esportes";
          break;
        case "entretenimento":
          activeNav = "Entretenimento";
          break;
        case "negocios-da-terra":
          activeNav = "Negócios da Terra";
          break;
        case "where-curitiba":
          activeNav = "Where Curitiba";
          break;
        case "blogs":
          activeNav = "Blogs";
          break;
        case "massa-news-live-on":
          activeNav = "Massa News Live On";
          break;
        case "descobrindo-curitiba":
          activeNav = "Descobrindo Curitiba";
          break;
      }

      ViewBag.ActiveNav = activeNav;

      var model = new VideoCategoria
      {
        //Base
        Title = $"Vídeos de {ViewBag.ActiveNav} - Massa News",
        Description = $"Veja todos os vídeos de {ViewBag.ActiveNav} no Massa News.",
        Robots = "index, follow",
        Canonical = $"{Constants.UrlWeb}/videos/{categoria}",
        ImgOpenGraph = $"{Constants.UrlWeb}/content/images/avatar/editorial/avatar-videos.jpg",
        //Model
        Category = categoria,
        Highlights = lstNoticias.Take(3).Select(VideoModel.Map).ToList(),
        Sections = GetVideoSections(lstNoticias.Skip(3).ToList())
      };

      //ViewBag's
      ViewBag.IsVideo = true;

      return View(model);
    }

    //[Route("Videos/{section}/{url}.html")]
    public ActionResult Interna(string url)
    {
      #region redirect

      var redirectUrl = !string.IsNullOrEmpty(NoticiaSrv.GetRedirectUrlContains(url)) ? NoticiaSrv.GetRedirectUrlContains(url).Split('/')[3] : null;

      if (!string.IsNullOrEmpty(redirectUrl) && !redirectUrl.Equals(url))
        return new RedirectResult($"/videos/{redirectUrl}.html", true);

      #endregion

      var noticiaUrl = redirectUrl ?? url;

      //Get the news object
      var objNoticia = Noticia.GetByUrl(noticiaUrl);

      var objVideo = VideoModel.Map(objNoticia);

      #region Validations
      //Redirect 301 to new url
      var originUrl = $"/videos/{noticiaUrl}.html";

      if (objNoticia != null)
      {
        if (originUrl != objVideo.InternaUrl)
          return new RedirectResult(objVideo.InternaUrl, true);
      }

      //Return not found
      if (objNoticia == null)
        return new HttpStatusCodeResult(HttpStatusCode.NotFound);

      //Caso a noticia for inativa responde com redirect permanente para a home
      if ((objNoticia.StatusId == Status.Inativa.Id || !objNoticia.Categoria.Status))
        return new RedirectResult("/", true);

      //Caso a noticia for diferente de publicada responde com 404
      if (objNoticia.StatusId != Status.Publicada.Id)
        return new HttpStatusCodeResult(HttpStatusCode.NotFound);

      #endregion

      var model = new VideoInterna
      {
        //Base
        Title = $"Vídeo: {objNoticia.Titulo} - Massa News",
        Description = $"{(objNoticia.Conteudo.Length > 220 ? Text.RemoveHtmlTags(objNoticia.Conteudo.Substring(0, 220) + "... Veja o vídeo no Massa News!") : Text.RemoveHtmlTags(objNoticia.Conteudo) + " Veja o vídeo no Massa News!")}",
        Robots = "noindex, follow",
        Canonical = $"{objNoticia.UrlFull}.html",
        Url = $"{objVideo.InternaUrl}",
        //Model
        Video = objVideo,
        SectionUrl = objVideo.Section,
        SectionLink = $"/videos/{objVideo.Section}",
        CloseLink = (Request.UrlReferrer != null && Request.UrlReferrer.ToString().StartsWith(Constants.UrlWeb)) ? "#" : $"/videos/{objVideo.Section}"
      };

      if (!string.IsNullOrEmpty(objNoticia.ImgLg))
        model.ImgOpenGraph = $"{Constants.UrlDominioEstaticoUploads}/{"noticias"}/{objNoticia.ImgLg}";

      var lstNoticias = new List<Noticia>();

      const int take = 20;

      switch (objVideo.Section)
      {
        case "curitiba-regiao":
          lstNoticias = NoticiaSrv.GetLastestVideoNewsByMicroregion(take, MicroRegionEnum.Curitiba.GetHashCode()).ToList();
          model.SectionTitle = "Curitiba e região";
          break;
        case "maringa-regiao":
          lstNoticias = NoticiaSrv.GetLastestVideoNewsByMicroregion(take, MicroRegionEnum.Maringa.GetHashCode()).ToList();
          model.SectionTitle = "Maringá e região";
          break;
        case "foz-do-iguacu-regiao":
          lstNoticias = NoticiaSrv.GetLastestVideoNewsByMicroregion(take, MicroRegionEnum.FozDoIguacu.GetHashCode()).ToList();
          model.SectionTitle = "Foz do Iguaçu e região";
          break;
        case "londrina-regiao":
          lstNoticias = NoticiaSrv.GetLastestVideoNewsByMicroregion(take, MicroRegionEnum.Londrina.GetHashCode()).ToList();
          model.SectionTitle = "Londrina e região";
          break;
        case "ponta-grossa-regiao":
          lstNoticias = NoticiaSrv.GetLastestVideoNewsByMicroregion(take, MicroRegionEnum.PontaGrossa.GetHashCode()).ToList();
          model.SectionTitle = "Ponta Grossa e região";
          break;
        case "noticias":
          lstNoticias = NoticiaSrv.GetLastestVideoNewsByEditory(take, EditorialEnum.Noticias.GetHashCode()).ToList();
          model.SectionTitle = "Notícias";
          break;
        case "esportes":
          lstNoticias = NoticiaSrv.GetLastestVideoNewsByEditory(take, EditorialEnum.Esportes.GetHashCode()).ToList();
          model.SectionTitle = "Esportes";
          break;
        case "entretenimento":
          lstNoticias = NoticiaSrv.GetLastestVideoNewsByEditory(take, EditorialEnum.Entretenimento.GetHashCode()).ToList();
          model.SectionTitle = "Entretenimento";
          break;
        case "negocios-da-terra":
          lstNoticias = NoticiaSrv.GetLastestVideoNewsByEditory(take, EditorialEnum.NegociosDaTerra.GetHashCode()).ToList();
          model.SectionTitle = "Negócios da Terra";
          break;
        case "where-curitiba":
          lstNoticias = NoticiaSrv.GetLastestVideoNewsByEditory(take, EditorialEnum.WhereCuritiba.GetHashCode()).ToList();
          model.SectionTitle = "Where Curitiba";
          break;
        case "blogs":
          lstNoticias = NoticiaSrv.GetLastestVideoNewsByEditory(take, EditorialEnum.Blogs.GetHashCode()).ToList();
          model.SectionTitle = "Blogs";
          break;
        case "massa-news-live-on":
          lstNoticias = NoticiaSrv.GetLastestVideoNewsByTag(take, 752).ToList();
          model.SectionTitle = "Massa News Live On";
          break;
        case "descobrindo-curitiba":
          lstNoticias = NoticiaSrv.GetLastestVideoNewsByTag(take, 754).ToList();
          model.SectionTitle = "Descobrindo Curitiba";
          break;
      }

      //Remove iten
      if (lstNoticias.Any(n => n.Id == objNoticia.Id))
        lstNoticias.Remove(lstNoticias.Single(n => n.Id == objNoticia.Id));

      //Add current iten at the fist position
      lstNoticias.Insert(0, objNoticia);

      model.MenuItems = lstNoticias.ToList().Select(MenuItemModel.Map).ToList();

      model.MenuItems.First().Selected = true;

      //ViewBag's
      ViewBag.IsVideo = true;
      ViewBag.IsInternaVideo = true;

      return View(model);
    }
    #endregion

    #region Partial Actions
    [Route("Videos/LoadNexNavegationLink")]
    [OutputCache(Duration = 60, VaryByParam = "*", Location = OutputCacheLocation.Server)]
    public ActionResult LoadNexNavegationLink(string firstId, string sectionUrl, string publishDate)
    {
      var firstItemId = Convert.ToInt32(firstId.Split('-').Last());

      var endDate = Convert.ToDateTime(publishDate);

      var lstNoticias = new List<Noticia>();

      const int take = 20;

      switch (sectionUrl)
      {
        case "curitiba-regiao":
          lstNoticias = NoticiaSrv.GetLastestVideoNewsByMicroregion(take, MicroRegionEnum.Curitiba.GetHashCode(), null, endDate).ToList();
          break;
        case "maringa-regiao":
          lstNoticias = NoticiaSrv.GetLastestVideoNewsByMicroregion(take, MicroRegionEnum.Maringa.GetHashCode(), null, endDate).ToList();
          break;
        case "foz-do-iguacu-regiao":
          lstNoticias = NoticiaSrv.GetLastestVideoNewsByMicroregion(take, MicroRegionEnum.FozDoIguacu.GetHashCode(), null, endDate).ToList();
          break;
        case "londrina-regiao":
          lstNoticias = NoticiaSrv.GetLastestVideoNewsByMicroregion(take, MicroRegionEnum.Londrina.GetHashCode(), null, endDate).ToList();
          break;
        case "ponta-grossa-regiao":
          lstNoticias = NoticiaSrv.GetLastestVideoNewsByMicroregion(take, MicroRegionEnum.PontaGrossa.GetHashCode(), null, endDate).ToList();
          break;
        case "noticias":
          lstNoticias = NoticiaSrv.GetLastestVideoNewsByEditory(take, EditorialEnum.Noticias.GetHashCode(), null, endDate).ToList();
          break;
        case "esportes":
          lstNoticias = NoticiaSrv.GetLastestVideoNewsByEditory(take, EditorialEnum.Esportes.GetHashCode(), null, endDate).ToList();
          break;
        case "entretenimento":
          lstNoticias = NoticiaSrv.GetLastestVideoNewsByEditory(take, EditorialEnum.Entretenimento.GetHashCode(), null, endDate).ToList();
          break;
        case "negocios-da-terra":
          lstNoticias = NoticiaSrv.GetLastestVideoNewsByEditory(take, EditorialEnum.NegociosDaTerra.GetHashCode(), null, endDate).ToList();
          break;
        case "where-curitiba":
          lstNoticias = NoticiaSrv.GetLastestVideoNewsByEditory(take, EditorialEnum.WhereCuritiba.GetHashCode(), null, endDate).ToList();
          break;
        case "blogs":
          lstNoticias = NoticiaSrv.GetLastestVideoNewsByEditory(take, EditorialEnum.Blogs.GetHashCode(), null, endDate).ToList();
          break;
      }

      //Remove iten
      if (lstNoticias.Any(n => n.Id == firstItemId))
        lstNoticias.Remove(lstNoticias.Single(n => n.Id == firstItemId));

      //Remove a ultima
      if (lstNoticias.Any(n => n.DataPublicacao == endDate))
        lstNoticias.Remove(lstNoticias.Single(n => n.DataPublicacao == endDate));

      return PartialView("_LastNewsItem", lstNoticias.ToList().Select(MenuItemModel.Map).ToList());

    }

    [Route("Videos/LoadNexCategoryLinks")]
    [OutputCache(Duration = 60, VaryByParam = "*", Location = OutputCacheLocation.Server)]
    public ActionResult LoadNexCategoryLinks(string lastDate, string category)
    {
      var date = Convert.ToDateTime(lastDate);

      var startDate = date.AddMonths(-1);

      var endDate = date.AddMilliseconds(-1);

      var lstNoticias = GetListOfNews(category, startDate, endDate);

      if (!lstNoticias.Any()) return null;

      var model = GetVideoSections(lstNoticias);

      ViewBag.noImageLazy = false;

      return PartialView("_CategoriaSections", model);
    }

    #endregion

    #region Methods
    //private void getTemperature(int locationId)
    //{
    //  var objCidade = LocalSrv.GetCidadeByIdCached(locationId);

    //  var obj = WeatherSrv.GetWeather(objCidade.Id);

    //  ViewBag.City = obj == null ? string.Empty : obj.City ?? string.Empty;
    //  ViewBag.Description = obj == null ? string.Empty : obj.Description ?? string.Empty;
    //  ViewBag.Icon = obj == null ? string.Empty : obj.Icon ?? string.Empty;
    //  ViewBag.Temperature = obj == null ? string.Empty : obj.Temperature == 0 ? string.Empty : obj.Temperature.ToString();
    //}

    private DestaqueVideoViewModel GetRegionHighlights(int microregionId)
    {
      var objModel = new DestaqueVideoViewModel
      {
        Url = "noticias",
        Titulo = "Notícias",
        Sections = new List<DestaqueVideoViewModel.VideoSection>()
      };

      var lstMicroregions = Microregiao.GetAllByStatus(true);

      foreach (var microregion in lstMicroregions)
      {
        var section = new DestaqueVideoViewModel.VideoSection
        {
          Url = microregion.Url,
          Title = microregion.Nome,
          Videos = NoticiaSrv.GetLastestVideoNewsByMicroregion(4, microregion.Id).Select(VideoModel.Map),
          ButtonText = "Ver mais",
          ButtonUrl = $"/videos/{microregion.Url}",
          Selected = microregionId == microregion.Id
        };

        objModel.Sections.Add(section);
      }

      if (objModel.Sections.All(s => !s.Selected))
        objModel.Sections.First().Selected = true;

      return objModel;
    }
    private DestaqueVideoViewModel GetSportsHighlights()
    {
      var section = new DestaqueVideoViewModel.VideoSection
      {
        Selected = true,
        Url = "esportes",
        Title = "Esportes",
        Videos = NoticiaSrv.GetLastestVideoNewsByEditory(4, EditorialEnum.Esportes.GetHashCode()).Select(VideoModel.Map),
        ButtonText = "Ver mais",
        ButtonUrl = "/videos/esportes"
      };

      var objModel = new DestaqueVideoViewModel
      {
        Url = "esportes",
        Titulo = "Esportes",
        Sections = { section }
      };

      return objModel;
    }
    private DestaqueVideoViewModel GetEntertainmentHighlights()
    {
      var section = new DestaqueVideoViewModel.VideoSection
      {
        Selected = true,
        Url = "entretenimento",
        Title = "Entretenimento",
        Videos = NoticiaSrv.GetLastestVideoNewsByEditory(4, EditorialEnum.Entretenimento.GetHashCode()).Select(VideoModel.Map),
        ButtonText = "Ver mais",
        ButtonUrl = "/videos/entretenimento"
      };

      var objModel = new DestaqueVideoViewModel
      {
        Url = "entretenimento",
        Titulo = "Entretenimento",
        Sections = { section }
      };

      return objModel;
    }
    private DestaqueVideoViewModel GetNegociosDaTerraHighlights()
    {
      var section = new DestaqueVideoViewModel.VideoSection
      {
        Selected = true,
        Url = "negocios-da-terra",
        Title = "Negócios da Terra",
        Videos = NoticiaSrv.GetLastestVideoNewsByEditory(4, EditorialEnum.NegociosDaTerra.GetHashCode()).Select(VideoModel.Map),
        ButtonText = "Ver mais",
        ButtonUrl = "/videos/negocios-da-terra"
      };

      var objModel = new DestaqueVideoViewModel
      {
        Url = "negocios-da-terra",
        Titulo = "Negócios da Terra",
        Sections = { section }
      };

      return objModel;
    }
    private DestaqueVideoViewModel GetBlogHighlights()
    {
      var section = new DestaqueVideoViewModel.VideoSection
      {
        Selected = true,
        Url = "blogs",
        Title = "Blogs",
        Videos = NoticiaSrv.GetLastestVideoNewsByEditory(4, EditorialEnum.Blogs.GetHashCode()).Select(VideoModel.Map),
        ButtonText = "Ver mais",
        ButtonUrl = "/videos/blogs"
      };

      var objModel = new DestaqueVideoViewModel
      {
        Url = "blogs",
        Titulo = "Blogs",
        Sections = { section }
      };

      return objModel;
    }
    private DestaqueVideoViewModel GetLiveOnHighlights()
    {
      var section = new DestaqueVideoViewModel.VideoSection
      {
        Selected = true,
        Url = "massa-news-live-on",
        Title = "Massa News Live On",
        Videos = NoticiaSrv.GetLastestVideoNewsByTag(4, 752).Select(VideoModel.Map),
        ButtonText = "Ver mais",
        ButtonUrl = "/videos/massa-news-live-on"
      };

      var objModel = new DestaqueVideoViewModel
      {
        Url = "descobrindo-curitiba",
        Titulo = "Massa News Live On",
        Sections = { section }
      };

      return objModel;
    }
    private DestaqueVideoViewModel GetDescobrindoCuritibaHighlights()
    {
      var section = new DestaqueVideoViewModel.VideoSection
      {
        Selected = true,
        Url = "descobrindo-curitiba",
        Title = "Descobrindo Curitiba",
        Videos = NoticiaSrv.GetLastestVideoNewsByTag(4, 754).Select(VideoModel.Map),
        ButtonText = "Ver mais",
        ButtonUrl = "/videos/descobrindo-curitiba"
      };
      var objModel = new DestaqueVideoViewModel
      {
        Url = "descobrindo-curitiba",
        Titulo = "Descobrindo Curitiba",
        Sections = { section }
      };
      return objModel;
    }

    private List<List<VideoModel>> GetLisofVideoModelSplited(List<VideoModel> lstVideos)
    {
      var result = new List<List<VideoModel>>();

      var count = 0;

      while (true)
      {
        var list = lstVideos.Skip(count * 4).Take(4).ToList();

        if (list.Any())
          result.Add(list);
        else
          break;

        count++;

      }

      return result;
    }

    private List<Noticia> GetListOfNews(string categoria, DateTime startDate, DateTime endDate)
    {
      var lstNoticias = new List<Noticia>();

      switch (categoria)
      {
        case "curitiba-regiao":
          lstNoticias = NoticiaSrv.GetLastestVideoNewsByMicroregion(null, MicroRegionEnum.Curitiba.GetHashCode(), startDate, endDate).ToList();
          break;
        case "maringa-regiao":
          lstNoticias = NoticiaSrv.GetLastestVideoNewsByMicroregion(null, MicroRegionEnum.Maringa.GetHashCode(), startDate, endDate).ToList();
          break;
        case "foz-do-iguacu-regiao":
          lstNoticias = NoticiaSrv.GetLastestVideoNewsByMicroregion(null, MicroRegionEnum.FozDoIguacu.GetHashCode(), startDate, endDate).ToList();
          break;
        case "londrina-regiao":
          lstNoticias = NoticiaSrv.GetLastestVideoNewsByMicroregion(null, MicroRegionEnum.Londrina.GetHashCode(), startDate, endDate).ToList();
          break;
        case "ponta-grossa-regiao":
          lstNoticias = NoticiaSrv.GetLastestVideoNewsByMicroregion(null, MicroRegionEnum.PontaGrossa.GetHashCode(), startDate, endDate).ToList();
          break;
        case "noticias":
          lstNoticias = NoticiaSrv.GetLastestVideoNewsByEditory(null, EditorialEnum.Noticias.GetHashCode(), startDate, endDate).ToList();
          break;
        case "esportes":
          lstNoticias = NoticiaSrv.GetLastestVideoNewsByEditory(null, EditorialEnum.Esportes.GetHashCode(), startDate, endDate).ToList();
          break;
        case "entretenimento":
          lstNoticias = NoticiaSrv.GetLastestVideoNewsByEditory(null, EditorialEnum.Entretenimento.GetHashCode(), startDate, endDate).ToList();
          break;
        case "negocios-da-terra":
          lstNoticias = NoticiaSrv.GetLastestVideoNewsByEditory(null, EditorialEnum.NegociosDaTerra.GetHashCode(), startDate, endDate).ToList();
          break;
        case "where-curitiba":
          lstNoticias = NoticiaSrv.GetLastestVideoNewsByEditory(null, EditorialEnum.WhereCuritiba.GetHashCode(), startDate, endDate).ToList();
          break;
        case "blogs":
          lstNoticias = NoticiaSrv.GetLastestVideoNewsByEditory(null, EditorialEnum.Blogs.GetHashCode(), startDate, endDate).ToList();
          break;
        case "massa-news-live-on":
          lstNoticias = NoticiaSrv.GetLastestVideoNewsByTag(null, 752, startDate, endDate).ToList();
          break;
        case "descobrindo-curitiba":
          lstNoticias = NoticiaSrv.GetLastestVideoNewsByTag(null, 754, startDate, endDate).ToList();
          break;
      }

      return lstNoticias;
    }

    private List<VideoCategoria.VideoSection> GetVideoSections(List<Noticia> lstNoticias)
    {
      var ci = CultureInfo.CreateSpecificCulture("pt-br");
      var mfi = ci.DateTimeFormat;
      var textInfo = new CultureInfo("pt-br", false).TextInfo;

      return lstNoticias.GroupBy(n => new { n.DataPublicacao.Value.Year, n.DataPublicacao.Value.Month })
        .Select(i => new VideoCategoria.VideoSection
        {
          Title = textInfo.ToTitleCase($"{mfi.GetMonthName(i.Key.Month)} {i.Key.Year}"),
          Videos = GetLisofVideoModelSplited(i.Select(VideoModel.Map).ToList()),
          StartDate = new DateTime(i.Key.Year, i.Key.Month, 01)
        }).ToList();
    }

    #endregion
  }
}