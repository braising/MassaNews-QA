using MassaNews.Portal.Functions;
using MassaNews.Portal.Models.Newsletter;
using MassaNews.Portal.ViewModels;
using MassaNews.Service.Models;
using MassaNews.Service.Services;
using MassaNews.Service.Util;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace MassaNews.Portal.Controllers
{
  public class NewsletterController : Controller
  {
    #region Actions

    [HttpGet]
    [Route("newsletter/atualizar/{hash}")]
    public ActionResult Update(string hash)
    {
      var id = ToolService.GetIdBHash(hash);

      if (!id.HasValue)
        return new HttpStatusCodeResult(HttpStatusCode.NotFound);

      var objNewsletter = Newsletter.Load(id.Value);

      if(objNewsletter == null)
        return new HttpStatusCodeResult(HttpStatusCode.NotFound);

      var goback = false;

      if (!objNewsletter.Ativo)
      {
        goback = true;
        objNewsletter.Ativo = true;
        objNewsletter.MotivoId = null;
        objNewsletter.Save();
      }

      var model = new NewsletterUpdateViewModel
      {
        //Base
        Title = "Atualizar preferências de e-mail - Massa News",
        Description = "Atualize suas preferências e receba e-mails personalizados para você.",
        Robots = "noindex, nofollow",
        Canonical = $"{Constants.UrlWeb}/newsletter/atualizar",
        //Model
        UserHash = hash,
        GoBack = goback,
        PreferenceGroups = PreferenceGroup.GetAll(),
        UserNewsletter = objNewsletter
      };

      ViewBag.ActiveNav = "Preferências de e-mail";
      ViewBag.CityLetter = new SelectList(Cidade.GetAllToDictionary(), "Key", "Value", objNewsletter.CidadeId);

      return View(model);
    }

    [HttpGet]
    [Route("newsletter/descadastrar/{hash}")]
    public ActionResult Unsubscribe(string hash)
    {
      var id = ToolService.GetIdBHash(hash);

      if (!id.HasValue)
        return new HttpStatusCodeResult(HttpStatusCode.NotFound);

      var objNewsletter = Newsletter.Load(id.Value);

      if (objNewsletter == null)
        return new HttpStatusCodeResult(HttpStatusCode.NotFound);

      var model = new NewsletterUnsubscribeViewModel
      {
        //Base
        Title = "Descadastrar e-mail - Massa News",
        Description = "Estamos tristes, mas esperamos que volte algum dia.",
        Robots = "noindex, nofollow",
        Canonical = $"{Constants.UrlWeb}/newsletter/descadastrar",
        //Model
        UserHash = hash,
        UserNewsletter = objNewsletter
      };

      ViewBag.ActiveNav = "Descadastrar";

      return View(model);
    }

    [HttpPost]
    [Route("newsletter/subscribe")]
    public ActionResult Subscribe(NewsletterSubscribe model)
    {
      //Get the user city
      var cityId = CookieFx.GetLocationId(Request);

      var objNewsletter = new Newsletter
      {
        Nome = model.Nome,
        Email = model.Email,
        CidadeId = cityId
      };

      objNewsletter.Subscribe();

      return Json("ok");
    }

    [HttpPost]
    [Route("newsletter/update")]
    public ActionResult Update(NewsletterUpdate model)
    {
        var id = ToolService.GetIdBHash(model.Hash);

        var objNewsletter = Newsletter.Load(id.Value);

        objNewsletter.Nome = model.Name;
        objNewsletter.Celular = model.CellPhone;
        objNewsletter.CidadeId = model.City;
        objNewsletter.PeriodoId = model.Period;

        if(model.Preferences != null)
          objNewsletter.SelectedPreferences = model.Preferences.ToList();

        objNewsletter.Save();

        return Json("ok");
    }

    [HttpPost]
    [Route("newsletter/descadastrar")]
    public ActionResult Unsubscribe(NewsletterUnsubscribe model)
    {
      var id = ToolService.GetIdBHash(model.Hash);

      var objNewsletter = Newsletter.Load(id.Value);

      objNewsletter.Ativo = false;
      objNewsletter.MotivoId = model.Reason;

      objNewsletter.Save();

      return Json("ok");
    }
    #endregion
  }
}