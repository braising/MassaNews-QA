using MassaNews.Service.Models;
using MassaNews.Service.Util;

namespace MassaNews.Portal.Models.Roteiros
{
  public class DestatqueItem
  {
    #region Properties
    public string Titulo { get; set; }
    public string Link { get; set; }
    public string Img { get; set; }
    public string Chapeu { get; set; }
    public string LinkChapeu { get; set; }
    #endregion

    #region Methods

    public static DestatqueItem Map(Estabelecimento obj)
    {
      return new DestatqueItem()
      {
        Titulo = obj.Titulo,
        Link = $"/roteiros/pr/{obj.CidadeUrl}/{obj.CategoriaPaiUrl}/{obj.CategoriaUrl}/{obj.Url}.html",
        Img = $"{Constants.UrlDominioEstaticoUploads}/noticias/{obj.ImgMd}",
        Chapeu = obj.Categoria.Nome,
        LinkChapeu = "#"
      };
    }
    #endregion
  }
}
