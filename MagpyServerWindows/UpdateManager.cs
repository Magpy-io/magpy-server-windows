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

            // check is app installed
            if (!mgr.IsInstalled)
            {
                return; // app not installed
            }

            // check for new version
            var newVersion = await mgr.CheckForUpdatesAsync();
            if (newVersion == null)
            {
                return; // no update available
            }

            // download new version
            await mgr.DownloadUpdatesAsync(newVersion);

            // install new version and restart app
            mgr.ApplyUpdatesAndRestart(newVersion);
        }
    }
}
