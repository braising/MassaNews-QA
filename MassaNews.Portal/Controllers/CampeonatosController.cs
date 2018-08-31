using System.Web.Mvc;
using System.Web.UI;
using MassaNews.Service.Enum;
using MassaNews.Service.Services;
using MassaNews.Portal.ViewModels;

namespace MassaNews.Portal.Controllers
{
    public class CampeonatosController : BaseController
    {
        #region Properts

        private CampeonatoService CampeonatoSrv { get; }

        #endregion

        #region Constructor

        public CampeonatosController()
        {
            CampeonatoSrv = new CampeonatoService();
        }

        #endregion

        #region Actions

        [Route("esportes/futebol/campeonatos/tabela-paranaense-2016")]
        public ActionResult Paranaense2016()
        {
            return new RedirectResult($"/esportes/futebol/campeonatos/tabela-paranaense", true);
        }

        [Route("esportes/futebol/campeonatos/tabela-paranaense")]
        [OutputCache(Duration = 5 * 60, VaryByCustom = "Location", Location = OutputCacheLocation.Server)]
        public ActionResult Paranaense()
        {
            var objModel = new CampeonatoModel
            {
                Title = "Tabela Paranaense - Massa News",
                Description = "Tabela da Campeonato Paranaense com a classificação e todos os jogos do campeonato. Confira!",
                Robots = "index, follow"
            };

            ViewBag.ActiveNav = "Campeonato Paranaense";
            ViewBag.TbClassificacao = CampeonatoSrv.GetHtmlClassificacao(CampeonatoEnum.Paranaense);
            ViewBag.LstRodadas = CampeonatoSrv.GetListRodadas((int)CampeonatoEnum.Paranaense);

            return View("paranaense", objModel);
        }

        [Route("esportes/futebol/campeonatos/tabela-copa-do-brasil")]
        [OutputCache(Duration = 5 * 60, VaryByCustom = "Location", Location = OutputCacheLocation.Server)]
        public ActionResult CampeonatoCopaBrasil()
        {
            var objModel = new CampeonatoModel
            {
                Title = "Copa do Brasil - Massa News",
                Description = "Tabela da Copa do Brasil 2016 com a classificação e todos os jogos do campeonato. Confira!",
                Robots = "index, follow"
            };

            ViewBag.ActiveNav = "Copa do Brasil";
            //ViewBag.TbClassificacao = CampeonatoSrv.GetHtmlClassificacao((int)CampeonatoEnum.campeonato.Copa_Brasil);
            ViewBag.LstRodadas = CampeonatoSrv.GetListRodadas((int)CampeonatoEnum.CopaBrasil);

            return View("copa-do-brasil-2016", objModel);
        }

        [Route("esportes/futebol/campeonatos/tabela-brasileirao-serie-a")]
        [OutputCache(Duration = 5 * 60, VaryByCustom = "Location", Location = OutputCacheLocation.Server)]
        public ActionResult CampeonatoBrasileiraoA()
        {
            var objModel = new CampeonatoModel
            {
                Title = "Tabela do Brasileirão série A - Massa News",
                Description = "Confira a Tabela do Campeonato Brasileiro série A 2018 com a classificação e todos os jogos do campeonato.",
                Robots = "index, follow"
            };

            ViewBag.ActiveNav = "Brasileirão série A";
            ViewBag.TbClassificacao = CampeonatoSrv.GetHtmlClassificacao(CampeonatoEnum.BrasileiraoA);
            ViewBag.LstRodadas = CampeonatoSrv.GetListRodadas((int)CampeonatoEnum.BrasileiraoA);
            ViewBag.TbActiveClassifA = true;
            ViewBag.TbActiveClassifB = false;

            return View("brasileirao-serie-a", objModel);
        }

        [Route("esportes/futebol/campeonatos/tabela-brasileirao-serie-b")]
        [OutputCache(Duration = 5 * 60, VaryByCustom = "Location", Location = OutputCacheLocation.Server)]
        public ActionResult CampeonatoBrasileiraoB()
        {
            var objModel = new CampeonatoModel
            {
                Title = "Tabela do Brasileirão série B - Massa News",
                Description = "Confira a Tabela do Campeonato Brasileiro série B 2018 com a classificação e todos os jogos do campeonato.",
                Robots = "index, follow"
            };

            ViewBag.ActiveNav = "Brasileirão série B";
            ViewBag.TbClassificacao = CampeonatoSrv.GetHtmlClassificacao(CampeonatoEnum.BrasileiraoB);
            ViewBag.LstRodadas = CampeonatoSrv.GetListRodadas((int)CampeonatoEnum.BrasileiraoB);
            ViewBag.TbActiveClassifA = false;
            ViewBag.TbActiveClassifB = true;

            return View("brasileirao-serie-b", objModel);
        }

        [Route("esportes/futebol/campeonatos/tabela-libertadores")]
        [OutputCache(Duration = 5 * 60, Location = OutputCacheLocation.Server)]
        public ActionResult CampeonatoLibertadores()
        {
            var objModel = new CampeonatoModel
            {
                Title = "Libertadores da América - Massa News",
                Description = "Tabela da Libertadores da América 2016 com a classificação e todos os jogos do campeonato. Confira!",
                Robots = "index, follow"
            };

            ViewBag.ActiveNav = "Libertadores da América";
            ViewBag.LstClassificacao = CampeonatoSrv.GetHtmlClassificacaoLibertadores();
            ViewBag.LstRodadas = CampeonatoSrv.GetListRodadas((int)CampeonatoEnum.Libertadores);

            return View("libertadores-2016", objModel);
        }

        // [Route("massa-news-na-russia/tabela")]
        // [OutputCache(Duration = 5 * 60, Location = OutputCacheLocation.Server)]
        // public ActionResult CampeonatoCopaDoMundo()
        // {
        //     var objModel = new CampeonatoModel
        //     {
        //         Title = "Tabela de jogos e classificação - Massa News na Rússia",
        //         Description = "Confira a tabela de jogos e classificação. Massa News na Rússia.",
        //         Robots = "index, follow"
        //     };

        //     ViewBag.ActiveNav = "Massa News na Rússia";
        //     ViewBag.LinkActiveNav = "/massa-news-na-russia";
        //     ViewBag.LstClassificacao = CampeonatoSrv.GetHtmlClassificacaoCopaDoMundo();
        //     ViewBag.LstRodadas = CampeonatoSrv.GetListRodadas((int)CampeonatoEnum.CopaDoMundo);

        //     return View("massa-news-na-russia", objModel);
        // }
        #endregion
    }
}