using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace SEInformationBridge
{
    public static class Utilities
    {

        public static string Serialize<T>(List<T> list)
        {
            if (Plugin.TorchInstance.CurrentSession == null)
                return "Server not running.";

            return JsonSerializer.Serialize(list, new JsonSerializerOptions { WriteIndented = true });
        }


    }
}
