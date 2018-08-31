using Entities.Tables;
using System.Collections.Generic;
using MassaNews.Portal.Models;
using System;

namespace MassaNews.Portal.ViewModels
{
  public class CotacoesCidadeProdutoViewModel : BaseModel
  {
    public Cotacao ProdutoSlugOutro { get; set; }
    public CotacaoTipo Tipo { get; set; }
    public CotacaoProduto Produto { get; set; }
    public List<Cotacao> Cotacoes { get; set; }
    public List<CotacaoPorEstado> CotacoesPorEstado { get; set; }
    public List<CotacoesPorCidade> CotacoesPorCidade { get; set; }
    public string GraficoLabels30 { get; set; }
    public string GraficoData30 { get; set; }
    public string GraficoLabels60 { get; set; }
    public string GraficoData60 { get; set; }
  }  
}