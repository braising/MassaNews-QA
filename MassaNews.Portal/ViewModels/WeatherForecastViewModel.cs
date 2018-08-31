using MassaNews.Service.Models;
using System;
using System.Collections.Generic;

namespace MassaNews.Portal.ViewModels
{
  public class WeatherForecastViewModel
  {
    #region Model Properties
    //public string City { get; set; }
    //public int? TempMaxima { get; set; }
    //public int? TempMinima { get; set; }
    //public string UltimaAtualizacao { get; set; }
    //public DateTime Data { get; set; }
    //public string DataFormatada { get; set; }
    //public string DescriptionWeather { get; set; }
    //public string Icon { get; set; }
    //public int? Temperature { get; set; }
    //public int? SensacaoTermica { get; set; }
    //public string Umidade { get; set; }
    //public WindDirection DirecaoVento { get; set; }
    //public string VelocidadeVento { get; set; }
    //public string NascerSol { get; set; }
    //public string PorSol { get; set; }
    public Weather WeatherPrincipal { get; set; }
    //public List<Weather> Weathers { get; set; }
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