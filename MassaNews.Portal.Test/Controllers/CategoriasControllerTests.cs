using MassaNews.Portal.Test.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Web.Mvc;
using System.Web.Routing;

namespace MassaNews.Portal.Controllers.Tests
{
  [TestClass()]
  public class CategoriasControllerTests : BaseControllerTests
  {
    [TestMethod()]
    public void TagTest()
    {
      //Get the controller 
      var objController = GetController();

      //Get the result view
      var result = objController.Tag("curitiba") as ViewResult;

      Assert.IsNotNull(result.Model);
    }

    #region Methods
    private CategoriasController GetController()
    {
      var controller = new CategoriasController();
      controller.ControllerContext = new ControllerContext(HttpContextBaseMock.Object, new RouteData(), controller);
      controller.Url = new UrlHelper(new RequestContext(HttpContextBaseMock.Object, new RouteData()), Routes);
      return controller;
    }
    #endregion

  }
}