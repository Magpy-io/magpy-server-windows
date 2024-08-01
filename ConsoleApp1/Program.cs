using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ConsoleApp1
{  
    internal class Program
    {
        static Process child;

        static void Main(string[] args)
        {
            Application.ApplicationExit += Application_ApplicationExit;
            Process.GetCurrentProcess().Exited += Program_Exited;

            NotificationIcon.StartNotificationIcon();

            child = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = ".\\node.exe",
                    Arguments = ".\\script.js",
                    UseShellExecute = false,
                    RedirectStandardInput = true,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    CreateNoWindow = true,

                }
            };
            child.Start();
            child.EnableRaisingEvents = true;
            child.Exited += Child_Exited;

            child.BeginOutputReadLine();
            child.OutputDataReceived += Child_OutputDataReceived;
            child.StandardInput.WriteLine("Hello world!\n");

            Application.Run();
        }

        private static void Program_Exited(object sender, EventArgs e)
        {
            if (child != null && !child.HasExited)
            {
                child.Kill();
            }
        }

        private static void Child_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            Console.WriteLine(e.Data);
        }

        private static void Child_Exited(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private static void Application_ApplicationExit(object sender, EventArgs e)
        {
            if (child != null && !child.HasExited)
            {
                child.Kill();
            }
        }
    }
}
