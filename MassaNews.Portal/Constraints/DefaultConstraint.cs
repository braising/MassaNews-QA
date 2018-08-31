using System.Web;
using System.Web.Routing;

namespace MassaNews.Portal.Constraints
{
  public class DefaultConstraint : IRouteConstraint
  {
    public bool Match(HttpContextBase httpContext, Route route, string parameterName, RouteValueDictionary values, RouteDirection routeDirection)
    {
      return true;
    }
  }
}
