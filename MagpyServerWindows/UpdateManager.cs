using Serilog;
using System;
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
                   NodeManager.KillNodeServer();
                   LoggingManager.DisposeLoggers();
                   PathManager.ClearAppDataFolder();
                   AutoStartupSetup.DisableAutoStartup();
               })
               .WithFirstRun((v) => {
                   ServerManager.OpenWebInterface();
               })
           .Run(LoggingManager.SerilogToMicrosoftLogger(LoggingManager.LoggerInstaller));
        }

        public static async Task UpdateMyApp()
        {
            var mgr = new Velopack.UpdateManager("https://magpy-update-win.s3.eu-west-3.amazonaws.com");

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

        public static async Task SetupDelayedUpdate(int minutes = 3)
        {
            int millisecondsDelay = minutes * 1000 * 60;

            await Task.Delay(millisecondsDelay);

            try
            {
                await UpdateMyApp();
            }
            catch (Exception ex)
            {
                Log.Debug(ex, "Error while trying to update server.");
            }
        }
    }
}
