using MassaNews.Portal.Models;
using MassaNews.Service.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MassaNews.Portal.ViewModels
{
  public class ComunidadeTopicoRespostaViewModel : BaseModel
  {
    public Perguntas Pergunta { get; set; }
    public List<ComunidadeEspecialista> ComunidadeEspecialistas { get; set; }
    public List<ComunidadeVideo> ComunidadeVideos { get; set; }
    public List<Respostas> ComunidadeRespostas { get; set; }

    public class Respostas
    {
      public int Id { get; set; }
      public string Resposta { get; set; }
      public string Usuario { get; set; }
      public bool IsEspecialista { get; set; }
      public DateTime Data { get; set; }
      public int CountCurtida { get; set; }
      public bool UserCurtida { get; set; }
      public int CountNaoCurtida { get; set; }
      public bool UserNaoCurtida { get; set; }

      public List<Comentarios> Comentarios {get; set;}
    }

    public class Comentarios
    {
      public int Id { get; set; }
      public string Comentario { get; set; }
      public string Usuario { get; set; }
      public bool IsEspecialista { get; set; }
      public DateTime Data { get; set; }
      public int CountCurtida { get; set; }
      public bool UserCurtida { get; set; }
      public int CountNaoCurtida { get; set; }
      public bool UserNaoCurtida { get; set; }
    }
  }
}