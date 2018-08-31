using MassaNews.Portal.Test.Controllers;
using MassaNews.Service.Util;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace MassaNews.Portal.Controllers.Tests
{
  [TestClass()]
  public class HomeControllerTests : BaseControllerTests
  {
    #region Tests
    [TestMethod()]
    public void IndexTest()
    {
      Stopwatch stopWatch = new Stopwatch();

      stopWatch.Start();

      //Create a cookie object
      var cookie = new HttpCookie(Constants.CookieName)
      {
        Value = "12",
        Expires = DateTime.MaxValue
      };

      //Create a cookie ccollection
      var cookieCollection = new HttpCookieCollection { cookie };

      //Add Cookie
      HttpRequestMock.SetupGet(x => x.Cookies).Returns(cookieCollection);

      //Get the controller 
      var objController = GetController();

      //Get the result view
      var result = objController.Index() as ViewResult;

      stopWatch.Stop();
      // Get the elapsed time as a TimeSpan value.
      TimeSpan ts = stopWatch.Elapsed;

      // Format and display the TimeSpan value.
      string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}", ts.Hours, ts.Minutes, ts.Seconds, ts.Milliseconds / 10);

      Assert.IsNotNull(result.Model);
    }

    #endregion

    #region Methods
    private HomeController GetController()
    {
      var controller = new HomeController();
      controller.ControllerContext = new ControllerContext(HttpContextBaseMock.Object, new RouteData(), controller);
      controller.Url = new UrlHelper(new RequestContext(HttpContextBaseMock.Object, new RouteData()), Routes);
      return controller;
    }
    #endregion
  }
}