using System.Collections.Generic;
using MassaNews.Portal.Models;
using MassaNews.Portal.Models.Roteiros;

namespace MassaNews.Portal.ViewModels
{
  public class RoteirosCategory : BaseModel
  {
    public IEnumerable<MenuItem> Menu { get; set; }
    public Destaque Destaque { get; set; }
  }
}
