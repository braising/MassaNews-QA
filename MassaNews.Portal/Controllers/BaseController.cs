using Entities.Classes;
using Entities.Contexts;
using MassaNews.Service.Services;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MassaNews.Service.Models;
using MassaNews.Portal.Functions;
using MassaNews.Service.Util;

namespace MassaNews.Portal.Controllers
{
  public class BaseController : Controller
  {

    #region Private Properties
    public EntitiesDb Db { get; }
    private LocalService LocalSrv { get; }
    #endregion

    #region Public Properts
    public int GetMyLocationId => CookieFx.GetLocationId(Request);
    #endregion

    #region Constructor
    public BaseController()
    {
      Db = new EntitiesDb();
      LocalSrv = new LocalService();
    }
    #endregion

    #region Actions

    [Route("changecidade")]
    [HttpPost]
    public JsonResult ChangeCidade(int id)
    //public JsonResult ChangeCidade(int oldId, int newId)
    {
      //var oldcity = Cidade.Load(oldId);
      //var newcity = Cidade.Load(newId) ?? Cidade.Load(12);

      //SetLocation(newcity.Id);
      //return Json($"{oldcity.MicroregiaoId},{newcity.MicroregiaoId},{oldcity.Microregion.Url},{newcity.Microregion.Url}");

      var c = Db.Cidades.Find(id);

      if (c != null)
        SetLocation(c.Id);
      else
        SetLocation(12);

      return Json("ok");
    }

    [HttpGet]
    [Route("getcityidbyname")]
    public JsonResult GetCityIdByName(string cityName)
    {
      var result = 12;

      var objCidade = Cidade.GetCidadeByName(cityName);

      if (objCidade != null)
        result = objCidade.Id;

      return Json(result, JsonRequestBehavior.AllowGet);
    }

    #endregion

    #region Methods

    #region SetLocation

    protected HttpCookie SetLocation(int cidadeId)
    {
      var cookie = Request.Cookies[Constants.CookieName];

      if (cookie == null)
        cookie = new HttpCookie(Constants.CookieName);

      cookie.Value = cidadeId.ToString();
      cookie.Expires = DateTime.MaxValue;
      ViewBag.Cidade = cidadeId;
      Response.Cookies.Add(cookie);

      return cookie;
    }

    #endregion

    #region GetKeywords

    protected string GetKeywords()
    {
      var cookie = Request.Cookies[Constants.CookieKeywords];

      return cookie == null ? null : cookie.Value;
    }

    #endregion

    #region SetKeywords

    protected HttpCookie SetKeywords(string keywords)
    {
      var cookie = Request.Cookies[Constants.CookieKeywords];
      if (cookie == null)
        cookie = new HttpCookie(Constants.CookieKeywords);

      cookie.Value = cookie.Value != null ? string.Format("{0} {1}", keywords, cookie.Value) : keywords;
      cookie.Expires = DateTime.MaxValue;
      ViewBag.Keywords = keywords;
      Response.Cookies.Add(cookie);

      return cookie;
    }

    #endregion

    protected override void Dispose(bool disposing)
    {
      if (disposing)
        Db.Dispose();

      base.Dispose(disposing);
    }

    #endregion
  }
}