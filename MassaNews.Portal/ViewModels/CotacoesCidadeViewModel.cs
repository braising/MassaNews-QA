using Entities.Tables;
using System.Collections.Generic;
using MassaNews.Portal.Models;
using System;

namespace MassaNews.Portal.ViewModels
{
  public class CotacoesCidadeViewModel : BaseModel
  {
    public DateTime DataCotacao { get; set; }
    public CotacaoTipo Tipo { get; set; }
    public List<CotacoesPorCidade> Cotacoes { get; set; }
    public CotacaoCidade Cidade { get; set; }
    public List<CotacaoCidade> Cidades { get; set; }
  }

  public class CotacoesPorCidade
  {
    public string Cidade { get; set; }
    public string CidadeSlug { get; set; }
    public decimal Valor { get; set; }
    public decimal ValorAnterior { get; set; }
    public DateTime DataCotacao { get; set; }
    public string ProdutoNome { get; set; }
    public string ProdutoSlug { get; set; }
  }
}