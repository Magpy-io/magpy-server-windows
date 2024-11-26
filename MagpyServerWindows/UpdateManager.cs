using Serilog;
using System;
using System.Threading.Tasks;
using Velopack;

using static MagpyServerWindows.Utils;

namespace MagpyServerWindows
{
    class UpdateManager
    {
        static readonly string UPDATE_URL = "https://magpy-update-win.s3.eu-west-3.amazonaws.com";

        static bool isUpdateRunning = false;

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
            if (isUpdateRunning)
            {
                return;
            }

            isUpdateRunning = true;

            try
            {
                var mgr = new Velopack.UpdateManager(UPDATE_URL);

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
            finally
            {
                isUpdateRunning = false;
            }
        }

        public static void SetupPeriodicUpdate()
        {
            // 12 hours
            int secondsDelay = 12 * 60 * 60;

            SchedulePeriodicTask(async () => {
                try
                {
                    await UpdateMyApp();
                }
                catch (Exception ex)
                {
                    Log.Debug(ex, "Error while trying to update server.");
                }
            }, secondsDelay);
        }
    }
}
