using MassaNews.Service.Services;
using MassaNews.Service.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using System.Xml;
using System.Xml.Linq;

namespace MassaNews.Portal.Controllers
{
  public class RssController : Controller
    {
        #region Services

        private NoticiaService NoticiaSrv { get; }
        private LocalService LocalSrv { get; }
        private SiteMapService SiteMapSrv { get; }

        #endregion

        #region Constructor
        public RssController()
        {
            NoticiaSrv = new NoticiaService();
            LocalSrv = new LocalService();
            SiteMapSrv = new SiteMapService();
        }
        #endregion

        #region Actions
        [Route("rss/massanews.xml")]
        public ActionResult GetMainRss()
        {
            var lstNoticias = Service.Models.Noticia.GetLastestNews(5, true);

            var lastDate = lstNoticias.First().DataPublicacao;

            if (lastDate == null) return null;

            var feed = new SyndicationFeed
            {
                Title = new TextSyndicationContent("Massa News - Todas as Noticias"),
                Language = "pt-BR",
                LastUpdatedTime = new DateTimeOffset(lastDate.Value),
                Copyright = new TextSyndicationContent("© Todos os direitos reservados.")
            };

            feed.Links.Add(SyndicationLink.CreateAlternateLink(new Uri($"{Constants.UrlWeb}/{"noticias"}")));

            feed.ImageUrl = new Uri($"{Constants.UrlCdn}/static/images/avatar/avatar-massanews-144x144.jpg");

            var feedItems = new List<SyndicationItem>();

            foreach (var noticia in lstNoticias)
            {
                var permalink = new Uri($"{Constants.UrlWeb}/{noticia.EditorialUrl}/{noticia.CategoriaUrl}/{noticia.Url}.html");

                var feedItem = new SyndicationItem
                {
                    Title = new TextSyndicationContent(noticia.Titulo),
                    Content = new TextSyndicationContent(StripHtml(noticia.Conteudo) + "... Leia mais no <a href='" + permalink + "'>Massa News</a>!"),
                    PublishDate = noticia.DataPublicacao ?? DateTime.Now,
                };

                //Adiciona Imagem ao item
                if (!string.IsNullOrEmpty(noticia.ImgLg))
                    feedItem.ElementExtensions.Add(new XElement("image",
                      $"{Constants.UrlDominioEstaticoUploads}/noticias/{noticia.ImgLg}"));

                feedItem.Links.Add(SyndicationLink.CreateAlternateLink(permalink));

                feedItems.Add(feedItem);
            }

            feed.Items = feedItems;

            SyndicationFeedFormatter rssFormatter = new Rss20FeedFormatter(feed);

            MemoryStream output = new MemoryStream();

            XmlWriterSettings ws = new XmlWriterSettings
            {
                Indent = true,
                Encoding = new UTF8Encoding(false)
            };

            XmlWriter w = XmlWriter.Create(output, ws);
            rssFormatter.WriteTo(w);
            w.Flush();

            return Content(Encoding.UTF8.GetString(output.ToArray()), "text/xml");
        }

        [Route("rss/{microRegionUrl}.xml")]
        public ActionResult GetRssByRegion(string microRegionUrl)
        {
            var objMicroRegion = LocalSrv.GetMicroRegiaoByUrlCached(microRegionUrl);

            var lstNoticias = Service.Models.Noticia.GetLastestNews(5, null, null, objMicroRegion.Id, null, true, true).ToList();
            var lastDate = lstNoticias.First().DataPublicacao;

            if (lastDate == null) return null;

            var feed = new SyndicationFeed
            {
                Title = new TextSyndicationContent($"Massa News - {objMicroRegion.Nome}"),
                Language = "pt-BR",
                LastUpdatedTime = new DateTimeOffset(lastDate.Value),
                Copyright = new TextSyndicationContent("© Todos os direitos reservados.")
            };

            feed.Links.Add(SyndicationLink.CreateAlternateLink(new Uri($"{Constants.UrlWeb}/{"noticias/plantao"}")));

            feed.ImageUrl = new Uri($"{Constants.UrlCdn}/static/images/avatar/avatar-massanews-144x144.jpg");

            var feedItems = new List<SyndicationItem>();

            foreach (var noticia in lstNoticias)
            {
                var permalink = new Uri($"{Constants.UrlWeb}/{noticia.EditorialUrl}/{noticia.CategoriaUrl}/{noticia.Url}.html");

                var feedItem = new SyndicationItem
                {
                    Title = new TextSyndicationContent(noticia.Titulo),
                    Content = new TextSyndicationContent(StripHtml(noticia.Conteudo) + "... Leia mais no <a href='" + permalink + "'>Massa News</a>!"),
                    PublishDate = noticia.DataPublicacao ?? DateTime.Now,
                };

                //Adiciona Imagem ao item
                if (!string.IsNullOrEmpty(noticia.ImgLg))
                    feedItem.ElementExtensions.Add(new XElement("image",
                      $"{Constants.UrlDominioEstaticoUploads}/noticias/{noticia.ImgLg}"));
                
                feedItem.Links.Add(SyndicationLink.CreateAlternateLink(permalink));

                feedItems.Add(feedItem);
            }

            feed.Items = feedItems;

            SyndicationFeedFormatter rssFormatter = new Rss20FeedFormatter(feed);

            MemoryStream output = new MemoryStream();

            XmlWriterSettings ws = new XmlWriterSettings
            {
                Indent = true,
                Encoding = new UTF8Encoding(false)
            };

            XmlWriter w = XmlWriter.Create(output, ws);
            rssFormatter.WriteTo(w);
            w.Flush();

            return Content(Encoding.UTF8.GetString(output.ToArray()), "text/xml");
        }

        [Route("rss/massanews-suggestion.xml")]
        public ActionResult GetRssEditorSuggestion()
        {
            StringBuilder str = new StringBuilder();
            string xmlFinal = string.Empty;

            var lstNoticias = SiteMapSrv.GetSiteMapSuggest().ToList();

            var lastDate = lstNoticias.First().DataPublicacao;

            if (lastDate == null) return null;

            var feed = new SyndicationFeed("Massa News - Todas as Noticias", "MassaNews - A notícia em movimento!", new Uri($"{Constants.UrlWeb}/{"noticias/plantao"}"));

            XNamespace dc = "http://purl.org/dc/elements/1.1/";
            feed.AttributeExtensions.Add(
                  new XmlQualifiedName("dc", XNamespace.Xmlns.ToString()),
                  "http://purl.org/dc/elements/1.1/");

            feed.ElementExtensions.Add(new XElement("image",
                  new XElement("url", null, $"{Constants.UrlCdn}/static/images/logos/massa-news-250x40.png"),
                  new XElement("title", null, "MassaNews - A notícia em movimento!"),
                  new XElement("link", null, $"{Constants.UrlWeb}/{"noticias/plantao"}")).CreateReader());

            var feedItems = new List<SyndicationItem>();
            //Cria os itens
            foreach (var noticia in lstNoticias)
            {
                var feedItem = new SyndicationItem
                {
                    Title = new TextSyndicationContent(noticia.Titulo),
                };

                foreach (var item in noticia.Creators)
                {
                    if (item == noticia.Creators.Last())
                        str.Append(item.Nome);
                    else
                        str.Append(item.Nome).Append(", ");
                }

                feedItem.ElementExtensions.Add(new XElement("link",
                  $"{Constants.UrlWeb}/{Url.Encode(noticia.EditorialUrl)}/{Url.Encode(noticia.CategoriaUrl)}/{Url.Encode(noticia.Url)}.html"));

                feedItem.ElementExtensions.Add(new XElement("description",
                  $"{(noticia.Description.Length > 220 ? Text.RemoveHtmlTags(noticia.Description.Substring(0, 220) + "... Leia mais no Massa News!") : Text.RemoveHtmlTags(noticia.Description) + " Leia no Massa News!")}"));

                feedItem.ElementExtensions.Add(new XElement(dc + "creator", str.ToString()));

                feedItems.Add(feedItem);
                str.Clear();
            }

            feed.Items = feedItems;
            MemoryStream output = new MemoryStream();
            //transforma para RSS
            SyndicationFeedFormatter rssFormatter = new Rss20FeedFormatter(feed);

            var xmlDoc = new XmlDocument();

            using (var xw = xmlDoc.CreateNavigator().AppendChild())
            {
                rssFormatter.WriteTo(xw);
            }
            //Adiciona a referência do namespace no cabeçalho do RSS
            xmlDoc.DocumentElement.SetAttribute("xmlns:dc", "http://purl.org/dc/elements/1.1/");

            using (XmlWriter writer = XmlWriter.Create(output, new XmlWriterSettings { Indent = true }))
            {
                xmlDoc.WriteTo(writer);
            }

            xmlFinal = Encoding.UTF8.GetString(output.ToArray());

            var toReplace = $"channel xmlns:dc=\"http://purl.org/dc/elements/1.1/\"";

            xmlFinal = Regex.Replace(xmlFinal, toReplace, "channel");

            return Content(xmlFinal.ToString(), "text/xml");
        }

        #endregion

        #region Private Methods
        private static string StripHtml(string input)
        {
            var texto = Text.RemoveHtmlTags(input);
            texto = Text.HtmlDecode(texto);

            return texto.Length > 160 ? texto.Substring(0, 160) : texto;
        }
        #endregion
    }
}