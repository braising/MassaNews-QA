using MassaNews.Portal.Models;
using MassaNews.Service.Models;

namespace MassaNews.Portal.ViewModels
{
  public class NewsletterUnsubscribeViewModel : BaseModel
  {
    public string UserHash { get; set; }
    public Newsletter UserNewsletter { get; set; }
  }
}