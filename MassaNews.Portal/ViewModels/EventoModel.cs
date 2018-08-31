using Entities.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MassaNews.Portal.Models
{
  public class EventoModel : BaseModel
  {
    public Evento Evento { get; set; }
  }
}