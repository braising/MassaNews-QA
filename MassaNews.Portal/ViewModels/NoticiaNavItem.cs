using System;
using System.Linq;
using MassaNews.Service.Models;
using Entities.Classes;
using MassaNews.Service.Util;

namespace MassaNews.Portal.ViewModels
{
  public class NoticiaNavItem
  {
    #region Constructor
    public NoticiaNavItem(Noticia objNoticia, bool firstTitle)
    {

      FirstTitle = firstTitle;
      HasThumb = !string.IsNullOrEmpty(objNoticia.ImgThumb);
      IsVideo = objNoticia.DestaqueId == Destaque.Video.Id && !string.IsNullOrEmpty(objNoticia.Video);
      IsGaleria = objNoticia.DestaqueId == Destaque.Galeria.Id && objNoticia.Galeria.Imagens.Any();

      if (IsGaleria)
        GaleriaImgCount = objNoticia.Galeria.Imagens.Count();

      NewsId = objNoticia.Id;
      PublishDate = objNoticia.DataPublicacao.Value;
      DateShow = PublishDate.Date == DateTime.Today ? PublishDate.ToString("HH\\hmm") : PublishDate.ToString("dd \\de MMM");
      NewsUrl = $"{objNoticia.UrlFull}.html";
      Chamada = objNoticia.Chamada;
      ImgThumb =  $"{Constants.UrlDominioEstaticoUploads}/noticias/{objNoticia.ImgThumb}";
    }

    public NoticiaNavItem(Estabelecimento establishment, bool firstTitle)
    {
      FirstTitle = firstTitle;
      HasThumb = !string.IsNullOrEmpty(establishment.ImgThumb);
      IsVideo = establishment.DestaqueId == Destaque.Video.Id && !string.IsNullOrEmpty(establishment.Video);
      IsGaleria = establishment.DestaqueId == Destaque.Galeria.Id;

      if (IsGaleria)
        GaleriaImgCount = establishment.Galeria.Imagens.Count();

      NewsId = establishment.Id;
      PublishDate = establishment.DataCadastro;
      DateShow = PublishDate.Date == DateTime.Today ? PublishDate.ToString("HH\\hmm") : PublishDate.ToString("dd \\de MMM");
      NewsUrl = $"{establishment.UrlFull}.html";
      Chamada = establishment.Titulo;
      ImgThumb = $"{Constants.UrlDominioEstaticoUploads}/noticias/{establishment.ImgThumb}";
    }
    #endregion

    #region Properties
    public bool FirstTitle { get; set; }
    public bool HasThumb { get; set; }
    public bool IsVideo { get; set; }
    public bool IsGaleria { get; set; }

    public int NewsId { get; set; }
    public DateTime PublishDate{ get; set; }
    public string DateShow { get; set; }
    public string NewsUrl { get; set; }
    public string Chamada { get; set; }
    public int GaleriaImgCount { get; set; }
    public string ImgThumb { get; set; }

    #endregion
  }
}
