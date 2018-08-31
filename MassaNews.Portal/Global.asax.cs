using Entities.Contexts;
using MassaNews.Portal.Functions;
using MassaNews.Service.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace MassaNews.Portal
{
  public class MvcApplication : HttpApplication
  {
    protected void Application_Start()
    {
      //Areas
      AreaRegistration.RegisterAllAreas();

      //Filters
      FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);

      //Rotes
      RouteConfig.RegisterRoutes(RouteTable.Routes);

      //Bundles
      BundleConfig.RegisterBundles(BundleTable.Bundles);

      //Db
      Database.SetInitializer<EntitiesDb>(null);
    }
    protected void Application_Error(object sender, EventArgs e)
    {

      //Get the context
      var context = HttpContext.Current;


      //Get the error
      var ex = Server.GetLastError();

      //Create a list of vars for new realic
      var vars = new Dictionary<string, string>();

      if (context != null && context.Request != null)
      {
        vars.Add("URL", context.Request.Url.ToString());
        vars.Add("Origin", context.Request.UrlReferrer?.ToString() ?? "NA");
      }

      //Send error to new realic
      NewRelic.Api.Agent.NewRelic.NoticeError(new Exception("Custon Error Trace", ex), vars);

      if (ex.GetType() == typeof(HttpException))
      {
        var httpEx = (HttpException)ex;

        if (httpEx.GetHttpCode() == 404)
        {
          //Notify request
          Util.RequestNotify(new HttpRequestWrapper(context.Request));
        }
      }

    }
    public override string GetVaryByCustomString(HttpContext context, string custom)
    {
      var key = new StringBuilder(string.Empty);

      var parameters = custom.Split(',');

      foreach (var parameter in parameters)
        key.Append(GetKeyByCustonParmeters(context, parameter));

      return string.IsNullOrEmpty(key.ToString()) ? base.GetVaryByCustomString(context, custom) : key.ToString();
    }
    private string GetKeyByCustonParmeters(HttpContext context, string custom)
    {
      var request = new HttpRequestWrapper(context.Request);

      switch (custom)
      {
        case "Microregion":
          {
            var locationId = CookieFx.GetLocationId(request);
            var city = Cidade.Load(locationId);
            return $"MicroregiaoId:{city.MicroregiaoId}";
          }
        case "Location":
          {
            var locationId = CookieFx.GetLocationId(request);
            return $"Location:{locationId}";
          }
        case "Origin":
          {
            var origem = context.Request.UrlReferrer?.AbsolutePath.TrimEnd('/') ?? string.Empty;

            if (origem.StartsWith("/tag"))
              return "Origin:Tag";

            if (origem.StartsWith("/fotos"))
              return "Origin:Fotos";

            if (origem.StartsWith("/videos"))
              return "Origin:Videos";

            return "Origin:Default";
          }
        default:
          {
            return base.GetVaryByCustomString(context, custom);
          }
      }
    }

  }
}