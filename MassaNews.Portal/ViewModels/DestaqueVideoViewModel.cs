using System.Collections.Generic;
using MassaNews.Portal.Models;

namespace MassaNews.Portal.ViewModels
{
  public class DestaqueVideoViewModel
  {
    #region Construtor

    public DestaqueVideoViewModel()
    {
      Sections = new List<VideoSection>();
    }

    #endregion

    #region Nested Class
    public class VideoSection
    {
      #region Constructor

      public VideoSection()
      {
        Videos = new List<VideoModel>();
      }

      #endregion

      #region Properties
      public string Url { get; set; }
      public string Title { get; set; }
      public bool Selected { get; set; }
      public IEnumerable<VideoModel> Videos { get; set; }
      public string ButtonText { get; set; }
      public string ButtonUrl { get; set; }
      #endregion
    }
    #endregion

    #region Properties
    public string Url { get; set; }
    public string Titulo { get; set; }
    public List<VideoSection> Sections { get; set; }
    #endregion
  }
}
