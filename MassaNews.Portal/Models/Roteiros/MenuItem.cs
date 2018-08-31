using MassaNews.Service.Models;

namespace MassaNews.Portal.Models.Roteiros
{
  public class MenuItem
  {
    #region Properties
    public string Titulo { get; set; }
    public string Link { get; set; }
    public string Icone { get; set; }
    public bool IsCurrent { get; set; }
    public string Current => IsCurrent ? "is-current" : string.Empty;
    #endregion

    #region Methods

    public static MenuItem Map(CategoriaEstabelecimento category, string uf, string city, string categoriaUrl)
    {
      return new MenuItem
      {
        Titulo = category.Nome,
        Link =  $"/roteiros/{uf}/{city}/{category.Url}",
        Icone = category.Icone,
        IsCurrent = category.Url == categoriaUrl
      };
    }

    #endregion
  }
}
