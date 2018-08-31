using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MassaNews.Portal.Models.Comunidade
{
  public class TopicoPergunta
  {
    public int Topico { get; set; }
    public string Titulo { get; set; }
    public string Descricao { get; set; }
  }
}