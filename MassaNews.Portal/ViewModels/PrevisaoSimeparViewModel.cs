using MassaNews.Service.Models;
using System;
using System.Collections.Generic;

namespace MassaNews.Portal.ViewModels
{
  public class PrevisaoSimeparViewModel
  {
    #region Model Properties
    
    public SimeparTempo WeatherPrincipal { get; set; }
    public List<WeatherCities> WeatherCities1 { get; set; }
    public List<WeatherCities> WeatherCities2 { get; set; }
    #endregion

    #region Properties
    public string Title { get; set; }
    public string Description { get; set; }
    public string Robots { get; set; }
    public string Canonical { get; set; }
    public string Url { get; set; }
    public string ImgOpenGraph { get; set; }
    #endregion
  }
}