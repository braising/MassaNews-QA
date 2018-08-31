using System;
using MassaNews.Service.Enum;
using MassaNews.Service.Models;
using MassaNews.Service.Util;
using System.Web;
using System.Linq;

namespace MassaNews.Portal.Models
{
  public class VideoModel
  {
    #region Properties
    public string Section { get; set; }
    public string ImgSm { get; set; }
    public string ImgMd { get; set; }
    public string ImgLg { get; set; }
    public string VideoUrl { get; set; }
    public string InternaUrl { get; set; }
    public string NoticiaUrl { get; set; }
    public string FacebookUrl { get; set; }
    public string TwitterUrl { get; set; }
    public string WattsUrl { get; set; }
    public string GPlusUrl { get; set; }
    public string Chamada { get; set; }
    public string Chapeu { get; set; }
    public DateTime Date { get; set; }

    #endregion

    #region Methods
    public static VideoModel Map(Noticia objNoticia)
    {
      #region Section
      var sectionUrl = string.Empty;

      if (objNoticia.Tags.Any(t => t.Id == 752))
        sectionUrl = "massa-news-live-on";

      if (objNoticia.Tags.Any(t => t.Id == 754))
        sectionUrl = "descobrindo-curitiba";

      if (string.IsNullOrEmpty(sectionUrl) && objNoticia.CidadeId.HasValue && objNoticia.EditorialId == EditorialEnum.Noticias.GetHashCode() && objNoticia.CategoriaId == 1)
        sectionUrl = objNoticia.Cidade.Microregion.Url;

      if(string.IsNullOrEmpty(sectionUrl))
        sectionUrl = objNoticia.EditorialUrl;

      #endregion

      var model = new VideoModel
      {
        Section = sectionUrl,
        InternaUrl = $"/videos/{objNoticia.Url}.html",
        VideoUrl = objNoticia.Video,
        ImgSm = $"{Constants.UrlDominioEstaticoUploads}/noticias/{objNoticia.ImgSm}",
        ImgMd = $"{Constants.UrlDominioEstaticoUploads}/noticias/{objNoticia.ImgMd}",
        ImgLg = $"{Constants.UrlDominioEstaticoUploads}/noticias/{objNoticia.ImgLg}",
        NoticiaUrl = objNoticia.UrlFull,
        FacebookUrl = "https://" + $"www.facebook.com/dialog/share?app_id={Constants.FacebookAppId}&display=popup&picture={Constants.UrlDominioEstaticoUploads}/noticias/{objNoticia.ImgLg}&href={Constants.UrlShare}/videos/{objNoticia.Url}.html&redirect_uri={Constants.UrlShare}/videos/{objNoticia.Url}.html&caption=massanews.com",
        TwitterUrl = "https://" + $"twitter.com/intent/tweet?text={HttpUtility.UrlEncode(objNoticia.Chamada)} {$"{Constants.UrlShare}/videos/{objNoticia.Url}.html"}",
        WattsUrl = $"whatsapp://send?text={HttpUtility.UrlEncode(objNoticia.Chamada)} {Constants.UrlShare}/videos/{objNoticia.Url}.html",
        GPlusUrl = $"https://plus.google.com/share?url={Constants.UrlShare}/videos/{objNoticia.Url}.html",
        Chamada = objNoticia.Chamada,
        Chapeu = GetHat(objNoticia),
        Date = objNoticia.DataPublicacao.Value
      };

      return model;
    }

    private static string GetHat(Noticia objNoticia)
    {
      if (objNoticia.EditorialId == (int)EditorialEnum.Blogs)
        return objNoticia.Blog.Titulo;

      if (objNoticia.EditorialId == (int)EditorialEnum.Noticias && objNoticia.CidadeId.HasValue)
        return objNoticia.Cidade.Nome;

      if (objNoticia.EditorialId == (int)EditorialEnum.Noticias && !objNoticia.CidadeId.HasValue)
        return "Paraná";

      return objNoticia.Categoria.Titulo;
    }
    #endregion
  }
}
