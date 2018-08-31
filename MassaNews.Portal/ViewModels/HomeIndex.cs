using MassaNews.Portal.Models;
using MassaNews.Service.Models;
using System;
using System.Collections.Generic;

namespace MassaNews.Portal.ViewModels
{
  [Serializable]
  public class HomeIndex : BaseModel
  {
    public int TemplateId { get; set; }

    public Cidade Cidade { get; set; }

    public List<Noticia> Highlights { get; set; }

    public List<Noticia> HighlightsFotos { get; set; }

    public List<Blog> Blogs { get; set; }

    public SidebarHighlight SidebarHighlight { get; set; }

    public CategoriasDestaquesModel CategoriasDestaquesModel { get; set; }

    public DestaqueComTags DestaqueComTagsWhereCuritiba { get; set; }

    public DestaqueComTags DestaqueComTagsSuaRegiao { get; set; }

    public DestaqueComTags DestaqueComTagsEsportes { get; set; }

    public DestaqueComTags DestaqueComTagsParana { get; set; }

    public DestaqueComTags DestaqueComTagsEntretedimento { get; set; }

    public DestaqueComTags DestaqueComTagsViajarEMassa { get; set; }

    public bool ShowEnquete { get; set; }

    public int? EnqueteId { get; set; }

    public DestaqueVideoViewModel DestaqueVideo { get; set; }

    public DestaqueComTags DestaqueComTagsNegociosDaTerra { get; set; }

    public List<CotacaoEconomiaViewModel> CotacoesEconomia { get; set; }

  }
}