using System.Collections.Generic;
using MassaNews.Portal.Models;

namespace MassaNews.Portal.ViewModels
{
  public class VideoInterna : BaseModel
  {
    public string SectionUrl { get; set; }
    public string SectionTitle { get; set; }
    public string SectionLink { get; set; }
    public string CloseLink { get; set; }
    public VideoModel Video { get; set; }
    public List<MenuItemModel> MenuItems { get; set; }
  }
}
