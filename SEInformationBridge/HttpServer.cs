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
            bool displayHomePage = false;

            switch (path.ToLower())
            {

                case "/grids":
                    responseString = Utilities.Serialize(PlayerGrids.GetGridList());
                    break;
                case "/planets":
                    responseString = Utilities.Serialize(Planets.GetPlanets());
                    break;
                case "/factions":
                    responseString = Utilities.Serialize(Factions.GetFactions());
                    break;
                case "/players":
                    responseString = Utilities.Serialize(Players.GetPlayers());
                    break;
                case "/chat":
                    responseString = Utilities.Serialize(ChatLog.GetChat());
                    break;
                case "/settings":
                    responseString = Utilities.Serialize(ServerInfo.GetServerSettings());
                    break;
                default:
                    responseString = GenerateHomePage(context.Request.UserHostName);
                    displayHomePage = true;
                    break;

            }

            byte[] buffer = Encoding.UTF8.GetBytes(responseString);

            context.Response.ContentLength64 = buffer.Length;
            if(displayHomePage)
                context.Response.ContentType = "text/html";
            else
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

        private string GenerateHomePage(string url)
        {

            StringBuilder page = new StringBuilder(part1);
            if (Plugin.TorchInstance.CurrentSession != null)
            {
                page.AppendLine($"<li><a href=\"http://{url}/Grids\">Grids</a></li>");
                page.AppendLine($"<li><a href=\"http://{url}/Players\">Players</a></li>");
                page.AppendLine($"<li><a href=\"http://{url}/Factions\">Factions</a></li>");
                page.AppendLine($"<li><a href=\"http://{url}/Planets\">Planets</a></li>");            
                page.AppendLine($"<li><a href=\"http://{url}/Settings\">Settings</a></li>");
                
            }
            else
            {
                page.AppendLine("-Server Offline-");
            }
            page.AppendLine(part2);
            return page.ToString();
        }

        private string part1 = @"<!DOCTYPE html>
        <html lang=""en"">
        <head>
        <meta charset=""UTF-8"">
        <meta http-equiv=""refresh"" content=""5"">
        <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
        <title>Server Data</title>
        <style>
        body {
            font-family: Arial, sans-serif;
            padding: 20px;
            background-color: #2C2C2C;
            color: #EAEAEA;
        }
        .box {
            max-width: 300px;
            margin: 50px auto;
            padding: 20px;
            border: 1px solid #555;
            border-radius: 10px;
            background-color: #3C3C3C;
            box-shadow: 0px 4px 8px rgba(0, 0, 0, 0.2);
        }
        ul {
            list-style-type: none;
            padding: 0;
        }
        li {
            margin-bottom: 10px;
        }
        a {
            color: #007BFF;
            text-decoration: none;
        }
        a:hover {
            text-decoration: underline;
        }
        </style>
        </head>
        <body>
        <div class=""box"">
        <h2>Data Request</h2>
        <ul>";

        private string part2 = @"</ul>
        </div>
        </body>
        </html>";

    }
}
