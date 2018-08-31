using Hangfire;
using Hangfire.Annotations;
using Hangfire.Dashboard;
using Hangfire.SqlServer;
using IdentityModel.Client;
using Microsoft.IdentityModel.Protocols;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OpenIdConnect;
using Owin;
using System;
using System.IdentityModel.Tokens;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

[assembly: OwinStartup(typeof(MassaNews.Portal.Startup))]
namespace MassaNews.Portal
{

  public class HangFireAuthorizationFilter : IDashboardAuthorizationFilter
  {
    public bool Authorize([NotNull] DashboardContext context)
    {
      //return HttpContext.Current.User.Identity.IsAuthenticated;
      return true;
    }
  }

  public class Startup
  {
    public void Configuration(IAppBuilder app)
    {
      #region Hanggire
      if (MassaNews.Service.Util.Constants.Environment == "prod")
      {

        GlobalConfiguration.Configuration.UseSqlServerStorage("HangFireConnection", new SqlServerStorageOptions { QueuePollInterval = TimeSpan.FromMinutes(1) });

        app.UseHangfireDashboard("/jobs", new DashboardOptions()
        {
          Authorization = new[] { new HangFireAuthorizationFilter() }
        });

        app.UseHangfireServer();

      }
      #endregion

      #region Authentication
      app.UseCookieAuthentication(new CookieAuthenticationOptions
      {
        AuthenticationType = "Cookies",
      });

      var settings = System.Configuration.ConfigurationManager.AppSettings;

      app.UseOpenIdConnectAuthentication(new OpenIdConnectAuthenticationOptions
      {
        AuthenticationType = "oidc",
        ClientId = "massanews.portal",
        ClientSecret = "pLDbDf0wGgaac5D2EK5X5QejC9x4x4xx",
        Authority = settings["FlorindaUrl"], //ID Server
        RedirectUri = settings["SiteUrl"], //URL of website
        PostLogoutRedirectUri = settings["SiteUrl"],
        ResponseType = "code id_token",
        Scope = "openid profile email offline_access",

        TokenValidationParameters = new System.IdentityModel.Tokens.TokenValidationParameters
        {
          RequireSignedTokens = true,
          IssuerSigningKey = new X509SecurityKey(new X509Certificate2(settings["FlorindaCertificatePath"])),
          ValidAudience = "massanews.portal",
          ValidIssuer = "identityserver",
        },

        SignInAsAuthenticationType = "Cookies",

        //Notifications = new OpenIdConnectAuthenticationNotifications
        //{
        //  AuthorizationCodeReceived = async n =>
        //  {
        //    var disco = DiscoveryClient.GetAsync("http://localhost:5000").Result;
        //    var tokenClient = new TokenClient(disco.TokenEndpoint, "massanews.console2", "secret");

        //    var tokenResponse = await tokenClient.RequestAuthorizationCodeAsync(n.Code, n.RedirectUri);
        //    //var tokenResponse = await tokenClient.RequestClientCredentialsAsync("api");

        //    // use the access token to retrieve claims from userinfo
        //    var userInfoClient = new UserInfoClient(disco.UserInfoEndpoint);

        //    var userInfoResponse = await userInfoClient.GetAsync(tokenResponse.AccessToken);

        //    // create new identity
        //    var id = new ClaimsIdentity(n.AuthenticationTicket.Identity.AuthenticationType);

        //    id.AddClaims(userInfoResponse.Claims);

        //    id.AddClaim(new Claim("access_token", tokenResponse.AccessToken));
        //    id.AddClaim(new Claim("expires_at", DateTime.Now.AddSeconds(tokenResponse.ExpiresIn).ToLocalTime().ToString()));
        //    id.AddClaim(new Claim("refresh_token", tokenResponse.RefreshToken));
        //    id.AddClaim(new Claim("id_token", n.ProtocolMessage.IdToken));
        //    id.AddClaim(new Claim("sid", n.AuthenticationTicket.Identity.FindFirst("sid").Value));

        //    n.AuthenticationTicket = new AuthenticationTicket(
        //          new ClaimsIdentity(id.Claims, n.AuthenticationTicket.Identity.AuthenticationType),
        //          n.AuthenticationTicket.Properties);
        //  },

        //  RedirectToIdentityProvider = n =>
        //  {
        //    // if signing out, add the id_token_hint
        //    if (n.ProtocolMessage.RequestType == OpenIdConnectRequestType.LogoutRequest)
        //    {
        //      var idTokenHint = n.OwinContext.Authentication.User.FindFirst("id_token");

        //      if (idTokenHint != null)
        //      {
        //        n.ProtocolMessage.IdTokenHint = idTokenHint.Value;
        //      }

        //    }

        //    return Task.FromResult(0);
        //  }
        //}

      });      
      #endregion
    }
  }
}
