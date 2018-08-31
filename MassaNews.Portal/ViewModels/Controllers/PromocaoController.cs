using MassaNews.Service.Enum;
using MassaNews.Service.Models;
using MassaNews.Service.Util;
using System;
using System.Linq;
using System.Web.Mvc;
using MassaNews.Portal.ViewModels;
using System.Collections.Generic;
using System.IO;

namespace MassaNews.Portal.Controllers
{
  public class PromocaoController : BaseController
  {
    [Route("natal-de-luz/{id?}")]
    public ActionResult Index(int? id)
    {
      int promocaoId = 2;
      var promocao = Promocao.Load(promocaoId);

      var orderBy = string.Empty;

      if (promocao.DestaqueDefault == DestaqueInscricaoStatus.MaisRecentes.Id)
        orderBy = "data";
      else if (promocao.DestaqueDefault == DestaqueInscricaoStatus.MaisVotados.Id)
        orderBy = "votos";
      else if (promocao.DestaqueDefault == DestaqueInscricaoStatus.Finalistas.Id)
        orderBy = "posicao";

      var lstInscricoes = InscricaoNatalDeLuz.GetAllGetAllByStatus(promocaoId, 0, promocao.DestaqueDefault == DestaqueInscricaoStatus.Finalistas.Id ? InscricaoStatus.Finalista.Id : InscricaoStatus.Aprovado.Id, 1 , orderBy).ToList();

      InscricaoNatalDeLuz inscriptionNatalDeLuz = null;

      if (id.HasValue)
      {
        inscriptionNatalDeLuz = InscricaoNatalDeLuz.Load(id.Value);

        if (inscriptionNatalDeLuz != null && inscriptionNatalDeLuz.StatusInscricaoId == InscricaoStatus.Aprovado.Id)
          lstInscricoes.Insert(0, inscriptionNatalDeLuz);
      }

      var model = new PromocaoNatalDeLuzCasaIndex
      {
        //Base
        Title = inscriptionNatalDeLuz == null ? "Concurso Cultural Natal de Luz de Curitiba" : $"{inscriptionNatalDeLuz.Titulo} - Concurso Cultural Natal de Luz de Curitiba",
        Description = inscriptionNatalDeLuz == null ? "Uma das épocas mais bonitas do ano está chegando e que tal começar a decorar a sua casa para o Natal?! A sua casa ou rua pode ser eleita a mais bem decorada de Curitiba. Participe!" : $"Foto '{inscriptionNatalDeLuz.Titulo}' de {inscriptionNatalDeLuz.Nome}. Vote para escolher os finalistas do Concurso Cultural Natal de Luz de Curitiba.",
        Robots = "index, follow",
        Canonical = $"{Constants.UrlWeb}/natal-de-luz",
        ImgOpenGraph = $"{Constants.UrlWeb}/content/images/landing/natal-de-luz/avatar.jpg",
        //Model
        pages = Inscricao.CountPages(promocao.DestaqueDefault == DestaqueInscricaoStatus.Finalistas.Id ? InscricaoStatus.Finalista.Id : InscricaoStatus.Aprovado.Id),
        Inscricoes = lstInscricoes.Take(20),
        Destaque = promocao.DestaqueDefault,
        ExibirFinalistas = promocao.ExibirFinalistas,
        Status = promocao.Status
      };

      //ViewBag
      ViewBag.ActiveNav = "Natal de Luz";
      //ViewBag.Editorial = Editorial.Load(EditorialEnum.WhereCuritiba.GetHashCode());
      //ViewBag.ExibirLogo = true;
      ViewBag.ConcursoAberto = promocao.Status;

      return View(model);
    }

    [Route("natal-de-luz/rua/{id?}")]
    public ActionResult IndexRua(int? id)
    {
      int promocaoId = 2;
      var promocao = Promocao.Load(promocaoId);

      var orderBy = string.Empty;

      if (promocao.DestaqueDefault == DestaqueInscricaoStatus.MaisRecentes.Id)
        orderBy = "data";
      else if (promocao.DestaqueDefault == DestaqueInscricaoStatus.MaisVotados.Id)
        orderBy = "votos";
      else if (promocao.DestaqueDefault == DestaqueInscricaoStatus.Finalistas.Id)
        orderBy = "posicao";

      var lstInscricoes = InscricaoNatalDeLuz.GetAllGetAllByStatus(promocaoId, 0, promocao.DestaqueDefault == DestaqueInscricaoStatus.Finalistas.Id ? InscricaoStatus.Finalista.Id : InscricaoStatus.Aprovado.Id, 2, orderBy).ToList();

      InscricaoNatalDeLuz inscriptionNatalDeLuz = null;

      if (id.HasValue)
      {
        inscriptionNatalDeLuz = InscricaoNatalDeLuz.Load(id.Value);

        if (inscriptionNatalDeLuz != null && inscriptionNatalDeLuz.StatusInscricaoId == InscricaoStatus.Aprovado.Id)
          lstInscricoes.Insert(0, inscriptionNatalDeLuz);
      }

      var model = new PromocaoNatalDeLuzRuaIndex
      {
        //Base
        Title = inscriptionNatalDeLuz == null ? "Concurso Cultural Natal de Luz de Curitiba" : $"{inscriptionNatalDeLuz.Titulo} - Concurso Cultural Natal de Luz de Curitiba",
        Description = inscriptionNatalDeLuz == null ? "Uma das épocas mais bonitas do ano está chegando e que tal começar a decorar a sua casa para o Natal?! A sua casa ou rua pode ser eleita a mais bem decorada de Curitiba. Participe!" : $"Foto '{inscriptionNatalDeLuz.Titulo}' de {inscriptionNatalDeLuz.Nome}. Vote para escolher os finalistas do Concurso Cultural Natal de Luz de Curitiba.",
        Robots = "index, follow",
        Canonical = $"{Constants.UrlWeb}/natal-de-luz",
        ImgOpenGraph = $"{Constants.UrlWeb}/content/images/landing/natal-de-luz/avatar.jpg",
        //Model
        pages = Inscricao.CountPages(promocao.DestaqueDefault == DestaqueInscricaoStatus.Finalistas.Id ? InscricaoStatus.Finalista.Id : InscricaoStatus.Aprovado.Id),
        Inscricoes = lstInscricoes.Take(20),
        Destaque = promocao.DestaqueDefault,
        ExibirFinalistas = promocao.ExibirFinalistas,
        Status = promocao.Status
      };

      //ViewBag
      ViewBag.ActiveNav = "Natal de Luz";
      //ViewBag.Editorial = Editorial.Load(EditorialEnum.WhereCuritiba.GetHashCode());
      //ViewBag.ExibirLogo = true;
      ViewBag.ConcursoAberto = promocao.Status;

      return View(model);
    }

    [HttpGet]
    [Route("natal-de-luz/enviar")]
    public ActionResult Enviar()
    {
      var model = new PromocaoEnviar();

      ViewBag.ActiveNav = "Natal de Luz";

      /* base model defaults */
      model.Title = "Envie sua foto para o Concurso Cultural Natal de Luz de Curitiba";
      model.Description = "Uma das épocas mais bonitas do ano está chegando e que tal começar a decorar a sua casa para o Natal?! A sua casa ou rua pode ser eleita a mais bem decorada de Curitiba. Envie sua foto e Participe!";
      model.Robots = "index, follow";
      model.Canonical = $"{Constants.UrlWeb}/natal-de-luz";
      model.ImgOpenGraph = $"{Constants.UrlWeb}/content/images/landing/natal-de-luz/avatar.jpg";

      int promocaoId = 2;
      var promocao = Promocao.Load(promocaoId);

      //ViewBag.Editorial = Editorial.Load(4);
      //ViewBag.ExibirLogo = true;
      ViewBag.ConcursoAberto = promocao.Status;

      return View(model);
    }

    [HttpPost]
    [Route("natal-de-luz/enviar")]
    public ActionResult EnviarPost(PromocaoEnviar model)
    {
      try
      {
        int promocaoId = 2;

        var objInscricao = new InscricaoNatalDeLuz
        {
          PromocaoId = promocaoId,
          Nome = model.Nome,
          Email = model.Email,
          Telefone = model.Telefone,
          DtNascimento = model.Nascimento,
          Cidade = model.Cidade,
          Titulo = model.Titulo,
          //ImagemCasaTb = filesCasa[0],
          //ImagemCasaOr = filesCasa[1],
          StatusInscricaoId = 1,
          DtCadastro = DateTime.Now,
          OptInNews = model.OptInNews,
          Cpf = model.Cpf,
          //ImagemRuaTb = filesRua[0],
          //ImagemRuaOr = filesRua[1],
          //TipoInscricaoId = 1
          //ComprovanteResidencia = ""
        };        
        
        var imagemComprovante = model.ImagemComprovante.InputStream;

        if (model.TipoCasa)
        {
          //model.ImagemCasa.SaveAs(@"D:\Esfera\MassaNews.Portal\MassaNews.Portal\bin\bufu.jpg");
          //model.ImagemComprovante.SaveAs(@"D:\Esfera\MassaNews.Portal\MassaNews.Portal\bin\bufu2.jpg");
          var imagemCasa = model.ImagemCasa.InputStream;
          var filesCasa = FileManager.UploadSingleImageToPromotion(imagemCasa, "\\uploads\\promocao\\natal-de-luz\\casa");
          objInscricao.ImagemCasaTb = filesCasa[0];
          objInscricao.ImagemCasaOr = filesCasa[1];
          objInscricao.ComprovanteResidencia = FileManager.UploadSingleFilePromotion(model.ImagemComprovante, "\\uploads\\promocao\\natal-de-luz\\casa",null,true);
          objInscricao.TipoInscricaoId = 1;
          //objInscricao. = filesCasa;
          //objInscricao.ImagemCasaOr = filesCasa;

          //filesComprovante = FileManager.UploadSingleImageToPromotion(imagemComprovante, "\\uploads\\promocao\\natal-de-luz\\casa");
        }
        else if (model.TipoRua)
        {
          var imagemRua = model.ImagemRua.InputStream;
          var filesRua = FileManager.UploadSingleImageToPromotion(imagemRua, "\\uploads\\promocao\\natal-de-luz\\rua");
          objInscricao.ImagemRuaTb = filesRua[0];
          objInscricao.ImagemRuaOr = filesRua[1];
          objInscricao.ComprovanteResidencia = FileManager.UploadSingleFilePromotion(model.ImagemComprovante, "\\uploads\\promocao\\natal-de-luz\\rua",null,true);
          objInscricao.TipoInscricaoId = 2;
        }

        objInscricao.Save();

        //newsletter
        if (model.OptInNews)
        {
          var objNewsletter = new Newsletter
          {
            Nome = model.Nome,
            Email = model.Email,
            CidadeId = 12
          };
          objNewsletter.Subscribe();
        }

        return Json("ok");
      }
      catch (Exception ex)
      {
        Response.StatusCode = 500;
        return Json(new { Error = ex.Message });
      }
    }

    public static Stream Base64ToStream(string base64String)
    {
      if (base64String.Contains("data:image/"))
        base64String = base64String.Split(',')[1];

      // Convert base 64 string to Stream
      var imageBytes = Convert.FromBase64String(base64String);

      return new MemoryStream(imageBytes, 0, imageBytes.Length);
    }

    [Route("natal-de-luz/premiacao")]
    public ActionResult Premiacao()
    {
      var model = new HomeIndex
      {
        Title = "Premiação do Concurso Cultural Natal de Luz de Curitiba",
        Description = "Confira a Premiação do Concurso Cultural Natal de Luz de Curitiba. Envie sua foto mostrando 'Qual casa/rua mais decorada de Curitiba;'. Participe!",
        Robots = "index, follow",
        Canonical = $"{Constants.UrlWeb}/natal-de-luz/premiacao",
        ImgOpenGraph = $"{Constants.UrlWeb}/content/images/landing/natal-de-luz/avatar.jpg"
      };
      int promocaoId = 2;
      var promocao = Promocao.Load(promocaoId);

      ViewBag.ActiveNav = "Natal de Luz";
      //ViewBag.Editorial = Editorial.Load(EditorialEnum.WhereCuritiba.GetHashCode());
      //ViewBag.ExibirLogo = true;
      ViewBag.ConcursoAberto = promocao.Status;

      return View(model);
    }

    [Route("natal-de-luz/regulamento")]
    public ActionResult Regulamento()
    {

      var model = new HomeIndex
      {
        Title = "Regulamento do Concurso Cultural Natal de Luz de Curitiba",
        Description = "Confira o Regulamento do Concurso Cultural Natal de Luz de Curitiba. Envie sua foto mostrando 'Qual é a casa/rua mais decorada de Curitiba para você?'. Participe!",
        Robots = "index, follow",
        Canonical = $"{Constants.UrlWeb}/natal-de-luz/regulamento",
        ImgOpenGraph = $"{Constants.UrlWeb}/content/images/landing/natal-de-luz/avatar.jpg"
      };

      var promocao = Promocao.Load(2);

      ViewBag.ActiveNav = "Natal de Luz";
      //ViewBag.Editorial = Editorial.Load(EditorialEnum.WhereCuritiba.GetHashCode());
      //ViewBag.ExibirLogo = true;
      ViewBag.ConcursoAberto = promocao.Status;

      return View(model);
    }

    #region Json

    [HttpGet]
    [AllowAnonymous]
    [Route("natal-de-luz/GeLatestImagesCasa")]
    public JsonResult GetLastestImages(int page, string orderBy)
    {
      int promocaoId = 2;

      var data = InscricaoNatalDeLuz.GetAllGetAllByStatus(promocaoId, page, orderBy.Equals("posicao") ? InscricaoStatus.Finalista.Id : InscricaoStatus.Aprovado.Id, 1, orderBy).Select(s => new
      {
        id = s.Id,
        nome = $"{s.Nome} ({s.QtdeVotos} votos)",
        titulo = s.Titulo,
        imageCasaTb = s.ImagemCasaTbFull,
        imageCasaOr = s.ImagemCasaOrFull
      });

      var pages = InscricaoNatalDeLuz.CountPages(orderBy == "posicao" ? InscricaoStatus.Finalista.Id : InscricaoStatus.Aprovado.Id);

      //return Json(data, JsonRequestBehavior.AllowGet);
      return Json(new
      {
        data = data,
        pages = pages
      }, JsonRequestBehavior.AllowGet);
    }

    [HttpGet]
    [AllowAnonymous]
    [Route("natal-de-luz/GeLatestImagesRua")]
    public JsonResult GetLastestImagesRua(int page, string orderBy)
    {
      int promocaoId = 2;

      var data = InscricaoNatalDeLuz.GetAllGetAllByStatus(promocaoId, page, orderBy.Equals("posicao") ? InscricaoStatus.Finalista.Id : InscricaoStatus.Aprovado.Id, 2, orderBy).Select(s => new
      {
        id = s.Id,
        nome = $"{s.Nome} ({s.QtdeVotos} votos)",
        titulo = s.Titulo,
        imageRuaTb = s.ImagemRuaTbFull,
        imageRuaOr = s.ImagemRuaOrFull
      });

      var pages = InscricaoNatalDeLuz.CountPages(orderBy == "posicao" ? InscricaoStatus.Finalista.Id : InscricaoStatus.Aprovado.Id);

      //return Json(data, JsonRequestBehavior.AllowGet);
      return Json(new
      {
        data = data,
        pages = pages
      }, JsonRequestBehavior.AllowGet);
    }

    [HttpGet]
    [AllowAnonymous]
    [Route("natal-de-luz/voted")]
    public JsonResult GetScore(string facebookId, int imageId)
    {
      if (VotoNatalDeLuz.GetVoteByInscriptionAndFacebookId(imageId, facebookId))
        return Json("ok", JsonRequestBehavior.AllowGet);
      else
        return Json("nok", JsonRequestBehavior.AllowGet);
    }

    [HttpPost]
    [Route("natal-de-luz/voto")]
    public JsonResult VotePost(PromocaoVoto model)
    {
      try
      {
        var voteNatalDeLuz = new VotoNatalDeLuz
        {
          IdFacebook = model.IdFacebook,
          Nome = model.Nome,
          Email = model.Email,
          InscricaoNatalDeLuzId = model.IncricaoId
        };

        voteNatalDeLuz.Save();

        return Json(new { status = "ok", label = $"{voteNatalDeLuz.InscricaoNatalDeLuz.Nome} ({voteNatalDeLuz.InscricaoNatalDeLuz.QtdeVotos} votos)" });
      }
      catch (Exception ex)
      {
        return Json(new { Error = ex.Message });
      }
    }
    #endregion
  }
}

















/*
 * WHERE CURITIBA 15 ANOS
 */


// using MassaNews.Service.Enum;
// using MassaNews.Service.Models;
// using MassaNews.Service.Util;
// using System;
// using System.Linq;
// using System.Web.Mvc;
// using MassaNews.Portal.ViewModels;

// namespace MassaNews.Portal.Controllers
// {
//   public class PromocaoController : BaseController
//   {
//     #region Actions
//     [Route("promocao/where-15-anos/{id?}")]
//     public ActionResult Index(int? id)
//     {

//       var promocao = Promocao.Load(1);

//       var orderBy = string.Empty;

//       if (promocao.DestaqueDefault == DestaqueInscricaoStatus.MaisRecentes.Id)
//         orderBy = "data";
//       else if (promocao.DestaqueDefault == DestaqueInscricaoStatus.MaisVotados.Id)
//         orderBy = "votos";
//       else if (promocao.DestaqueDefault == DestaqueInscricaoStatus.Finalistas.Id)
//         orderBy = "posicao";

//       var lstInscricoes = Inscricao.GetAllGetAllByStatus(0, promocao.DestaqueDefault == DestaqueInscricaoStatus.Finalistas.Id ? InscricaoStatus.Finalista.Id : InscricaoStatus.Aprovado.Id, orderBy).ToList();

//       Inscricao inscription = null;

//       if (id.HasValue)
//       {
//         inscription = Inscricao.Load(id.Value);

//         if (inscription != null && inscription.StatusInscricaoId == InscricaoStatus.Aprovado.Id)
//           lstInscricoes.Insert(0, inscription);
//       }

//       var model = new PromocaoIndex
//       {
//         //Base
//         Title = inscription == null ? "Concurso Cultural - Where Curitiba 15 anos" : $"{inscription.Titulo} - Where Curitiba 15 anos",
//         Description = inscription == null ? "Envie sua foto mostrando 'Qual é a cara de Curitiba para você?' e concorra a 15 experiências incríveis. Participe!" : $"Foto '{inscription.Titulo}' de {inscription.Nome}. Vote para escolher os finalistas!",
//         Robots = "index, follow",
//         Canonical = $"{Constants.UrlWeb}/promocao/where-15-anos",
//         ImgOpenGraph = $"{Constants.UrlWeb}/content/images/where/where-15-anos/avatar.jpg",
//         //Model
//         pages = Inscricao.CountPages(promocao.DestaqueDefault == DestaqueInscricaoStatus.Finalistas.Id ? InscricaoStatus.Finalista.Id : InscricaoStatus.Aprovado.Id),
//         Inscricoes = lstInscricoes.Take(20),
//         Destaque = promocao.DestaqueDefault,
//         ExibirFinalistas = promocao.ExibirFinalistas,
//         Status = promocao.Status
//       };

//       //ViewBag
//       ViewBag.ActiveNav = "Where 15 anos";
//       ViewBag.Editorial = Editorial.Load(EditorialEnum.WhereCuritiba.GetHashCode());
//       ViewBag.ExibirLogo = true;
//       ViewBag.ConcursoAberto = promocao.Status;

//       return View(model);
//     }

//     [HttpGet]
//     [Route("promocao/where-15-anos/enviar")]
//     public ActionResult Enviar()
//     {
//       var model = new PromocaoEnviar();

//       ViewBag.ActiveNav = "Where 15 anos";

//       /* base model defaults */
//       model.Title = "Envie sua foto para o Concurso Cultural - Where Curitiba 15 anos";
//       model.Description = "Envie sua foto mostrando 'Qual é a cara de Curitiba para você?' e concorra a 15 experiências incríveis. Envie e Participe!";
//       model.Robots = "index, follow";
//       model.Canonical = $"{Constants.UrlWeb}/promocao/where-15-anos";
//       model.ImgOpenGraph = $"{Constants.UrlWeb}/content/images/where/where-15-anos/avatar.jpg";

//       var promocao = Promocao.Load(1);

//       ViewBag.Editorial = Editorial.Load(4);
//       ViewBag.ExibirLogo = true;
//       ViewBag.ConcursoAberto = promocao.Status;

//       return View(model);
//     }

//     [HttpPost]
//     [Route("promocao/where-15-anos/enviar")]
//     public ActionResult EnviarPost(PromocaoEnviar model)
//     {
//       try
//       {
//         var image = FileManager.Base64ToStream(model.Imagem);

//         var files = FileManager.UploadSingleImageToPromotion(image, "\\uploads\\promocao\\where-15-anos");

//         var objInscricao = new Inscricao
//         {
//           PromocaoId = 1,
//           Nome = model.Nome,
//           Email = model.Email,
//           Telefone = model.Telefone,
//           DtNascimento = model.Nascimento,
//           Cidade = model.Cidade,
//           Titulo = model.Titulo,
//           ImagemTb = files[0],
//           ImagemOr = files[1],
//           StatusInscricaoId = 1,
//           DtCadastro = DateTime.Now,
//           OptInNews = model.OptInNews
//         };

//         objInscricao.Save();

//         return Json("ok");
//       }
//       catch (Exception ex)
//       {
//         return Json(new { Error = ex.Message });
//       }
//     }

//     [Route("promocao/where-15-anos/premiacao")]
//     public ActionResult Premiacao()
//     {
//       var model = new HomeIndex
//       {
//         Title = "Premiação do Concurso Cultural - Where Curitiba 15 anos",
//         Description = "Confira a Premiação do Concurso Cultural. Envie sua foto mostrando 'Qual é a cara de Curitiba para você?' e concorra a 15 experiências incríveis. Participe!",
//         Robots = "index, follow",
//         Canonical = $"{Constants.UrlWeb}/promocao/where-15-anos/premiacao",
//         ImgOpenGraph = $"{Constants.UrlWeb}/content/images/where/where-15-anos/avatar.jpg"
//       };

//       var promocao = Promocao.Load(1);

//       ViewBag.ActiveNav = "Where 15 anos";
//       ViewBag.Editorial = Editorial.Load(EditorialEnum.WhereCuritiba.GetHashCode());
//       ViewBag.ExibirLogo = true;
//       ViewBag.ConcursoAberto = promocao.Status;

//       return View(model);
//     }

//     [Route("promocao/where-15-anos/regulamento")]
//     public ActionResult Regulamento()
//     {

//       var model = new HomeIndex
//       {
//         Title = "Regulamento do Concurso Cultural - Where Curitiba 15 anos",
//         Description = "Confira o Regulamento do Concurso Cultural. Envie sua foto mostrando 'Qual é a cara de Curitiba para você?' e concorra a 15 experiências incríveis. Participe!",
//         Robots = "index, follow",
//         Canonical = $"{Constants.UrlWeb}/promocao/where-15-anos/regulamento",
//         ImgOpenGraph = $"{Constants.UrlWeb}/content/images/where/where-15-anos/avatar.jpg"
//       };

//       var promocao = Promocao.Load(1);

//       ViewBag.ActiveNav = "Where 15 anos";
//       ViewBag.Editorial = Editorial.Load(EditorialEnum.WhereCuritiba.GetHashCode());
//       ViewBag.ExibirLogo = true;
//       ViewBag.ConcursoAberto = promocao.Status;

//       return View(model);
//     }

//     [Route("promocao/where-15-anos/jurados")]
//     public ActionResult Jurados()
//     {
//       var model = new HomeIndex
//       {
//         Title = "Jurados do Concurso Cultural - Where Curitiba 15 anos",
//         Description = "Conheça os Jurados do Concurso Cultural. Envie sua foto mostrando 'Qual é a cara de Curitiba para você?' e concorra a 15 experiências incríveis. Participe!",
//         Robots = "index, follow",
//         Canonical = $"{Constants.UrlWeb}/promocao/where-15-anos/jurados",
//         ImgOpenGraph = $"{Constants.UrlWeb}/content/images/where/where-15-anos/avatar.jpg"
//       };

//       var promocao = Promocao.Load(1);

//       ViewBag.ActiveNav = "Where 15 anos";
//       ViewBag.Editorial = Editorial.Load(EditorialEnum.WhereCuritiba.GetHashCode());
//       ViewBag.ExibirLogo = true;
//       ViewBag.ConcursoAberto = promocao.Status;

//       return View(model);
//     }
//     #endregion

//     #region Json

//     [HttpGet]
//     [AllowAnonymous]
//     [Route("promocao/where-15-anos/GeLatestImages")]
//     public JsonResult GetLastestImages(int page, string orderBy)
//     {
//       var data = Inscricao.GetAllGetAllByStatus(page, orderBy.Equals("posicao") ? InscricaoStatus.Finalista.Id : InscricaoStatus.Aprovado.Id, orderBy).Select(s => new
//       {
//         id = s.Id,
//         nome = $"{s.Nome} ({s.QtdeVotos} votos)",
//         titulo = s.Titulo,
//         imageTb = s.ImagemTbFull,
//         imageOr = s.ImagemOrFull
//       });

//       var pages = Inscricao.CountPages(orderBy == "posicao" ? InscricaoStatus.Finalista.Id : InscricaoStatus.Aprovado.Id);

//       //return Json(data, JsonRequestBehavior.AllowGet);
//       return Json(new
//       {
//         data = data,
//         pages = pages
//       }, JsonRequestBehavior.AllowGet);
//     }

//     [HttpGet]
//     [AllowAnonymous]
//     [Route("promocao/where-15-anos/voted")]
//     public JsonResult GetScore(string facebookId, int imageId)
//     {
//       if (Voto.GetVoteByInscriptionAndFacebookId(imageId, facebookId))
//         return Json("ok", JsonRequestBehavior.AllowGet);
//       else
//         return Json("nok", JsonRequestBehavior.AllowGet);
//     }

//     [HttpPost]
//     [Route("promocao/where-15-anos/voto")]
//     public JsonResult VotePost(PromocaoVoto model)
//     {
//       try
//       {
//         var vote = new Voto
//         {
//           IdFacebook = model.IdFacebook,
//           Nome = model.Nome,
//           Email = model.Email,
//           IncricaoId = model.IncricaoId
//         };

//         vote.Save();

//         return Json(new { status = "ok", label = $"{vote.Inscricao.Nome} ({vote.Inscricao.QtdeVotos} votos)" });
//       }
//       catch (Exception ex)
//       {
//         return Json(new { Error = ex.Message });
//       }
//     }
//     #endregion
//   }
// }