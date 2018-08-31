using Entities.Contexts;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Data.Entity;
using System.Web;
using System.Web.Routing;

namespace MassaNews.Portal.Test.Controllers
{

  [TestClass()]
  public class BaseControllerTests
  {
    #region Properties
    protected RouteCollection Routes;
    protected Mock<HttpContextBase> HttpContextBaseMock;
    protected Mock<HttpRequestBase> HttpRequestMock;
    protected Mock<HttpResponseBase> HttpResponseMock;
    #endregion

    [TestInitialize]
    public void TestInitialize()
    {
      Database.SetInitializer<EntitiesDb>(null);

      Routes = new RouteCollection();

      HttpResponseMock = new Mock<HttpResponseBase>();
      HttpContextBaseMock = new Mock<HttpContextBase>();
      HttpRequestMock = new Mock<HttpRequestBase>();

      HttpContextBaseMock.SetupGet(x => x.Request).Returns(HttpRequestMock.Object);
      HttpContextBaseMock.SetupGet(x => x.Response).Returns(HttpResponseMock.Object);
    }
  }
}
