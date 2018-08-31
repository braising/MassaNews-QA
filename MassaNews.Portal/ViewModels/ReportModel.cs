using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MassaNews.Portal.Models
{
  public class ReportModel : BaseModel
  {
    public string Nome { get; set; }
    public string Email { get; set; }
    public string Mensagem { get; set; }
    public string Url { get; set; }
    public DateTime Data { get; set; }
  }
}