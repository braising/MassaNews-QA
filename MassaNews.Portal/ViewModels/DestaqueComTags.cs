using System.Collections.Generic;
using MassaNews.Service.Models;

namespace MassaNews.Portal.Models
{
  public class DestaqueComTags
  {
    public string Key { get; set; }
    public string Theme { get; set; }
    public string LinkImgTitulo { get; set; }
    public string Titulo { get; set; }
    public string LinkTitulo { get; set; }
    public List<Tag> Tags { get; set; }
    public List<Noticia> Highlights { get; set; }
    public bool isLazy { get; set; }
  }
}
