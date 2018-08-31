using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MassaNews.Portal.ViewModels
{
  public class CotacaoEconomiaViewModel
  {
    public string Simbolo { get; set; }
    public string Moeda { get; set; }
    public decimal Varicao { get; set; }
    public decimal TaxaVenda { get; set; }


  }
}