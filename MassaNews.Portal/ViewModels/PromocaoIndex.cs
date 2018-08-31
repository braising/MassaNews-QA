using MassaNews.Service.Models;
using System.Collections.Generic;
using MassaNews.Portal.Models;

namespace MassaNews.Portal.ViewModels
{
  public class PromocaoIndex : BaseModel
  {
    #region Properties
    public int pages { get; set; }
    public IEnumerable<Inscricao> Inscricoes { get; set; }
    public int Destaque { get; set; }
    public bool ExibirFinalistas { get; set; }
    public bool Status { get; set; }
    #endregion
  }
}