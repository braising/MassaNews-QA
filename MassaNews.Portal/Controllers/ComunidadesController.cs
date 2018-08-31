using Entities.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MassaNews.Service.Enum;
using MassaNews.Service.Services;
using MassaNews.Portal.ViewModels;
using MassaNews.Service.Util;
using MassaNews.Portal.Models.Comunidade;
using MassaNews.Service.Models;
using Hangfire;
using MassaNews.Portal.Models;
using static MassaNews.Portal.ViewModels.ComunidadeTopicoRespostaViewModel;

namespace MassaNews.Portal.Controllers
{
  public class ComunidadesController : Controller
  {
    #region Constructor
    public ComunidadesController()
    { }
    #endregion

    #region Actions
    [Route("comunidades-virtuais/{slugtopico}")]
    public ActionResult Topicos(string slugtopico)
    {
      var objTopico = ComunidadeService.GetComunidadeTopicoBySlug(slugtopico);

      var lstPerguntas = new List<Perguntas>();

      foreach (var pergunta in ComunidadeService.GetAllPerguntasByTopicoAndStatus(objTopico.Id, StatusComunidade.Publicada.Id, 0))
      {
        Perguntas obj = new Perguntas
        {
          Id = pergunta.Id,
          Pergunta = pergunta.Pergunta,
          Descricao = pergunta.Descricao,
          Slug = pergunta.Slug,
          Usuario = pergunta.Usuario.Nome,
          Data = pergunta.Data,
          QtdResposta = ComunidadeService.CountRepostaByPergunta(pergunta.Id)
        };

        lstPerguntas.Add(obj);
      }

      var model = new ComunidadeTopicoViewModel
      {
        /* base model defaults */
        Title = $"{objTopico.Titulo} - Comunidades Virtuais Negócios da Terra",
        Description = $"{objTopico.Descricao}",
        Robots = "index, follow",
        Canonical = $"{Constants.UrlWeb}/comunidades-virtuais/{slugtopico}",

        /* model comunidade topico perguntas */
        SlugTopico = slugtopico,
        Nome = objTopico.Titulo,
        Descricao = objTopico.Descricao,
        ComunidadeEspecialistas = ComunidadeService.GetEspecialistasByTopico(objTopico.Id).ToList(),
        ComunidadeVideos = ComunidadeService.GetVideosByTopico(objTopico.Id).ToList(),
        ComunidadePerguntas = lstPerguntas
      };

      //Set viewbag's
      ViewBag.ActiveNav = "Negócios da Terra";
      ViewBag.Editorial = Service.Models.Editorial.Load(EditorialEnum.NegociosDaTerra.GetHashCode());
      ViewBag.ExibirLogo = true;
      ViewBag.LinkActiveNav = "/negocios-da-terra";

      return View(model);
    }

    [HttpGet]
    [Route("comunidades-virtuais/{slugtopico}/pergunta")]
    public ActionResult Pergunta(string slugtopico)
    {
      if (CurrentUser.Data == null)
      {
        return RedirectToAction("Topicos");
      }

      var objTopico = ComunidadeService.GetComunidadeTopicoBySlug(slugtopico);

      var model = new ComunidadeTopicoPerguntaViewModel
      {
        /* base model defaults */
        Title = "Enviar pergunta sobre " + objTopico.Titulo + " - Comunidades Virtuais Negócios da Terra",
        Description = "Enviar pergunta sobre " + objTopico.Titulo + " - Comunidades Virtuais Negócios da Terra",
        Robots = "index, follow",
        Canonical = $"{Constants.UrlWeb}/comunidades-virtuais/{slugtopico}/pergunta",

        TopicoId = objTopico.Id,
        TopicoTitulo = objTopico.Titulo,
        TopicoDescricao = objTopico.Descricao,
        TopicoSlug = slugtopico,
        CriadoPergunta = false
      };

      if (Session["Aviso1"] != null)
      {
        model.CriadoPergunta = true;
        model.Aviso1 = Session["Aviso1"].ToString();
        Session["Aviso1"] = null;
      }

      if (Session["Aviso2"] != null)
      {
        model.Aviso2 = Session["Aviso2"].ToString();
        Session["Aviso2"] = null;
      }

      //Set viewbag's
      ViewBag.ActiveNav = "Negócios da Terra";
      ViewBag.Editorial = Service.Models.Editorial.Load(EditorialEnum.NegociosDaTerra.GetHashCode());
      ViewBag.ExibirLogo = true;
      ViewBag.LinkActiveNav = "/negocios-da-terra";

      return View(model);
    }

    [HttpPost]    
    [Route("comunidades-virtuais/{slugtopico}/pergunta")]
    public ActionResult Pergunta(TopicoPergunta pergunta)
    {
      try
      {
        var usuario = Usuario.Load(CurrentUser.Data.SubjectId);
        var topico = ComunidadeTopico.Load(pergunta.Topico);

        var objPergunta = new ComunidadePergunta
        {
          Pergunta = pergunta.Titulo,
          Descricao = pergunta.Descricao,
          Slug = Text.SetFriendlyName(pergunta.Titulo), // ConvertToSlug(pergunta.Titulo),
          Data = DateTime.Now,
          StatusPublicacaoId = topico.AtivaModeracao ? StatusComunidade.Pendente.Id : StatusComunidade.Publicada.Id,
          ComunidadeTopicoId = pergunta.Topico,
          UsuarioId = usuario.Id
        };

        objPergunta.Save();

        Session["Aviso1"] = "Pergunta registrada com sucesso!";
        Session["Aviso2"] = topico.AtivaModeracao ? "Estamos analisando a sua pergunta. Em breve ela será publicada!" : "Sua pergunta já foi publicada.";

        if (topico.AtivaModeracao)
        {
          var body = $@"<!DOCTYPE html>
                    <html lang='pt-br'>
                    <head>
                      <meta charset='UTF-8'>
                      <title>Comunidades Virtuais - Negócio da Terra - {topico.Titulo}</title>
                    </head>
                    <body>
                      <table bgcolor='#ffffff' width='600' border='0' cellpadding='0' cellspacing='0' align='center' style='font-family: Arial, sans-serif; font-size: 14px; color: #4f4f4f;'>
                        <tr>
                          <td style='text-align: center;'>
                            <a href='https://massanews.com/negocios-da-terra' target='_blank'><img src='https://cdn.massanews.com/static/images/logos/negocios-da-terra-horiz.png' alt='Negócio da Terra'></a>
                            <h3>Pergunta criada aguardando aprovação:</h3>
                            <h4>{objPergunta.Pergunta}</h4>
                            <h5>{objPergunta.Descricao}</h5>
                          </td>
                        </tr>
                      </table>
                    </body>
                    </html>";

          foreach (var email in topico.EmailsNotificacao.Split(';'))
          {
            BackgroundJob.Enqueue(() => ToolService.SendEmail($"Pergunta criada - Comunidades Virtuais - {topico.Titulo} ", body, email));
          }
        }

        return RedirectToAction("Pergunta");
      }
      catch (Exception)
      {
        var topico = ComunidadeTopico.Load(pergunta.Topico);

        var model = new ComunidadeTopicoPerguntaViewModel
        {
          TopicoTitulo = topico.Titulo,
          TopicoDescricao = topico.Descricao,
          TopicoSlug = topico.Slug,
          CriadoPergunta = true,
          Aviso1 = "Ops! Ocorreu um erro ao enviar a sua pergunta.",
          Aviso2 = "Por favor, tente novamente!"
        };

        return View(model);
      }

      //return Json("ok");
    }

    [HttpGet]
    [Route("comunidades-virtuais/{slugtopico}/{slugpergunta}.html")]
    public ActionResult TopicosRespostas(string slugtopico, string slugpergunta)
    {
      Usuario usuario = new Usuario();

      if (CurrentUser.Data != null)
        usuario = Usuario.Load(CurrentUser.Data.SubjectId);

      var objTopico = ComunidadeService.GetComunidadeTopicoBySlug(slugtopico);
      var objPergunta = ComunidadeService.GetComunidadePerguntaBySlug(slugpergunta);

      var lstResposta = new List<Respostas>();

      foreach (var resposta in ComunidadeService.GetAllRespostasByPerguntaAndStatus(objPergunta.Id, StatusComunidade.Publicada.Id, 0))
      {
        Respostas objResposta = new Respostas
        {
          Id = resposta.Id,
          Resposta = resposta.Resposta,
          Usuario = resposta.Usuario.Nome,
          IsEspecialista = resposta.Usuario.IsEspecialista,
          Data = resposta.Data,
          UserCurtida = usuario.Id > 0 ? ComunidadeService.RespostaUserCurtida(usuario.Id, resposta.Id, true) : false,
          CountCurtida = ComunidadeService.CountCurtidaResposta(resposta.Id, true),
          UserNaoCurtida = usuario.Id > 0 ? ComunidadeService.RespostaUserCurtida(usuario.Id, resposta.Id, false) : false,
          CountNaoCurtida = ComunidadeService.CountCurtidaResposta(resposta.Id, false)
        };

        var lstComentario = new List<Comentarios>();

        foreach (var comentario in ComunidadeService.GetAllComentariosByRespostaAndStatus(resposta.Id, StatusComunidade.Publicada.Id, 0))
        {
          Comentarios objComentario = new Comentarios
          {
            Id = comentario.Id,
            Comentario = comentario.Comentario,
            Usuario = comentario.Usuario.Nome,
            IsEspecialista = comentario.Usuario.IsEspecialista,
            Data = comentario.Data,
            UserCurtida = usuario.Id > 0 ? ComunidadeService.ComentarioUserCurtida(usuario.Id, comentario.Id, true) : false,
            CountCurtida = ComunidadeService.CountCurtidaComentario(comentario.Id, true),
            UserNaoCurtida = usuario.Id > 0 ? ComunidadeService.ComentarioUserCurtida(usuario.Id, comentario.Id, false) : false,
            CountNaoCurtida = ComunidadeService.CountCurtidaComentario(comentario.Id, false),
          };

          lstComentario.Add(objComentario);
        }

        objResposta.Comentarios = lstComentario;

        lstResposta.Add(objResposta);
      }

      var model = new ComunidadeTopicoRespostaViewModel
      {
        /* base model defaults */
        Title = $"{objTopico.Titulo} - Comunidades Virtuais Negócios da Terra",
        Description = "",
        Robots = "index, follow",
        Canonical = $"{Constants.UrlWeb}/comunidades-virtuais/{slugtopico}/{slugpergunta}",

        Pergunta = new Perguntas
        {
          Id = objPergunta.Id,
          Topico = objTopico.Titulo,
          Pergunta = objPergunta.Pergunta,
          Descricao = objPergunta.Descricao,
          Data = objPergunta.Data,
          Usuario = objPergunta.Usuario.Nome,
          Slug = slugtopico,
          QtdResposta = ComunidadeService.CountRepostaByPergunta(objPergunta.Id)
        },
        ComunidadeRespostas = lstResposta,
        ComunidadeEspecialistas = ComunidadeService.GetEspecialistasByTopico(objTopico.Id).ToList(),
        ComunidadeVideos = ComunidadeService.GetVideosByTopico(objTopico.Id).ToList()
      };      

      //Set viewbag's
      ViewBag.ActiveNav = "Negócios da Terra";
      ViewBag.Editorial = Service.Models.Editorial.Load(EditorialEnum.NegociosDaTerra.GetHashCode());
      ViewBag.ExibirLogo = true;
      ViewBag.LinkActiveNav = "/negocios-da-terra";

      return View(model);
    }    

    [HttpPost]
    [Route("comunidades-virtuais/{slugtopico}/{slugpergunta}.html")]
    public ActionResult CriarResposta(int perguntaId, string resposta)
    {
      var usuario = Usuario.Load(CurrentUser.Data.SubjectId);
      var pergunta = ComunidadePergunta.Load(perguntaId);
      var topico = ComunidadeTopico.Load(pergunta.ComunidadeTopicoId);

      var objResposta = new ComunidadeResposta
      {
        Resposta = resposta,
        Data = DateTime.Now,
        StatusPublicacaoId = topico.AtivaModeracao ? StatusComunidade.Pendente.Id : StatusComunidade.Publicada.Id,
        UsuarioId = usuario.Id,
        ComunidadePerguntaId = pergunta.Id
      };

      objResposta.Save();

      if (topico.AtivaModeracao)
      {
        var body = $@"<!DOCTYPE html>
                    <html lang='pt-br'>
                    <head>
                      <meta charset='UTF-8'>
                      <title>Comunidades Virtuais - Negócio da Terra - {topico.Titulo}</title>
                    </head>
                    <body>
                      <table bgcolor='#ffffff' width='600' border='0' cellpadding='0' cellspacing='0' align='center' style='font-family: Arial, sans-serif; font-size: 14px; color: #4f4f4f;'>
                        <tr>
                          <td style='text-align: center;'>
                            <a href='https://massanews.com/negocios-da-terra' target='_blank'><img src='https://cdn.massanews.com/static/images/logos/negocios-da-terra-horiz.png' alt='Negócio da Terra'></a>
                            <h3>Resposta criada aguardando aprovação:</h3>
                            <h4>{resposta}</h4>
                          </td>
                        </tr>
                      </table>
                    </body>
                    </html>";

        foreach (var item in topico.EmailsNotificacao.Split(';'))
        {
          BackgroundJob.Enqueue(() => ToolService.SendEmail($"Resposta criada - Comunidades Virtuais - {topico.Titulo} ", body, item));
        }
      }

      //return RedirectToRoute("TopicosRespostas", topico.Slug, pergunta.Slug);

      return RedirectToRoute(new
      {
        controller = "Comunidades",
        action = "TopicosRespostas",
        slugtopico = topico.Slug,
        slugpergunta = pergunta.Slug
      });

      //return Json("ok");
    }

    [HttpPost]
    [Route("comunidades/criarcomentario/{respostaId}")]
    public ActionResult CriarComentario(int respostaId, string comentario)
    {
      var usuario = Usuario.Load(CurrentUser.Data.SubjectId);
      var resposta = ComunidadeResposta.Load(respostaId);
      var topico = ComunidadeTopico.Load(resposta.ComunidadePergunta.ComunidadeTopicoId);

      var objComentario = new ComunidadeComentario
      {
        Comentario = comentario,
        Data = DateTime.Now,
        StatusPublicacaoId = topico.AtivaModeracao ? StatusComunidade.Pendente.Id : StatusComunidade.Publicada.Id,
        UsuarioId = usuario.Id,
        ComunidadeRespostaId = resposta.Id
      };

      objComentario.Save();

      if (topico.AtivaModeracao)
      {
        var body = $@"<!DOCTYPE html>
                    <html lang='pt-br'>
                    <head>
                      <meta charset='UTF-8'>
                      <title>Comunidades Virtuais - Negócio da Terra - {topico.Titulo}</title>
                    </head>
                    <body>
                      <table bgcolor='#ffffff' width='600' border='0' cellpadding='0' cellspacing='0' align='center' style='font-family: Arial, sans-serif; font-size: 14px; color: #4f4f4f;'>
                        <tr>
                          <td style='text-align: center;'>
                            <a href='https://massanews.com/negocios-da-terra' target='_blank'><img src='https://cdn.massanews.com/static/images/logos/negocios-da-terra-horiz.png' alt='Negócio da Terra'></a>
                            <h3>Comentário criado aguardando aprovação:</h3>
                            <h4>{comentario}</h4>
                          </td>
                        </tr>
                      </table>
                    </body>
                    </html>";

        foreach (var item in topico.EmailsNotificacao.Split(';'))
        {
          BackgroundJob.Enqueue(() => ToolService.SendEmail($"Comentário criado - Comunidades Virtuais - {topico.Titulo} ", body, item));
        }
      }

      //return RedirectToAction("TopicosRespostas",topico.Slug,resposta.ComunidadePergunta.Slug);

      return RedirectToRoute(new
      {
        controller = "Comunidades",
        action = "TopicosRespostas",
        slugtopico = topico.Slug,
        slugpergunta = resposta.ComunidadePergunta.Slug
      });

      //return Json("ok");
    }

    [HttpPost]
    [Route("comunidades/curtir")]
    public JsonResult Curtir(string tipo, int id, bool curtida)
    {
      var usuario = Usuario.Load(CurrentUser.Data.SubjectId);

      int sucesso = 0;
      int countCurtida = 0;
      int countNaoCurtida = 0;

      if (tipo == "r")
      {
        var obj = ComunidadeRespostaCurtida.CurtidaByUsuario(id, usuario.Id);        

        if (obj == null)
        {
          var objCurtida = new ComunidadeRespostaCurtida()
          {
            ComunidadeRespostaId = id,
            Data = DateTime.Now,
            Curtida = curtida,
            UsuarioId = usuario.Id
          };

          objCurtida.Save();

          sucesso = 1;
        }
        else
        {
          if (obj.Curtida != curtida)
          {
            obj.Curtida = curtida;
            obj.Data = DateTime.Now;

            obj.Save();

            sucesso = 1;
          }
          else
          {
            obj.Delete();

            sucesso = 0;
          }
        }

        countCurtida = ComunidadeService.CountCurtidaResposta(id, true);
        countNaoCurtida = ComunidadeService.CountCurtidaResposta(id, false);
      }
      else if (tipo == "c")
      {
        var obj = ComunidadeComentarioCurtida.CurtidaByUsuario(id, usuario.Id);

        if (obj == null)
        {
          var objCurtida = new ComunidadeComentarioCurtida()
          {
            ComunidadeComentarioId = id,
            Data = DateTime.Now,
            Curtida = curtida,
            UsuarioId = usuario.Id
          };

          objCurtida.Save();

          sucesso = 1;
        }
        else
        {
          if (obj.Curtida != curtida)
          {
            obj.Curtida = curtida;
            obj.Data = DateTime.Now;

            obj.Save();

            sucesso = 1;
          }
          else
          {
            obj.Delete();

            sucesso = 0;
          }
        }

        countCurtida = ComunidadeService.CountCurtidaComentario(id, true);
        countNaoCurtida = ComunidadeService.CountCurtidaComentario(id, false);
      }

      return Json(new {
        Sucesso = sucesso,
        Tipo = tipo,
        Id = id,
        Curtida = curtida,
        CountCurtida = countCurtida,
        CountNaoCurtida = countNaoCurtida,
      });
    }
    #endregion

    #region Private Methods


    #endregion
  }
}