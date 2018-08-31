using MassaNews.Service.Models;
using MassaNews.Portal.Models;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;

namespace MassaNews.Portal.ViewModels
{
  public class ContaViewModel : BaseModel
  {
    public UsuarioPortal Usuario { get; set; }
    public List<SelectListItem> Estados { get; set; }
    public List<SelectListItem> Cidades { get; set; }
    public List<SelectListItem> Sexos { get; set; }
  }
}