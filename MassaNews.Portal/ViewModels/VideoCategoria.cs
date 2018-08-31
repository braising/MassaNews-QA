using System;
using System.Collections.Generic;
using MassaNews.Portal.Models;

namespace MassaNews.Portal.ViewModels
{
  public class VideoCategoria : BaseModel
  {
    #region Nested Class
    public class VideoSection
    {
      #region Constructor
      public VideoSection()
      {
        Videos = new List<List<VideoModel>>();
      }
      #endregion

      #region Properties
      public string Title { get; set; }
      public List<List<VideoModel>> Videos { get; set; }
      public DateTime StartDate { get; set; }
      #endregion
    }
    #endregion

    #region Properties
    public string Category { get; set; }
    public List<VideoModel> Highlights { get; set; }
    public  List<VideoSection> Sections { get; set; }
    #endregion
  }
}
