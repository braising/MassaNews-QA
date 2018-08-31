using MassaNews.Service.Util;

namespace MassaNews.Portal.Models
{
  public class BaseModel
  {
    #region Properties
    public string Title { get; set; }
    public string Description { get; set; }
    public string Robots { get; set; }
    public string Canonical { get; set; }
    public string Url { get; set; }
    public string ImgOpenGraph { get; set; }
    #endregion

    #region Constructor
    public BaseModel()
    {
      ImgOpenGraph = $"{Constants.UrlWeb}{"/content/images/banners/avatar-massanews.jpg"}";
    }
    #endregion
  }
}