using Newtonsoft.Json;

namespace MagpyServerWindows
{
    public class NodeEvents
    {
        private static string FormatEventSystrayExit()
        {
            var e = new { source = "NOTIFICAITON_ICON", name="EXIT" };
            return JsonConvert.SerializeObject(e);
        }

        private static string FormatEventSystrayAbout()
        {
            var e = new { source = "NOTIFICAITON_ICON", name = "ABOUT" };
            return JsonConvert.SerializeObject(e);
        }

        private static string FormatActionOpenWebInterface()
        {
            var e = new { source = "ACTION", name = "OPEN_WEB_INTERFACE" };
            return JsonConvert.SerializeObject(e);
        }

        public static void SendEventSystrayExit()
        {
            NodeManager.SendData(FormatEventSystrayExit());
        }
    }
}
