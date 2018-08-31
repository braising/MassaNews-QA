using System;
using System.Diagnostics;
using System.IO;
using System.Web;

namespace MassaNews.Portal
{
    public class SvgCompressionModule : IHttpModule
    {
        public void Init(HttpApplication httpApp)
        {
            //httpApp.BeginRequest += OnBeginRequest;
            //httpApp.EndRequest += OnEndRequest;
            //httpApp.PreSendRequestHeaders += OnHeaderSent;
        }

        public void OnHeaderSent(object sender, EventArgs e)
        {
            var httpApp = (HttpApplication)sender;
            httpApp.Context.Items["HeadersSent"] = true;
        }

        private void OnBeginRequest(object sender, EventArgs eventArgs)
        {
            var httpApp = (HttpApplication)sender;
            if (httpApp.Request.Path.StartsWith("/media/")) return;
            var timer = new Stopwatch();
            httpApp.Context.Items["Timer"] = timer;
            httpApp.Context.Items["HeadersSent"] = false;
            timer.Start();

            //-----------

            //var app = (HttpApplication)sender;

            //if (app == null)
            //    return;

            //var context = app.Context;

            //if (!IsSvgRequest(context.Request))
            //    return;

            //context.Response.Filter = new GZipStream(context.Response.Filter, CompressionMode.Compress);

            //context.Response.AddHeader("Content-encoding", "gzip");
        }

        public void OnEndRequest(Object sender, EventArgs e)
        {
            var httpApp = (HttpApplication)sender;

            if (httpApp.Request.Path.StartsWith("/media/")) return;

            var timer = (Stopwatch)httpApp.Context.Items["Timer"];

            if (timer != null)
            {
                timer.Stop();

                if (!(bool)httpApp.Context.Items["HeadersSent"])
                    httpApp.Context.Response.AppendHeader("ProcessTime", ((double)timer.ElapsedTicks / Stopwatch.Frequency) * 1000 + " ms.");
            }

            httpApp.Context.Items.Remove("Timer");
            httpApp.Context.Items.Remove("HeadersSent");
        }

        public void Dispose() { }

        //protected virtual bool IsSvgRequest(HttpRequest request)
        //{
        //    var path = request.Url.AbsolutePath;

        //    return Path.HasExtension(path) && Path.GetExtension(path).Equals(".svg", StringComparison.InvariantCultureIgnoreCase);
        //}
    }
}