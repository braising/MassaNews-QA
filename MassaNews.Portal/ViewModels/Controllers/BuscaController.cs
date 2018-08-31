using Entities.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using MassaNews.Service.Response;
using MassaNews.Portal.Functions;
using MassaNews.Portal.Models;
using MassaNews.Service.Services;
using MassaNews.Service.Util;

namespace MassaNews.Portal.Controllers
{
  public class BuscaController : BaseController
  {
    #region Properties
    private EntitiesDb Db { get; }
    private LocalService LocalSrv { get; }
    private WeatherService WeatherSrv { get; }
    #endregion

    #region Constructor
    public BuscaController()
    {
      Db = new EntitiesDb();
      LocalSrv = new LocalService();
      WeatherSrv = new WeatherService();
    }
    #endregion

    #region Actions

    [Route("busca/{filtro?}/")]
    public ActionResult Index(string filtro = null, string o = "relevancia", string q = null, int p = 1)
    {
      /* verificação da página */
      if (p <= 0)
        return new HttpStatusCodeResult(HttpStatusCode.NotFound);

      if (q == null)
        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

      /* "cooka" o usuário com as palavras utilizadas */

      q = q.TrimEnd();

      SetKeywords(q);

      var resultados = new List<SearchResponse>();

      var oBusca = new SearchService();

      /* noticias */
      var lstNoticias = oBusca.Search(Constants.ElasticIndexName, q, p, Constants.TakeNoticias, 0.6);

      resultados.AddRange(lstNoticias);

      /* counts do menu */
      var model = new BuscaModel
      {
        Count = resultados.Count,
        CountNoticias = resultados.Count(),
      };

      /* aplica filtro de tipo */
      if (filtro == null)
      {
        resultados = resultados.ToList();
        ViewBag.TotalRegistros = model.Count;
      }
      else if (filtro == "noticias")
      {
        resultados = resultados.ToList();
        ViewBag.TotalRegistros = model.CountNoticias;
      }

      //Ordena por data
      model.Resultados = resultados.OrderByDescending(r=> r.Data).Skip((p - 1) * Constants.TakeNoticias).Take(Constants.TakeNoticias).ToList();

      ViewBag.PaginaAtual = p;

      var totalPaginas = Math.Ceiling(((double)ViewBag.TotalRegistros / Constants.TakeNoticias));

      if (Convert.ToInt32(totalPaginas) > 1)
        ViewBag.Paginacao = Pagination.AddPagination(ViewBag.PaginaAtual, Convert.ToInt32(totalPaginas), 5, true);

      ViewBag.ActiveNav = "Resultado da busca";
      ViewBag.SearchWords = q;
      ViewBag.Filter = filtro;
      ViewBag.Order = o;

      model.Title = string.Format("Resultado da busca por {0} - Massa News", q);
      model.Description = string.Format("Confira todos os resultados da busca por {0}. Notícias, posts, fotos, vídeos e muito mais no Massa News.", q);
      model.Robots = "index, follow";

      return View(model);
    }

    //public void getTemperature(int locationId)
    //{
    //  var objCidade = LocalSrv.GetCidadeByIdCached(locationId);

    //  var obj = WeatherSrv.GetWeather(objCidade.Id);

    //  ViewBag.City = obj == null ? string.Empty : obj.City ?? string.Empty;
    //  ViewBag.Description = obj == null ? string.Empty : obj.Description ?? string.Empty;
    //  ViewBag.Icon = obj == null ? string.Empty : obj.Icon ?? string.Empty;
    //  ViewBag.Temperature = obj == null ? string.Empty : obj.Temperature == 0 ? string.Empty : obj.Temperature.ToString();
    //}

    protected override void Dispose(bool disposing)
    {
      if (disposing)
        Db.Dispose();

      base.Dispose(disposing);
    }

    #endregion
  }
}
