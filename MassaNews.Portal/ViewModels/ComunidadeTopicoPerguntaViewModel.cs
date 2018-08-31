using MassaNews.Portal.Models;
using MassaNews.Service.Models;
using System;
using System.Collections;
using System.Collections.Generic;

namespace MassaNews.Portal.ViewModels
{
  public class ComunidadeTopicoPerguntaViewModel : BaseModel
  {
    public int TopicoId { get; set; }
    public string TopicoTitulo { get; set; }
    public string TopicoDescricao { get; set; }
    public string TopicoSlug { get; set; }
    public bool CriadoPergunta { get; set; }
    public string Aviso1 { get; set; }
    public string Aviso2 { get; set; }
  }
}