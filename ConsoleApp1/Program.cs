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
            if (args.Length == 0)
            {
                Application.ApplicationExit += Application_ApplicationExit;
                child = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = ".\\ConsoleApp1.exe",
                        Arguments = "arg",
                        UseShellExecute = false,
                        RedirectStandardInput = true,
                        RedirectStandardOutput = true,
                        RedirectStandardError = true,
                    }
                };
                child.Start();
                child.StandardInput.WriteLine("Hello world!\n");
            }
            else
            {
                string s;
                int a;
                FileStream logstream = File.OpenWrite(".\\childLog.txt");
                while (true)
                {
                    if((a = Console.OpenStandardInput().ReadByte()) != -1)
                    {
                        logstream.WriteByte((byte)a);
                        logstream.Flush();
                    }
                    Thread.Sleep(10);
                }
                if(a == 'H')
                {
                    NotificationIcon.StartNotificationIcon();
                }
            }

            Application.Run();
        }

        private static void Application_ApplicationExit(object sender, EventArgs e)
        {
            if (child != null)
            {
                child.Kill();
            }
        }
    }
}
