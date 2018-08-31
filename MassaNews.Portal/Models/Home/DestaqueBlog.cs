using MassaNews.Service.Models;
using System.Collections.Generic;

namespace MassaNews.Portal.Models.Home
{
  public class DestaqueBlog
  {
    public bool Status { get; set; }

    public Blog Blogs1 { get; set; }

    public Blog Blogs2 { get; set; }

    public Blog Blogs3 { get; set; }

    public List<Noticia> Noticias { get; set; }
  }
}