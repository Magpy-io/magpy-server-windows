using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagpyServerWindows
{
    public class NodeEvents
    {
        public static string FormatEventSystrayExit()
        {
            var e = new { source = "NOTIFICAITON_ICON", name="EXIT" };
            return JsonConvert.SerializeObject(e);
        }

        public static string FormatEventSystrayAbout()
        {
            var e = new { source = "NOTIFICAITON_ICON", name = "ABOUT" };
            return JsonConvert.SerializeObject(e);
        }

        public static string FormatActionOpenWebInterface()
        {
            var e = new { source = "ACTION", name = "OPEN_WEB_INTERFACE" };
            return JsonConvert.SerializeObject(e);
        }
    }
}
