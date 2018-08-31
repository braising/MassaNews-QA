using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Web.Mvc;
using System.Xml.Linq;
using Entities.Classes;
using Entities.Enum;
using MassaNews.Portal.Functions;
using System.Linq;
using MassaNews.Service.Services;
using System.IO;
using System.Text.RegularExpressions;
using MassaNews.Service.Enum;
using MassaNews.Service.Models;
using MassaNews.Service.Util;

namespace MassaNews.Portal.Controllers
{
    public class SiteMapController : Controller
    {
        #region Services

        private SiteMapService SiteMapSrv { get; }
        #endregion

        #region Constructor

        public SiteMapController()
        {
            SiteMapSrv = new SiteMapService();
        }

        #endregion

        #region Actions

        [Route("sitemap.xml")]
        public ActionResult Index()
        {
            var lstSitemapNode = new List<SitemapNode>
      {
        new SitemapNode {Url = $"{Constants.UrlWeb}/sitemap/categorias.xml"},
        new SitemapNode {Url = $"{Constants.UrlWeb}/sitemap/tags.xml"},
        new SitemapNode {Url = $"{Constants.UrlWeb}/sitemap/blogs.xml"},
        new SitemapNode {Url = $"{Constants.UrlWeb}/sitemap/institucionais.xml"},
        new SitemapNode {Url = $"{Constants.UrlWeb}/sitemap/previsao-do-tempo.xml"}
      };

            //Adiciona mapeamento dos editoriais
            var editoriais = SiteMapSrv.GetEditorialUrlList();

            foreach (var editorial in editoriais)
            {
                var total = SiteMapSrv.CountNoticiasByEditorialUrl(editorial);
                var nPage = Math.Ceiling(((double)total / Constants.SiteMapPageItens));
                for (var i = 0; i < nPage; i++)
                    lstSitemapNode.Add(new SitemapNode { Url = $"{Constants.UrlWeb}/sitemap/posts/{editorial}/pagina-{i}.xml" });
            }

            //Converte objeto em XML
            var xml = GetSitemapDocument(lstSitemapNode, true);

            //Retorna o XML
            return Content(xml, "text/xml", Encoding.UTF8);
        }

        [Route("sitemap/previsao-do-tempo.xml")]
        public ActionResult GetBySubcategory(string categoria)
        {
            var lstCities = Cidade.GetAll();

            var lstSitemapNode = new List<SitemapNode>();

            var objPrevisaoTipo = PrevisaoTempo.Load(1);

            foreach (var cidade in lstCities)
            {
                lstSitemapNode.Add(new SitemapNode
                {
                    Url = $"{Constants.UrlWeb}/previsao-do-tempo/{cidade.Url}",
                    Frequency = SitemapFrequency.Weekly,
                    Priority = 0.5
                });
            }

            //Converte objeto em XML
            var xml = GetSitemapDocument(lstSitemapNode);

            //Retorna o XML
            return Content(xml, "text/xml", Encoding.UTF8);
        }

        [Route("sitemap/{categoria}.xml")]
        public ActionResult GetByCategory(string categoria)
        {
            var lstSitemapNode = new List<SitemapNode>();

            switch (categoria)
            {
                case "institucionais":
                    {
                        lstSitemapNode.Add(new SitemapNode
                        {
                            Url = $"{Constants.UrlWeb}/{"institucional"}/{"sobre"}",
                            Frequency = SitemapFrequency.Weekly,
                            Priority = 0.5
                        });

                        lstSitemapNode.Add(new SitemapNode
                        {
                            Url = $"{Constants.UrlWeb}/{"institucional"}/{"envie-sua-noticia"}",
                            Frequency = SitemapFrequency.Weekly,
                            Priority = 0.5
                        });

                        lstSitemapNode.Add(new SitemapNode
                        {
                            Url = $"{Constants.UrlWeb}/{"institucional"}/{"fale-conosco"}",
                            Frequency = SitemapFrequency.Weekly,
                            Priority = 0.5
                        });

                        lstSitemapNode.Add(new SitemapNode
                        {
                            Url = $"{Constants.UrlWeb}/{"institucional"}/{"cookies"}",
                            Frequency = SitemapFrequency.Weekly,
                            Priority = 0.5
                        });

                        break;
                    }
                case "categorias":
                    {

                        lstSitemapNode.Add(new SitemapNode
                        {
                            Url = $"{Constants.UrlWeb}/{"blogs"}",
                            Frequency = SitemapFrequency.Weekly,
                            Priority = 0.5
                        });

                        //Editoriais
                        var editoriais = SiteMapSrv.GetEditorialUrlList().ToList();

                        editoriais.ForEach(e => lstSitemapNode.Add(new SitemapNode
                        {
                            Url = $"{Constants.UrlWeb}/{Url.Encode(e)}",
                            Frequency = SitemapFrequency.Daily,
                            Priority = 0.5
                        }));

                        //Fotos
                        lstSitemapNode.Add(new SitemapNode
                        {
                            Url = $"{Constants.UrlWeb}/{Url.Encode("fotos")}",
                            Frequency = SitemapFrequency.Daily,
                            Priority = 0.5
                        });

                        //Videos
                        lstSitemapNode.Add(new SitemapNode
                        {
                            Url = $"{Constants.UrlWeb}/{Url.Encode("videos")}",
                            Frequency = SitemapFrequency.Daily,
                            Priority = 0.5
                        });

                        //Categorias
                        var categorias = SiteMapSrv.GetCategoriasUrlList().ToList();

                        categorias.ForEach(c => lstSitemapNode.Add(new SitemapNode
                        {
                            Url = $"{Constants.UrlWeb}/{Url.Encode(c.EditorialUrl)}/{c.CategoriaUrl}",
                            Frequency = SitemapFrequency.Daily,
                            Priority = 0.5
                        }));

                        //Categorias de Blog
                        var categoriablogs = Categoria.GetAllByEditorial(EditorialEnum.Blogs.GetHashCode(), true).ToList();

                        categoriablogs.ForEach(cb => lstSitemapNode.Add(new SitemapNode
                        {
                            Url = $"{Constants.UrlWeb}/{"blogs"}/{cb.Url}",
                            Frequency = SitemapFrequency.Daily,
                            Priority = 0.5
                        }));

                        break;
                    }
                case "tags":
                    {
                        var tags = SiteMapSrv.GetTagsUrlList().ToList();

                        tags.ForEach(t => lstSitemapNode.Add(new SitemapNode
                        {
                            Url = $"{Constants.UrlWeb}/{"tags"}/{Url.Encode(t)}",
                            Frequency = SitemapFrequency.Daily,
                            Priority = 0.5
                        }));
                        break;
                    }
                case "blogs":
                    {
                        var blogs = SiteMapSrv.GetBlogsUrlList().ToList();

                        blogs.ForEach(b => lstSitemapNode.Add(new SitemapNode
                        {
                            Url = $"{Constants.UrlWeb}/{"blogs"}/{Url.Encode(b.CategoriaUrl)}/{Url.Encode(b.BlogUrl)}/{"posts"}",
                            Frequency = SitemapFrequency.Weekly,
                            Priority = 0.5
                        }));
                        break;
                    }
            }

            //Converte objeto em XML
            var xml = GetSitemapDocument(lstSitemapNode);

            //Retorna o XML
            return Content(xml, "text/xml", Encoding.UTF8);
        }

        [Route("sitemap/posts/{editorial}/pagina-{page}.xml")]
        public ActionResult GetNoticias(string editorial, int page)
        {
            var lstSitemapNode = new List<SitemapNode>();

            string xml;

            if (editorial == "blogs")
            {
                var posts = SiteMapSrv.GetBlogPostResultList(page, Constants.SiteMapPageItens).ToList();

                //Posts
                posts.ForEach(n => lstSitemapNode.Add(new SitemapNode
                {
                    Url = $"{Constants.UrlWeb}/blogs/{n.CategoriaUrl}/{n.BlogUrl}/{n.Posturl}.html",
                    Frequency = SitemapFrequency.Weekly,
                    Priority = 0.8
                }));

                //Converte objeto em XML
                xml = GetSitemapDocument(lstSitemapNode);
            }
            else
            {
                var noticias = SiteMapSrv.GetNoticiasByEditorialList(editorial, page, Constants.SiteMapPageItens).ToList();

                //Editoriais
                noticias.ForEach(n => lstSitemapNode.Add(new SitemapNode
                {
                    Url = $"{Constants.UrlWeb}/{Url.Encode(n.EditorialUrl)}/{Url.Encode(n.CategoriaUrl)}/{Url.Encode(n.NoticiaUrl)}.html",
                    Frequency = SitemapFrequency.Weekly,
                    Priority = 0.8
                }));

                //Converte objeto em XML
                xml = GetSitemapDocument(lstSitemapNode);

            }

            //Retorna o XML
            return Content(xml, "text/xml", Encoding.UTF8);

        }

        [Route("sitemap-news.xml")]
        public ActionResult GetSiteMap()
        {
            var lstSitemapNode = new List<SitemapNode>();

            string xml = string.Empty;

            StringBuilder str = new StringBuilder();

            var lstNoticias = SiteMapSrv.GetSiteMapByCategoriaList().ToList();

            var imageBaseUrlPath = $"{Constants.UrlDominioEstaticoUploads}/noticias";

            foreach (var item in lstNoticias)
            {
                item.Loc = $"{Constants.UrlWeb}/{Url.Encode(item.EditorialUrl)}/{Url.Encode(item.CategoriaUrl)}/{Url.Encode(item.NoticiaUrl)}.html";
                item.Publication_Date = item.DataPublicacao.ToString("yyyy-MM-ddTHH:mm:sszzz");
                item.ImagemUrl = string.IsNullOrEmpty(item.ImagemUrl) == false ? $"{imageBaseUrlPath}/{item.ImagemUrl}" : string.Empty;
                item.ImagemLegenda = string.IsNullOrEmpty(item.ImagemLegenda) == false ? $"<![CDATA[({item.ImagemLegenda})]]>" : string.Empty;

                str.Append(item.Categoria);
                if (!string.IsNullOrEmpty(item.SubCategoria))
                    str.Append(", ").Append(item.SubCategoria);
                if (!string.IsNullOrEmpty(item.Editorial))
                    str.Append(", ").Append(item.Editorial);
                if (!string.IsNullOrEmpty(item.Cidade))
                    str.Append(", ").Append(item.Cidade);
                item.Keywords = str.ToString();
                str.Clear();
            }

            XNamespace ns = "http://www.google.com/schemas/sitemap-news/0.9";
            var sitemap = new XDocument(
              new XDeclaration("1.0", "utf-8", "yes"),
              new XElement(ns + "urlset",
                new XAttribute("xmlns", "http://www.sitemaps.org/schemas/sitemap/0.9"),
                new XAttribute(XNamespace.Xmlns + "news", "http://www.google.com/schemas/sitemap-news/0.9"),
                from item in lstNoticias
                select new XElement("url",
                    new XElement("loc", item.Loc),
                    new XElement(ns + "news",
                      new XElement(ns + "publication",
                        new XElement(ns + "name", item.Name),
                        new XElement(ns + "language", item.Language)
                        ),
                      new XElement(ns + "publication_date", item.Publication_Date),
                      new XElement(ns + "title", item.Title),
                      new XElement(ns + "keywords", item.Keywords)
                    )
                  )
            ));

            using (var sw = new MemoryStream())
            {
                using (var strw = new StreamWriter(sw, Encoding.UTF8))
                {
                    sitemap.Save(strw);
                    xml = Encoding.UTF8.GetString(sw.ToArray());
                    var finalXml = Regex.Replace(xml, $"{"news"}:{"urlset"}", "urlset");
                    xml = Regex.Replace(finalXml, "url xmlns=\"\"", "url");
                }
            }

            return Content(xml.ToString(), "text/xml");
        }

        [Route("robots.txt")]
        public ContentResult RobotsText()
        {
            var robots = "User-agent: *\nDisallow: /";

            if (Constants.Environment == "prod")
                robots = System.IO.File.ReadAllText(Server.MapPath("~/robots-prod.txt"));

            return Content(robots, "text/plain", Encoding.UTF8);
        }
        #endregion

        #region Private Methos

        private string GetSitemapDocument(IEnumerable<SitemapNode> sitemapNodes, bool isIndex = false)
        {

            XNamespace xmlns = "http://www.sitemaps.org/schemas/sitemap/0.9";

            var root = new XElement(xmlns + (isIndex ? "sitemapindex" : "urlset"));

            foreach (var sitemapNode in sitemapNodes)
            {
                var urlElement = new XElement(xmlns + (isIndex ? "sitemap" : "url"));

                urlElement.Add(new XElement(xmlns + "loc", Uri.EscapeUriString(sitemapNode.Url)));

                if (sitemapNode.LastModified != null)
                    urlElement.Add(new XElement(xmlns + "lastmod",
                      sitemapNode.LastModified.Value.ToLocalTime().ToString("yyyy-MM-ddTHH:mm:sszzz")));

                if (sitemapNode.Frequency != null)
                    urlElement.Add(new XElement(xmlns + "changefreq", sitemapNode.Frequency.Value.ToString().ToLowerInvariant()));

                if (sitemapNode.Priority != null)
                    urlElement.Add(new XElement(xmlns + "priority",
                      sitemapNode.Priority.Value.ToString("F1", CultureInfo.InvariantCulture)));

                root.Add(urlElement);
            }

            var document = new XDocument(root);

            return document.ToString();
        }

        #endregion
    }
}