using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net.Http;
using System.Web.SessionState;
using IdentityModel.Client;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using MassaNews.Service.Models;
using System.Security.Claims;
using MassaNews.Service.Services;
using System.Configuration;
using MassaNews.Portal.Functions;

namespace MassaNews.Portal.Models
{
  public class UsuarioPortal : Usuario
  {
    public string PrimeiroNome { get; set; }
    public string UltimoNome { get; set; }
    public string Fone { get; set; }
    public string Foto { get; set; }
    public string Estado { get; set; }
    public string Cidade { get; set; }
    public bool NoticiasPersonalizadas { get; set; }
    public int NoticiasPersonalizadasCheckbox { get; set; }
    public string Cpf { get; set; }
    public string DataNascimento { get; set; }
    public string Sexo { get; set; }
    public int CityId { get; set; }
  }

  public class CurrentUser
  {
    private static HttpSessionState Session
    {
      get { return HttpContext.Current.Session; }
    }

    public static ClaimsPrincipal User
    {
      get { return HttpContext.Current.User as ClaimsPrincipal; }
    }

    public static UsuarioPortal Data
    {
      get
      {
        if (Session["CurrentUser"] == null)
        {
          if (User != null && User.Identity.IsAuthenticated)
          {
            Load();
          }
        }

        return Session["CurrentUser"] as UsuarioPortal;
      }
    }
    
    public static bool Valido
    {
      get
      {
        if(Session["CurrentUser"] == null)
        {
          if(User != null && User.Identity.IsAuthenticated)
          {
            Load();
          }
        }
        return Session["CurrentUser"] != null;        
      }      
    }

    public static void Unload()
    {
      Session["CurrentUser"] = null;
    }

    public static void Load()
    {
      var session = Session;
      var user = User;

      var florindaUrl = ConfigurationManager.AppSettings["FlorindaUrl"];

      var subjectId = user.Claims.First(a => a.Type == "http://grupomassa.com.br/2018/subjectId").Value;

      var disco = DiscoveryClient.GetAsync(florindaUrl).Result;

      var tokenClient = new TokenClient(disco.TokenEndpoint, "massanews.portal", "pLDbDf0wGgaac5D2EK5X5QejC9x4x4xx");
      var tokenResponse = tokenClient.RequestClientCredentialsAsync("api.florinda").Result;

      if (tokenResponse.IsError)
      {
        Console.WriteLine(tokenResponse.Error);
        return;
      }

      var client = new HttpClient();
      client.SetBearerToken(tokenResponse.AccessToken);

      var florindaApiUrl = ConfigurationManager.AppSettings["FlorindaApiUrl"];

      var response = client.GetAsync(florindaApiUrl + "/identity/" + subjectId).Result;

      if (!response.IsSuccessStatusCode)
      {
        Console.WriteLine(response.StatusCode);
        return;
      }

      var content = response.Content.ReadAsStringAsync().Result;

      var usuarioPortal = JsonConvert.DeserializeObject<UsuarioPortal>(content);

      var usuario = Usuario.Load(subjectId);

      if(usuario == null)
      {
        usuario = new Usuario();
      }

      if (usuario.Id < 1)
      {
        usuario.SubjectId = subjectId;        
      }

      usuario.Nome = usuarioPortal.Nome;
      usuario.Email = usuarioPortal.Email;
      usuario.IsEspecialista = ComunidadeService.IsEspecialista(usuarioPortal.Email);
      usuario.Status = true;

      usuario.Save();

      var objNewsletter = Service.Models.Newsletter.GetByEmail(usuarioPortal.Email);

      if (objNewsletter == null)
      {
        var obj = new Service.Models.Newsletter
        {
          Nome = usuarioPortal.Nome,
          Email = usuarioPortal.Email,
          Ativo = true,
          CidadeId = usuarioPortal.CityId == 0 ? 12 : usuarioPortal.CityId
        };

        obj.Save();
      }
      else
      {
        if (usuarioPortal.NoticiasPersonalizadas != objNewsletter.Ativo)
        {
          objNewsletter.Ativo = usuarioPortal.NoticiasPersonalizadas;

          objNewsletter.Save();
        }
      }

      if (string.IsNullOrEmpty(usuarioPortal.PrimeiroNome))
      {
        if(usuarioPortal.Nome.Contains(" "))
        {
          usuarioPortal.PrimeiroNome = usuario.Nome.Split(' ')[0];
        }
        else
        {
          usuarioPortal.PrimeiroNome = usuario.Nome;
        }
      }
        
      session["CurrentUser"] = usuarioPortal;
    }

    public static void Update(UsuarioPortal model)
    {
      var user = User;

      var florindaUrl = ConfigurationManager.AppSettings["FlorindaUrl"];

      var subjectId = user.Claims.First(a => a.Type == "http://grupomassa.com.br/2018/subjectId").Value;

      var disco = DiscoveryClient.GetAsync(florindaUrl).Result;

      var tokenClient = new TokenClient(disco.TokenEndpoint, "massanews.portal", "pLDbDf0wGgaac5D2EK5X5QejC9x4x4xx");
      var tokenResponse = tokenClient.RequestClientCredentialsAsync("api.florinda").Result;

      if (tokenResponse.IsError)
      {
        Console.WriteLine(tokenResponse.Error);
        return;
      }

      var client = new HttpClient();
      client.SetBearerToken(tokenResponse.AccessToken);

      var florindaApiUrl = ConfigurationManager.AppSettings["FlorindaApiUrl"];

      model.Nome = model.PrimeiroNome + " " + model.UltimoNome;
      var data = JsonConvert.SerializeObject(model, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore });
      var body = new StringContent(data);
      body.Headers.ContentType.MediaType = "application/json";
      
      var response = client.PostAsync(florindaApiUrl + "/identity/" + subjectId, body).Result;

      if (response.IsSuccessStatusCode)
      {
        Load();       
      }
      else
      {
        var content = response.Content.ReadAsStringAsync().Result;
        throw new Exception("Erro: " + content);
      }      
    }

    public static void UpdatePicture(string extension, string base64Data)
    {
      var user = User;

      var florindaUrl = ConfigurationManager.AppSettings["FlorindaUrl"];

      var subjectId = user.Claims.First(a => a.Type == "http://grupomassa.com.br/2018/subjectId").Value;

      var disco = DiscoveryClient.GetAsync(florindaUrl).Result;

      var tokenClient = new TokenClient(disco.TokenEndpoint, "massanews.portal", "pLDbDf0wGgaac5D2EK5X5QejC9x4x4xx");
      var tokenResponse = tokenClient.RequestClientCredentialsAsync("api.florinda").Result;

      if (tokenResponse.IsError)
      {
        Console.WriteLine(tokenResponse.Error);
        return;
      }

      var client = new HttpClient();
      client.SetBearerToken(tokenResponse.AccessToken);

      var florindaApiUrl = ConfigurationManager.AppSettings["FlorindaApiUrl"];

      var data = JsonConvert.SerializeObject(new { Extension = extension, Base64Data = base64Data });
      var body = new StringContent(data);

      var response = client.PostAsync(florindaApiUrl + "/identity/" + subjectId + "/picture", body).Result;

      if (response.IsSuccessStatusCode)
      {
        Load();
      }
      else
      {
        var content = response.Content.ReadAsStringAsync().Result;
        throw new Exception("Erro: " + content);
      }
    }
  }
}