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
    #region Services
    private NoticiaService NoticiaSrv { get; }
    #endregion

    #region Constructor

    public LandController()
    {
      NoticiaSrv = new NoticiaService();
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



  }
}