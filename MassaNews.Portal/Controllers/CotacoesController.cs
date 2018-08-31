using Entities.Contexts;
using System;
using System.Net;
using System.Web.Mvc;
using MassaNews.Portal.ViewModels;
using MassaNews.Service.Enum;
using System.Linq;
using System.Collections.Generic;
using MassaNews.Service.Util;
using Newtonsoft.Json;
using Entities.Tables;

namespace MassaNews.Portal.Controllers
{
  public class CotacoesController : BaseController
  {

    [Route("cotacoes")]
    public ActionResult Cotacoes()
    {
      return new RedirectResult($"cotacoes/parana", true);
    }

    [Route("cotacoes/{tipo}")]
    //[OutputCache(Duration = 60, VaryByCustom = "Location", VaryByParam = "*", Location = OutputCacheLocation.ServerAndClient)]
    public ActionResult Cotacoes(string tipo)
    {
      CotacaoTipo objCotacaoTipo = GetCotacaoTipoBySlug(tipo);
      List<Cotacao> objCotacao = GetCotacao(objCotacaoTipo.Id).ToList();
      List<CotacoesPorEstado> objCotacoesPorEstado = GetCotacoesPorEstadoWithCotacoes(objCotacao);      
      
      var model = new CotacoesViewModel();

      /* base model defaults */
      model.Title = "Cotações em " + objCotacaoTipo.Tipo + " - Negócios da Terra";
      model.Description = "Confira as cotações " + objCotacaoTipo.Tipo + " - Negócios da Terra";
      model.Robots = "index, follow";
      model.Canonical = $"{Constants.UrlWeb}/cotacoes/" + tipo + "";

      //model page cotacao
      model.Tipo = objCotacaoTipo;
      if (tipo == "cepea-usp" || tipo == "cia-ufpr" || tipo == "lapsui")
      {
        model.Cotacoes = objCotacao;
      }
      if (tipo == "parana" || tipo == "santa-catarina")
      {
        model.CotacoesPorEstados = objCotacoesPorEstado;
        model.Cidades = GetCotacaoCidades(tipo == "parana" ? 1 : 2).ToList();
      }

      //Set viewbag's
      //ViewBag.Pagina = "cotacoes-cidade";
      ViewBag.ActiveNav = "Negócios da Terra";
      ViewBag.Editorial = Service.Models.Editorial.Load(EditorialEnum.NegociosDaTerra.GetHashCode());
      ViewBag.ExibirLogo = true;
      ViewBag.LinkActiveNav = "/negocios-da-terra";

      return View(model);
    }

    [Route("cotacoes/{tipo}/{cidade}")]
    //[OutputCache(Duration = 60, VaryByCustom = "Location", VaryByParam = "*", Location = OutputCacheLocation.ServerAndClient)]
    public ActionResult CotacoesCidade(string tipo, string cidade)
    {
      DateTime dataCotacao = new DateTime();
      CotacaoTipo objCotacaoTipo = GetCotacaoTipoBySlug(tipo);
      List<Cotacao> objCotacao = GetCotacao(objCotacaoTipo.Id).ToList();
      CotacaoCidade objCotacaoCidade = GetCotacaoCidadeBySlug(cidade);
      List<CotacoesPorCidade> objCotacoesPorCidade = GetCotacoesPorCidadeWithCotacoes(objCotacao, objCotacaoCidade.Id, dataCotacao);

      ViewBag.CotacaoCidadeId = new SelectList(GetCotacaoCidades(tipo == "parana" ? 1 : 2), "Id", "Cidade");

      var model = new CotacoesCidadeViewModel();

      /* base model defaults */
      model.Title = "Cotações em " + cidade + " - Negócios da Terra";
      model.Description = "Confira as cotações em " + cidade + " - Negócios da Terra";
      model.Robots = "index, follow";
      model.Canonical = $"{Constants.UrlWeb}/" + tipo + "/" + cidade + "/";

      //
      model.Tipo = objCotacaoTipo;
      model.Cidade = objCotacaoCidade;
      model.Cotacoes = objCotacoesPorCidade;
      if (tipo == "parana" || tipo == "santa-catarina")
      {
        model.Cidades = GetCotacaoCidades(tipo == "parana" ? 1 : 2).ToList();
      }

      //Set viewbag's
      //ViewBag.Pagina = "cotacoes-" + tipo;
      ViewBag.ActiveNav = "Negócios da Terra";
      ViewBag.Editorial = Service.Models.Editorial.Load(EditorialEnum.NegociosDaTerra.GetHashCode());
      ViewBag.ExibirLogo = true;
      ViewBag.LinkActiveNav = "/negocios-da-terra";

      //return the model to the view
      return View(model);
    }

    [Route("cotacoes/{tipo}/{cidade}/{ano}/{mes}/{dia}")]
    //[OutputCache(Duration = 60, VaryByCustom = "Location", VaryByParam = "*", Location = OutputCacheLocation.ServerAndClient)]
    public ActionResult CotacoesCidade(string tipo, string cidade, int ano, int mes, int dia)
    {
      DateTime dataCotacao = new DateTime(ano, mes, dia);
      CotacaoTipo objCotacaoTipo = GetCotacaoTipoBySlug(tipo);
      List<Cotacao> objCotacao = GetCotacao(objCotacaoTipo.Id).ToList();
      CotacaoCidade objCotacaoCidade = GetCotacaoCidadeBySlug(cidade);
      List<CotacoesPorCidade> objCotacoesPorCidade = GetCotacoesPorCidadeWithCotacoes(objCotacao, objCotacaoCidade.Id, dataCotacao);

      ViewBag.CotacaoCidadeId = new SelectList(GetCotacaoCidades(tipo == "parana" ? 1 : 2), "Id", "Cidade");

      var model = new CotacoesCidadeViewModel();

      /* base model defaults */
      model.Title = "Cotações em " + cidade + " - Negócios da Terra";
      model.Description = "Confira as cotações em " + cidade + " - Negócios da Terra";
      model.Robots = "index, follow";
      model.Canonical = $"{Constants.UrlWeb}/" + tipo + "/" + cidade + "/";

      //
      model.DataCotacao = dataCotacao;
      model.Tipo = objCotacaoTipo;
      model.Cidade = objCotacaoCidade;
      model.Cotacoes = objCotacoesPorCidade;
      if (tipo == "parana" || tipo == "santa-catarina")
      {
        model.Cidades = GetCotacaoCidades(tipo == "parana" ? 1 : 2).ToList();
      }

      //Set viewbag's
      //ViewBag.Pagina = "cotacoes-" + tipo;
      ViewBag.ActiveNav = "Negócios da Terra";
      ViewBag.Editorial = Service.Models.Editorial.Load(EditorialEnum.NegociosDaTerra.GetHashCode());
      ViewBag.ExibirLogo = true;
      ViewBag.LinkActiveNav = "/negocios-da-terra";

      //return the model to the view
      return View(model);
    }

    [Route("cotacoes/{tipo}/produto/{produto}")]
    //[OutputCache(Duration = 60, VaryByCustom = "Location", VaryByParam = "*", Location = OutputCacheLocation.ServerAndClient)]
    public ActionResult CotacoesProduto(string tipo, string produto)
    {
      CotacaoTipo objCotacaoTipo = GetCotacaoTipoBySlug(tipo);
      CotacaoProduto objCotacaoProduto = GetCotacaoProdutoBySlug(produto);
      Cotacao objCotacao = GetCotacao(objCotacaoTipo.Id, objCotacaoProduto.Id).FirstOrDefault();
      List<Cotacao> objCotacoes = GetCotacao(objCotacaoTipo.Id).ToList();
      List<CotacaoPorEstado> objCotacoesPorEstado = new List<CotacaoPorEstado>();
      List<CotacoesPorCidade> objCotacoesPorCidade = new List<CotacoesPorCidade>();
      List<decimal> mediasEstaduas = GetMediaEstadual(tipo == "parana" ? 1 : 2, objCotacao.Id, 60);

      if (objCotacao != null)
      {
        objCotacoesPorEstado = GetCotacaoPorEstado(objCotacao.Id, 10).ToList();
        objCotacoesPorCidade = GetCotacoesPorCidadeAndProduto(objCotacao.Id, 10).ToList();
      }

      var model = new CotacoesCidadeProdutoViewModel();

      /* base model defaults */
      model.Title = "Cotações de " + objCotacaoProduto.Nome + " - Negócios da Terra";
      model.Description = "Confira as cotações do produto '" + objCotacaoProduto.Nome + "' no estado de " + objCotacaoTipo.Tipo + " - Negócios da Terra";
      model.Robots = "index, follow";
      model.Canonical = $"{Constants.UrlWeb}/" + tipo + "/produto/" + produto + "/";

      //
      model.Tipo = objCotacaoTipo;
      model.Produto = objCotacaoProduto;

      model.Cotacoes = objCotacoes;
      model.CotacoesPorEstado = objCotacoesPorEstado;
      model.CotacoesPorCidade = objCotacoesPorCidade;

      if (tipo == "parana")
        model.ProdutoSlugOutro = GetCotacao(5, objCotacaoProduto.Id).FirstOrDefault();
      else
        model.ProdutoSlugOutro = GetCotacao(4, objCotacaoProduto.Id).FirstOrDefault();

      //Set viewbag's
      //ViewBag.Pagina = "cotacoes-produto";
      ViewBag.ActiveNav = "Negócios da Terra";
      ViewBag.Editorial = Service.Models.Editorial.Load(EditorialEnum.NegociosDaTerra.GetHashCode());
      ViewBag.ExibirLogo = true;
      ViewBag.LinkActiveNav = "/negocios-da-terra";

      //model.GraficoLabels = "1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,16,17,18,19,20,21,22,23,24,25,26,27,29,29,30";
      int count = 1;
      foreach (var item in mediasEstaduas)
      {
        if (count <= 30)
          model.GraficoData30 = model.GraficoData30 + item.ToString().Replace(",", ".") + ",";

        if (count <= 60 )
          model.GraficoData60 = model.GraficoData60 + item.ToString().Replace(",", ".") + ",";

        count++;
      }

      for (int i = 1; i < mediasEstaduas.Count + 1; i++)
      {
        if (i <= 30)
          model.GraficoLabels30 = model.GraficoLabels30 + i + ",";

        if (i <= 60)
          model.GraficoLabels60 = model.GraficoLabels60 + i + ",";
      }
      //model.GraficoLabels60 = "1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,16,17,18,19,20,21,22,23,24,25,26,27,29,29,30,1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,16,17,18,19,20,21,22,23,24,25,26,27,29,29,30";
      //model.GraficoData60 = "1.40,142.00,16.00,20.99,4.30,20,55,10.5,10,25,25,25,12,13,14,17,18,18,18,18,10,25,25,25,12,13,14,17,18,18,18,18,10,25,25,25,12,13,14,17,18,18,18,18";

      //return the model to the view
      return View(model);
    }

    private CotacaoTipo GetCotacaoTipoBySlug(string slug)
    {
      return Db.CotacaoTipo.FirstOrDefault(e => e.Slug == slug);
    }
    private CotacaoCidade GetCotacaoCidadeBySlug(string slug)
    {
      return Db.CotacaoCidade.FirstOrDefault(e => e.Slug == slug);
    }
        private CotacaoProduto GetCotacaoProdutoBySlug(string url)
    {
      return Db.CotacaoProduto.FirstOrDefault(e => e.Url == url);
    }

    private IEnumerable<Cotacao> GetCotacao(int cotacaoTipo)
    {
      return Db.Cotacao.Where(e => e.CotacaoTipoId == cotacaoTipo);
    }
    private IEnumerable<Cotacao> GetCotacao(int cotacaoTipo, int cotocaoProdutoId)
    {
      if (cotocaoProdutoId == 0)
      {
        return Db.Cotacao.Where(e => e.CotacaoTipoId == cotacaoTipo);
      }
      else
      {
        return Db.Cotacao.Where(e => e.CotacaoTipoId == cotacaoTipo && e.CotacaoProdutoId == cotocaoProdutoId);
      }
    }

    private IEnumerable<CotacaoCidade> GetCotacaoCidades(int cotacaoEstadoId)
    {
      return Db.CotacaoCidade.Where(e => e.CotacaoEstadoId == cotacaoEstadoId);
    }

    private IEnumerable<CotacaoPorEstado> GetCotacaoPorEstado(int cotacaoId, int take)
    {
      return Db.CotacaoPorEstado.Where(c => c.CotacaoId == cotacaoId).OrderByDescending(c => c.DataCotacao).Take(take);
    }

    private IEnumerable<CotacaoPorCidade> GetCotacaoPorCidade(int cotacaoId, int cotacaoCidadeId, int take)
    {
      return Db.CotacaoPorCidade.Where(c => c.CotacaoId == cotacaoId && c.CotacaoCidadeId == cotacaoCidadeId ).OrderByDescending(c => c.DataCotacao).Take(take);
    }

    private IEnumerable<CotacaoPorCidade> GetCotacaoPorCidadeByData(int cotacaoId, int cotacaoCidadeId, DateTime dataCotacao, int take)
    {
      return Db.CotacaoPorCidade.Where(c => c.CotacaoId == cotacaoId && c.CotacaoCidadeId == cotacaoCidadeId && c.DataCotacao == dataCotacao).OrderByDescending(c => c.DataCotacao).Take(take);
    }
    private List<int> GetCotacaoProdutoCidade(int cotacaoId)
    {
      return Db.CotacaoPorCidade.Where(c => c.CotacaoId == cotacaoId).Select(c => c.CotacaoCidadeId).Distinct().ToList();
      //return Db.CotacaoPorCidade.Where(c => c.CotacaoId == cotacaoId).GroupBy(c => c.CotacaoCidadeId).Select(c => c.Key).ToList();
    }
    private List<decimal> GetMediaEstadual(int estadoId, int cotacaoId, int take)
    {
      return Db.CotacaoPorEstado.Where(c => c.CotacaoId == cotacaoId && c.CotacaoEstadoId == estadoId).OrderByDescending(c => c.DataCotacao).Select(c => c.Media).Take(30).ToList();
    }

    private List<CotacoesPorEstado> GetCotacoesPorEstadoWithCotacoes(List<Cotacao> objCotacao)
    {
      List<CotacoesPorEstado> objCotacoesPorEstado = new List<CotacoesPorEstado>();

      foreach (var cotacao in objCotacao)
      {
        List<CotacaoPorEstado> aux = GetCotacaoPorEstado(cotacao.Id, 2).ToList();
        if (aux.Count == 1)
        {
          CotacoesPorEstado aux2 = new CotacoesPorEstado();
          aux2.EstadoSlug = aux[0].CotacaoEstado.Slug;
          aux2.Media = aux[0].Media;
          aux2.DataCotacao = aux[0].DataCotacao;
          aux2.ProdutoNome = aux[0].Cotacao.CotacaoProduto.Nome;
          aux2.ProdutoSlug = aux[0].Cotacao.CotacaoProduto.Url;

          objCotacoesPorEstado.Add(aux2);
        }
        else if (aux.Count == 2)
        {
          CotacoesPorEstado aux2 = new CotacoesPorEstado();
          aux2.EstadoSlug = aux[0].CotacaoEstado.Slug;
          aux2.Media = aux[0].Media;
          aux2.MediaAnterior = aux[1].Media;
          aux2.DataCotacao = aux[0].DataCotacao;
          aux2.ProdutoNome = aux[0].Cotacao.CotacaoProduto.Nome;
          aux2.ProdutoSlug = aux[0].Cotacao.CotacaoProduto.Url;

          objCotacoesPorEstado.Add(aux2);
        }
      }
      return objCotacoesPorEstado;
    }

    private List<CotacoesPorCidade> GetCotacoesPorCidadeWithCotacoes(List<Cotacao> objCotacao, int cotacaoCidadeId, DateTime dataCotacao)
    {
      List<CotacoesPorCidade> objCotacoesPorCidade = new List<CotacoesPorCidade>();

      foreach (var cotacao in objCotacao)
      {
        List<CotacaoPorCidade> aux = new List<CotacaoPorCidade>();
        if (dataCotacao == DateTime.MinValue)
        {
          aux = GetCotacaoPorCidade(cotacao.Id, cotacaoCidadeId, 2).ToList();
        }
        else
        {
          aux = GetCotacaoPorCidadeByData(cotacao.Id, cotacaoCidadeId, dataCotacao, 2).ToList();
        }

        if (aux.Count == 1)
        {
          CotacoesPorCidade aux2 = new CotacoesPorCidade();
          aux2.Valor = aux[0].Valor;
          aux2.DataCotacao = aux[0].DataCotacao;
          aux2.ProdutoNome = aux[0].Cotacao.CotacaoProduto.Nome;
          aux2.ProdutoSlug = aux[0].Cotacao.CotacaoProduto.Url;

          objCotacoesPorCidade.Add(aux2);
        }
        else if (aux.Count == 2)
        {
          CotacoesPorCidade aux2 = new CotacoesPorCidade();
          aux2.Valor = aux[0].Valor;
          aux2.ValorAnterior = aux[1].Valor;
          aux2.DataCotacao = aux[0].DataCotacao;
          aux2.ProdutoNome = aux[0].Cotacao.CotacaoProduto.Nome;
          aux2.ProdutoSlug = aux[0].Cotacao.CotacaoProduto.Url;

          objCotacoesPorCidade.Add(aux2);
        }
      }
      return objCotacoesPorCidade;
    }

    private List<CotacoesPorCidade> GetCotacoesPorCidadeAndProduto(int cotacaoId, int take)
    {
      List<CotacoesPorCidade> objCotacoesPorCidade = new List<CotacoesPorCidade>();

      List<int> idCidades = GetCotacaoProdutoCidade(cotacaoId).ToList();

      foreach (var cidadeId in idCidades)
      {
        List<CotacaoPorCidade> aux = GetCotacaoPorCidade(cotacaoId, cidadeId, 2).ToList();
        if (aux.Count == 1)
        {
          CotacoesPorCidade aux2 = new CotacoesPorCidade();
          aux2.Cidade = aux[0].CotacaoCidade.Cidade;
          aux2.CidadeSlug = aux[0].CotacaoCidade.Slug;
          aux2.Valor = aux[0].Valor;
          aux2.DataCotacao = aux[0].DataCotacao;
          aux2.ProdutoNome = aux[0].Cotacao.CotacaoProduto.Nome;
          aux2.ProdutoSlug = aux[0].Cotacao.CotacaoProduto.Url;

          objCotacoesPorCidade.Add(aux2);
        }
        else if (aux.Count == 2)
        {
          CotacoesPorCidade aux2 = new CotacoesPorCidade();
          aux2.Cidade = aux[0].CotacaoCidade.Cidade;
          aux2.CidadeSlug = aux[0].CotacaoCidade.Slug;
          aux2.Valor = aux[0].Valor;
          aux2.ValorAnterior = aux[1].Valor;
          aux2.DataCotacao = aux[0].DataCotacao;
          aux2.ProdutoNome = aux[0].Cotacao.CotacaoProduto.Nome;
          aux2.ProdutoSlug = aux[0].Cotacao.CotacaoProduto.Url;

          objCotacoesPorCidade.Add(aux2);
        }
      }
      return objCotacoesPorCidade;
    }
  }
}