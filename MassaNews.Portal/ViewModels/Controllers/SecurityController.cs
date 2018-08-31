using Amazon;
using Amazon.WAF;
using Amazon.WAF.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace MassaNews.Portal.Controllers
{
  public class SecurityController : Controller
  {
    [Route("hp-9396a4d143cf")]
    public ActionResult Index()
    {
      #region Debug
      if (!string.IsNullOrEmpty(Request.QueryString["debug"]))
      {
        var values = new Dictionary<string, string>();

        foreach (var key in Request.ServerVariables.AllKeys)
          values.Add(key, Request.ServerVariables[key]);

        values.Add("DateTime", DateTime.Now.ToString());

        return Content(JsonConvert.SerializeObject(values), "application/json");
      }
      #endregion

      if (Request.ServerVariables["HTTP_X_FORWARDED_FOR"] == null)
        return null;

      var lstIp = Request.ServerVariables["HTTP_X_FORWARDED_FOR"].Split(',');

      if (lstIp.Any())
      {
        var accesKey = "AKIAIRODRD7TFCQ5XS5Q";
        var secretKey = "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";

        var ipSetId = "6c8ff7fb-7f06-48fa-8ee6-9b9fbfa02e8b";
        var userIp = $"{lstIp[0]}/32";

        var client = new AmazonWAFClient(accesKey, secretKey, RegionEndpoint.USEast1);

        var ipset = client.GetIPSet(ipSetId);

        if (!ipset.IPSet.IPSetDescriptors.Exists(p => p.Value == userIp))
        {
          var token = client.GetChangeToken();

          var listIp = new List<IPSetUpdate>
          {
              new IPSetUpdate() { Action = new ChangeAction("INSERT"), IPSetDescriptor = new IPSetDescriptor() { Type = IPSetDescriptorType.IPV4, Value = $"{lstIp[0]}/32"} }
          };

          client.UpdateIPSet(ipSetId, listIp, token.ChangeToken);
        }

        return new RedirectResult("/", true);

      }

      return null;

    }
  }
}