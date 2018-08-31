using System.Collections.Generic;
using MassaNews.Service.Models;

namespace MassaNews.Portal.ViewModels
{
  public class CategoriasDestaquesModel
  {
    #region Properties
    public List<Noticia> HighlightsEducacao { get; set; }
    public List<Noticia> HighlightsBrasil { get; set; }
    public List<Noticia> HighlightsMundo { get; set; }
    public List<Noticia> HighlightsPolitica { get; set; }
    public List<Noticia> HighlightsEconomia { get; set; }
    public List<Noticia> HighlightsMaisNoticias { get; set; }
    //public List<Noticia> HighlightsConcursosEmpregos { get; set; }
    public List<Noticia> All => GetAll();
    #endregion

    #region Private Methods

    private List<Noticia> GetAll()
    {
      var listNoticias = new List<Noticia>();

      listNoticias.AddRange(HighlightsEducacao);
      listNoticias.AddRange(HighlightsBrasil);
      listNoticias.AddRange(HighlightsMundo);
      listNoticias.AddRange(HighlightsPolitica);
      listNoticias.AddRange(HighlightsEconomia);
      listNoticias.AddRange(HighlightsMaisNoticias);
      //listNoticias.AddRange(HighlightsConcursosEmpregos);

      return listNoticias;
    }

    #endregion
  }
}
