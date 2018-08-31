using Entities.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Entities.Tables;
using MassaNews.Service.Enum;
using MassaNews.Service.Services;
using MassaNews.Portal.Functions;
using MassaNews.Portal.ViewModels;
using MassaNews.Service.Util;
using System.Configuration;
using MongoDB.Bson;
using MassaNews.Portal.App_Start;
using MongoDB.Driver;

namespace MassaNews.Portal.Controllers
{
  public class EditorialController : BaseController
  {
    private MongoDBContext dBContext;

    #region Services
    private NoticiaService NoticiaSrv { get; }
    #endregion

    #region Constructor

    public EditorialController()
    {
      NoticiaSrv = new NoticiaService();
      dBContext = new MongoDBContext();
    }

    #endregion

    #region Actions

    [Route("noticias")]
    public ActionResult Noticias(int p = 1)
    {
      var objEditorial = GetEditorialByUrl("noticias");

      //todos
      String categories = ConfigurationManager.AppSettings["CategoriaNoticias"];
      var listCategoriaId = categories.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

      return CustomActionEditorial(objEditorial, p, listCategoriaId);
    }

    [Route("esportes")]
    public ActionResult Esportes(int p = 1)
    {
      var CampeonatoSrv = new CampeonatoService();
      ViewBag.ShowResumo = true;
      //ViewBag.TbParanaense = CampeonatoSrv.GetHtmlResumoClassificacao(CampeonatoEnum.Paranaense);
      //ViewBag.TbClassificacaoA = CampeonatoSrv.GetHtmlResumoClassificacao(CampeonatoEnum.BrasileiraoA);
      //ViewBag.TbClassificacaoB = CampeonatoSrv.GetHtmlResumoClassificacao(CampeonatoEnum.BrasileiraoB);
      //ViewBag.TbActiveClassifA = true;
      //ViewBag.TbActiveClassifB = false;
      var objEditorial = GetEditorialByUrl("esportes");

      String categories = ConfigurationManager.AppSettings["CategoriaEsportes"];
      var listCategoriaId = categories.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

      return CustomActionEditorial(objEditorial, p, listCategoriaId);
    }

    [Route("entretenimento")]
    public ActionResult Entretenimento(int p = 1)
    {
      var objEditorial = GetEditorialByUrl("entretenimento");

      String categories = ConfigurationManager.AppSettings["CategoriaEntretenimento"];
      var listCategoriaId = categories.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

      return CustomActionEditorial(objEditorial, p ,listCategoriaId);
    }

    [Route("where-curitiba")]
    public ActionResult WhereCuritiba(int p = 1)
    {
      var objEditorial = GetEditorialByUrl("where-curitiba");

      String categories = ConfigurationManager.AppSettings["CategoriaWhereCuritiba"];
      var listCategoriaId = categories.Split(new char[] {','}, StringSplitOptions.RemoveEmptyEntries);

      return CustomActionEditorial(objEditorial, p, listCategoriaId);
    }

    [Route("agronegocio")]
    public ActionResult Agronegocio(int p = 1)
    {
      return new RedirectResult($"/negocios-da-terra", true);
    }

    [Route("negocios-da-terra")]
    public ActionResult NegociosDaTerra(int p = 1)
    {
      var objEditorial = GetEditorialByUrl("negocios-da-terra");

      String categories = ConfigurationManager.AppSettings["CategoriaNegociosDaTerra"];
      var listCategoriaId = categories.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

      return CustomActionEditorial(objEditorial, p, listCategoriaId);
    }

    [Route("viajar-e-massa")]
    public ActionResult ViajarEMassa(int p = 1)
    {
      var objEditorial = GetEditorialByUrl("viajar-e-massa");

      String categories = ConfigurationManager.AppSettings["CategoriaViajarEMassa"];
      var listCategoriaId = categories.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

      return CustomActionEditorial(objEditorial, p, listCategoriaId);
    }

    [Route("fotos")]
    public ActionResult Foto(int p = 1)
    {
      /* verificação da página */
      if (p <= 0)
      {
        return new HttpStatusCodeResult(HttpStatusCode.NotFound);
      }

      var model = new CategoriaModel();
      ViewBag.ActiveNav = "Fotos";

      /* Paginação */
      model.Noticias =
        Db.Noticias.Where(
          n =>
            n.StatusId == Status.Publicada.Id && n.DestaqueId == Destaque.Galeria.Id &&
            n.Galerias.Any(ga => ga.Imagens.Any()) && !string.IsNullOrEmpty(n.ImgSm))
          .OrderByDescending(n => n.DataPublicacao)
          .Skip((p - 1) * Constants.TakeNoticias)
          .Take(Constants.TakeNoticias)
          .ToList();

      ViewBag.PaginaAtual = p;
      ViewBag.TotalRegistros =
        Db.Noticias.Count(
          n =>
            n.StatusId == Status.Publicada.Id && n.DestaqueId == Destaque.Galeria.Id &&
            n.Galerias.Any(ga => ga.Imagens.Any()) && !string.IsNullOrEmpty(n.ImgSm));

      var totalPaginas = Math.Ceiling(((double)ViewBag.TotalRegistros / Constants.TakeNoticias));
      if (Convert.ToInt32(totalPaginas) > 1)
        ViewBag.Paginacao = Pagination.AddPagination(ViewBag.PaginaAtual, Convert.ToInt32(totalPaginas), 5, true);

      //Recupera as 4 ultimas notícias que são exibidas na sidebar
      ViewBag.Lastest4News = Service.Models.Noticia.GetLastestNews(4).ToList();

      //Recuper as 5 ultimas notícias mais acessadas 
      ViewBag.Popular5News = Service.Models.Noticia.GetMoreAccessedNews(null);

      /* base model defaults */
      model.Title = $"{"Fotos"} - Massa News";
      model.Description = $"Veja todas as {"Fotos"} no Massa News.";
      model.Robots = "index, follow";
      model.Canonical = $"{Constants.UrlWeb}/fotos";

      return View("Foto", model);
    }

    #endregion

    #region Custom Actions

    //public void getTemperature(int locationId)
    //{
    //  var objCidade = LocalSrv.GetCidadeByIdCached(locationId);

    //  var obj = WeatherSrv.GetWeather(objCidade.Id);

    //  ViewBag.City = obj == null ? string.Empty : obj.City ?? string.Empty;
    //  ViewBag.Description = obj == null ? string.Empty : obj.Description ?? string.Empty;
    //  ViewBag.Icon = obj == null ? string.Empty : obj.Icon ?? string.Empty;
    //  ViewBag.Temperature = obj == null ? string.Empty : obj.Temperature == 0 ? string.Empty : obj.Temperature.ToString();
    //}

    public ActionResult CustomActionEditorial(Editorial objEditorial, int page, string[] listCategoriaId)
    {
      if (objEditorial == null)
        return new HttpStatusCodeResult(HttpStatusCode.NotFound);

      //Verificação da página
      if (page <= 0)
        return new HttpStatusCodeResult(HttpStatusCode.NotFound);

      //int totalRegistros;

      var lstNoticiasHighlights = page == 1 ? GetNoticiasByEditorialHighlight(objEditorial.Id) : null;

      //var lstNoticias = GetNoticiasByEditorial(objEditorial.Id, page, out totalRegistros, lstNoticiasHighlights?.Select(s => s.Id).ToList());

      List<NoticiaByCategoria> lstNoticiasByCategoria = new List<NoticiaByCategoria>();

      foreach (var item in listCategoriaId)
      {
        var categoria = GetCategoriaById(Convert.ToInt32(item));
        var lstNoticiasComImg = GetNoticiasByEditorialComImg(objEditorial.Id, page, lstNoticiasHighlights?.Select(s => s.Id).ToList(), Convert.ToInt32(item));
        var lstNoticiasAux = GetNoticiasByEditorial(objEditorial.Id, page, lstNoticiasHighlights?.Select(s => s.Id).ToList(), Convert.ToInt32(item));

        var noticiaporcategoria = new NoticiaByCategoria();
        noticiaporcategoria.UrlEditorial = objEditorial.Url;
        noticiaporcategoria.Categoria = categoria;
        //noticiaporcategoria.NoticiasHighlights = lstNoticiasAux.Where(n => n.ImgLg != null).Take(2).ToList();
        noticiaporcategoria.NoticiasHighlights = lstNoticiasComImg;
        noticiaporcategoria.Noticias = lstNoticiasAux.Where(n => !noticiaporcategoria.NoticiasHighlights.Any(n2 => n2.Id == n.Id)).Take(3).ToList();

        lstNoticiasByCategoria.Add(noticiaporcategoria);
      }

      List<Post> lstPostsNegociosDaTerra = new List<Post>();
      if (objEditorial.Id == 6)
        lstPostsNegociosDaTerra = GetLastPostNegociosDaTerra();

      var model = new CategoriaModel
      {
        //Base
        Title = $"{objEditorial.Titulo} - Massa News",
        Description = $"Leia tudo sobre {objEditorial.Titulo} no Massa News.",
        Robots = "index, follow",
        Canonical = $"{Constants.UrlWeb}/{objEditorial.Url}",
        ImgOpenGraph = $"{Constants.UrlWeb}/content/images/avatar/editorial/avatar-{objEditorial.Url}.jpg",
        //Model
        Editorial = objEditorial,
        NoticiasHighlights = lstNoticiasHighlights?.Select(Service.Models.Noticia.Map).ToList(),
        //Noticias = lstNoticias,
        NoticiaByCategoria = lstNoticiasByCategoria,
        Pots = lstPostsNegociosDaTerra
      };

      ViewBag.ActiveNav = objEditorial.Titulo;
      ViewBag.PaginaAtual = page;
      //ViewBag.TotalRegistros = totalRegistros;

      //var totalPaginas = Math.Ceiling(((double)totalRegistros / Constants.TakeNoticias));

      //if (totalPaginas > 1)
      //    ViewBag.Paginacao = Pagination.AddPagination(ViewBag.PaginaAtual, Convert.ToInt32(totalPaginas), 5, true);

      //Recupera as 4 ultimas notícias que são exibidas na sidebar
      ViewBag.Lastest4News = Service.Models.Noticia.GetLastestNews(4).ToList();

      //Recuper as 5 ultimas notícias mais acessadas 
      ViewBag.Popular5News = Service.Models.Noticia.GetMoreAccessedNews(objEditorial.Id);

      ViewBag.Editorial = Service.Models.Editorial.Map(objEditorial);

      ViewBag.ExibirLogo = true;

      if (objEditorial.Id == 4)
        ViewBag.Categorias = NoticiaSrv.GetCategoriasByEditorial(objEditorial.Id);

      ViewBag.TbActiveClassifA = true;
      ViewBag.TbActiveClassifB = false;

      return View("Editorial", model);
    }

    #endregion

    #region Entity

    private Editorial GetEditorialByUrl(string editorialUrl)
    {
      return Db.Editoriais.FirstOrDefault(e => e.Status && e.Url == editorialUrl);
    }

    private Categoria GetCategoriaById(int categoriaId)
    {
      return Db.Categorias.FirstOrDefault(e => e.Id == categoriaId);
    }

    private List<Noticia> GetNoticiasByEditorial(int editorialId, int page, out int totalRegistros,
      ICollection<int> notInList)
    {
      if (notInList == null)
        notInList = new List<int>();

      var query = from n in Db.Noticias
                  where
                    n.StatusId == Status.Publicada.Id &&
                    n.Categoria.Status &&
                    n.Categoria.Editorial.Status &&
                    n.Categoria.EditorialId == editorialId &&
                    !notInList.Contains(n.Id)
                  orderby
                    n.DataPublicacao descending
                  select n;

      totalRegistros = query.Count();

      return query.Skip((page - 1) * Constants.TakeNoticias).Take(Constants.TakeNoticias).ToList();
    }

    private List<Noticia> GetNoticiasByEditorial(int editorialId, int page, ICollection<int> notInList, int categoriaId)
    {
      if (notInList == null)
        notInList = new List<int>();

      var take = 5;

      var query = from n in Db.Noticias
                  where
                    n.StatusId == Status.Publicada.Id &&
                    n.Categoria.Status &&
                    n.Categoria.Editorial.Status &&
                    n.Categoria.EditorialId == editorialId &&
                    !notInList.Contains(n.Id) && 
                    (n.CategoriaId == categoriaId ||
                    n.Categoria.CategoriaPaiId == categoriaId)

                  orderby
                    n.DataPublicacao descending
                  select n;

      //totalRegistros = query.Count();

      return query.Skip((page - 1) * take).Take(take).ToList();
    }

    private List<Noticia> GetNoticiasByEditorialComImg(int editorialId, int page, ICollection<int> notInList, int categoriaId)
    {
      if (notInList == null)
        notInList = new List<int>();

      var take = 2;

      var query = from n in Db.Noticias
                  where
                    n.StatusId == Status.Publicada.Id &&
                    n.Categoria.Status &&
                    n.Categoria.Editorial.Status &&
                    n.Categoria.EditorialId == editorialId &&
                    !notInList.Contains(n.Id) &&
                    (n.CategoriaId == categoriaId ||
                    n.Categoria.CategoriaPaiId == categoriaId) &&
                    n.ImgLg != null
                  orderby
                    n.DataPublicacao descending
                  select n;

      return query.Skip((page - 1) * take).Take(take).ToList();
    }

    private List<Noticia> GetNoticiasByEditorialHighlight(int editorialId)
    {
      var take = 5;

      var lstNoticias = new List<Noticia>();

      var sectionId = 0;

      switch (editorialId)
      {
        case 2: // Esportes
          sectionId = Section.Esportes.Id;
          break;
        case 3: // Entretenimento
          sectionId = Section.Entretenimento.Id;
          break;
        case 4: // Where Curitiba
          sectionId = Section.WhereCuritiba.Id;
          break;
        case 6: // Negócios da Terra
          sectionId = Section.NegociosDaTerra.Id;
          break;
        case 7: // Viajar é Massa
          sectionId = Section.ViajarEMassa.Id;
          break;
      }

      if (editorialId != 1)
      {
        var objCidade = Service.Models.Cidade.Load(GetMyLocationId);

        var query = from n in Db.NoticiaSecondaryHighlights
                    where
                      n.SecondaryHighlight.Home.MicroregiaoId == objCidade.MicroregiaoId &&
                      n.Noticia.StatusId == Status.Publicada.Id &&
                      n.Noticia.Categoria.EditorialId == editorialId &&
                      (n.Noticia.ImgLg != null && n.Noticia.ImgMd != null && n.Noticia.ImgSm != null) &&
                      n.SecondaryHighlight.SectionId == sectionId
                    orderby
                      n.Ordem ascending
                    select
                      n.Noticia;

        lstNoticias = query.Take(take).ToList();

        if (lstNoticias.Count == take)
          return lstNoticias;
      }

      take = take - lstNoticias.Count;

      var notInList = lstNoticias.Select(s => s.Id).ToList();

      var queryRemain = from n in Db.Noticias
                        where
                          n.StatusId == Status.Publicada.Id &&
                          n.Categoria.EditorialId == editorialId &&
                          (n.ImgLg != null && n.ImgMd != null && n.ImgSm != null) &&
                          !notInList.Contains(n.Id)
                        orderby
                          n.DataPublicacao descending
                        select n;

      lstNoticias.AddRange(queryRemain.Take(take).ToList());

      return lstNoticias;
    }

    private List<Post> GetLastPostNegociosDaTerra()
    {
      var result = new List<Post>();

      var collection = dBContext.database.GetCollection<Models.Roteiros.Post>(typeof(Models.Roteiros.Post).Name);

      var filter =
      Builders<Models.Roteiros.Post>.Filter.Eq(p => p.IsDeleted, false) &
      Builders<Models.Roteiros.Post>.Filter.Eq(p => p.Status, true) &
      Builders<Models.Roteiros.Post>.Filter.Eq(p => p.ShowId, "59a0332565d03f63ec371679");

      var sort = Builders<Models.Roteiros.Post>.Sort.Descending(p => p.Published);

      var postAux = collection.Find(filter).Sort(sort).Limit(4).ToList();

      var RedeMassaUrlWeb = "https://redemassa.com.br";
      var RedeMassaCDNUrl = "https://s3.amazonaws.com/cdn.massadigital.com";
      foreach (var item in postAux)
      {
        var post = new Post
        {
          Call = item.Call,
          PostUrl = $"{RedeMassaUrlWeb}/negocios-da-terra-2/{item.Published.Year}/{item.Published.ToString("MM")}/{item.Published.ToString("dd")}/{item.SlugHash}/v/",
          Hat = item.Published.ToString("dd/MM/yyyy"),
          ImageUrl = item.Images.Select(i => $"{RedeMassaCDNUrl}/{i.ImageUrl}").ToList(),
        };

        result.Add(post);
      }
      return result;
    }

    #endregion
  }
}