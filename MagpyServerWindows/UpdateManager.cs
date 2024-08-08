using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Velopack;

namespace MagpyServerWindows
{
    class UpdateManager
    {
        public static void Init()
        {
            VelopackApp.Build()
               .WithAfterInstallFastCallback((v) =>
               {
                   AutoStartupSetup.EnableAutoStartup();
               })
               .WithBeforeUninstallFastCallback((v) =>
               {
                   AutoStartupSetup.DisableAutoStartup();
               })
               .WithFirstRun((v) => {
                   ServerManager.OpenWebInterface();
               })
           .Run(LoggingManager.SerilogToMicrosoftLogger(LoggingManager.LoggerInstaller));
        }

        public static async Task UpdateMyApp()
        {
            var mgr = new Velopack.UpdateManager("E:\\Libraries\\Documents\\Projects\\MagpyServerWindows\\Releases");

            Log.Debug("Checking for updates.");

            // check is app installed
            if (!mgr.IsInstalled)
            {
                Log.Debug("App is not installed.");
                return; // app not installed
            }

            // check for new version
            var newVersion = await mgr.CheckForUpdatesAsync();
            if (newVersion == null)
            {
                Log.Debug("No new version.");
                return; // no update available
            }

            Log.Debug("Downloading new version.");
            // download new version
            await mgr.DownloadUpdatesAsync(newVersion);

            Log.Debug("Restarting and applying new version.");
            // install new version and restart app
            mgr.ApplyUpdatesAndRestart(newVersion);
        }
    }
}
