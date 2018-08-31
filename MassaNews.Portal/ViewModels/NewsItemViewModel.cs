using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using HtmlAgilityPack;
using MassaNews.Service.Models;
using MassaNews.Service.Services;
using MassaNews.Portal.Functions;
using Entities.Classes;

namespace MassaNews.Portal.ViewModels
{
  public class NewsItemViewModel
  {
    #region Services
    private NoticiaService NoticiaSrv { get; }
    #endregion

    #region Constructors
    public NewsItemViewModel(Noticia objNews, ControllerContext context)
    {
      NoticiaSrv = new NoticiaService();

      objNews.Conteudo = Replace(objNews, context);

      News = objNews;
      Tags = objNews.Tags.Where(t => t.Status).OrderBy(t => t.Titulo).ToList();

      var microregiaoId = News.CidadeId.HasValue ? NoticiaSrv.GetMicroregiaoIdByCidade(News.CidadeId.Value) : (int?)null;

      if (objNews.DestaqueId == Destaque.Video.Id) 
        NewsVideosRelacionados = NoticiaSrv.GetNewsVideosRelacionados(objNews.Id ,objNews.CategoriaId, objNews.EditorialId, microregiaoId).ToList();

      //Get Autors
      Autors = objNews.GetAutors();

      //Set the tag news
      Tags.ForEach(t => t.Noticias = Noticia.GetKnowMoreNewsByTagCached(t.Id).Where(n=> n.Id != objNews.Id).Take(3));
    }

    public NewsItemViewModel() : base()
    {
    }
    #endregion

    #region Properties
    public Noticia News { get; set; }
    public string Autors { get; set; }
    public List<Tag> Tags { get; set; }
    public List<Noticia> NewsVideosRelacionados { get; set; }
    #endregion

    #region Methods
    private string Replace(Noticia objNews, ControllerContext context)
    {
      var html = new HtmlDocument();

      html.LoadHtml(objNews.Conteudo);

      #region Subtitles
      var galleries = html.DocumentNode.Descendants("span").Where(p => p.GetAttributeValue("class", string.Empty) == "inner-gallery").ToList();

      //objNews.Galerias = NoticiaSrv.GetGalleriesByTheNewsId(objNews.Id);

      if (galleries.Any())
      {
        if (objNews.Galeria != null && objNews.Galeria.Imagens.Any()) { 

          var galleryHtml = Util.RenderViewToString(context, "_GaleriaInterna", objNews, true);

          var galleryNode = HtmlNode.CreateNode(galleryHtml);

          foreach (var gallery in galleries)
            gallery.ParentNode.ReplaceChild(galleryNode, gallery);
        }
        else
        {
          foreach (var gallery in galleries)
            gallery.ParentNode.ReplaceChild(null, gallery);
        }
      }

      #endregion

      #region Scripts
      var scripts = html.DocumentNode.Descendants("span").Where(p => p.GetAttributeValue("class", string.Empty) == "inner-script").ToList();

      foreach (var script in scripts)
      {
        int stripId;

        if (!int.TryParse(script.GetAttributeValue("data-script", string.Empty), out stripId)) continue;

        var objScript = NoticiaSrv.GetScriptById(stripId);

        var scriptNode = HtmlNode.CreateNode($"<span>{objScript.ContentScript}</span>");

        script.ParentNode.ReplaceChild(scriptNode, script);
      }

      #endregion

      return html.DocumentNode.OuterHtml;
    }
    #endregion
  }
}