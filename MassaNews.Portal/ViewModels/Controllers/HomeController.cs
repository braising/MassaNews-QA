using Admin.Enum;
using Entities.Classes;
using MassaNews.Portal.Models;
using MassaNews.Portal.ViewModels;
using MassaNews.Service.Enum;
using MassaNews.Service.Models;
using MassaNews.Service.Services;
using MassaNews.Service.Util;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.UI;

namespace MassaNews.Portal.Controllers
{
  public class HomeController : BaseController
  {
    #region Services
    private NoticiaService NoticiaSrv { get; }
    private EnqueteService EnqueteSrv { get; }
    #endregion

    #region Constructor

    public HomeController()
    {
      NoticiaSrv = new NoticiaService();
      EnqueteSrv = new EnqueteService();
    }

    #endregion

    #region Actions
    [OutputCache(Duration = 60, VaryByCustom = "Location", Location = OutputCacheLocation.Server)]
    public ActionResult Index()
    {
      //Get the model
      var objModel = GetHomeIndexModel(GetMyLocationId);

      //Set the Live
      ViewBag.IsLive = objModel.Highlights[0].IsLive.HasValue ? objModel.Highlights[0].IsLive.Value : false;

      //Recupera as 4 ultimas notícias que são exibidas na sidebar
      ViewBag.Lastest4News = Noticia.GetLastestNews(4).ToList();

      //Recuper as 5 ultimas notícias mais acessadas 
      ViewBag.Popular5News = Noticia.GetMoreAccessedNews(null);

      return View(objModel);
    }

    public ActionResult Regua(string tagUrl)
    {
      return View(Tag.GetByUrl(tagUrl));
    }

    #endregion

    #region Partial Actions
    [Route("home/loadsectionmodule")]
    [OutputCache(Duration = 60, VaryByParam = "*", Location = OutputCacheLocation.Server)]
    public ActionResult LoadSectionModule(int locationId, int sectionId)
    {
      //Get the city 
      var city = Cidade.Load(locationId);

      var home = city.Microregion.Home;

      //Get list of news highlights
      var lstHighlights = Noticia.GetHighlightNewsByHome(home.Status ? home.Id : 1).ToList();

      //Create initial negation list with hiloghts id's
      var notInList = lstHighlights.Select(s => s.Id).ToList();

      //Get the section
      var section = Section.All().Where(s => s.Id == sectionId).Single();

      if (sectionId == Section.Videos.Id)
        return PartialView("_DestaqueVideos", GetVideoHighlights(city, ref notInList));

      //Get the other modules
      var model = GetDestaquesComtags(section, home, ref notInList);

      model.isLazy = false;

      return PartialView("_DestaqueComTags", model);
    }

    [Route("home/loadblogsmodule")]
    [OutputCache(Duration = 60, VaryByParam = "*", Location = OutputCacheLocation.Server)]
    public ActionResult LoadBlogsModule(int microregionId)
    {
      var model = GetBlogListByMicroregion(microregionId);

      return PartialView("_Blogs", model);
    }

    #endregion

    #region Methods
    private HomeIndex GetHomeIndexModel(int locationId)
    {
      var cacheSrv = new RedisCacheService();

      //Get the city 
      var city = Cidade.Load(locationId);

      //Get the home object
      var objHome = city.Microregion.Home;

      #region GET CACHE

      //Set the const time value
      const int time = 1;
      
      //Set the cache key
      var key = $"Home:{objHome.Id}";

      //Find in the cache for the model
      var objModel = cacheSrv.Get<HomeIndex>(key);

      //If model exists in cache return the object
      if (objModel != null)
        return objModel;
      #endregion

      //Save the current home state 
      var currentHome = objHome;

      //If Home is disabled load the main home
      if (!objHome.Status)
        objHome = Home.Load(1); //Home Curitiba

      //Get list of news highlights
      var lstHighlights = Noticia.GetHighlightNewsByHome(objHome.Id).ToList();

      //Create initial negation list with hiloghts id's
      var notInList = lstHighlights.Select(s => s.Id).ToList();

      objModel = new HomeIndex
      {
        //Base
        Title = "Massa News - A notícia em movimento!",
        Description = "O Massa News oferece a melhor cobertura jornalística do Paraná com notícias personalizadas da sua região.",
        Robots = "index, follow",
        Canonical = Constants.UrlWeb,

        //Model
        Cidade = Cidade.Load(12), //Curitiba //objCidade,
        SidebarHighlight = objHome.SidebarHighlight,
        TemplateId = objHome.Highlight.TemplateId,
        Highlights = lstHighlights
      };

      //SUA REGIÃO
      //Este módulo recebe a cidade original
      objModel.DestaqueComTagsSuaRegiao = GetDestaquesComtags(Section.SuaRegiao, currentHome, ref notInList);

      //ESPORTES
      objModel.DestaqueComTagsEsportes = GetDestaquesComtags(Section.Esportes, currentHome, ref notInList);

      //ENTRETEDIMENTO
      objModel.DestaqueComTagsEntretedimento = GetDestaquesComtags(Section.Entretenimento, currentHome, ref notInList);

      //PARANÁ - TODOS OS PLANTÕES
      objModel.DestaqueComTagsParana = GetDestaquesComtags(Section.Parana, currentHome, ref notInList);

      //CATEGORIAS DESTAQUES 
      objModel.CategoriasDestaquesModel = GetCategoriaDestaquesModel(objHome.Id, ref notInList);

      //VIAJAR É MASSA
      objModel.DestaqueComTagsViajarEMassa = GetDestaquesComtags(Section.ViajarEMassa, currentHome, ref notInList);

      //WHERE CURITIBA
      if(currentHome.Id == 1)
        objModel.DestaqueComTagsWhereCuritiba = GetDestaquesComtags(Section.WhereCuritiba, currentHome, ref notInList);

      //NEGÓCIOS DA TERRA
      objModel.DestaqueComTagsNegociosDaTerra = GetDestaquesComtags(Section.NegociosDaTerra, currentHome, ref notInList);

      //VIDEOS
      objModel.DestaqueVideo = GetVideoHighlights(city, ref notInList);

      //FOTOS (GALERIAS)
      objModel.HighlightsFotos = Noticia.GetBySection(8, Section.Fotos, 1, ref notInList,  true).ToList();

      //BLOGS
      objModel.Blogs = GetBlogListByMicroregion(currentHome.MicroregiaoId);

      #region ENQUETE
      var statusEnquete = EnqueteSrv.GetStatusEnquete();

      switch (statusEnquete.Status)
      {
        case (int)EnqueteEnum.Estadual:
          objModel.ShowEnquete = true;
          objModel.EnqueteId = Constants.EnqueteRegionalId;
          break;
        case (int)EnqueteEnum.Regional:
          foreach (var item in statusEnquete.RegioesEnquetes.Where(item => objHome.MicroregiaoId == item.MicroregiaoId && item.Status))
          {
            objModel.ShowEnquete = true;
            objModel.EnqueteId = item.EnqueteId;
          }
          break;
        default:
          objModel.ShowEnquete = false;
          break;
      }
      #endregion

      //Set the cache
      cacheSrv.Set(key, objModel, time);

      return objModel;
    }

    private CategoriasDestaquesModel GetCategoriaDestaquesModel(int homeId, ref List<int> notInList)
    {
      const int take = 2;

      var model = new CategoriasDestaquesModel
      {
        HighlightsEducacao = Noticia.GetBySection(take, Section.Educacao, homeId, ref notInList, true).ToList(),
        HighlightsBrasil = Noticia.GetBySection(take, Section.Brasil, homeId, ref notInList, true).ToList(),
        HighlightsMundo = Noticia.GetBySection(take, Section.Mundo, homeId, ref notInList, true).ToList(),
        HighlightsPolitica = Noticia.GetBySection(take, Section.Politica, homeId, ref notInList, true).ToList(),
        HighlightsEconomia = Noticia.GetBySection(take, Section.Economia, homeId, ref notInList, true).ToList(),
        HighlightsConcursosEmpregos = Noticia.GetBySection(take, Section.ConcursosEmpregos, homeId, ref notInList, true).ToList()
        //HighlightsMaisNoticias = Noticia.GetBySection(take, Section.MaisNoticias, homeId, ref notInList, true).ToList()
      };

      return model;
    }

    private DestaqueComTags GetDestaquesComtags(Section section, Home home, ref List<int> notInList)
    {
      if (!section.Editorialid.HasValue)
        return null;

      var objEditorial = Editorial.Load(section.Editorialid.Value);

      var lstHighlights = Noticia.GetBySection(5, section, (home.Status || Section.SuaRegiao.Id == section.Id) ? home.Id : 1, ref notInList, true).ToList();

      var objDestaqueComTags = new DestaqueComTags
      {
        Key = section.Key,
        Theme = section.Theme,
        Titulo = section.Desc,
        LinkTitulo = section.Url,
        LinkImgTitulo = objEditorial.IconUrl,
        Highlights = lstHighlights,
        Tags = NoticiaSrv.GetTagsBySection(section.Id, home.Id).ToList(),
        isLazy = !(section.Id == 1) //1 = sua região
      };

      return objDestaqueComTags;
    }

    private DestaqueVideoViewModel GetVideoHighlights(Cidade objCidade, ref List<int> notInList)
    {
      var sectionDestaque = new DestaqueVideoViewModel.VideoSection
      {
        Url = "destaques",
        Title = "Destaques",
        Selected = true,
        Videos = Service.Models.Noticia.GetBySection(4, Section.Videos, 1, ref notInList, true).Take(4).Select(VideoModel.Map),
        ButtonText = "Ver todos os vídeos",
        ButtonUrl = "/videos"
      };

      var sectionMicroregion = new DestaqueVideoViewModel.VideoSection
      {
        Url = objCidade.Url,
        Title = objCidade.Microregion.Nome,
        Videos = NoticiaSrv.GetLastestVideoNewsByMicroregion(4, objCidade.MicroregiaoId).Select(VideoModel.Map),
        ButtonText = "Ver todos os vídeos",
        ButtonUrl = "/videos"
      };

      var sectionEsportes = new DestaqueVideoViewModel.VideoSection
      {
        Url = "esportes",
        Title = "Esportes",
        Videos = NoticiaSrv.GetLastestVideoNewsByEditory(4, EditorialEnum.Esportes.GetHashCode()).Select(VideoModel.Map),
        ButtonText = "Ver todos os vídeos",
        ButtonUrl = "/videos"
      };

      var sectionEntretenimento = new DestaqueVideoViewModel.VideoSection
      {
        Url = "entretenimento",
        Title = "Entretenimento",
        Videos = NoticiaSrv.GetLastestVideoNewsByEditory(4, EditorialEnum.Entretenimento.GetHashCode()).Select(VideoModel.Map),
        ButtonText = "Ver todos os vídeos",
        ButtonUrl = "/videos"
      };

      var sectionDescobrindoCuritiba = new DestaqueVideoViewModel.VideoSection
      {
        Url = "descobrindo-curitiba",
        Title = "Descobrindo Curitiba",
        Videos = NoticiaSrv.GetLastestVideoNewsByTag(4, 754).Select(VideoModel.Map),
        ButtonText = "Ver todos os vídeos",
        ButtonUrl = "/videos"
      };

      var objModel = new DestaqueVideoViewModel
      {
        Titulo = "Vídeos",
        Sections = { sectionDestaque, sectionMicroregion, sectionEsportes, sectionEntretenimento, sectionDescobrindoCuritiba }
      };

      return objModel;
    }

    private List<Blog> GetBlogListByMicroregion(int microregionId)
    {
      //Get the microregion
      var microregion = Microregiao.Load(microregionId);

      //Get the home
      var home = microregion.Home;

      //Get secondary hilight by section
      var sh = home.GetSecondaryHighlight(Section.Blogs.Id);

      //Get the list of blog in the microregion
      //var lstBlogs = Blog.GetByMicroregion(home.MicroregiaoId).Where(b => b.Id != sh.BlogId1 && b.Id != sh.BlogId2 && b.Id != sh.BlogId3).Take(5).ToList();
      var lstBlogs = Blog.GetByMicroregion(home.MicroregiaoId).Where(b => b.Id != sh.BlogId1 && b.Id != sh.BlogId2).Take(5).ToList();

      if (sh.Status)
      {
        lstBlogs.Insert(0, Blog.LoadWithLastNews(sh.BlogId1.Value));
        lstBlogs.Insert(1, Blog.LoadWithLastNews(sh.BlogId2.Value));
        //lstBlogs.Insert(2, Blog.LoadWithLastNews(sh.BlogId3.Value));
      }

      foreach (var objBlog in lstBlogs)
      {
        if (string.IsNullOrEmpty(objBlog.Img))
        {
          var objautor = objBlog.Autores.FirstOrDefault();

          if (objautor != null)
            objBlog.Img = $"{Constants.UrlDominioEstaticoUploads}/autores/{objautor.Avatar}";
          else
            objBlog.Img = Url.Content("~/content/images/placeholders/no-avatar.png");
        }
        else
        {
          objBlog.Img = $"{Constants.UrlDominioEstaticoUploads}/{"blog"}/{objBlog.Img}";
        }
      }

      return lstBlogs.Take(5).ToList();
    }

    #endregion

    #region Json
    [HttpGet]
    [AllowAnonymous]
    [Route("home/GetWeatherByLocationId")]
    public JsonResult GetWeatherByLocationId(int locationId)
    {
      return Json(new WeatherService().GetSimepar(locationId), JsonRequestBehavior.AllowGet);
    }

    [HttpGet]
    public JsonResult GetNotification(int? idNoticia)
    {
      var objCidade = Cidade.Load(GetMyLocationId);

      var objHome = objCidade.Microregion.Home;

      var highlight = Highlight.GetByHome(objHome.Id);

      var noticia = highlight.NoticiaHighlight.OrderBy(a => a.Ordem).FirstOrDefault().Noticia;

      var showNoticia = false;

      if (idNoticia.HasValue && idNoticia.Value != noticia.Id)
        showNoticia = true;

      var urlCategoria = noticia.Categoria.CategoriaPaiId.HasValue ? noticia.Categoria.CategoriaPai.Url : noticia.Categoria.Url;

      var iconUrl = string.Empty;

      if (noticia.ImgThumb != null)
        iconUrl = $"{Constants.UrlDominioEstaticoUploads}/noticias/{noticia.ImgThumb}";
      else
        iconUrl = "/content/images/favicons/android-chrome-144x144.png";

      return Json(new
      {
        ShowNoticia = showNoticia,
        Id = noticia.Id,
        Title = noticia.Categoria.Titulo,
        Body = noticia.Chamada,
        IconUrl = iconUrl,
        NoticiaURL = $"/{noticia.Categoria.Editorial.Url}/{urlCategoria}/{noticia.Url}"
      }, JsonRequestBehavior.AllowGet);
    }
    #endregion
  }
}