using MassaNews.Service.Models;
using MassaNews.Portal.Models;
using System.Collections.Generic;

namespace MassaNews.Portal.ViewModels
{
  public class ContaInteracoesViewModel : BaseModel
  {
    public UsuarioPortal Usuario { get; set; }
    public List<ComunidadePergunta> Perguntas { get; set; }
    public List<ComunidadeResposta> Respostas { get; set; }
    public List<ComunidadeComentario> Comentarios { get; set; }

  }
}