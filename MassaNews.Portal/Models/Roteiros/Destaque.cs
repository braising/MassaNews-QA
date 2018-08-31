using System.Collections.Generic;

namespace MassaNews.Portal.Models.Roteiros
{
  public class Destaque
  {
    public string Titulo { get; set; }
    public string Link { get; set; }
    public IEnumerable<DestatqueItem> Estabelecimentos { get; set; }
  }
}
