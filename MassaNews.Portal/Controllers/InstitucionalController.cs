using Entities.Contexts;
using System;
using System.Net;
using System.Web.Mvc;
using MassaNews.Portal.Models;
using MassaNews.Portal.ViewModels;
using MassaNews.Service.Services;
using MassaNews.Service.Models;
using System.Linq;
using System.Collections.Generic;
using MassaNews.Service.Util;
using Newtonsoft.Json;

namespace MassaNews.Portal.Controllers
{
  public class InstitucionalController : BaseController
  {
    #region Properties

    private EntitiesDb Db { get; }
    private LocalService LocalSrv { get; }
    private WeatherService WeatherSrv { get; }
    #endregion

    #region Constructor

    public InstitucionalController()
    {
      Db = new EntitiesDb();
      LocalSrv = new LocalService();
      WeatherSrv = new WeatherService();
    }

    #endregion

    #region Actions
    [Route("institucional/sobre")]
    public ActionResult Sobre()
    {
      var model = new HomeIndex();
      ViewBag.ActiveNav = "Sobre nós";

      /* base model defaults */
      model.Title = "Sobre nós - Massa News";
      model.Description = "O Massa News é uma empresa de conteúdo e tecnologia do Grupo Massa, focada na produção e distribuição online de jornalismo local, com cobertura em todas as regiões do Paraná. Saiba mais!";
      model.Robots = "index, follow";
      model.Canonical = $"{Constants.UrlWeb}/institucional/sobre";

      return View(model);
    }

    [Route("institucional/fale-conosco")]
    public ActionResult FaleConosco()
    {
      var model = new HomeIndex();
      ViewBag.ActiveNav = "Fale Conosco";

      /* base model defaults */
      model.Title = "Fale Conosco - Massa News";
      model.Description = "Fale conosco para sugerir ideias, anunciar, enviar pautas, reportar erros ou trabalhar no Massa News.";
      model.Robots = "index, follow";
      model.Canonical = $"{Constants.UrlWeb}/institucional/fale-conosco";

      return View(model);
    }

    [Route("institucional/cookies")]
    public ActionResult Cookies()
    {
      var model = new HomeIndex();
      ViewBag.ActiveNav = "Política de Uso de Cookies";

      /* base model defaults */
      model.Title = "Política de Uso de Cookies - Massa News";
      model.Description = "Cookies são pequenos arquivos de textos gerados pelo portal Massa News enquanto você o visita. Saiba mais!";
      model.Robots = "index, follow";
      model.Canonical = $"{Constants.UrlWeb}/institucional/cookies";

      return View(model);
    }

    [Route("institucional/reportar")]
    [HttpPost]
    public ActionResult Reportar(ReportModel model)
    {
      try
      {
        var body = $@"<!DOCTYPE html>
                    <html lang='pt-br'>
                    <head>
                      <meta charset='UTF-8'>
                      <title>{model.Title}</title>
                    </head>
                    <body>
                      <div>
                        <span class='HOEnZb'>
                          <font color='#888888'><h3>Massa News | Reportar erro</h3>
                            <strong>Nome:</strong> {model.Nome}<br>
                            <strong>E-mail:</strong> <a href='mailto:{model.Email}' target='_blank'>{model.Email}</a><br>
                            <strong>Data do contato:</strong> {DateTime.Now}<br>
                            <strong>URL:</strong> <a href='{model.Url}' target='_blank'>{model.Url}</a><br>
                            <strong>Mensagem:</strong> <br> {model.Mensagem}.
                          </font>
                        </span>
                      </div>
                    </body>
                    </html>";

        ToolService.SendEmail("Massa News | Reportar erro", body, "patricia.tressoldi@massanews.com");

        return Json("ok");
      }
      catch (Exception ex)
      {
        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
      }
    }

    [Route("institucional/envie-sua-noticia")]
    public ActionResult Envie()
    {
      var model = new HomeIndex();
      ViewBag.ActiveNav = "Envie sua Notícia";

      /* base model defaults */
      model.Title = "Envie sua Notícia - Massa News";
      model.Description = "Envie sua Notícia para a redação do Massa News por e-mail, Whatsapp ou Facebook. Não esqueça de anexar fotos, áudios ou vídeos sobre o assunto.";
      model.Robots = "index, follow";
      model.Canonical = $"{Constants.UrlWeb}/institucional/envie-sua-noticia";

      return View(model);
    }

    [Route("placar-do-impeachment")]
    public ActionResult PlacarImpeachment()
    {
      var model = new HomeIndex();
      ViewBag.ActiveNav = "Placar do Impeachment";

      /* base model defaults */
      model.Title = "Placar do Impeachment da presidente Dilma Rousseff - Massa News";
      model.Description = "Intenções declaradas de voto dos deputados federais no processo de impeachment da presidente Dilma Rousseff. A votação na Câmara está prevista para ocorrer no domingo (17).";
      model.Robots = "index, follow";
      model.Canonical = $"{Constants.UrlWeb}/placar-do-impeachment";
      model.ImgOpenGraph = $"{Constants.UrlWeb}/content/images/banners/avatar-placar-impeachment.jpg";

      return View(model);
    }

    [Route("placar-do-impeachment-senado")]
    public ActionResult PlacarImpeachmentSenado()
    {
      var model = new HomeIndex();
      ViewBag.ActiveNav = "Placar do Impeachment";

      /* base model defaults */
      model.Title = "Placar do Impeachment da presidente Dilma Rousseff no Senado - Massa News";
      model.Description = "Intenções declaradas de voto dos senadores em relação à abertura de processo por crime de responsabilidade contra a presidente Dilma Rousseff.";
      model.Robots = "index, follow";
      model.Canonical = $"{Constants.UrlWeb}/placar-do-impeachment-senado";
      model.ImgOpenGraph = $"{Constants.UrlWeb}/content/images/banners/avatar-placar-impeachment-senado.jpg";

      return View(model);
    }

    [Route("calendario-competicoes-jogos-olimpicos")]
    public ActionResult CalendarioCompeticoes()
    {
      var model = new HomeIndex();
      ViewBag.ActiveNav = "Calendário de Competições";

      /* base model defaults */
      model.Title = "Calendário de competição dos Jogos Olímpicos de 2016 - Massa News";
      model.Description = "Calendário de competição dos Jogos Olímpicos de 2016 no Massa News";
      model.Robots = "index, follow";
      model.Canonical = $"{Constants.UrlWeb}/calendario-competicoes-jogos-olimpicos";

      return View(model);
    }

    [Route("legado-olimpico")]
    public ActionResult LegadoOlimpico()
    {
      var model = new HomeIndex();
      ViewBag.ActiveNav = "Legado Olímpico";

      /* base model defaults */
      model.Title = "Rio 2016: O Legado Olímpico - Massa News";
      model.Description = "omplexos esportivos em construção na Barra e Deodoro vão transformar a cidade após os Jogos de 2016, prometem organizadores e poder público. O Legado Olímpico!";
      model.Robots = "index, follow";
      model.Canonical = $"{Constants.UrlWeb}/legado-olimpico";

      return View(model);
    }

    [Route("tocha-olimpica")]
    public ActionResult ATochaOlimpica()
    {
      var model = new HomeIndex();
      ViewBag.ActiveNav = "A Tocha Olímpica";

      /* base model defaults */
      model.Title = "Por dentro da tocha olímpica Rio 2016 - Massa News";
      model.Description = "Tocha olímpica tem sistema inédito – pela primeira vez o famoso símbolo terá movimento para receber a chama.";
      model.Robots = "index, follow";
      model.Canonical = $"{Constants.UrlWeb}/tocha-olimpica";

      return View(model);
    }

    [Route("tags/eleicoes-2016/candidatos")]
    public ActionResult CandidatosEleicoes()
    {
      var model = new HomeIndex();
      ViewBag.ActiveNav = "Guia de Candidatos";

      /* base model defaults */
      model.Title = "Eleições Municipais 2016 - Candidaturas e Contas Eleitorais - Massa News";
      model.Description = "Informações detalhadas sobre todos os candidatos que pediram registro à Justiça Eleitoral e sobre as suas contas eleitorais e as dos partidos políticos. Eleições Municipais 2016.";
      model.Robots = "index, follow";
      model.Canonical = $"{Constants.UrlWeb}/tags/eleicoes-2016/candidatos";

      return View(model);
    }

    [Route("tags/eleicoes-2016/calendario")]
    public ActionResult CalendarioEleicoes()
    {
      var model = new HomeIndex();
      ViewBag.ActiveNav = "Calendário";

      /* base model defaults */
      model.Title = "Calendário das Eleições Municipais 2016 - Massa News";
      model.Description = "Acompanhe os eventos mais importantes das Eleições Municipais 2016.";
      model.Robots = "index, follow";
      model.Canonical = $"{Constants.UrlWeb}/tags/eleicoes-2016/calendario";

      return View(model);
    }

    [Route("aniversario")]
    public ActionResult WhereAniversario()
    {
      //Redirect 301 to Home
      return new RedirectResult("/", true);

      var model = new HomeIndex();
      ViewBag.ActiveNav = "Where 15 anos";

      /* base model defaults */
      model.Title = "Where Curitiba 15 anos - Massa News";
      model.Description = "A Where Curitiba completa 15 anos de idade, e oferece 15 experiências incríveis para você. Participe!";
      model.Robots = "index, follow";
      model.Canonical = $"{Constants.UrlWeb}/aniversario";

      return View(model);
    }

    // [Route("natal-de-luz")]
    // public ActionResult NatalDeLuz()
    // {
    //   //Redirect 301 to Home
    //   //return new RedirectResult("/", true);

    //   var model = new HomeIndex();
    //   ViewBag.ActiveNav = "Natal de Luz";
    //   ViewBag.Pagina = "natal-de-luz";

    //   /* base model defaults */
    //   model.Title = "Natal de Luz Curitiba - Massa News";
    //   model.Description = "";
    //   model.Robots = "index, follow";
    //   model.Canonical = $"{Constants.UrlWeb}/natal-de-luz";

    //   return View(model);
    // }

    [Route("previsao-do-tempo-simepar/{cidadeUrl}")]
    public ActionResult PrevisaoTempoByCitySimepar(string cidadeUrl)
    {
      return new RedirectResult($"/previsao-do-tempo/{cidadeUrl}", true);
    }

    [Route("previsao-do-tempo/{cidadeUrl}")]
    public ActionResult PrevisaoTempoByCity(string cidadeUrl)
    {
      var objPrevisaoTipo = MassaNews.Service.Models.PrevisaoTempo.Load(1);

      if (objPrevisaoTipo.Tipo == 1)//Yahoo
        return PrevisaoTempoYahoo(cidadeUrl);
      else
        return PrevisaoTempoSimepar(cidadeUrl);

    }

    [HttpGet]
    [Route("quiz-unifil")]
    public ActionResult QuizUnifil()
    {
      //Redirect 301 to Home
      //return new RedirectResult("/", true);

      //Create a model
      var model = new HomeIndex
      {
        Title = "Quiz UniFil – Descubra a sua profissão",
        Description = "Qual profissão combina com você? Responsa o Quiz e em poucos minutos você descobrirá. É rápido, fácil e grátis!",
        Robots = "index, follow",
        Canonical = $"{Constants.UrlWeb}/quiz-unifil",
        ImgOpenGraph = $"{Constants.UrlWeb}/content/images/quiz-unifil/avatar.jpg"
      };

      ViewBag.Pagina = "quiz-unifil";
      ViewBag.ActiveNav = "Quiz UniFil";

      return View(model);
    }

    [Route("quiz-unifil")]
    [HttpPost]
    public ActionResult QuizUnifilPost()
    {
      var obj = new
      {
        Nome = Request.Form["Nome"],
        Email = Request.Form["Email"],
        CPF = Request.Form["Cpf"],
        Resultado = Request.Form["Resultado"],
        OptinNews = Request.Form["OptinNews"]
      };

      var objData = new TempData
      {
        Description = "Quiz UniFil",
        Data = JsonConvert.SerializeObject(obj),
        Registered = DateTime.Now
      };

      objData.Save();

      return Json("ok");

    }
    #endregion

    #region Methods
    private ActionResult PrevisaoTempoYahoo(string cidadeUrl)
    {

      var objCidade = cidadeUrl != null ? Cidade.GetCidadeByUrl(cidadeUrl) : null;

      var objWeather = WeatherSrv.GetWeatherForecastByLocation(objCidade != null ? objCidade.Id : 12);

      if (objWeather == null || (objWeather != null && objWeather.WeatherPrincipal == null && !objWeather.WeatherCities.Any()))
        return new HttpStatusCodeResult(HttpStatusCode.NotFound);
      else if (objWeather != null && objWeather.WeatherPrincipal == null && objWeather.WeatherCities.Any())
        ViewBag.ErrorWeather = "<p><div><h2>Ops!</h2></div><div><h4>Não conseguimos carregar a previsão do tempo para sua cidade. Tente novamente mais tarde.</div></h4></p>";

      var model = new WeatherForecastViewModel
      {
        WeatherPrincipal = objWeather.WeatherPrincipal,
        Title = $"Previsão do tempo em {objCidade.Nome}-PR - Massa News",
        Description = $"Confira a previsão do tempo em {objCidade.Nome}-PR. Sensação térmica, umidade, direção e velocidade do vento e muita mais.",
        Robots = "index, follow",
        Canonical = $"{Constants.UrlWeb}/previsao-do-tempo/{objCidade.Url}"
      };

      if (objWeather.WeatherCities.Count() > 1)
      {
        var result = (double)objWeather.WeatherCities.Count() / (double)2;

        var metadeTamanhoLista = Convert.ToInt32(Math.Ceiling(result));

        model.WeatherCities1 = objWeather.WeatherCities.Take(metadeTamanhoLista).ToList();
        model.WeatherCities2 = objWeather.WeatherCities.Skip(metadeTamanhoLista).ToList();
      }
      else
      {
        model.WeatherCities1 = objWeather.WeatherCities;
        model.WeatherCities2 = new List<WeatherCities>();
      }

      ViewBag.ActiveNav = "Previsão do tempo";

      return View("PrevisaoTempo", model);

    }
    private ActionResult PrevisaoTempoSimepar(string cidadeUrl)
    {
      var objCidade = cidadeUrl != null ? Cidade.GetCidadeByUrl(cidadeUrl) : null;

      var objWeather = WeatherSrv.GetForecastSimepar(objCidade != null ? objCidade.Id : 12);

      if (objWeather == null || (objWeather != null && objWeather.previsaoPrincipal == null))
        ViewBag.ErrorWeather = "<p><div><h2>Ops!</h2></div><div><h4>Não conseguimos carregar a previsão do tempo para sua cidade. Tente novamente mais tarde.</div></h4></p>";

      var model = new PrevisaoSimeparViewModel
      {
        WeatherPrincipal = objWeather.previsaoPrincipal,
        Title = $"Previsão do tempo em {objCidade.Nome}-PR - Massa News",
        Description = $"Confira a previsão do tempo em {objCidade.Nome}-PR. Sensação térmica, umidade, direção e velocidade do vento e muita mais.",
        Robots = "index, follow",
        Canonical = $"{Constants.UrlWeb}/previsao-do-tempo/{objCidade.Url}"
      };

      if (objWeather.previsaoCidades.Count() > 1)
      {
        var result = (double)objWeather.previsaoCidades.Count() / (double)2;//necessário fazer o cast para double por conta da divisão

        var metadeTamanhoLista = Convert.ToInt32(Math.Ceiling(result));

        model.WeatherCities1 = objWeather.previsaoCidades.Take(metadeTamanhoLista).ToList();
        model.WeatherCities2 = objWeather.previsaoCidades.Skip(metadeTamanhoLista).ToList();
      }
      else
      {
        model.WeatherCities1 = objWeather.previsaoCidades;
        model.WeatherCities2 = new List<WeatherCities>();
      }

      ViewBag.ActiveNav = "Previsão do tempo";

      return View("PrevisaoTempoSimepar", model);
    }
    #endregion

    protected override void Dispose(bool disposing)
    {
      if (disposing)
      {
        Db.Dispose();
      }

      base.Dispose(disposing);
    }
  }
}