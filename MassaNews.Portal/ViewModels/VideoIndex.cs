using System.Collections.Generic;
using MassaNews.Portal.Models;

namespace MassaNews.Portal.ViewModels
{
  public class VideoIndex : BaseModel
  {
    public IEnumerable<VideoModel> Highlights { get; set; }

    public DestaqueVideoViewModel RegionHighlights { get; set; }

    public DestaqueVideoViewModel SportsHighlights { get; set; }

    public DestaqueVideoViewModel EntertainmentHighlights { get; set; }

    public DestaqueVideoViewModel NegociosDaTerraHighlights { get; set; }

    public DestaqueVideoViewModel BlogsHighlights { get; set; }

    public DestaqueVideoViewModel LiveOnHighlights { get; set; }

    public DestaqueVideoViewModel DescobrindoCuritibaHighlights { get; set; }

  }
}
