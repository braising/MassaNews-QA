using MassaNews.Service.Models;
using System.Linq;

namespace MassaNews.Portal.ViewModels
{
  public class GaleriaViewModel
  {
    #region Properties
    public string Chamada { get; set; }

    public Galeria Galeria { get; set; }

    public Imagem ImagemPrincipal
    {
      get { return Galeria.Imagens.OrderBy(o => o.Ordem).First(); }
    }
    #endregion

  }
}
