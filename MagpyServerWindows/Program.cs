using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Velopack;

namespace MagpyServerWindows
{
    public class Program
   {
        static async Task MainInner(string[] args)
        {
            LoggingManager.Init();

            UpdateManager.Init();

            await UpdateManager.UpdateMyApp();

            bool nodefound = NodeManager.VerifyNodeExe();

            if (!nodefound)
            {
                throw new Exception("Node executable not found.");
            }

            NodeManager.StartNodeServer();

            Process.GetCurrentProcess().Exited += Program_Exited;

            NotificationIcon.StartNotificationIcon();

            Application.Run();
        }

        static async Task Main(string[] args)
        {
            try
            {
                bool instanceCreated = InstanceManager.HoldInstance();
                if (instanceCreated)
                {
                    await MainInner(args);
                }
                else
                {
                    ServerManager.OpenWebInterface();
                    Application.Exit();
                }
            }
            catch (Exception e)
            {
                LoggingManager.LoggerWinApp.Error(e, "Error launching app");
#if DEBUG
                Console.ReadKey();
#endif
            }
            finally
            {
                InstanceManager.ReleaseInstance();
            }
        }

        private static void Program_Exited(object sender, EventArgs e)
        {
            NodeManager.KillNodeServer();
        }
    }
}
