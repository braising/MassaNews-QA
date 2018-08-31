using Entities.Tables;
using System.Collections.Generic;
using MassaNews.Portal.Models;

namespace MassaNews.Portal.ViewModels
{
  public class BlogPostViewModel : BaseModel
  {
    public Blog Blog { get; set; }
    public IEnumerable<MassaNews.Service.Models.Autor> Autors { get; set; }
    public List<Service.Models.Noticia> NoticiasHighlights { get; set; }
  }
}