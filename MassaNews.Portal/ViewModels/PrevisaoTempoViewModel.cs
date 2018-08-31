using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MassaNews.Portal.ViewModels
{
  public class PrevisaoTempoViewModel
  {
    public int tipo { get; set; }
    public PrevisaoSimeparViewModel Simepar { get; set; }
    public WeatherForecastViewModel Yahoo { get; set; }
  }
}