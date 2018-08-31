using ActionMailer.Net.Mvc;
using MassaNews.Portal.Functions;
using MassaNews.Portal.Models;
using MassaNews.Service.Util;

namespace MassaNews.Portal.Controllers
{
  public class MailController : MailerBase
  {
    public EmailResult SendMailReport(ReportModel model)
    {
      To.Add(Constants.FromInbox);
      From = Constants.FromInbox;
      Subject = "Massa News | Reportar erro";
      return Email("SendMailReport", model);
    }
  }
}