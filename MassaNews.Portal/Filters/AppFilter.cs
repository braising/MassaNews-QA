using MassaNews.Portal.Functions;
using MassaNews.Service.Models;
using MassaNews.Service.Services;
using System.Linq;
using System.Web.Mvc;

namespace MassaNews.Portal.Filters
{
  public class AppFilter : ActionFilterAttribute
  {
    #region Override
    public override void OnActionExecuting(ActionExecutingContext filterContext)
    {
      //Notify request
      Util.RequestNotify(filterContext.HttpContext.Request);

      base.OnActionExecuting(filterContext);
    }
    public override void OnActionExecuted(ActionExecutedContext filterContext)
    {
      var request = filterContext.HttpContext.Request;

      //Recupera o id da cidade que está gravada no cookie
      var cidadeId = CookieFx.GetLocationId(request);

      //Get current city object
      var currentCity = Cidade.Load(cidadeId);

      //Set the whether
      GetTemperature(filterContext, cidadeId);

      //Current city
      if (filterContext.Controller.ViewBag.CurrentCity == null)
        filterContext.Controller.ViewBag.CurrentCity = currentCity;

      //Microregions
      if (filterContext.Controller.ViewBag.Microregions == null)
        filterContext.Controller.ViewBag.Microregions = Microregiao.GetAllUrlToDictionary();

      //Cities
      if (filterContext.Controller.ViewBag.Cidades == null)
        filterContext.Controller.ViewBag.Cidades = new SelectList(Cidade.GetAllToDictionary(), "Key", "Value", cidadeId);
    }
    #endregion

    #region Methods
    private void GetTemperature(ActionExecutedContext filterContext, int locationId)
    {
      var WeatherSrv = new WeatherService();

      var objCidade = Cidade.Load(locationId);

      var objPrevisaoTipo = PrevisaoTempo.Load(1);

      if (objPrevisaoTipo.Tipo == 1)//Yahoo
      {
        var obj = WeatherSrv.GetWeather(locationId);

        filterContext.Controller.ViewBag.City = obj == null ? string.Empty : obj.City ?? string.Empty;
        filterContext.Controller.ViewBag.Description = obj == null ? string.Empty : obj.Description ?? string.Empty;
        filterContext.Controller.ViewBag.Icon = obj == null ? string.Empty : obj.Icon ?? string.Empty;
        filterContext.Controller.ViewBag.TempMax = obj == null ? string.Empty : obj.TempMaxima == 0 ? string.Empty : obj.TempMaxima.ToString().Trim();
        filterContext.Controller.ViewBag.TempMin = obj == null ? string.Empty : obj.TempMinima == 0 ? string.Empty : obj.TempMinima.ToString().Trim();
      }
      else if (objPrevisaoTipo.Tipo == 2)//Simepar
      {
        var obj = WeatherSrv.GetSimepar(locationId);

        var todayWeather = new Simepar();

        if (obj != null)
          todayWeather = obj.simeparPrevisoes.FirstOrDefault();

        filterContext.Controller.ViewBag.City = objCidade.Nome;
        filterContext.Controller.ViewBag.Description = obj == null ? string.Empty : todayWeather.simeparPeriodos.FirstOrDefault().description ?? string.Empty;
        filterContext.Controller.ViewBag.Icon = obj == null ? string.Empty : todayWeather.simeparPeriodos.FirstOrDefault().icon ?? string.Empty;
        filterContext.Controller.ViewBag.TempMax = obj == null ? string.Empty : todayWeather.tempMax == 0 ? string.Empty : todayWeather.tempMax.ToString().Trim();
        filterContext.Controller.ViewBag.TempMin = obj == null ? string.Empty : todayWeather.tempMin == 0 ? string.Empty : todayWeather.tempMin.ToString().Trim();
      }

    }
    #endregion
  }
}
