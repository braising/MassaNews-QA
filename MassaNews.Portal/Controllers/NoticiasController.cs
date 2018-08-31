using Entities.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using System.Web.UI;
using MassaNews.Service.Models;
using MassaNews.Service.Services;
using MassaNews.Portal.Functions;
using MassaNews.Portal.ViewModels;
using MassaNews.Service.Util;

namespace MassaNews.Portal.Controllers
{
  public class NoticiasController : Controller
  {
    #region Services
    private NoticiaService NoticiaSrv { get; }
    private LocalService LocalSrv { get; }
    private WeatherService WeatherSrv { get; }
    #endregion

    #region Constructor

    public NoticiasController()
    {
      NoticiaSrv = new NoticiaService();
      LocalSrv = new LocalService();
      WeatherSrv = new WeatherService();
    }

    #endregion

    #region Actions

    //[Route("{editorial}/{categoria}/{url}.html")]
    [OutputCache(Duration = 60, VaryByParam = "*", VaryByCustom = "Location,Origin", Location = OutputCacheLocation.Server)]
    public ActionResult Index(string editorial, string categoria, string url)
    {
      return LoadIndexNews(editorial, categoria, null, url);
    }

    [Route("blogs/{categoria}/{blog}/{url:regex(^(?!.*[.]html$)(?!posts$).*$)}")]
    public ActionResult PostWithouHtml(string categoria, string blog, string url)
    {
      return new RedirectResult($"/blogs/{categoria}/{blog}/{url}.html", true);
    }

    [Route("blogs/{categoria}/{blog}/{url}.html")]
    [OutputCache(Duration = 60, VaryByParam = "*", VaryByCustom = "Location", Location = OutputCacheLocation.Server)]
    public ActionResult Post(string categoria, string blog, string url)
    {
      return LoadIndexNews("blogs", categoria, blog, url);
    }

    [Route("blogs/{categoria}/preview/{url}.html")]
    public ActionResult PostPreview(string categoria, string url)
    {
      return LoadIndexNews("blogs", categoria, null, url, true);
    }

    [Route("{editorial}/{categoria}/preview/{url}.html")]
    public ActionResult Preview(string editorial, string categoria, string url)
    {
      return LoadIndexNews(editorial, categoria, null, url, true);
    }

    #endregion

    #region Partial Actions

    [Route("Noticia/LoadNexNavegationLink")]
    [OutputCache(Duration = 60, VaryByParam = "*", Location = OutputCacheLocation.Server)]
    public ActionResult LoadNexNavegationLink(string firstId, string scopeId, string publishDate, string navegationType)
    {
      var first = Convert.ToInt32(firstId.Split('-').Last());

      int idScope;

      int.TryParse(scopeId, out idScope);

      var locationId = CookieFx.GetLocationId(Request);

      //Get the user city
      var objCidade = Cidade.Load(locationId);

      var objNoticia = (Noticia)null;

      switch (navegationType)
      {
        case "plantao":
          {
            //Sua Região
            objNoticia = NoticiaSrv.GetNextNewsCached(objCidade.MicroregiaoId, DateTime.Parse(publishDate), new[] { first });
            break;
          }
        case "parana":
          {
            //Paraná
            objNoticia = NoticiaSrv.GetNextNewsCached(objCidade.MicroregiaoId, DateTime.Parse(publishDate), new[] { first },
              true);
            break;
          }
        case "categoria":
          {
            //Categoria
            objNoticia = NoticiaSrv.GetNextNewsByCategoryCached(idScope, DateTime.Parse(publishDate), new[] { first });
            break;
          }
        case "tags":
          {
            //Tags
            objNoticia = NoticiaSrv.GetNextNewsByTagCached(idScope, DateTime.Parse(publishDate), new[] { first });
            break;
          }
        case "fotos":
          {
            //Fotos
            objNoticia = NoticiaSrv.GetNextNewsByFeaturedCached(Destaque.Galeria.Id, DateTime.Parse(publishDate), new[] { first });
            break;
          }
        case "videos":
          //Videos
          objNoticia = NoticiaSrv.GetNextNewsByFeaturedCached(Destaque.Video.Id, DateTime.Parse(publishDate), new[] { first });
          break;
        case "blog":
          {
            //Categoria
            objNoticia = NoticiaSrv.GetNextNewsByBlog(idScope, DateTime.Parse(publishDate), new[] { first });
            break;
          }
      }

      return objNoticia == null ? null : PartialView("_NavItem", new NoticiaNavItem(objNoticia, false));

    }

    [Route("Noticia/LoadNexNews")]
    [OutputCache(Duration = 60, VaryByParam = "*", Location = OutputCacheLocation.Server)]
    public ActionResult LoadNexNews(string noticiaUrl, bool allowComments)
    {
      //Recupera a origem
      var origem = Request.UrlReferrer?.AbsolutePath.TrimEnd('/') ?? string.Empty;

      //Get the location id
      var locationId = CookieFx.GetLocationId(Request);

      //Remove .html
      noticiaUrl = noticiaUrl.Remove(noticiaUrl.Length - 5, 5);

      var url = noticiaUrl.TrimStart('/').Split('/').Last();

      var objNoticia = Noticia.GetByUrl(url);

      ViewBag.AllowComments = allowComments;

      return PartialView("_NoticiaItem", new NewsItemViewModel(objNoticia, ControllerContext));
    }

    #endregion

    #region Private Methods

    private ActionResult LoadIndexNews(string editorialUrl, string categoryUrl, string blogurl, string newsUrl, bool isPreview = false)
    {
      try
      {

        // redirect where curitiba
        if (editorialUrl == "where-curitiba"){
          return new RedirectResult("http://www.wherecuritiba.com.br", true);
        }

        var newsId = ToolService.GetIdByUrl(Request.Url.ToString());

        //Get the news object
        var objNoticia = null as Noticia;

        if (newsId.HasValue)
          objNoticia = Noticia.Load(newsId.Value);

        if (objNoticia == null)
          objNoticia = Noticia.GetByUrl(newsUrl);

        #region Validations
        //Redirect 301 to new url
        if (!isPreview)
        {
          #region Redirect

          var originUrl = Request.RawUrl.Remove(Request.RawUrl.Length - 5);

          if (objNoticia != null)
          {
            if (originUrl != objNoticia.UrlFull)
              return new RedirectResult($"{objNoticia.UrlFull}.html", true);
          }
          else
          {
            var redirectUrl = UrlRedirect.GetByUrl(originUrl);

            if (!string.IsNullOrEmpty(redirectUrl))
              return new RedirectResult($"{redirectUrl}.html", true);
          }

          #endregion
        }

        if (objNoticia == null)
          return new HttpStatusCodeResult(HttpStatusCode.NotFound);

        if (objNoticia.Autores != null && objNoticia.Autores.Count() > 0)
        {
          //19 = Estadão, 65 = Folhapress
          var objAutorGoogleNews = objNoticia.Autores.Where(a => a.Id == 19 || a.Id == 65).FirstOrDefault();

          ViewBag.isAutorGoogleNews = objAutorGoogleNews != null ? true : false;
        }

        //Caso a noticia for inativa responde com redirect permanente para a home
        if ((objNoticia.StatusId == Status.Inativa.Id || !objNoticia.Categoria.Status) && !isPreview)
          return new RedirectResult("/", true);

        //Caso a noticia for diferente de publicada responde com 404
        if (objNoticia.StatusId != Status.Publicada.Id && !isPreview)
          return new HttpStatusCodeResult(HttpStatusCode.NotFound);

        #endregion

        //Recupera a origem
        var urlReferrer = Request.UrlReferrer?.AbsolutePath.TrimEnd('/') ?? string.Empty;

        //Ultimas Notícias
        var lastestNews = new List<Noticia>();

        //Get the location id
        var locationId = CookieFx.GetLocationId(Request);

        //Get the user city
        var objCidade = Cidade.Load(locationId);

        //getTemperature(locationId);

        if (urlReferrer.StartsWith("/tag"))
        {
          var tagUrl = urlReferrer.Split('/').Last();

          var tag = Tag.GetByUrl(tagUrl);

          if (tag == null) return null;

          var isNewsWithTag = false;

          var newsWithTag = NoticiaSrv.GetTagsByNewsIdCached(objNoticia.Id);

          foreach (var item in newsWithTag)
          {
            if (item.Id == tag.Id)
            {
              isNewsWithTag = true;
              break;
            }
          }

          if (isNewsWithTag)
            lastestNews = GetValuesOfTag(urlReferrer, objNoticia).ToList();
          else if (editorialUrl == "blogs")
            lastestNews = GetValuesOfBlogCategory(objNoticia).ToList();
          else
            if (objNoticia.CidadeId.HasValue && objNoticia.CategoriaUrl.Equals("plantao"))
          {
            if (objNoticia.Cidade.MicroregiaoId == objCidade.MicroregiaoId)
              lastestNews = GetValuesOfMyRegion(objCidade.MicroregiaoId, objNoticia).ToList();
            else if (objNoticia.Cidade.MicroregiaoId != objCidade.MicroregiaoId)
              lastestNews = GetValuesOfParana(objCidade.MicroregiaoId, objNoticia).ToList();
          }
          else if (!objNoticia.CidadeId.HasValue && objNoticia.CategoriaUrl.Equals("plantao"))
            lastestNews = GetValuesOfParana(objCidade.MicroregiaoId, objNoticia).ToList();
          else
            lastestNews = GetValuesOfCategory(objNoticia).ToList();

        }
        else if (urlReferrer.StartsWith("/fotos"))
        {
          lastestNews = GetValuesOfGallery().ToList();
        }
        else if (editorialUrl == "blogs")
        {
          lastestNews = GetValuesOfBlogCategory(objNoticia).ToList();
        }
        else
        {

          if (objNoticia.CidadeId.HasValue)
          {
            //If the news has the category 'plantao'
            if (objNoticia.CategoriaUrl.Equals("plantao"))
            {
              if (objNoticia.Cidade.MicroregiaoId == objCidade.MicroregiaoId)
              {
                //Sua Região
                lastestNews = GetValuesOfMyRegion(objCidade.MicroregiaoId, objNoticia).ToList();
              }
              else if (objNoticia.Cidade.MicroregiaoId != objCidade.MicroregiaoId)
              {
                //Paraná
                lastestNews = GetValuesOfParana(objCidade.MicroregiaoId, objNoticia).ToList();
              }
            }
            else
            {
              //If the news is's category 'plantao
              lastestNews = GetValuesOfCategory(objNoticia).ToList();
            }
          }
          else
          {
            //If the news has the category 'plantao'
            if (objNoticia.CategoriaUrl.Equals("plantao"))
            {
              //Paraná
              lastestNews = GetValuesOfParana(objCidade.MicroregiaoId, objNoticia).ToList();
            }
            else
            {
              //If the news is's category 'plantao
              lastestNews = GetValuesOfCategory(objNoticia).ToList();
            }
          }
        }

        //Habilita Comentários
        ViewBag.AllowComments = !isPreview;

        //Caso a notícia exista na lista é removida para evitar duplicidade
        if (lastestNews.Any(n => n.Id == objNoticia.Id))
          lastestNews.Remove(lastestNews.Single(n => n.Id == objNoticia.Id));

        //insere noticia na lista de notícias
        lastestNews.Insert(0, objNoticia);

        ViewBag.NavItems = lastestNews.Select(n => new NoticiaNavItem(n, lastestNews.IndexOf(n).Equals(0)));

        ViewBag.Lastest4News = lastestNews.Take(4).ToList();

        //Set the list of categories
        if (objNoticia.Categoria.EditorialId == 4 && ViewBag.NavigationType == "categoria")
        {
          ViewBag.Categorias = NoticiaSrv.GetCategoriasByEditorial(objNoticia.Categoria.EditorialId);
          ViewBag.MenuCategoriaId = objNoticia.Categoria.CategoriaPaiId.HasValue ? objNoticia.Categoria.CategoriaPaiId : objNoticia.Categoria.Id;
        }

        ViewBag.AmpLink = $"{Constants.UrlWeb}/amp{objNoticia.UrlFull}.html";

        /* base model defaults */
        var model = new NoticiaModel
        {
          //Title = $"{objNoticia.Titulo} - Massa News {objNoticia.Cidade.Nome}",
          Description = $"{(objNoticia.Conteudo.Length > 220 ? Text.RemoveHtmlTags(objNoticia.Conteudo.Substring(0, 220) + "... Leia mais no Massa News!") : Text.RemoveHtmlTags(objNoticia.Conteudo) + " Leia no Massa News!")}",
          Robots = isPreview ? "noindex, nofollow" : "index, follow",
          Canonical = $"{Constants.UrlWeb}{objNoticia.UrlFull}.html",
          News = new NewsItemViewModel(objNoticia, ControllerContext)
        };

        if (objNoticia.Cidade != null && !string.IsNullOrEmpty(objNoticia.Cidade.Nome))
          model.Title = $"{objNoticia.Titulo} - Massa News {objNoticia.Cidade.Nome}";
        else if (objNoticia.Blog != null && !string.IsNullOrEmpty(objNoticia.Blog.Titulo))
          model.Title = $"{objNoticia.Titulo} - {objNoticia.Blog.Titulo} - Massa News";
        else
          model.Title = $"{objNoticia.Titulo} - Massa News";

        if (!string.IsNullOrEmpty(objNoticia.ImgLg))
          model.ImgOpenGraph = $"{Constants.UrlDominioEstaticoUploads}/{"noticias"}/{objNoticia.ImgLg}";
        else if (objNoticia.Blog != null)
          model.ImgOpenGraph = $"{Constants.UrlWeb}/content/images/avatar/blogs/{objNoticia.Blog.Url}.jpg";

        // Página
        ViewBag.Pagina = "interna";

        // ID
        ViewBag.Id = objNoticia.Id;

        // Editoria
        ViewBag.EditoriaUrl = objNoticia.Categoria.Editorial.Url;
        ViewBag.EditoriaTitulo = objNoticia.Categoria.Editorial.Titulo;

        // Categoria
        ViewBag.Categoria = objNoticia.CategoriaUrl;

        //Formata Noticia Site Antigo Negocio da Terra
        if (editorialUrl == "negocios-da-terra" && objNoticia.DataPublicacao < new DateTime(2017, 09, 12))
        {
          var aux = model.News.News.Conteudo.Split('\n');
          model.News.News.Conteudo = "";

          foreach (var item in aux)
          {
            if (!String.IsNullOrEmpty(item))
            {
              model.News.News.Conteudo = model.News.News.Conteudo + "<p>" + item + "</p>";
            }
          }
        }       

        return View("index", model);
      }
      catch (Exception ex)
      {
        var vars = new Dictionary<string, string>
        {
          {"Editorial", editorialUrl},
          {"Category", categoryUrl},
          {"NewsUrl", newsUrl}
        };

        NewRelic.Api.Agent.NewRelic.NoticeError(ex, vars);

        throw;
      }
    }

    private IEnumerable<Noticia> GetValuesOfTag(string origem, Noticia objNoticia)
    {
      var tagUrl = origem.Split('/').Last();

      var tag = Tag.GetByUrl(tagUrl);

      if (tag == null) return null;

      var lastestNews = tag.GetLastestNews(0, Constants.TakeNoticias, null).ToList();

      ViewBag.ActiveNav = tag.Titulo;
      ViewBag.ScopeId = tag.Id;
      ViewBag.LinkActiveNav = $"/tags/{tag.Url}";
      ViewBag.NavigationType = "tags";
      ViewBag.ThemeValue = null;

      return lastestNews;
    }

    private IEnumerable<Noticia> GetValuesOfGallery()
    {
      var lastestNews = Noticia.GetLastestNewsByFeaturedCached(Constants.TakeNoticias, Destaque.Galeria.Id);

      ViewBag.ActiveNav = "Fotos";
      ViewBag.LinkActiveNav = $"/{"fotos"}";
      ViewBag.NavigationType = "fotos";
      ViewBag.ThemeValue = "fotos";

      return lastestNews;
    }

    private IEnumerable<Noticia> GetValuesOfMyRegion(int microregiaoId, Noticia objNoticia)
    {
      var lastestNews = Noticia.GetLastestNews(Constants.TakeNoticias, 1, 1, microregiaoId, null);

      ViewBag.ActiveNav = "Sua Região";
      ViewBag.LinkActiveNav = $"/{objNoticia.EditorialUrl}/{objNoticia.CategoriaUrl}";
      ViewBag.NavigationType = "plantao";
      ViewBag.ThemeValue = "noticias";

      return lastestNews;
    }

    private IEnumerable<Noticia> GetValuesOfParana(int microregiaoId, Noticia objNoticia)
    {
      var lastestNews = Noticia.GetLastestNews(Constants.TakeNoticias, 1, 1, null, null);

      ViewBag.ActiveNav = "Paraná";
      ViewBag.LinkActiveNav = $"/{objNoticia.EditorialUrl}/{"parana"}";
      ViewBag.NavigationType = "parana";
      ViewBag.ThemeValue = "noticias";

      return lastestNews;
    }

    private IEnumerable<Noticia> GetValuesOfCategory(Noticia objNoticia)
    {
      var lastestNews = Noticia.GetLastestNews(Constants.TakeNoticias, objNoticia.EditorialId, objNoticia.CategoriaId, null, null);

      ViewBag.ActiveNav = objNoticia.Categoria.Titulo;
      ViewBag.LinkActiveNav = $"/{objNoticia.EditorialUrl}/{objNoticia.CategoriaUrl}";
      ViewBag.NavigationType = "categoria";
      ViewBag.ThemeValue = objNoticia.Categoria.Theme.Value;

      ViewBag.ScopeId = objNoticia.Categoria.Id;
      ViewBag.Editorial = objNoticia.Categoria.Editorial;

      return lastestNews;
    }

    private IEnumerable<Noticia> GetValuesOfBlogCategory(Noticia objNoticia)
    {
      var lastestNews = NoticiaSrv.GetLastestBlogNews(Constants.TakeNoticias, objNoticia.BlogId.Value);

      ViewBag.ActiveNav = objNoticia.Blog.Titulo;
      ViewBag.LinkActiveNav = $"/blogs/{objNoticia.CategoriaUrl}/{objNoticia.BlogUrl}/posts";
      ViewBag.NavigationType = "blog";
      ViewBag.ThemeValue = objNoticia.Categoria.Theme.Value;
      ViewBag.ScopeId = objNoticia.BlogId;
      ViewBag.Editorial = objNoticia.Categoria.Editorial;

      return lastestNews;
    }

    #endregion

    #region Json
    [HttpGet]
    [AllowAnonymous]
    [Route("Noticia/NewsAccessedEvent")]
    public JsonResult NewsAccessedEvent(string newsId)
    {
      var id = Convert.ToInt32(newsId.Substring(3));

      var cookie = Request.Cookies[Constants.CookieName];

      if (cookie != null)
      {
        var locationId = Convert.ToInt32(cookie.Value);
        EventService.NewsAccessed(id, locationId);
      }

      return Json("Ok", JsonRequestBehavior.AllowGet);
    }

    //[HttpPost]
    //[AllowAnonymous]
    //[Route("Noticia/ShareEmail")]
    //public JsonResult ShareEmailPost(string nome, string email, string emailDest, string mensagem, string url)
    //{
    //  var newsid = ToolService.GetIdByUrl(url);

    //  if (!newsid.HasValue)
    //    return Json(new { status = "nok" });

    //  var news = Noticia.Load(newsid.Value);

    //  var msg = @"
    //  <div class='content'>
    //    <br>
    //    <table width='100%' border='0' cellpadding='0' cellspacing='0' align='center' style='border-bottom:1px solid #e5e5e5; padding-bottom: 15px;'>
    //      <tr>
    //        <td align='center'>
    //          <a href='" + Constants.UrlWeb + @"' target='_blank'>
    //            <img src='" + Constants.UrlWeb + @"/content/images/logos/massa-news.png' alt='Massa News' width='150'>
    //          </a>
    //        </td>
    //      </tr>
    //    </table>
    //    <br>
    //    <table width='100%' border='0' cellpadding='0' cellspacing='0' align='center'>
    //      <tr>
    //        <td style='font-size: 14px; text-align: center; color: #4f4f4f;'>
    //          <p>
    //            Olá, seu contato <strong>" + nome + @"</strong> (" + email + @")
    //            <br>compartilhou um link do Massa News com você. Confira:
    //          </p>
    //        </td>
    //      </tr>
    //    </table>
    //    <br>
    //    <table bgcolor='#f8f8f8' width='100%' border='0' cellpadding='0' cellspacing='0' align='center'>
    //      <tr>
    //        <td colspan='3'>&nbsp;</td>
    //      </tr>
    //      <tr>
    //        <td class='aspas' width='75' style='font-size: 80px; color: #d2232a; text-align: center; vertical-align: top;'><img src='" + Constants.UrlWeb + @"/content/images/share/aspas-esquerda.gif'></td>
    //        <td style='font-size: 14px; text-align: center; color: #838383;'>
    //          <p><i>" + mensagem + @"</i></p>
    //        </td>
    //        <td class='aspas' width='75' style='font-size: 80px; color: #d2232a; text-align: center; vertical-align: bottom;'><img src='" + Constants.UrlWeb + @"/content/images/share/aspas-direita.gif'></td>
    //      </tr>
    //      <tr>
    //        <td colspan='3'>&nbsp;</td>
    //      </tr>
    //    </table>
    //    <br>
    //    <table width='100%' border='0' cellpadding='0' cellspacing='0' align='center'>
    //      <tr>
    //        <td>
    //          <hr style='border: 0; height:1px; background-color: #e5e5e5;'>
    //          <a href='" + url + "' style='text-decoration:none;' title='" + news.Titulo + @"'><h1 style='padding: 0 20px; font-size: 20px; text-align: center; color: #d2232a;'>" + news.Titulo + @"</h1></a>
    //        </td>
    //      </tr>";

    //  if (!string.IsNullOrEmpty(news.ImgMd))
    //  {
    //    msg +=
    //    @"<tr>
    //        <td align='center'>
    //          <a href='" + url + "' target='_blank' title='" + news.Titulo + @"'>
    //            <img src='" + $"{Constants.UrlDominioEstaticoUploads}/noticias/{news.ImgMd}" + "' alt='" + news.Titulo + @"'>
    //          </a>
    //        </td>
    //      </tr>";
    //  }

    //  msg +=
    //     @"<tr>
    //        <td style='text-align: center;' '>
    //          <br>
    //          <a href='" + url + @"' target='_blank' title='Clique aqui para ler' style='text-decoration: none;'>
    //            <img src='" + Constants.UrlWeb + @"/content/images/share/botao.gif' alt='Clique aqui para ler'>
    //          </a>
    //        </td>
    //      </tr>
    //    </table>
    //    <br>
    //    <hr style='border: 0; height:1px; background-color: #e5e5e5;'>
    //    <table width='100%' border='0' cellpadding='0' cellspacing='0' align='center'>
    //      <tr>
    //        <td style='font-size: 11px; text-align: center; color: #969696;'>
    //          <p style='margin-top: 15px;'>
    //            " + nome + @" (" + email + @") solicitou que enviássemos este e-mail para você.
    //            <br>Caso tenha alguma dúvida, visite nossa <a href='" + Constants.UrlWeb + @"/institucional/fale-conosco' target='_blank' style='color: #969696;' title='Fale conosco'><u>página de Contato</u></a>.
    //          </p>
    //          <br>
    //        </td>
    //      </tr>
    //    </table>
    //  </div>";

    //  var html = @"
    //  <!DOCTYPE html>
    //  <html lang='pt-br'>
    //    <head>
    //      <title>Massa News - A notícia em movimento</title>
    //      <meta http-equiv='Content-Type' content='text/html; charset=utf-8'>
    //      <meta name='viewport' content='width=device-width, initial-scale=1'>
    //      <style>
    //        img { max-width: 100%; }
    //        body { -webkit-font-smoothing:antialiased;  -webkit-text-size-adjust:none;  width: 100%!important;  height: 100%; }
    //        .content { padding:10px; max-width:600px; margin:0 auto; display:block; }
    //        .content table { width: 100%; }
    //        @media only screen and (max-width: 600px) { .aspas{ width: 25px; } }
    //      </style>
    //    </head>
    //    <body bgcolor='#FFFFFF' leftmargin='0' topmargin='0' marginwidth='0' marginheight='0' style='font-family: Arial, sans-serif;'>" + msg + @"</body>
    //  </html>";

    //  ToolService.SendEmail($"{nome} compartilhou um link do Massa News com você", html, emailDest);

    //  return Json(new { status = "ok" });
    //}
    #endregion
  }
}