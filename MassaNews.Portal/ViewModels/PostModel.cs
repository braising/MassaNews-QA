using Entities.Tables;
using System.Collections.Generic;

namespace MassaNews.Portal.Models
{
  public class PostModel : BaseModel
  {
    public List<Noticia> Posts { get; set; }
  }
}