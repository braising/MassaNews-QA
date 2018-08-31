using System.Collections.Generic;
using MassaNews.Portal.Models;

namespace MassaNews.Portal.ViewModels
{
  public class NoticiaModel : BaseModel
  {
    //public IEnumerable<NewsItemViewModel> Noticias { get; set; }
    public NewsItemViewModel News { get; set; }
  }
}