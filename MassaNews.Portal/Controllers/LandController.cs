using MassaNews.Portal.Functions;
using MassaNews.Portal.Models;
using MassaNews.Portal.ViewModels;
using MassaNews.Service.Models;
using MassaNews.Service.Util;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Web.Mvc;
using System.Web.UI;
using MassaNews.Service.Services;
using MassaNews.Service.Enum;

namespace MassaNews.Portal.Controllers
{
  public class LandController : Controller
  {
    #region Properts
    private CampeonatoService CampeonatoSrv { get; }
    #endregion

    #region Services
    private NoticiaService NoticiaSrv { get; }
    #endregion

    #region Constructor

    public LandController()
    {
      NoticiaSrv = new NoticiaService();
      CampeonatoSrv = new CampeonatoService();
    }
    #endregion

    [Route("planejamento-de-vida")]
    [HttpGet]
    [OutputCache(Duration = 60, VaryByCustom = "Location", VaryByParam = "*", Location = OutputCacheLocation.ServerAndClient)]
    public ActionResult LandBCK(int p = 1)
    {
      //get the tag
      var tagUrl = "planejamento-de-vida";

      //get the tag
      var objTag = Tag.GetByUrl(tagUrl);

      //Init the model
      var model = new LandViewModel()
      {
        Title = "Planejamento de Vida - BCK Corretora de Seguros",
        Description = "Quer saber mais sobre Seguros? Confira aqui!",
        Robots = "index, follow",
        Canonical = $"{Constants.UrlWeb}/planejamento-de-vida",
        ImgOpenGraph = $"{Constants.UrlWeb}/content/images/landing/bck/avatar.png"
      };

      //Set the news
      if (p == 1)
      {
        model.Highlights = objTag.GetLastestNews(p - 1, 5, null, true).ToList();
        model.NewsList = objTag.GetNewsList(p - 1, 20, model.Highlights.Select(s=> s.Id).ToList()).ToList();
      }
      else
      {
        model.Highlights = null;
        model.NewsList = objTag.GetNewsList(p - 1, 20, null).ToList();
      }

      //Get the news count 
      var nRowsCount = model.NewsList.Any() ? model.NewsList.First().Total : 0;

      //get the number of pages 
      var pages = Convert.ToInt32(Math.Ceiling(((double)nRowsCount / 20)));

      //Add pagination if has more than one page
      if (pages > 1)
        ViewBag.Paginacao = Pagination.AddPagination(p, Convert.ToInt32(pages), 5, true);

      //Set viewbag's
      ViewBag.ActiveNav = "Planejamento de Vida";

      // Página
      ViewBag.Pagina = "planejamento-de-vida";

      //return the model to the view
      return View(model);
    }

    [Route("planejamento-de-vida")]
    [HttpPost]
    public ActionResult LandBCKPost()
    {
      var obj = new
      {
        Nome = Request.Form["Nome"],
        Email = Request.Form["Email"],
        Telefone = Request.Form["Telefone"]
      };

      var objData = new TempData
      {
        Description = "Landing BCK",
        Data = JsonConvert.SerializeObject(obj),
        Registered = DateTime.Now
      };

      objData.Save();

      return Json("ok");

    }


    [Route("desafiando-chefs")]
    [HttpGet]
    [OutputCache(Duration = 60, VaryByCustom = "Location", VaryByParam = "*", Location = OutputCacheLocation.ServerAndClient)]
    public ActionResult DesafiandoChefs(int p = 1)
    {
      //get the tag
      var tagUrl = "desafiando-chefs";

      //get the tag
      var objTag = Tag.GetByUrl(tagUrl);

      //Init the model
      var model = new LandViewModel()
      {
        Title = "Desafiando Chefs - Temporada Damasco",
        Description = "O Grupo Massa e a Damasco desafiam chefs renomados para um desafio de dar água na boca! Eles terão que provar suas habilidades fazendo receitas incríveis com uma condição, utilizar Café Damasco em uma websérie exclusiva e deliciosa!",
        Robots = "index, follow",
        Canonical = $"{Constants.UrlWeb}/desafiando-chefs",
        ImgOpenGraph = $"{Constants.UrlWeb}/content/images/landing/desafiando-chefs/avatar.png"
      };

      //Set the news
      if (p == 1)
      {
        model.Highlights = objTag.GetLastestNews(p - 1, 5, null, true).ToList();
        model.NewsList = objTag.GetNewsList(p - 1, 20, model.Highlights.Select(s=> s.Id).ToList()).ToList();
      }
      else
      {
        model.Highlights = null;
        model.NewsList = objTag.GetNewsList(p - 1, 20, null).ToList();
      }

      //Get the news count 
      var nRowsCount = model.NewsList.Any() ? model.NewsList.First().Total : 0;

      //get the number of pages 
      var pages = Convert.ToInt32(Math.Ceiling(((double)nRowsCount / 20)));

      //Add pagination if has more than one page
      if (pages > 1)
        ViewBag.Paginacao = Pagination.AddPagination(p, Convert.ToInt32(pages), 5, true);

      //Vídeos
      var sectionDesafiandoChefs = new DestaqueVideoViewModel.VideoSection
      {
        Url = "desafiando-chefs",
        Title = "desafiando-chefs",
        Videos = NoticiaSrv.GetLastestVideoNewsByTag(4, 759).Select(VideoModel.Map),
        ButtonText = "Ver todos os vídeos",
        ButtonUrl = "/videos"
      };

      var objModel = new DestaqueVideoViewModel
      {
        Titulo = "Vídeos",
        Sections = { sectionDesafiandoChefs }
      };

      model.DestaqueVideo = objModel;

      //Set viewbag's
      ViewBag.ActiveNav = "Desafiando Chefs";

      // Página
      ViewBag.Pagina = "desafiando-chefs";

      //return the model to the view
      return View(model);
    }

    // [Route("desafiando-chefs")]
    // [HttpPost]
    // public ActionResult DesafiandoChefsPost()
    // {
    //   var obj = new
    //   {
    //     Nome = Request.Form["Nome"],
    //     Email = Request.Form["Email"],
    //     Telefone = Request.Form["Telefone"]
    //   };

    //   var objData = new TempData
    //   {
    //     Description = "Desafiando Chefs",
    //     Data = JsonConvert.SerializeObject(obj),
    //     Registered = DateTime.Now
    //   };

    //   objData.Save();

    //   return Json("ok");
    // }


    [Route("farol-do-saber-e-inovacao")]
    [HttpGet]
    [OutputCache(Duration = 60, VaryByCustom = "Location", VaryByParam = "*", Location = OutputCacheLocation.ServerAndClient)]
    public ActionResult FarolDoSaberEInovacao(int p = 1)
    {
      //get the tag
      var tagUrl = "farol-do-saber-e-inovacao";

      //get the tag
      var objTag = Tag.GetByUrl(tagUrl);

      //Init the model
      var model = new LandViewModel()
      {
        Title = "Farol do Saber e Inovação - Massa News",
        Description = "Farol do Saber e Inovação. Confira!",
        Robots = "index, follow",
        Canonical = $"{Constants.UrlWeb}/farol-do-saber-e-inovacao",
        ImgOpenGraph = $"{Constants.UrlWeb}/content/images/landing/farol-do-saber-e-inovacao/avatar.png"
      };

      //Set the news
      if (p == 1)
      {
        model.Highlights = objTag.GetLastestNews(p - 1, 5, null, true).ToList();
        model.NewsList = objTag.GetNewsList(p - 1, 20, model.Highlights.Select(s=> s.Id).ToList()).ToList();
      }
      else
      {
        model.Highlights = null;
        model.NewsList = objTag.GetNewsList(p - 1, 20, null).ToList();
      }

      //Get the news count 
      var nRowsCount = model.NewsList.Any() ? model.NewsList.First().Total : 0;

      //get the number of pages 
      var pages = Convert.ToInt32(Math.Ceiling(((double)nRowsCount / 20)));

      //Add pagination if has more than one page
      if (pages > 1)
        ViewBag.Paginacao = Pagination.AddPagination(p, Convert.ToInt32(pages), 5, true);

      //Set viewbag's
      ViewBag.ActiveNav = "Farol do Saber e Inovação";

      // Página
      ViewBag.Pagina = "farol-do-saber-e-inovacao";

      //return the model to the view
      return View(model);
    }


    [Route("viaje-pelos-campos-gerais")]
    [HttpGet]
    [OutputCache(Duration = 60, VaryByCustom = "Location", VaryByParam = "*", Location = OutputCacheLocation.ServerAndClient)]
    public ActionResult ViajePelosCamposGerais(int p = 1)
    {
      //get the tag
      var tagUrl = "viaje-pelos-campos-gerais";

      //get the tag
      var objTag = Tag.GetByUrl(tagUrl);

      //Init the model
      var model = new LandViewModel()
      {
        Title = "Viaje pelos Campos Gerais - Massa News",
        Description = "Histórias, Cultura, Gastronomia e Diversão. Tudo isso bem pertinho de você. Viaje pelos Campos Gerais.",
        Robots = "index, follow",
        Canonical = $"{Constants.UrlWeb}/viaje-pelos-campos-gerais",
        ImgOpenGraph = $"{Constants.UrlWeb}/content/images/landing/viaje-pelos-campos-gerais/avatar.png"
      };

      //Set the news
      if (p == 1)
      {
        model.Highlights = objTag.GetLastestNews(p - 1, 5, null, true).ToList();
        model.NewsList = objTag.GetNewsList(p - 1, 20, model.Highlights.Select(s=> s.Id).ToList()).ToList();
      }
      else
      {
        model.Highlights = null;
        model.NewsList = objTag.GetNewsList(p - 1, 20, null).ToList();
      }

      //Get the news count 
      var nRowsCount = model.NewsList.Any() ? model.NewsList.First().Total : 0;

      //get the number of pages 
      var pages = Convert.ToInt32(Math.Ceiling(((double)nRowsCount / 20)));

      //Add pagination if has more than one page
      if (pages > 1)
        ViewBag.Paginacao = Pagination.AddPagination(p, Convert.ToInt32(pages), 5, true);

      //Set viewbag's
      ViewBag.ActiveNav = "Viaje pelos Campos Gerais";

      // Página
      ViewBag.Pagina = "viaje-pelos-campos-gerais";

      //return the model to the view
      return View(model);
    }


    [Route("narussia")]
    [HttpGet]
    //[OutputCache(Duration = 60, VaryByCustom = "Location", VaryByParam = "*", Location = OutputCacheLocation.ServerAndClient)]
    //[OutputCache(Duration = 60, Location = OutputCacheLocation.Server)]
    [OutputCache(Duration = 60, VaryByCustom = "Location", Location = OutputCacheLocation.Server)]
    public ActionResult MassaNaRussia(int p = 1)
    {
      //get the tag
      var tagUrl = "massa-news-na-russia";
      var tagModuleUrl = "especial-russia";

      //get the tag
      var objTag = Tag.GetByUrl(tagUrl);
      var objTagModule = Tag.GetByUrl(tagModuleUrl);

      //Init the model
      var model = new LandViewModel()
      {
        Title = "Massa na Rússia - Tudo sobre o maior evento de Futebol do Mundo!",
        Description = "Notícias, Tabela de Jogos, Classificação, Bolão e muito mais do maior evento de Futebol do Mundo. Confira!",
        Robots = "index, follow",
        Canonical = $"{Constants.UrlWeb}/narussia",
        ImgOpenGraph = $"{Constants.UrlWeb}/content/images/landing/massa-na-russia/avatar.png"
      };

      //Set the news
      if (p == 1)
      {
        model.Highlights = objTag.GetLastestNews(p - 1, 5, null, true).ToList();
        model.NewsList = objTag.GetNewsList(p - 1, 20, model.Highlights.Select(s=> s.Id).ToList()).ToList();

        // 1) LINE NEWS
        model.LineNews = objTagModule.GetLastestNews(p - 1, 4, null, true).ToList();
      }
      else
      {
        model.Highlights = null;
        model.NewsList = objTag.GetNewsList(p - 1, 20, null).ToList();
      }

      //Get the news count 
      var nRowsCount = model.NewsList.Any() ? model.NewsList.First().Total : 0;

      //get the number of pages 
      var pages = Convert.ToInt32(Math.Ceiling(((double)nRowsCount / 20)));

      //Add pagination if has more than one page
      if (pages > 1)
        ViewBag.Paginacao = Pagination.AddPagination(p, Convert.ToInt32(pages), 5, true);

      // // 2) VÍDEOS
      // var sectionVideos = new DestaqueVideoViewModel.VideoSection
      // {
      //   //Videos = NoticiaSrv.GetLastestVideoNewsByTag(4, 797).Select(VideoModel.Map),
      //   Videos = NoticiaSrv.GetLastestVideoNewsByTag(4, 794).Select(VideoModel.Map)
      // };

      // var objModel = new DestaqueVideoViewModel
      // {
      //   Titulo = "Direto da Rússia",
      //   //Url = "",
      //   Sections = { sectionVideos }
      // };

      // model.DestaqueVideo = objModel;

      // Tables
      ViewBag.LstClassificacao = CampeonatoSrv.GetHtmlClassificacaoCopaDoMundo();
      ViewBag.LstRodadas = CampeonatoSrv.GetListRodadas((int)CampeonatoEnum.CopaDoMundo);

      //Set viewbag's
      ViewBag.ActiveNav = "Massa na Rússia";

      // Página
      ViewBag.Pagina = "massa-na-russia";

      //return the model to the view
      return View(model);
    }



    [Route("educacao-financeira")]
    [HttpGet]
    [OutputCache(Duration = 60, VaryByCustom = "Location", VaryByParam = "*", Location = OutputCacheLocation.ServerAndClient)]
    public ActionResult EducacaoFinanceira(int p = 1)
    {
      //get the tag
      var tagUrl = "educacao-financeira";

      //get the tag
      var objTag = Tag.GetByUrl(tagUrl);

      //Init the model
      var model = new LandViewModel()
      {
        Title = "Educação Financeira - Massa News",
        Description = "Educação Financeira. Realização: Sicredi",
        Robots = "index, follow",
        Canonical = $"{Constants.UrlWeb}/educacao-financeira",
        ImgOpenGraph = $"{Constants.UrlWeb}/content/images/landing/educacao-financeira/avatar.png"
      };

      //Set the news
      if (p == 1)
      {
        model.Highlights = objTag.GetLastestNews(p - 1, 5, null, true).ToList();
        model.NewsList = objTag.GetNewsList(p - 1, 20, model.Highlights.Select(s=> s.Id).ToList()).ToList();
      }
      else
      {
        model.Highlights = null;
        model.NewsList = objTag.GetNewsList(p - 1, 20, null).ToList();
      }

      //Get the news count 
      var nRowsCount = model.NewsList.Any() ? model.NewsList.First().Total : 0;

      //get the number of pages 
      var pages = Convert.ToInt32(Math.Ceiling(((double)nRowsCount / 20)));

      //Add pagination if has more than one page
      if (pages > 1)
        ViewBag.Paginacao = Pagination.AddPagination(p, Convert.ToInt32(pages), 5, true);

      //Vídeos
      var sectionEducacaoFinanceira = new DestaqueVideoViewModel.VideoSection
      {
        Url = "educacao-financeira",
        Title = "educacao-financeira",
        Videos = NoticiaSrv.GetLastestVideoNewsByTag(4, 794).Select(VideoModel.Map),
        ButtonText = "Ver todos os vídeos",
        ButtonUrl = "/videos"
      };

      var objModel = new DestaqueVideoViewModel
      {
        Titulo = "Vídeos",
        Url = "https://www.youtube.com/playlist?list=PLIsicGjQ0iOvxJoseEwr3_66qxHOng4aq",
        Sections = { sectionEducacaoFinanceira }
      };

      model.DestaqueVideo = objModel;

      //Set viewbag's
      ViewBag.ActiveNav = "Educação Financeira";

      // Página
      ViewBag.Pagina = "educacao-financeira";

      //return the model to the view
      return View(model);
    }

    [Route("transmita-calor")]
    [HttpGet]
    [OutputCache(Duration = 60, VaryByCustom = "Location", VaryByParam = "*", Location = OutputCacheLocation.ServerAndClient)]
    public ActionResult TransmitaCalor(int p = 1)
    {
      //get the tag
      var tagUrl = "transmita-calor";

      //get the tag
      var objTag = Tag.GetByUrl(tagUrl);

      //Init the model
      var model = new LandViewModel()
      {
        Title = "Nós Transmitimos Calor - Massa News",
        Description = "Campanha do Agasalho 2018. Nós Transmitimos Calor. #transmitacalor",
        Robots = "index, follow",
        Canonical = $"{Constants.UrlWeb}/transmita-calor",
        ImgOpenGraph = $"{Constants.UrlWeb}/content/images/landing/transmita-calor/avatar.png"
      };

      //Set the news
      if (p == 1)
      {
        model.Highlights = objTag.GetLastestNews(p - 1, 5, null, true).ToList();
        model.NewsList = objTag.GetNewsList(p - 1, 20, model.Highlights.Select(s=> s.Id).ToList()).ToList();
      }
      else
      {
        model.Highlights = null;
        model.NewsList = objTag.GetNewsList(p - 1, 20, null).ToList();
      }

      //Get the news count 
      var nRowsCount = model.NewsList.Any() ? model.NewsList.First().Total : 0;

      //get the number of pages 
      var pages = Convert.ToInt32(Math.Ceiling(((double)nRowsCount / 20)));

      //Add pagination if has more than one page
      if (pages > 1)
        ViewBag.Paginacao = Pagination.AddPagination(p, Convert.ToInt32(pages), 5, true);

      //Vídeos
      // var sectionTransmitaCalor = new DestaqueVideoViewModel.VideoSection{
      //   Url = "transmita-calor",
      //   Title = "transmita-calor",
      //   Videos = NoticiaSrv.GetLastestVideoNewsByTag(4, 796).Select(VideoModel.Map),
      //   ButtonText = "Ver todos os vídeos",
      //   ButtonUrl = "/videos"
      // };

      // var objModel = new DestaqueVideoViewModel
      // {
      //   Titulo = "Vídeos",
      //   Url = "https://www.youtube.com/user/redemassa?sub_confirmation=1",
      //   Sections = { sectionTransmitaCalor }
      // };

      // model.DestaqueVideo = objModel;

      //Set viewbag's
      ViewBag.ActiveNav = "Nós Transmitimos Calor";

      // Página
      ViewBag.Pagina = "transmita-calor";

      //return the model to the view
      return View(model);
    }


    [Route("curitiba-criativa")]
    [HttpGet]
    [OutputCache(Duration = 60, VaryByCustom = "Location", VaryByParam = "*", Location = OutputCacheLocation.ServerAndClient)]
    public ActionResult CuritibaCriativa(int p = 1)
    {
      //get the tag
      var tagUrl = "curitiba-criativa";

      //get the tag
      var objTag = Tag.GetByUrl(tagUrl);

      //Init the model
      var model = new LandViewModel()
      {
        Title = "Curitiba Criativa - Massa News",
        Description = "Curitiba Criativa - Massa News",
        Robots = "noindex, nofollow",
        Canonical = $"{Constants.UrlWeb}/curitiba-criativa",
        ImgOpenGraph = $"{Constants.UrlWeb}/content/images/landing/curitiba-criativa/avatar.png"
      };

      //Set the news
      if (p == 1)
      {
        model.Highlights = objTag.GetLastestNews(p - 1, 5, null, true).ToList();
        model.NewsList = objTag.GetNewsList(p - 1, 20, model.Highlights.Select(s=> s.Id).ToList()).ToList();
      }
      else
      {
        model.Highlights = null;
        model.NewsList = objTag.GetNewsList(p - 1, 20, null).ToList();
      }

      //Get the news count 
      var nRowsCount = model.NewsList.Any() ? model.NewsList.First().Total : 0;

      //get the number of pages 
      var pages = Convert.ToInt32(Math.Ceiling(((double)nRowsCount / 20)));

      //Add pagination if has more than one page
      if (pages > 1)
        ViewBag.Paginacao = Pagination.AddPagination(p, Convert.ToInt32(pages), 5, true);

      //Vídeos
      // var sectionTransmitaCalor = new DestaqueVideoViewModel.VideoSection{
      //   Url = "curitiba-criativa",
      //   Title = "curitiba-criativa",
      //   Videos = NoticiaSrv.GetLastestVideoNewsByTag(4, 796).Select(VideoModel.Map),
      //   ButtonText = "Ver todos os vídeos",
      //   ButtonUrl = "/videos"
      // };

      // var objModel = new DestaqueVideoViewModel
      // {
      //   Titulo = "Vídeos",
      //   Url = "https://www.youtube.com/user/redemassa?sub_confirmation=1",
      //   Sections = { sectionTransmitaCalor }
      // };

      // model.DestaqueVideo = objModel;

      //Set viewbag's
      ViewBag.ActiveNav = "Curitiba Criativa";

      // Página
      ViewBag.Pagina = "curitiba-criativa";

      //return the model to the view
      return View(model);
    }


  }
}