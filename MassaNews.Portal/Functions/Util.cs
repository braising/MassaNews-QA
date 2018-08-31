using MassaNews.Service.Util;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace MassaNews.Portal.Functions
{
  public static class Util
  {
    #region Methods
    public static string RenderViewToString(ControllerContext context, string viewPath, object model = null, bool partial = false)
    {
      // first find the ViewEngine for this view
      ViewEngineResult viewEngineResult = null;

      viewEngineResult = partial ? ViewEngines.Engines.FindPartialView(context, viewPath) : ViewEngines.Engines.FindView(context, viewPath, null);

      if (viewEngineResult == null)
        throw new FileNotFoundException("View cannot be found.");

      // get the view and attach the model to view data
      var view = viewEngineResult.View;

      context.Controller.ViewData.Model = model;

      string result;

      using (var sw = new StringWriter())
      {
        var ctx = new ViewContext(context, view, context.Controller.ViewData, context.Controller.TempData, sw);

        view.Render(ctx, sw);

        result = sw.ToString();
      }

      return result;
    }
    public static void RequestNotify(HttpRequestBase request)
    {
      //var request = filterContext.HttpContext.Request;

      var lstIp = request.ServerVariables["HTTP_X_FORWARDED_FOR"];

      //Send Call Request
      if (Constants.Environment == "prod" && request != null && lstIp != null)
      {
        var obj = new
        {
          IP = lstIp.Split(',')[0],
          URL = request.Url,
          REFERENCE = request.UrlReferrer?.AbsolutePath ?? "No Reference"
        };

        SendToUDP(JsonConvert.SerializeObject(obj));
      }
    }
    private static void SendToUDP(string data)
    {
      var IPServer = "172.31.31.60";

      // Create a socket object. This is the fundamental device used to network
      // communications. When creating this object we specify:
      // Internetwork: We use the internet communications protocol
      // Dgram: We use datagrams or broadcast to everyone rather than send to
      // a specific listener
      // UDP: the messages are to be formated as user datagram protocal.
      // The last two seem to be a bit redundant.
      Socket sending_socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

      // create an address object and populate it with the IP address that we will use
      // in sending at data to. This particular address ends in 255 meaning we will send
      // to all devices whose address begins with 192.168.2.
      // However, objects of class IPAddress have other properties. In particular, the
      // property AddressFamily. Does this constructor examine the IP address being
      // passed in, determine that this is IPv4 and set the field. If so, the notes
      // in the help file should say so.
      var send_to_address = IPAddress.Parse(IPServer);

      // IPEndPoint appears (to me) to be a class defining the first or final data
      // object in the process of sending or receiving a communications packet. It
      // holds the address to send to or receive from and the port to be used. We create
      // this one using the address just built in the previous line, and adding in the
      // port number. As this will be a broadcase message, I don't know what role the
      // port number plays in this.
      IPEndPoint sending_end_point = new IPEndPoint(send_to_address, 11000);

      // The below three lines of code will not work. They appear to load
      // the variable broadcast_string witha broadcast address. But that
      // address causes an exception when performing the send.

      //string broadcast_string = IPAddress.Broadcast.ToString();
      //Console.WriteLine("broadcast_string contains {0}", broadcast_string);
      //send_to_address = IPAddress.Parse(broadcast_string);

      if (data.Length == 0)
        return;

      // the socket object must have an array of bytes to send.
      // this loads the string entered by the user into an array of bytes.
      byte[] send_buffer = Encoding.ASCII.GetBytes(data);

      // Remind the user of where this is going.
      Console.WriteLine("sending to address: {0} port: {1}", sending_end_point.Address, sending_end_point.Port);

      try
      {
        sending_socket.SendTo(send_buffer, sending_end_point);
      }
      catch (Exception ex)
      {
        //Console.WriteLine(" Exception {0}", ex.Message);
      }
    }
    #endregion
  }
}
