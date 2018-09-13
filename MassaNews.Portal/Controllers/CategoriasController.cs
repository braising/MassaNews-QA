using Entities.Classes;
using Entities.Contexts;
using Entities.Tables;
using MassaNews.Portal.Functions;
using MassaNews.Portal.ViewModels;
using MassaNews.Service.Enum;
using MassaNews.Service.Services;
using MassaNews.Service.Util;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Web.Mvc;
using System.Web.UI;

namespace MassaNews.Portal.Controllers
{
  public class CategoriasController : BaseController
  {
    #region Actions

    #region Paraná

    [Route("noticias/parana")]
    [OutputCache(Duration = 60, VaryByCustom = "Location", VaryByParam = "*", Location = OutputCacheLocation.ServerAndClient)]
    public ActionResult Parana(int p = 1)
    {
      //if the page is lower than 0 will return a 404 error
      if (p <= 0)
        return new HttpStatusCodeResult(HttpStatusCode.NotFound);

      var model = Service.Models.Noticia.GetLastestNewsInParanaByPage(p);

      ViewBag.Pagina = "parana";
      ViewBag.ActiveNav = "Paraná";
      ViewBag.UrlPaginacao = "/noticias/parana";

      if (model.TotalPages > 1)
        ViewBag.Paginacao = Pagination.AddPagination(p, Convert.ToInt32(model.TotalPages), 5, true);

      //Recupera as 4 ultimas notícias que são exibidas na sidebar
      ViewBag.Lastest4News = Service.Models.Noticia.GetLastestNews(4).ToList();

      //Recuper as 5 ultimas notícias mais acessadas 
      ViewBag.Popular5News = Service.Models.Noticia.GetMoreAccessedNews(1); //editorial 1 = Noticias

      return View("Parana", model);
    }
    #endregion 

    #region Noticias
    [Route("noticias/{categoria}")]
    [OutputCache(Duration = 60, VaryByCustom = "Location", VaryByParam = "*", Location = OutputCacheLocation.ServerAndClient)]
    public ActionResult NoticiasWithCategory(string categoria, int p = 1)
    {
      return CustomActionCategory("noticias", null, categoria, p);
    }

    [Route("noticias/{categoria}/{subCategoria:regex(^(?!.*[.]html$).*$)}")]
    [OutputCache(Duration = 60, VaryByCustom = "Location", VaryByParam = "*", Location = OutputCacheLocation.ServerAndClient)]
    public ActionResult NoticiasWithCategory(string categoria, string subCategoria, int p = 1)
    {
      return CustomActionCategory("noticias", categoria, subCategoria, p);
    }

    #endregion

    #region Esportes

    [Route("esportes/{categoria}")]
    [OutputCache(Duration = 60, VaryByCustom = "Location", VaryByParam = "*", Location = OutputCacheLocation.ServerAndClient)]
    public ActionResult EsportesWithCategory(string categoria, int p = 1)
    {
      if (categoria == "futebol")
      {
        var CampeonatoSrv = new CampeonatoService();
        ViewBag.ShowResumo = true;
        ViewBag.TbParanaense = CampeonatoSrv.GetHtmlResumoClassificacao(CampeonatoEnum.Paranaense);
        ViewBag.TbClassificacaoA = CampeonatoSrv.GetHtmlResumoClassificacao(CampeonatoEnum.BrasileiraoA);
        ViewBag.TbClassificacaoB = CampeonatoSrv.GetHtmlResumoClassificacao(CampeonatoEnum.BrasileiraoB);
      }

      return CustomActionCategory("esportes", null, categoria, p);
    }

    [Route("esportes/{categoria}/{subCategoria:regex(^(?!.*[.]html$).*$)}")]
    [OutputCache(Duration = 60, VaryByCustom = "Location", VaryByParam = "*", Location = OutputCacheLocation.ServerAndClient)]
    public ActionResult EsportesWithCategory(string categoria, string subCategoria, int p = 1)
    {
      return CustomActionCategory("esportes", categoria, subCategoria, p);
    }

    #endregion

    #region Entretenimento

    [Route("entretenimento/{categoria}")]
    [OutputCache(Duration = 60, VaryByCustom = "Location", VaryByParam = "*", Location = OutputCacheLocation.ServerAndClient)]
    public ActionResult EntretenimentoWithCategory(string categoria, int p = 1)
    {
      return CustomActionCategory("entretenimento", null, categoria, p);
    }

    [Route("entretenimento/{categoria}/{subCategoria:regex(^(?!.*[.]html$).*$)}")]
    [OutputCache(Duration = 60, VaryByCustom = "Location", VaryByParam = "*", Location = OutputCacheLocation.ServerAndClient)]
    public ActionResult EntretenimentoWithCategory(string categoria, string subCategoria, int p = 1)
    {
      return CustomActionCategory("entretenimento", categoria, subCategoria, p);
    }

    #endregion

    #region NegociosDaTerra
    //redirect
    [Route("agronegocio/{categoria}")]
    [OutputCache(Duration = 60, VaryByCustom = "Location", VaryByParam = "*", Location = OutputCacheLocation.ServerAndClient)]
    public ActionResult AgronegocioWithCategory(string categoria, int p = 1)
    {
      return new RedirectResult($"/negocios-da-terra/{categoria}", true);
    }

    [Route("negocios-da-terra/{categoria}")]
    [OutputCache(Duration = 60, VaryByCustom = "Location", VaryByParam = "*", Location = OutputCacheLocation.ServerAndClient)]
    public ActionResult NegociosDaTerraWithCategory(string categoria, int p = 1)
    {
      return CustomActionCategory("negocios-da-terra", null, categoria, p);
    }

    //redirect
    [Route("agronegocio/{categoria}/{subCategoria:regex(^(?!.*[.]html$).*$)}")]
    [OutputCache(Duration = 60, VaryByCustom = "Location", VaryByParam = "*", Location = OutputCacheLocation.ServerAndClient)]
    public ActionResult AgronegocioWithCategory(string categoria, string subCategoria, int p = 1)
    {
      return new RedirectResult($"/negocios-da-terra/{categoria}/{subCategoria:regex(^(?!.*[.]html$).*$)}", true);
    }
    [Route("negocios-da-terra/{categoria}/{subCategoria:regex(^(?!.*[.]html$).*$)}", Order = 1)]
    [OutputCache(Duration = 60, VaryByCustom = "Location", VaryByParam = "*", Location = OutputCacheLocation.ServerAndClient)]
    public ActionResult NegociosDaTerraWithCategory(string categoria, string subCategoria, int p = 1)
    {
      return CustomActionCategory("negocios-da-terra", categoria, subCategoria, p);
    }

    #endregion

    #region Where Curitiba

    [Route("where-curitiba/{categoria}")]
    [OutputCache(Duration = 60, VaryByCustom = "Location", VaryByParam = "*", Location = OutputCacheLocation.ServerAndClient)]
    public ActionResult WhereCuritibaWithCategory(string categoria, int p = 1)
    {
      return new RedirectResult("http://www.wherecuritiba.com.br", true);
      //return CustomActionCategory("where-curitiba", null, categoria, p);
    }

    [Route("where-curitiba/{categoria}/{subCategoria:regex(^(?!.*[.]html$).*$)}")]
    [OutputCache(Duration = 60, VaryByCustom = "Location", VaryByParam = "*", Location = OutputCacheLocation.ServerAndClient)]
    public ActionResult WhereCuritibaWithCategory(string categoria, string subCategoria, int p = 1)
    {
      return new RedirectResult("http://www.wherecuritiba.com.br", true);
      //return CustomActionCategory("where-curitiba", categoria, subCategoria, p);
    }

    #endregion

    #region Viajar é Massa

    [Route("viajar-e-massa/{categoria}")]
    [OutputCache(Duration = 60, VaryByCustom = "Location", VaryByParam = "*", Location = OutputCacheLocation.ServerAndClient)]
    public ActionResult ViajarEMassaWithCategory(string categoria, int p = 1)
    {
      return CustomActionCategory("viajar-e-massa", null, categoria, p);
    }

    [Route("viajar-e-massa/{categoria}/{subCategoria:regex(^(?!.*[.]html$).*$)}")]
    [OutputCache(Duration = 60, VaryByCustom = "Location", VaryByParam = "*", Location = OutputCacheLocation.ServerAndClient)]
    public ActionResult ViajarEMassaWithCategory(string categoria, string subCategoria, int p = 1)
    {
      return CustomActionCategory("viajar-e-massa", categoria, subCategoria, p);
    }

    #endregion

    #region Tags
    [Route("tags/{tag}")]
    [OutputCache(Duration = 60, VaryByCustom = "Location", VaryByParam = "*", Location = OutputCacheLocation.ServerAndClient)]
    public ActionResult Tag(string tag, int p = 1)
    {

      // BCK
      if (tag == "planejamento-de-vida")
        return new RedirectResult("/planejamento-de-vida", true);

      // Desafiando Chefs
      if (tag == "desafiando-chefs")
        return new RedirectResult("/desafiando-chefs", true);

      // Episódios do Desafiando Chefs
      if (tag == "episodios-do-desafiando-chefs")
        return new RedirectResult("/desafiando-chefs", true);

      // Farol do Saber e Inovação
      if (tag == "farol-do-saber-e-inovacao")
        return new RedirectResult("/farol-do-saber-e-inovacao", true);

      // Viaje pelo Campos Gerais
      if (tag == "viaje-pelos-campos-gerais")
        return new RedirectResult("/viaje-pelos-campos-gerais", true);

      // Educação Financeira
      if (tag == "educacao-financeira")
        return new RedirectResult("/educacao-financeira", true);

      // Copa do Mundo
      if (tag == "especial-massa-na-russia")
        return new RedirectResult("/narussia", true);

      if (tag == "direto-da-russia")
        return new RedirectResult("/narussia", true);

      if (tag == "direto-na-russia")
        return new RedirectResult("/narussia", true);

      // Transmita Calor
      if (tag == "transmita-calor")
        return new RedirectResult("/transmita-calor", true);

      // Curitiba Criativa
      if (tag == "curitiba-criativa")
        return new RedirectResult("/curitiba-criativa", true);

      // Festival Gastronômico
      if (tag == "festival-gastronomico")
        return new RedirectResult("/festival-gastronomico", true);

      #region Log Tag
      if (Constants.LogTags && Request.ServerVariables["HTTP_X_FORWARDED_FOR"] != null)
      {
        var th0 = new Thread(() =>
        {
          var lstIp = Request.ServerVariables["HTTP_X_FORWARDED_FOR"].Split(',');

          var cookie = Request.Cookies[Constants.CookieName];

          var obj = new
          {
            IP = lstIp[0],
            REFERENCE = Request.UrlReferrer?.AbsolutePath ?? "No Reference",
            AGENT = Request.UserAgent,
            TAG = tag,
            COOKIE = cookie != null ? Convert.ToInt32(cookie.Value) : (int?)null
          };

          var objData = new Service.Models.TempData
          {
            Description = "Log Tag",
            Data = JsonConvert.SerializeObject(obj),
            Registered = DateTime.Now
          };

          objData.Save();
        });

        th0.Start();
      }
      #endregion

      //if the page is lower than 0 will return a 404 error
      if (p <= 0)
        return new HttpStatusCodeResult(HttpStatusCode.NotFound);

      //Get response of news by tag 
      var model = Service.Models.Tag.GetLastestNewsByPage(tag, p);

      #region redirect
      //if the tag do not exist will search for an redirect
      if (model == null)
      {
        var redirectUrl = Service.Models.UrlRedirect.GetByUrl(tag);

        if (!string.IsNullOrEmpty(redirectUrl) && !redirectUrl.Equals(tag))
          return new RedirectResult($"{redirectUrl}", true);
      }
      #endregion

      //If the object tag is not found it will return a 404 error
      if (model == null)
        return new HttpStatusCodeResult(HttpStatusCode.NotFound);

      //Get list of lastest news
      var lstLastestNews = Service.Models.Noticia.GetLastestNews(4).ToList();

      //Get list of more accessed news
      var lstMoreAccessedNews = Service.Models.Noticia.GetMoreAccessedNews(null).ToList();

      //------------------------------------

      ViewBag.Pagina = "tags";
      ViewBag.Tags = model.TagUrl;
      ViewBag.ActiveNav = model.TagTitle;
      ViewBag.PaginaAtual = p;

      if (model.TotalPages > 1)
        ViewBag.Paginacao = Pagination.AddPagination(p, Convert.ToInt32(model.TotalPages), 5, true);

      //Set the sidebar lastest news
      ViewBag.Lastest4News = lstLastestNews;

      //Set the sidebar more accessed news
      ViewBag.Popular5News = lstMoreAccessedNews;

      #region Tabela Facil
      var CampeonatoSrv = new CampeonatoService();

      if (model.TagUrl == "campeonato-paranaense")
      {
        ViewBag.ShowResumo = true;
        ViewBag.TbParanaense = CampeonatoSrv.GetHtmlResumoClassificacao(CampeonatoEnum.Paranaense);
      }

      if (model.TagUrl.Equals("brasileirao-serie-a"))
      {
        ViewBag.TbClassificacaoA = CampeonatoSrv.GetHtmlResumoClassificacao(CampeonatoEnum.BrasileiraoA);
        ViewBag.TbActiveClassifA = true;
      }

      if (model.TagUrl.Equals("brasileirao-serie-b"))
      {
       ViewBag.TbClassificacaoB = CampeonatoSrv.GetHtmlResumoClassificacao(CampeonatoEnum.BrasileiraoB);
       ViewBag.TbActiveClassifB = true;
      }
      #endregion

      //Gamb
      //if (model.Highlights != null && model.Highlights.Any())
      //{
      //  var objTag = Service.Models.Tag.Load(model.TagId);
      //  ViewBag.TagHighlights = objTag.GetLastestNews(0, 5, null, true).ToList();
      //}

      return View("Tag", model);
    }
    #endregion

    #region Ultimas Noticias

    [Route("ultimas-noticias")]
    [OutputCache(Duration = 60, VaryByCustom = "Location", VaryByParam = "*", Location = OutputCacheLocation.ServerAndClient)]
    public ActionResult Lastest(int p = 1)
    {
      //if the page is lower than 0 will return a 404 error
      if (p <= 0)
        return new HttpStatusCodeResult(HttpStatusCode.NotFound);

      var model = Service.Models.Noticia.GetAllLastestNewsByPage(p);

      if (model.TotalPages > 1)
        ViewBag.Paginacao = Pagination.AddPagination(p, Convert.ToInt32(model.TotalPages), 5, true);

      ViewBag.Pagina = "ultimas-noticias";
      ViewBag.ActiveNav = "Últimas Notícias";
      ViewBag.UrlPaginacao = "/ultimas-noticias";

      //Recuper as 5 ultimas notícias mais acessadas 
      ViewBag.Popular5News = Service.Models.Noticia.GetMoreAccessedNews(null);

      return View(model);
    }

    [Route("ultimas-noticias/{editorial}")]
    [OutputCache(Duration = 60, VaryByCustom = "Location", VaryByParam = "*", Location = OutputCacheLocation.ServerAndClient)]
    public ActionResult LastestEditorial(string editorial, int p = 1)
    {
      //if the page is lower than 0 will return a 404 error
      if (p <= 0)
        return new HttpStatusCodeResult(HttpStatusCode.NotFound);

      var objEditorial = GetEditorialByUrl(editorial);

      if (objEditorial == null)
        return new HttpStatusCodeResult(HttpStatusCode.NotFound);

      var model = Service.Models.Noticia.GetAllLastestNewsEditorialByPage(p, editorial);

      if (model.TotalPages > 1)
        ViewBag.Paginacao = Pagination.AddPagination(p, Convert.ToInt32(model.TotalPages), 5, true);

      ViewBag.Pagina = "ultimas-noticias-" + editorial;
      ViewBag.ActiveNav = "Últimas Notícias";
      ViewBag.UrlPaginacao = "/ultimas-noticias/" + editorial;
      ViewBag.EditorialTitulo = objEditorial.Titulo;
      ViewBag.EditoriaUrl = editorial;

      //Recuper as 5 ultimas notícias mais acessadas 
      ViewBag.Popular5News = Service.Models.Noticia.GetMoreAccessedNews(null);

      return View(model);
    }

    #endregion

    #endregion

    #region Custom Actions
    [OutputCache(Duration = 60, VaryByCustom = "Location", VaryByParam = "*", Location = OutputCacheLocation.ServerAndClient)]
    public ActionResult CustomActionCategory(string editoryUrl, string categoryPaiUrl, string categoryUrl, int p)
    {

      // Desafiando Chefs
      if (categoryUrl == "desafiando-chefs")
        return new RedirectResult("/desafiando-chefs", true);

      // Farol do Saber e Inovação
      if (categoryUrl == "farol-do-saber-e-inovacao")
        return new RedirectResult("/farol-do-saber-e-inovacao", true);

      // Viaje pelo Campos Gerais
      if (categoryUrl == "viaje-pelos-campos-gerais")
        return new RedirectResult("/viaje-pelos-campos-gerais", true);

      // Educação Financeira
      if (categoryUrl == "educacao-financeira")
        return new RedirectResult("/educacao-financeira", true);

      // Transmita Calor
      if (categoryUrl == "transmita-calor")
        return new RedirectResult("/transmita-calor", true);

      // Curitiba Criativa
      if (categoryUrl == "curitiba-criativa")
        return new RedirectResult("/curitiba-criativa", true);

      // Festival Gastronômico
      if (categoryUrl == "festival-gastronomico")
        return new RedirectResult("/festival-gastronomico", true);

      var isCategory = Service.Models.Categoria.IsCategoryByUrl(categoryUrl);

      var categoryPaiUrlRedirect = Service.Models.UrlRedirect.GetByUrl(categoryPaiUrl) ?? categoryPaiUrl;

      var categoryUrlRedirect = Service.Models.UrlRedirect.GetByUrl(categoryUrl) ?? categoryUrl;

      if (isCategory)
      {
        if (string.IsNullOrEmpty(categoryPaiUrl))
        {
          if (!string.IsNullOrEmpty(categoryUrlRedirect) && !categoryUrlRedirect.Equals(categoryUrl))
            return new RedirectResult($"/{editoryUrl}/{categoryUrlRedirect}", true);
        }
        else
        {
          if ((!string.IsNullOrEmpty(categoryPaiUrlRedirect) && !categoryPaiUrlRedirect.Equals(categoryPaiUrl)) ||
              (!string.IsNullOrEmpty(categoryUrlRedirect) && !categoryUrlRedirect.Equals(categoryUrl)))
            return new RedirectResult($"/{editoryUrl}/{categoryPaiUrlRedirect}/{categoryUrlRedirect}", true);
        }
      }

      //Obtem a categoria
      var objCategoria = GetCategoryByEditoryUrlAndCategoryUrl(editoryUrl, categoryPaiUrlRedirect, categoryUrlRedirect);

      //Verefica se existe uma noticia relacionada (Ref. Alteração da regra do html)
      if (!string.IsNullOrEmpty(editoryUrl) && !string.IsNullOrEmpty(categoryPaiUrlRedirect) &&
          !string.IsNullOrEmpty(categoryUrlRedirect))
      {
        if (ExistNews(editoryUrl, categoryPaiUrlRedirect, categoryUrlRedirect))
        {
          return new RedirectResult($"/{editoryUrl}/{categoryPaiUrlRedirect}/{categoryUrlRedirect}.html", true);
        }
      }

      //Verify if the category exist
      if (objCategoria == null)
        return new HttpStatusCodeResult(HttpStatusCode.NotFound);

      //Verify if the page exist
      if (p <= 0)
        return new HttpStatusCodeResult(HttpStatusCode.NotFound);

      int totalRegistros;

      var lstNoticiasHighlights = GetNoticiasByCategorialHighlight(objCategoria.Id, objCategoria.Url.Equals("plantao"));

      var model = new CategoriaModel
      {
        Categoria = objCategoria,
        Title = $"{objCategoria.Titulo} - Massa News {objCategoria.Editorial.Titulo}",
        Description = $"Leia tudo sobre {objCategoria.Titulo} no Massa News {objCategoria.Editorial.Titulo}.",
        Robots = "index, follow",
        Canonical = objCategoria.CategoriaPai == null ? $"{Constants.UrlWeb}/{objCategoria.Editorial.Url}/{objCategoria.Url}" : $"{Constants.UrlWeb}/{objCategoria.Editorial.Url}/{objCategoria.CategoriaPai.Url}/{objCategoria.Url}"
      };

      if (p == 1)
        model.NoticiasHighlights = lstNoticiasHighlights?.Select(Service.Models.Noticia.Map).ToList();


      model.Noticias = GetNewsByCategoryWithSub(objCategoria.Id, objCategoria.Url.Equals("plantao"), p, lstNoticiasHighlights?.Select(s => s.Id).ToList(), out totalRegistros);

      var totalPaginas = Math.Ceiling(Convert.ToDecimal(totalRegistros) / Convert.ToDecimal(Constants.TakeNoticias));

      ViewBag.TotalRegistros = totalRegistros;
      ViewBag.PaginaAtual = p;
      ViewBag.Editorial = Service.Models.Editorial.Map(objCategoria.Editorial);
      ViewBag.ActiveNav = objCategoria.Url.Equals("plantao") ? "Sua Região" : objCategoria.Titulo;

      if (Convert.ToInt32(totalPaginas) > 1)
        ViewBag.Paginacao = Pagination.AddPagination(ViewBag.PaginaAtual, Convert.ToInt32(totalPaginas), 5, true);

      //Set the list of categories
      if (objCategoria.EditorialId == 4)
      {
        var NoticiaSrv = new NoticiaService();
        ViewBag.Categorias = NoticiaSrv.GetCategoriasByEditorial(objCategoria.EditorialId);
        ViewBag.MenuCategoriaId = objCategoria.CategoriaPaiId.HasValue ? objCategoria.CategoriaPaiId : objCategoria.Id;
      }

      //Recupera as 4 ultimas notícias que são exibidas na sidebar
      ViewBag.Lastest4News = Service.Models.Noticia.GetLastestNews(4).ToList();

      //Popular News 
      ViewBag.Popular5News = Service.Models.Noticia.GetMoreAccessedNews(objCategoria.EditorialId);

      ViewBag.TbActiveClassifA = true;
      ViewBag.TbActiveClassifB = false;

      return View("Index", model);
    }

    #endregion

    #region Entity

    private bool ExistNews(string editorialUrl, string categoriaUrl, string noticiaUrl)
    {
      var origemUrl = $"/{editorialUrl}/{categoriaUrl}/{noticiaUrl}";
      var query1 = (from n in Db.Noticias where n.Categoria.Url == categoriaUrl && n.Url == noticiaUrl select n).Any();
      var query2 = (from r in Db.Redirects where r.OrigemUrl == origemUrl select r).Any();
      return query1 || query2;
    }

    private Categoria GetCategoryByEditoryUrlAndCategoryUrl(string editoryUrl, string categoryPaiUrl, string categoryUrl)
    {
      var query = from c in Db.Categorias
                  where c.Status && c.Editorial.Url.Equals(editoryUrl) && c.Url.Equals(categoryUrl)
                  select c;

      if (!string.IsNullOrEmpty(categoryPaiUrl))
        query = query.Where(c => c.CategoriaPai.Url == categoryPaiUrl);
      else
        query = query.Where(c => !c.CategoriaPaiId.HasValue);

      return query.FirstOrDefault();
    }

    private Editorial GetEditorialByUrl(string editorial)
    {
      var query = from e in Db.Editoriais
                  where e.Status && e.Url.Equals(editorial)
                  select e;

      return query.FirstOrDefault();
    }

    private List<Noticia> GetNewsByCategoryWithSub(int categoryId, bool isPlantao, int page, ICollection<int> notInList, out int total)

    {
      var objCidade = Db.Cidades.Find(GetMyLocationId);

      var lstCategorias = GetChildCategoriesIdsByCategoryId(categoryId);

      var query = from n in Db.Noticias
                  where
                    n.StatusId == Status.Publicada.Id &&
                    lstCategorias.Any(c => n.CategoriaId == c) &&
                    (!isPlantao || n.Cidade.MicroregiaoId == objCidade.MicroregiaoId) &&
                    !notInList.Contains(n.Id)
                  orderby
                    n.DataPublicacao descending
                  select
                    n;

      total = query.Count();

      return query.Skip((page - 1) * Constants.TakeNoticias).Take(Constants.TakeNoticias).ToList();
    }

    private List<int> GetChildCategoriesIdsByCategoryId(int categoryId, List<int> lstIds = null)
    {
      if (lstIds == null)
        lstIds = new List<int> { categoryId };

      var query = from c in Db.Categorias where c.CategoriaPaiId == categoryId select c.Id;

      var ids = query.ToList();

      lstIds.AddRange(ids);

      return ids.Aggregate(lstIds, (current, categoria) => GetChildCategoriesIdsByCategoryId(categoria, current));
    }

    private List<Noticia> GetNoticiasByCategorialHighlight(int categoriaId, bool isPlantao)
    {
      var take = 5;

      var lstNoticias = new List<Noticia>();

      var sectionId = 0;

      var objCidade = Service.Models.Cidade.Load(GetMyLocationId);

      var query = from n in Db.NoticiaSecondaryHighlights
                  where
                    n.SecondaryHighlight.Home.MicroregiaoId == objCidade.MicroregiaoId &&
                    n.Noticia.StatusId == Status.Publicada.Id &&
                    n.Noticia.CategoriaId == categoriaId &&
                    (n.Noticia.ImgLg != null && n.Noticia.ImgMd != null && n.Noticia.ImgSm != null) &&
                    n.SecondaryHighlight.SectionId == sectionId
                  orderby
                    n.Ordem ascending
                  select
                    n.Noticia;

      lstNoticias = query.Take(take).ToList();

      if (lstNoticias.Count == take)
        return lstNoticias;

      take = take - lstNoticias.Count;

      var notInList = lstNoticias.Select(s => s.Id).ToList();

      var queryRemain = from n in Db.Noticias
                        where
                          n.StatusId == Status.Publicada.Id &&
                          n.CategoriaId == categoriaId &&
                          (n.ImgLg != null && n.ImgMd != null && n.ImgSm != null) &&
                          !notInList.Contains(n.Id) &&
                          (!isPlantao || n.Cidade.MicroregiaoId == objCidade.MicroregiaoId)
                        orderby
                          n.DataPublicacao descending
                        select n;

      lstNoticias.AddRange(queryRemain.Take(take).ToList());

      return lstNoticias;
    }
    #endregion

    protected override void Dispose(bool disposing)
    {
      if (disposing)
        Db.Dispose();

      base.Dispose(disposing);
    }
  }
}