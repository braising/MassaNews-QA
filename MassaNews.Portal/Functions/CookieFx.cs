using MassaNews.Service.Util;
using System;
using System.Web;

namespace MassaNews.Portal.Functions
{
  public static class CookieFx
  {
    public static int GetLocationId(HttpRequestBase request)
    {
      if (request.Cookies == null)
        return 12;

      var cookie = request.Cookies[Constants.CookieName];

      if (cookie == null)
        return 12;

      return Convert.ToInt32(cookie.Value);
    }
  }
}