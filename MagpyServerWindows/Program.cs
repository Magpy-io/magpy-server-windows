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
using Serilog;

namespace MagpyServerWindows
{
    public class Program
   {
        static async Task MainInner(string[] args)
        {
            LoggingManager.Init();
            Log.Logger = LoggingManager.LoggerWinApp;

            Log.Debug("Logging initialized.");

            UpdateManager.Init();

            Log.Debug("Updating setup finished.");

            try
            {
                await UpdateManager.UpdateMyApp();
            }
            catch (Exception ex)
            {
                Log.Debug(ex, "Error while trying to update server.");
            }

            Log.Debug("Looking for node executable.");

            bool nodefound = NodeManager.VerifyNodeExe();

            if (!nodefound)
            {
                throw new Exception("Node executable not found.");
            }

            Log.Debug("Node executable found.");
            Log.Debug("Staring node server.");

            NodeManager.StartNodeServer();

            Log.Debug("Node server started.");

            Process.GetCurrentProcess().Exited += Program_Exited;

            Log.Debug("Setting up NotificationIcon");
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
            Log.Debug("Program closing, Killing node server.");
            NodeManager.KillNodeServer();
        }
    }
}
