using MassaNews.Portal.Models;
using MassaNews.Service.Models;
using System;
using System.Collections;
using System.Collections.Generic;

namespace MassaNews.Portal.ViewModels
{
  public class ComunidadeTopicoViewModel : BaseModel
  {
    public string SlugTopico { get; set; }
    public string Nome { get; set; }
    public string Descricao { get; set; }
    public List<ComunidadeEspecialista> ComunidadeEspecialistas {get; set;}
    public List<ComunidadeVideo> ComunidadeVideos { get; set; }
    public List<Perguntas> ComunidadePerguntas { get; set; }
  }

  public class Perguntas
  {
    public int Id { get; set; }
    public string Topico { get; set; }
    public string Pergunta { get; set; }
    public string Descricao { get; set; }
    public string Slug { get; set; }
    public string Usuario { get; set; }
    public DateTime Data { get; set; }
    public int QtdResposta { get; set; }
  }
}