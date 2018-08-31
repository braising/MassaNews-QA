using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MassaNews.Portal.Functions
{
  public class Pagination
  {
    #region AddPagination
    public static Dictionary<string, int> AddPagination(int pagAtual, int qtdePaginasTotal, int qtdePaginasExibidas, bool hasAditionalButtons)
    {
      Dictionary<string, int> paginacao = new Dictionary<string, int>();

      // Se existir mais de uma página monta a exibição das páginas
      if (qtdePaginasTotal > 1)
      {
        if (hasAditionalButtons && pagAtual > 1)
          paginacao.Add("<", (pagAtual - 1));

        // Se a quantidade de páginas total for menor ou igual a quantidade de páginas a serem exibidas
        if (qtdePaginasTotal <= qtdePaginasExibidas)
        {
          AddPages(1, qtdePaginasTotal, ref paginacao);
        }
        // Se a quantidade de páginas total for maior que a quantidade de páginas a serem exibidas
        else
        {
          // Se for a primeira página
          if (pagAtual == 1)
          {
            AddPages(pagAtual, (qtdePaginasExibidas - 1), ref paginacao);

            paginacao.Add(string.Format("...{0}", qtdePaginasTotal.ToString()), qtdePaginasTotal);
          }
          // Se for a última página
          else if (pagAtual == qtdePaginasTotal)
          {
            paginacao.Add("1...", 1);

            AddPages((qtdePaginasTotal - (qtdePaginasExibidas - 2)), pagAtual, ref paginacao);
          }
          // Se for uma página do meio
          else
          {
            int metade = (int)Math.Floor((double)qtdePaginasExibidas / 2);

            // Início
            if (pagAtual - metade <= 1)
            {
              AddPages(1, (pagAtual - 1), ref paginacao);
            }
            else
            {
              paginacao.Add("1...", 1);

              if (pagAtual + metade >= qtdePaginasTotal)
                AddPages((pagAtual - metade + 1 - (metade - (qtdePaginasTotal - pagAtual))), (pagAtual - 1), ref paginacao);
              else
                AddPages((pagAtual - metade + 1), (pagAtual - 1), ref paginacao);

            }

            // Insere a do meio
            paginacao.Add(pagAtual.ToString(), pagAtual);

            // Fim
            if (pagAtual + metade >= qtdePaginasTotal)
            {
              AddPages((pagAtual + 1), qtdePaginasTotal, ref paginacao);
            }
            else
            {
              AddPages((pagAtual + 1), (pagAtual + metade + (metade - paginacao.Count)), ref paginacao);

              paginacao.Add(string.Format("...{0}", qtdePaginasTotal.ToString()), qtdePaginasTotal);
            }
          }
        }

        if (hasAditionalButtons && pagAtual < qtdePaginasTotal)
          paginacao.Add(">", (pagAtual + 1));
      }

      return paginacao;
    }
    #endregion

    #region AddPages
    private static void AddPages(int de, int ate, ref Dictionary<string, int> paginacao)
    {
      for (var i = de; i <= ate; i++)
      {
        paginacao.Add(i.ToString(), i);
      }
    }
    #endregion
  }
}