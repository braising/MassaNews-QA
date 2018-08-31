using Entities.Tables;
using System.Collections.Generic;
using MassaNews.Portal.Models;
using System;

namespace MassaNews.Portal.ViewModels
{
  public class CotacoesViewModel : BaseModel
  {
    public CotacaoTipo Tipo { get; set; }
    public List<Cotacao> Cotacoes { get; set; }
    public List<CotacoesPorEstado> CotacoesPorEstados { get; set; }
    public List<CotacaoCidade> Cidades { get; set; }
  }

  public class CotacoesPorEstado
  {
    public string EstadoSlug { get; set; }
    public decimal Media { get; set; }
    public decimal MediaAnterior { get; set; }
    public DateTime DataCotacao { get; set; }
    public string ProdutoNome { get; set; }
    public string ProdutoSlug { get; set; }
  }
}
