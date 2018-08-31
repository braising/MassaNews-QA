using MassaNews.Service.Services;
using System.Net;
using System.Web.Mvc;
using System.Web.UI;
using MassaNews.Service.Models;

namespace MassaNews.Portal.Controllers
{
    public class AmpController : Controller
    {
        #region Actions

        [Route("amp/{editorialUrl}/{categoriaUrl}/{noticiaUrl}.html")]
        [OutputCache(Duration = 60, VaryByParam = "*", Location = OutputCacheLocation.Server)]
        public ActionResult Noticias(string editorialUrl, string categoriaUrl, string noticiaUrl)
        {
            return LoadNewsResult(editorialUrl, categoriaUrl, null, noticiaUrl);
        }

        [Route("amp/blogs/{categoriaUrl}/{blogUrl}/{postUrl}.html")]
        [OutputCache(Duration = 60, VaryByParam = "*", Location = OutputCacheLocation.Server)]
        public ActionResult Posts(string categoriaUrl, string blogUrl, string postUrl)
        {
            return LoadNewsResult("blogs", categoriaUrl, blogUrl, postUrl);
        }

        [Route("amp/{editorialUrl}/{categoriaUrl}/{noticiaUrl:regex(^(?!.*[.]html$).*$)}")]
        public ActionResult PostNewRedirect(string editorialUrl, string categoriaUrl, string noticiaUrl)
        {
            return new RedirectResult($"/amp/{editorialUrl}/{categoriaUrl}/{noticiaUrl}.html", true);
        }

        [Route("amp/blogs/{categoriaUrl}/{blogUrl}/{postUrl:regex(^(?!.*[.]html$).*$)}")]
        public ActionResult PostBlogRedirect(string categoriaUrl, string blogUrl, string postUrl)
        {
            return new RedirectResult($"/amp/blogs/{categoriaUrl}/{blogUrl}/{postUrl}.html", true);
        }

        #endregion

        #region Methods

        private ActionResult LoadNewsResult(string editorialUrl, string categoryUrl, string blogurl, string newsUrl)
        {
            var requestUrl = Request.RawUrl;

            //Get Amp
            var model = AmpService.GetAmpModel(newsUrl);

            if (model == null)
            {
                var redirectUrl = UrlRedirect.GetByUrl(requestUrl.Substring(4, requestUrl.Length - 5));

                if (!string.IsNullOrEmpty(redirectUrl))
                    return new RedirectResult($"/amp/{redirectUrl}.html", true);
            }

            //Redirect if url is wrong
            if (model != null && requestUrl != $"{model.AmpUrlFull}.html")
                return new RedirectResult($"{model.AmpUrlFull}.html", true);

            //Verefica se a model é nul
            if (model == null)
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);

            //ViewBag
            ViewBag.ThemeValue = model.ThemeValue;

            return View("Noticias", model);
        }
        #endregion
    }
}