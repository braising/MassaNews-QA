using System;
using System.Web;
using MassaNews.Service.Models;
using MassaNews.Service.Util;

namespace MassaNews.Portal.Models
{
  public class MenuItemModel
  {
    #region Properties
    public int Id { get; set; }
    public DateTime DataPublicacao { get; set; }
    public string InternaUrl { get; set; }
    public string Chamada{ get; set; }
    public string ImagemUrl { get; set; }
    public string VideoUrl { get; set; }
    public string Chapeu { get; set; }
    public bool Selected{ get; set; }
    public string NoticiaUrl { get; set; }
    public string FacebookUrl { get; set; }
    public string TwitterUrl { get; set; }
    public string WattsUrl { get; set; }
    public string GPlusUrl { get; set; }

    #endregion

    #region Methods
    public static MenuItemModel Map(Noticia objNoticia)
    {
      return new MenuItemModel
      {
        Id = objNoticia.Id,
        DataPublicacao = objNoticia.DataPublicacao.Value,
        InternaUrl = $"/videos/{objNoticia.Url}.html",
        Chamada = objNoticia.Chamada,
        ImagemUrl = $"{Constants.UrlDominioEstaticoUploads}/noticias/{objNoticia.ImgThumb}",
        VideoUrl = objNoticia.Video,
        Chapeu = objNoticia.DataPublicacao.Value.ToString("dd/MM/yyyy"),
        FacebookUrl = "https://" + $"www.facebook.com/dialog/share?app_id={Constants.FacebookAppId}&display=popup&picture={Constants.UrlDominioEstaticoUploads}/noticias/{objNoticia.ImgLg}&href={Constants.UrlShare}/videos/{objNoticia.Url}.html&redirect_uri={Constants.UrlShare}/videos{objNoticia.Url}.html&caption=massanews.com",
        TwitterUrl = "https://" + $"twitter.com/intent/tweet?text={HttpUtility.UrlEncode(objNoticia.Chamada)} {$"{Constants.UrlShare}/videos/{objNoticia.Url}.html"}",
        WattsUrl = $"whatsapp://send?text={HttpUtility.UrlEncode(objNoticia.Chamada)} {Constants.UrlShare}/videos/{objNoticia.Url}.html",
        GPlusUrl = $"https://plus.google.com/share?url={Constants.UrlShare}/videos/{objNoticia.Url}.html",
        NoticiaUrl = objNoticia.UrlFull,
      };
    }
    #endregion
  }
}
