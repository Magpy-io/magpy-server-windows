using Serilog;
using Serilog.Extensions.Logging;
using System;
using System.IO;

namespace MagpyServerWindows
{
    class LoggingManager
    {
        public const string WIN_APP_LOGGING_FOLDER = "win";
        public const string INSTALLER_LOGGING_FOLDER = "installer";
        public const string NODE_LOGGING_FOLDER = "node";


        public static ILogger LoggerWinApp { get; private set; }
        public static ILogger LoggerInstaller { get; private set; }
        public static ILogger LoggerNode { get; private set; }

        public static void Init()
        {
#if DEBUG
            LoggerWinApp = createConsoleLogger();
            LoggerInstaller = createConsoleLogger();
            LoggerNode = createConsoleLogger();
#else
            LoggerWinApp = createFileLoggerInFolder(WIN_APP_LOGGING_FOLDER);
            LoggerInstaller = createFileLoggerInFolder(INSTALLER_LOGGING_FOLDER);
            LoggerNode = createFileLoggerInFolder(NODE_LOGGING_FOLDER);
#endif
        }

        private static ILogger createFileLoggerInFolder(string folder)
        {
            return new LoggerConfiguration()
               .MinimumLevel.Verbose()
               .WriteTo.File(PathManager.RelativeAppDataToAbsolute(Path.Combine("LogFiles", folder, "Log.txt")),
                   rollingInterval: RollingInterval.Day,
                   retainedFileTimeLimit: TimeSpan.FromDays(30),
                   outputTemplate: "[{Timestamp:yyyy-MM-dd HH:mm:ss.fff} [{Level}] {Message}{NewLine}{Exception}")
               .CreateLogger();
        }

        private static ILogger createConsoleLogger()
        {
            return new LoggerConfiguration()
               .MinimumLevel.Verbose()
               .WriteTo.Console()
               .CreateLogger();
        }

        public static Microsoft.Extensions.Logging.ILogger SerilogToMicrosoftLogger(ILogger logger)
        {
            return new SerilogLoggerFactory(logger).CreateLogger("Serilog");
        }
    }
}
