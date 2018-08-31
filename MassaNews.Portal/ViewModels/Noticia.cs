using Entities.Tables;
using System;

namespace MassaNews.Portal.Models
{
  public class LastBlogPostItemModel
  {
    public Noticia Post { get; set; }
    public bool FirstTitle { get; set; }
    public DateTime Data { get; set; }
  }
}
