using System.Collections.Generic;
using MassaNews.Portal.Models;
using MassaNews.Portal.Models.Roteiros;

namespace MassaNews.Portal.ViewModels
{
  public class RoteirosIndex : BaseModel
  {
    public IEnumerable<MenuItem> Menu { get; set; }
    public IEnumerable<Destaque> Destaques { get; set; }
  }
}
