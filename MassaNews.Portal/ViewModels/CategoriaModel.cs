using System.Collections.Generic;
using Entities.Tables;
using MassaNews.Portal.Models;
using System;

namespace MassaNews.Portal.ViewModels
{
  public class CategoriaModel : BaseModel
  {
    public Editorial Editorial { get; set; }
    public Categoria Categoria { get; set; }
    public Service.Models.Tag Tag { get; set; }
    public List<Service.Models.Noticia> NoticiasHighlights { get; set; }
    public List<Noticia> Noticias;
    public List<NoticiaByCategoria> NoticiaByCategoria { get; set; }
    public List<Post> Pots { get; set; }
    public List<Service.Models.ComunidadeTopico> ComunidadeTopicos { get; set; }
  }

  public class NoticiaByCategoria
  {
    public string UrlEditorial;
    public Categoria Categoria;
    public List<Noticia> NoticiasHighlights;
    public List<Noticia> Noticias;
  }

  public class Post
  {
    public string PostUrl { get; set; }
    public string Call { get; set; }
    public string Hat { get; set; }
    public IEnumerable<string> ImageUrl { get; set; }
  }
}