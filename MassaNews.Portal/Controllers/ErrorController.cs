using System.Web.Mvc;
using MassaNews.Portal.Models;

namespace MassaNews.Portal.Controllers
{
  public class ErrorController : Controller
  {
    #region Actions

    public ActionResult NotFound()
    {
      var model = new BaseModel();

      /* base model defaults */
      model.Title = "Página não encontrada - Massa News";
      model.Description = "";
      model.Robots = "index";
      //model.Canonical = "";

      Response.StatusCode = 404;
      return View(model);
    }

    public ActionResult BadRequest()
    {
      var model = new BaseModel();

      /* base model defaults */
      model.Title = "Página não encontrada - Massa News";
      model.Description = "";
      model.Robots = "index";
      //model.Canonical = "";

      Response.StatusCode = 400;
      return View("NotFound", model);
    }

    public ActionResult InternalServerError()
    {
      var model = new BaseModel();

      /* base model defaults */
      model.Title = "Página não encontrada - Massa News";
      model.Description = "";
      model.Robots = "index";
      //model.Canonical = "";

      Response.StatusCode = 500;
      return View("NotFound", model);
    }

    #endregion
  }
}