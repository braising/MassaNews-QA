using System.Web.Mvc;
using System.Web.Routing;

namespace MassaNews.Portal
{
  public class RouteConfig
  {
    public static void RegisterRoutes(RouteCollection routes)
    {
      routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

      routes.MapMvcAttributeRoutes();

      routes.MapRoute(
       name: "Videos",
       url: "videos/{url}.html",
       defaults: new { controller = "Video", action = "Interna", id = UrlParameter.Optional },
       namespaces: new[] { "MassaNews.Portal.Controllers" }//,
       //constraints: new { controller = new DefaultConstraint() }
       );

      routes.MapRoute(
       name: "Noticia",
       url: "{editorial}/{categoria}/{url}.html",
       defaults: new { controller = "Noticias", action = "Index", id = UrlParameter.Optional },
       namespaces: new[] { "MassaNews.Portal.Controllers" }//,
       //constraints: new { controller = new DefaultConstraint() }
       );

      routes.MapRoute(
        name: "Default",
        url: "{controller}/{action}/{id}",
        defaults: new {controller = "Home", action = "Index", id = UrlParameter.Optional},
        namespaces: new[] {"MassaNews.Portal.Controllers"}//,
        //constraints: new {controller = new DefaultConstraint()}
        );
    }
  }
}
