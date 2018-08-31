using Entities.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using MassaNews.Service.Enum;
using MassaNews.Service.Services;
using MassaNews.Portal.Functions;
using MassaNews.Portal.ViewModels;
using MassaNews.Service.Util;
using System.Configuration;
using System.IO;
using System.Web.Script.Serialization;
using IdentityModel.Client;
using System.Net.Http;
using System.Web;
using Microsoft.Owin.Security;
using MassaNews.Portal.Models;
using MassaNews.Service.Models;

namespace MassaNews.Portal.Controllers
{
  [Authorize]
  public class ContaController : Controller
  {
    private IAuthenticationManager Authentication
    {
      get { return Request.GetOwinContext().Authentication; }
    }

    #region Constructor
    public ContaController()
    {
    }
    #endregion

    #region Actions
    [Route("minha-conta")]
    public ActionResult Index()
    {
      if (Session["RedirectUrl"] != null)
      {
        string RedirectUrl = Session["RedirectUrl"].ToString();
        Session["RedirectUrl"] = null;
        return Redirect(RedirectUrl);
      }

      var model = BuildIndexViewModel();
      return View(model);
    }

    [HttpPost]
    [Route("minha-conta")]
    public ActionResult Index(ContaViewModel model)
    {
      model.Usuario.CityId = CookieFx.GetLocationId(Request);

      if (!ModelState.IsValid)
      {
        var vm = BuildIndexViewModel();
        vm.Usuario = model.Usuario;
        return View(vm);
      }

      if (string.IsNullOrEmpty(model.Usuario.PrimeiroNome))
      {
        ModelState.AddModelError("Usuario.PrimeiroNome", "Nome obrigatório!");
        var vm = BuildIndexViewModel();
        vm.Usuario = model.Usuario;
        return View(vm);
      }

      if (!string.IsNullOrEmpty(model.Usuario.DataNascimento))
      {
        DateTime dataValida;
        if (!DateTime.TryParse(model.Usuario.DataNascimento, out dataValida))
        {
          ModelState.AddModelError("Usuario.DataNascimento", "Data de nascimento inválida!");
          var vm = BuildIndexViewModel();
          vm.Usuario = model.Usuario;
          return View(vm);
        }

        if (Convert.ToDateTime(model.Usuario.DataNascimento) > DateTime.Now.AddYears(-16))
        {
          ModelState.AddModelError("Usuario.DataNascimento", "Idade permitida acima de 16 anos!");
          var vm = BuildIndexViewModel();
          vm.Usuario = model.Usuario;
          return View(vm);
        }
      }

      if (!string.IsNullOrEmpty(model.Usuario.Cpf))
      {
        if (!Text.ValidaCpf(model.Usuario.Cpf))
        {
          ModelState.AddModelError("Usuario.Cpf", "CPF inválido!");
          var vm = BuildIndexViewModel();
          vm.Usuario = model.Usuario;
          return View(vm);
        }
      }

      model.Usuario.NoticiasPersonalizadas = model.Usuario.NoticiasPersonalizadasCheckbox > 0;

      try
      {
        CurrentUser.Update(model.Usuario);

        var cookieUserName = Request.Cookies["username"];

        if (cookieUserName == null)
          cookieUserName = new HttpCookie("username");

        cookieUserName.Value = model.Usuario.PrimeiroNome.ToString();
        Response.Cookies.Add(cookieUserName);
      }
      catch (Exception exc)
      {
        ModelState.AddModelError("", exc);
        return View(model);
      }


      return RedirectToAction("Index");

      //ViewBag.ActiveNav = "Minha conta";

      ///* base model defaults */
      //model.Title = "Meus dados - Massa News";
      //model.Description = "Meus dados de cadastro - Massa News";
      //model.Robots = "noindex, nofollow";
      //model.Canonical = $"{Constants.UrlWeb}/minha-conta";

      //return View(model);
    }

    //[Route("minha-conta/cadastro")]
    //public ActionResult Cadastro()
    //{
    //  var model = new HomeIndex();
    //  ViewBag.ActiveNav = "Crie sua conta";

    //  /* base model defaults */
    //  model.Title = "Crie sua conta no Massa News";
    //  model.Description = "Crie sua conta no Massa News";
    //  model.Robots = "noindex, nofollow";
    //  model.Canonical = $"{Constants.UrlWeb}/minha-conta/cadastro";

    //  return View(model);
    //}

    [AllowAnonymous]
    [Route("minha-conta/entrar")]
    public ActionResult Entrar(string acao = "")
    {
      if (!CurrentUser.Valido && !User.Identity.IsAuthenticated)
      {
        if (acao == "perguntar")
        {
          Session["RedirectUrl"] = Request.UrlReferrer.ToString() + "/pergunta";
        }
        else
        {
          Session["RedirectUrl"] = Request.UrlReferrer;
        }
      }

      return RedirectToAction("Index");

      //var model = new HomeIndex();
      //ViewBag.ActiveNav = "Entrar";

      ///* base model defaults */
      //model.Title = "Acesse sua conta no Massa News";
      //model.Description = "Acesse sua conta no Massa News";
      //model.Robots = "noindex, nofollow";
      //model.Canonical = $"{Constants.UrlWeb}/minha-conta/entrar";

      //return View(model);
    }

    [AllowAnonymous]
    [Route("minha-conta/me")]
    public JsonResult Me()
    {
      //return Json(new { Name = CurrentUser.Data.PrimeiroNome }, JsonRequestBehavior.AllowGet);
      if (CurrentUser.Valido)
      {
        return Json(new { Name = CurrentUser.Data.PrimeiroNome, Status = true }, JsonRequestBehavior.AllowGet);
      }
      else
      {
        return Json(new { Name = "", Status = false }, JsonRequestBehavior.AllowGet);
      }

      //return Json("forbidden", JsonRequestBehavior.AllowGet);
    }

    //[Route("minha-conta/recuperar-senha")]
    //public ActionResult Senha()
    //{
    //  var model = new HomeIndex();
    //  ViewBag.ActiveNav = "Recuperar senha";

    //  /* base model defaults */
    //  model.Title = "Recuperar senha - Massa News";
    //  model.Description = "Recuperar sua senha no Massa News";
    //  model.Robots = "noindex, nofollow";
    //  model.Canonical = $"{Constants.UrlWeb}/minha-conta/recuperar-senha";

    //  return View(model);
    //}

    [Route("minha-conta/interacoes")]
    public ActionResult Interacoes()
    {
      var usuario = Usuario.Load(CurrentUser.Data.SubjectId);
      //var usuario = Usuario.Load("5ad361b3a5dd4855ec5c0336");

      var model = new ContaInteracoesViewModel
      {
        /* base model defaults */
        Title = "Interações - Massa News",
        Description = "Interações - Massa News",
        Robots = "noindex, nofollow",
        Canonical = $"{Constants.UrlWeb}/minha-conta/interacoes",

        Usuario = CurrentUser.Data,
        Perguntas = ComunidadeService.GetAllPerguntasByUsuarioId(usuario.Id).ToList(),
        Respostas = ComunidadeService.GetAllRespostasByUsuarioId(usuario.Id).ToList(),
        Comentarios = ComunidadeService.GetAllComentariosByUsuarioId(usuario.Id).ToList()
      };

      ViewBag.ActiveNav = "Minha conta";

      return View(model);
    }

    [Route("minha-conta/sair")]
    public RedirectResult Sair()
    {
      if (Request.Cookies["username"] != null)
      {
        Response.Cookies["username"].Expires = DateTime.Now.AddDays(-1);
      }

      CurrentUser.Unload();
      Session.Clear();
      Session.Abandon();
      Authentication.SignOut("Cookies");
      Authentication.SignOut("oidc");
      return RedirectPermanent("~/");
    }

    [Route("minha-conta/cidades/{uf}")]
    public JsonResult Cidades(string uf)
    {
      var cidades = from c in MunicipioService.GetByUf(uf)
                    select new { Text = c.Nome, Value = c.Codigo };
      return Json(cidades.ToArray(), JsonRequestBehavior.AllowGet);
    }
    #endregion

    #region Private Methods
    private ContaViewModel BuildIndexViewModel()
    {
      var model = new ContaViewModel();
      ViewBag.ActiveNav = "Minha conta";

      /* base model defaults */
      model.Title = "Meus dados - Massa News";
      model.Description = "Meus dados de cadastro - Massa News";
      model.Robots = "noindex, nofollow";
      model.Canonical = $"{Constants.UrlWeb}/minha-conta";
      model.Usuario = CurrentUser.Data;

      model.Estados = new List<SelectListItem>
      {
        new SelectListItem { Value ="AC", Text = "Acre" },
        new SelectListItem { Value ="AL", Text = "Alagoas" },
        new SelectListItem { Value ="AP", Text = "Amapá" },
        new SelectListItem { Value ="AM", Text = "Amazonas" },
        new SelectListItem { Value ="BA", Text = "Bahia" },
        new SelectListItem { Value ="CE", Text = "Ceará" },
        new SelectListItem { Value ="DF", Text = "Distrito Federal" },
        new SelectListItem { Value ="ES", Text = "Espírito Santo" },
        new SelectListItem { Value ="GO", Text = "Goiás" },
        new SelectListItem { Value ="MA", Text = "Maranhão" },
        new SelectListItem { Value ="MT", Text = "Mato Grosso" },
        new SelectListItem { Value ="MS", Text = "Mato Grosso do Sul" },
        new SelectListItem { Value ="MG", Text = "Minas Gerais" },
        new SelectListItem { Value ="PA", Text = "Pará" },
        new SelectListItem { Value ="PB", Text = "Paraíba" },
        new SelectListItem { Value ="PR", Text = "Paraná" },
        new SelectListItem { Value ="PE", Text = "Pernambuco" },
        new SelectListItem { Value ="PI", Text = "Piauí" },
        new SelectListItem { Value ="RJ", Text = "Rio de Janeiro" },
        new SelectListItem { Value ="RN", Text = "Rio Grande do Norte" },
        new SelectListItem { Value ="RS", Text = "Rio Grande do Sul" },
        new SelectListItem { Value ="RO", Text = "Rondônia" },
        new SelectListItem { Value ="RR", Text = "Roraima" },
        new SelectListItem { Value ="SC", Text = "Santa Catarina" },
        new SelectListItem { Value ="SP", Text = "São Paulo" },
        new SelectListItem { Value ="SE", Text = "Sergipe" },
        new SelectListItem { Value ="TO", Text = "Tocantins" },
      };

      if (!string.IsNullOrEmpty(model.Usuario.Estado) && !string.IsNullOrEmpty(model.Usuario.Cidade))
      {
        model.Cidades = (from c in MunicipioService.GetByUf(model.Usuario.Estado)
                         select new SelectListItem
                         {
                           Value = c.Codigo.ToString(),
                           Text = c.Nome,
                         }).ToList();
      }
      else
      {
        model.Cidades = new List<SelectListItem>();
      }

      model.Sexos = new List<SelectListItem>
      {
        new SelectListItem { Value ="M", Text = "Masculino" },
        new SelectListItem { Value ="F", Text = "Feminino" },
      };

      return model;
    }
    #endregion
  }
}