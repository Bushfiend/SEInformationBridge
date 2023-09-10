using NLog;
using ParallelTasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Torch;
using Torch.API;
using Task = System.Threading.Tasks.Task;

namespace SEInformationBridge
{
    public class Plugin : TorchPluginBase
    {

        public static ITorchBase TorchInstance;

        public static readonly Logger Log = LogManager.GetCurrentClassLogger();
        public static bool Setup = false;


        public override void Init(ITorchBase torch)
        {
            base.Init(torch);
            TorchInstance = torch;

            Log.Info("Information Bridge Plugin Loaded.");
            RunServer();

        }

        public static void RunServer()
        {
            var server = new HttpServer(8080);
            Task.Run(() => server.StartAsync());
        }

        public override void Update()
        {
            base.Update();
            if (Torch.CurrentSession == null)
                return;

            if(!Setup)
            {
                PlayerGrids.Setup();
                ChatLog.Setup();
                Setup = true;
            }
            
  

            
   

        }


    }
}
