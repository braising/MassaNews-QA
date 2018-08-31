using System.Collections.Generic;
using MassaNews.Service.Models;
using MassaNews.Portal.Models;

namespace MassaNews.Portal.ViewModels
{
  public class CategoriaBlogViewModel : BaseModel
  {
    public class Blog
    {
      public int Id { get; set; }
      public string Titulo { get; set; }
      public string Url { get; set; }
      public string Img { get; set; }
      public string CategoriaUrl { get; set; }
      public Noticia LastPost { get; set; }
    }

    public IEnumerable<Blog> Blogs { get; set; }

    public IEnumerable<Categoria> Categories { get; set; }
  }
}