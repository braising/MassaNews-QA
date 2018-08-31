using MassaNews.Portal.Models;
using MassaNews.Service.Models;
using System.Collections.Generic;

namespace MassaNews.Portal.ViewModels
{
  public class NewsletterUpdateViewModel : BaseModel
  {
    #region Properties
    public bool GoBack { get; set; }
    public string UserHash { get; set; }
    public Newsletter UserNewsletter { get; set; }
    public List<PreferenceGroup> PreferenceGroups { get; set; }
    #endregion
  }
}