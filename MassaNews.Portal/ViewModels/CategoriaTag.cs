using System.Collections.Generic;
using MassaNews.Portal.Models;

namespace MassaNews.Portal.ViewModels
{
  public class CategoriaTag : BaseModel
  {
    #region Properties
    public MassaNews.Service.Models.Tag Tag { get; set; }
    public List<MassaNews.Service.Models.Noticia> NoticiasHighlights { get; set; }
    public List<MassaNews.Service.Models.Noticia> Noticias { get; set; }
    #endregion
  }
}