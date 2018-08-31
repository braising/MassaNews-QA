using System;
using MassaNews.Portal.Models;
using System.Web;

namespace MassaNews.Portal.ViewModels
{
  public class PromocaoEnviar : BaseModel
  {
    #region Properties
    public string Nome { get; set; }
    public string Email { get; set; }
    public string Telefone { get; set; }
    public DateTime Nascimento { get; set; }
    public string Cidade { get; set; }
    public string Titulo { get; set; }
    public HttpPostedFileWrapper ImagemCasa { get; set; }
    public HttpPostedFileWrapper ImagemRua { get; set; }
    public HttpPostedFileWrapper ImagemComprovante { get; set; }
    public bool OptInNews { get; set; }
    public string Cpf { get; set; }
    public bool TipoCasa { get; set; }
    public bool TipoRua { get; set; }
    #endregion
  }
}