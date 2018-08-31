using Entities.Contexts;
using System;
using System.Net;
using System.Web.Mvc;
using MassaNews.Portal.Models;
using MassaNews.Portal.ViewModels;
using MassaNews.Service.Enum;
using MassaNews.Service.Services;
using MassaNews.Service.Models;
using System.Linq;
using System.Collections.Generic;
using MassaNews.Service.Util;
using Newtonsoft.Json;

namespace MassaNews.Portal.Controllers
{
  public class CotacoesController : BaseController
  {

    [Route("cotacoes")]
    //[OutputCache(Duration = 60, VaryByCustom = "Location", VaryByParam = "*", Location = OutputCacheLocation.ServerAndClient)]
    public ActionResult Cotacoes()
    {
      return new RedirectResult($"cotacoes/parana", true);
    }

    [Route("cotacoes/{tipo}")]
    //[OutputCache(Duration = 60, VaryByCustom = "Location", VaryByParam = "*", Location = OutputCacheLocation.ServerAndClient)]
    public ActionResult Cotacoes(string tipo)
    {
      //var objCotacaoTipo = GetCotacaoTipoBySlug(tipo);

      var model = new CotacoesViewModel();

      /* base model defaults */
      model.Title = "Cotações em " + tipo + " - Negócios da Terra";
      model.Description = "Confira as cotações " + tipo + " - Negócios da Terra";
      model.Robots = "index, follow";
      model.Canonical = $"{Constants.UrlWeb}/cotacoes/" + tipo +"";

      //Set viewbag's
      ViewBag.Pagina = "cotacoes-cidade";
      ViewBag.ActiveNav = "Negócios da Terra";
      ViewBag.Editorial = Editorial.Load(EditorialEnum.NegociosDaTerra.GetHashCode());
      ViewBag.ExibirLogo = true;
      ViewBag.LinkActiveNav = "/negocios-da-terra";

      //return the model to the view
      return View(model);
    }

    [Route("cotacoes/{tipo}/{cidade}")]
    //[OutputCache(Duration = 60, VaryByCustom = "Location", VaryByParam = "*", Location = OutputCacheLocation.ServerAndClient)]
    public ActionResult CotacoesCidade(string tipo, string cidade)
    {
      var model = new HomeIndex();

      /* base model defaults */
      model.Title = "Cotações em "+ cidade +" - Negócios da Terra";
      model.Description = "Confira as cotações em " + cidade + " - Negócios da Terra";
      model.Robots = "index, follow";
      model.Canonical = $"{Constants.UrlWeb}/" + tipo + "/" + cidade + "";

      //Set viewbag's
      ViewBag.Pagina = "cotacoes-" + tipo;
      ViewBag.ActiveNav = "Negócios da Terra";
      ViewBag.Editorial = Editorial.Load(EditorialEnum.NegociosDaTerra.GetHashCode());
      ViewBag.ExibirLogo = true;
      ViewBag.LinkActiveNav = "/negocios-da-terra";

      //return the model to the view
      return View(model);
    }

    [Route("cotacoes/{tipo}/{cidade}/{produto}")]
    //[OutputCache(Duration = 60, VaryByCustom = "Location", VaryByParam = "*", Location = OutputCacheLocation.ServerAndClient)]
    public ActionResult CotacoesProduto(string tipo, string cidade, string produto)
    {
      var model = new HomeIndex();

      /* base model defaults */
      model.Title = "Cotações de Arroz - Negócios da Terra";
      model.Description = "Confira as cotações de Arroz no Paraná e Santa Catarina - Negócios da Terra";
      model.Robots = "index, follow";
      model.Canonical = $"{Constants.UrlWeb}/cotacoes/arroz";

      //Set viewbag's
      ViewBag.Pagina = "cotacoes-produto";
      ViewBag.ActiveNav = "Negócios da Terra";
      ViewBag.Editorial = Editorial.Load(EditorialEnum.NegociosDaTerra.GetHashCode());
      ViewBag.ExibirLogo = true;
      ViewBag.LinkActiveNav = "/negocios-da-terra";

      //return the model to the view
      return View(model);
    }


    [Route("cotacoes/cepea-usp")]
    //[OutputCache(Duration = 60, VaryByCustom = "Location", VaryByParam = "*", Location = OutputCacheLocation.ServerAndClient)]
    public ActionResult CotacoesCepeaUSP()
    {
      var model = new HomeIndex();

      /* base model defaults */
      model.Title = "Cotações CEPEA USP - Negócios da Terra";
      model.Description = "Confira as cotações CEPEA USP - Negócios da Terra";
      model.Robots = "index, follow";
      model.Canonical = $"{Constants.UrlWeb}/cotacoes/cepea-usp";

      //Set viewbag's
      ViewBag.Pagina = "cotacoes-cepea-usp";
      ViewBag.ActiveNav = "Negócios da Terra";
      ViewBag.Editorial = Editorial.Load(EditorialEnum.NegociosDaTerra.GetHashCode());
      ViewBag.ExibirLogo = true;
      ViewBag.LinkActiveNav = "/negocios-da-terra";

      //return the model to the view
      return View(model);
    }


    [Route("cotacoes/cia-ufpr")]
    //[OutputCache(Duration = 60, VaryByCustom = "Location", VaryByParam = "*", Location = OutputCacheLocation.ServerAndClient)]
    public ActionResult CotacoesCiaUFPR()
    {
      var model = new HomeIndex();

      /* base model defaults */
      model.Title = "CIA UFPR - Negócios da Terra";
      model.Description = "Confira as cotações CIA UFPR - Negócios da Terra";
      model.Robots = "index, follow";
      model.Canonical = $"{Constants.UrlWeb}/cotacoes/cia-ufpr";

      //Set viewbag's
      ViewBag.Pagina = "cotacoes-cia-ufpr";
      ViewBag.ActiveNav = "Negócios da Terra";
      ViewBag.Editorial = Editorial.Load(EditorialEnum.NegociosDaTerra.GetHashCode());
      ViewBag.ExibirLogo = true;
      ViewBag.LinkActiveNav = "/negocios-da-terra";

      //return the model to the view
      return View(model);
    }


    [Route("cotacoes/lapsui")]
    //[OutputCache(Duration = 60, VaryByCustom = "Location", VaryByParam = "*", Location = OutputCacheLocation.ServerAndClient)]
    public ActionResult CotacoesLapsui()
    {
      var model = new HomeIndex();

      /* base model defaults */
      model.Title = "Lapsui - Negócios da Terra";
      model.Description = "Confira as cotações Lapsui - Negócios da Terra";
      model.Robots = "index, follow";
      model.Canonical = $"{Constants.UrlWeb}/cotacoes/lapsui";

      //Set viewbag's
      ViewBag.Pagina = "cotacoes-lapsui";
      ViewBag.ActiveNav = "Negócios da Terra";
      ViewBag.Editorial = Editorial.Load(EditorialEnum.NegociosDaTerra.GetHashCode());
      ViewBag.ExibirLogo = true;
      ViewBag.LinkActiveNav = "/negocios-da-terra";

      //return the model to the view
      return View(model);
    }

    //private CotacaoTipo GetCotacaoTipoBySlug(string slug)
    //{
    //  return Db.CotacaoTipo.FirstOrDefault(e => e.Status && e.Slug == slug);
    //}

  }
}