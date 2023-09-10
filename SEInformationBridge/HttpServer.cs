using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net;

namespace SEInformationBridge
{
    public class HttpServer
    {
        private readonly HttpListener _listener;
        private readonly int _port;

        public HttpServer(int port)
        {
            if (!HttpListener.IsSupported)
            {
                throw new NotSupportedException("are you running this on a potato?");
            }

            _port = port;
            _listener = new HttpListener();
            _listener.Prefixes.Add($"http://*:{port}/");
        }
        public async Task StartAsync()
        {
            _listener.Start();
            while (true)
            {
                var context = await _listener.GetContextAsync();
                _ = ProcessRequestAsync(context);
            }
        }

        private async Task ProcessRequestAsync(HttpListenerContext context)
        {          
            var path = context.Request.Url.AbsolutePath;

            string responseString;

            switch (path)
            {
                case "/grids":
                    responseString = GridInfo.Serialize();
                    break;
                case "/planets":
                    responseString = "Not implemented yet.";
                    break;
                case "/factions":
                    responseString = "Not implemented yet.";
                    break;
                default:
                    responseString = "/grids \n /planets \n /factions";
                    break;
            }

            byte[] buffer = Encoding.UTF8.GetBytes(responseString);

            context.Response.ContentLength64 = buffer.Length;
            context.Response.ContentType = "text/plain";

            using (var output = context.Response.OutputStream)
            {
                await output.WriteAsync(buffer, 0, buffer.Length);
            }
        }

        public void Stop()
        {
            _listener.Stop();
        }

        public static async Task Main(string[] args)
        {
            var server = new HttpServer(8080);
            await server.StartAsync();
        }

    }
}
