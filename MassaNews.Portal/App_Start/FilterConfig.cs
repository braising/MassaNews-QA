using System.Web.Mvc;
using MassaNews.Portal.Filters;

namespace MassaNews.Portal
{
  public class FilterConfig
  {
    public static void RegisterGlobalFilters(GlobalFilterCollection filters)
    {
      filters.Add(new HandleErrorAttribute());
      filters.Add(new AppFilter());
    }
  }
}
