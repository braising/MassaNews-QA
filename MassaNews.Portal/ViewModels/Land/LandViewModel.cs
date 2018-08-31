using MassaNews.Models.Portal.Generic;
using MassaNews.Portal.Models;
using System.Collections.Generic;

namespace MassaNews.Portal.ViewModels
{
  public class LandViewModel : BaseModel
  {

    public List<Service.Models.Noticia> Highlights { get; set; }
    public List<Service.Models.Noticia> LineNews { get; set; }
    public List<NewsList> NewsList { get; set; }
    public DestaqueVideoViewModel DestaqueVideo { get; set; }
  }
}