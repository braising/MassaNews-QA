using MassaNews.Service.Response;
using System.Collections.Generic;

namespace MassaNews.Portal.Models
{
  public class BuscaModel : BaseModel
  {
    public int Count { get; set; }
    public int CountNoticias { get; set; }
    public List<SearchResponse> Resultados { get; set; }
  }
}