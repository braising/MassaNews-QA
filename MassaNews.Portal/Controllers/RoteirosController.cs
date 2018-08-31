using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MassaNews.Service.Models;
using MassaNews.Service.Services;
using MassaNews.Portal.Functions;
using MassaNews.Portal.Models.Roteiros;
using MassaNews.Portal.ViewModels;
using MassaNews.Service.Util;
using System.Web.UI;

namespace MassaNews.Portal.Controllers
{
  public class RoteirosController : Controller
  {
    #region Services
    private LocalService LocalSrv { get; }
    #endregion

    #region Constructor
    public RoteirosController()
    {
      LocalSrv = new LocalService();
    }
    #endregion

    #region Actions
    [Route("roteiros")]
    public ActionResult Index()
    {
      var city = Cidade.Load(CookieFx.GetLocationId(Request));
      return RedirectToAction("Category", new { uf = "pr" , city = city.Url, category = "destaque" });
    }

    [Route("roteiros/{uf}/{city}/{category}")]
    public ActionResult Category(string uf, string city, string category)
    {
      return category == "destaque" ? LoadIndex("pr", city, category) : LoadCategory(uf, city, category);
    }

    [Route("roteiros/{uf}/{city}/{category}/{subCategory}")]
    public ActionResult SubCategory(string uf, string city, string category, string subCategory)
    {
      //return category == "destaque" ? Loadindex("pr", uf, category) : LoadCategory(uf, city, category);
      return null;
    }

    [Route("roteiros/{uf}/{city}/{category}/{subCategory}/{url}.html")]
    public ActionResult Establishment(string uf, string city, string category, string subCategory, string url)
    {
      return LoadEstablishment(uf, city, category, subCategory, url);
    }

    #endregion

    #region Partial Actions

    [Route("roteiros/loadnexestablishment")]
    [OutputCache(Duration = 60, VaryByParam = "*", Location = OutputCacheLocation.Server)]
    public ActionResult LoadNextEstablishment(string establishmentUrl)
    {
      //Remove .html
      establishmentUrl = establishmentUrl.Remove(establishmentUrl.Length - 5, 5);

      var url = establishmentUrl.TrimStart('/').Split('/').Last();

      var establishment = RoteirosService.GetEstablishmentByUrl(url);

      return PartialView("_EstablishmentItem", establishment);
    }
    #endregion

    #region Methods

    private ActionResult LoadIndex(string uf, string city, string categoryUrl)
    {
      var model = new RoteirosIndex
      {
        //Base
        Title = "Roteiros - Massa News",
        Description = "Roteiros. Saiba mais!",
        Robots = "index, follow",
        Canonical = $"{Constants.UrlWeb}/dev/roteiros",

        //Model
        Menu = GetMenu(uf, city, categoryUrl),
        Destaques = GetDestaques(uf, city, categoryUrl).ToList()
      };

      ViewBag.ActiveNav = "Roteiros";

      return View("Index",model);
    }

    private ActionResult LoadCategory(string uf, string city, string categoryUrl)
    {
      var category = RoteirosService.GetCategoryByUrl(categoryUrl);

      var model = new RoteirosCategory
      {
        //Base
        Title = "Roteiros - Massa News",
        Description = "Roteiros. Saiba mais!",
        Robots = "index, follow",
        Canonical = $"{Constants.UrlWeb}/dev/roteiros",

        //Model
        Menu = GetMenu(uf, city, categoryUrl),
        Destaque = new Destaque
        {
          Titulo = category.Nome,
          Link = $"/roteiros/{uf}/{city}/{category.Url}",
          Estabelecimentos = category.GetEstablishments().Select(DestatqueItem.Map)
        }
      };

      ViewBag.ActiveNav = category.Nome;

      return View("Categoria", model);
    }

    private ActionResult LoadEstablishment(string uf, string city, string category, string subCategory, string url)
    {
      var establishment = RoteirosService.GetEstablishmentByUrl(url);

      var model = new RoteirosEstablishments
      {
        //Base
        Title = "Roteiros - Massa News",
        Description = "Roteiros. Saiba mais!",
        Robots = "index, follow",
        Canonical = $"{Constants.UrlWeb}/dev/roteiros",
        //Model
        Establishment = establishment
      };

      ViewBag.NavItems = establishment.Categoria.GetEstablishments().Select(s => new NoticiaNavItem(s, s.Id == establishment.Id));
      ViewBag.ActiveNav = establishment.CategoriaPaiNome;

      return View("Establishment", model);
    }

    private IEnumerable<Destaque> GetDestaques(string uf, string city, string categoryUrl)
    {
      var lstCategorias = RoteirosService.GetAllMainCategories().ToList();

      lstCategorias.Insert(0, new CategoriaEstabelecimento
      {
        Nome = "Destaque",
        Url = "destaque",
        Icone = "fa-star",
      });

      var lstDestaques = lstCategorias.Select(c => new Destaque
      {
        Titulo = c.Nome,
        Link = $"/roteiros/{uf}/{city}/{c.Url}",
        Estabelecimentos = c.GetHighlights().Select(DestatqueItem.Map)
      });

      return lstDestaques;
    }

    private IEnumerable<Models.Roteiros.MenuItem> GetMenu(string uf, string city, string categoryUrl)
    {
      var lstCategorias = RoteirosService.GetAllMainCategories().ToList();

      lstCategorias.Insert(0, new CategoriaEstabelecimento
      {
        Nome = "Destaque",
        Url = "destaque",
        Icone = "fa-star",
      });

      return lstCategorias.Select(s => Models.Roteiros.MenuItem.Map(s, uf, city, categoryUrl));
    }

    #endregion
  }
}