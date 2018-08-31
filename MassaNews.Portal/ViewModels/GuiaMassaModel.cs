using Entities.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MassaNews.Portal.Models
{
  public class GuiaMassaModel : BaseModel
  {
    public List<CategoriaEvento> Categorias { get; set; }
    public List<Evento> Populares { get; set; }
    public List<Evento> Hoje { get; set; }
    public List<Evento> Amanha { get; set; }
  }
}