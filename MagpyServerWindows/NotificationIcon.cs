using System;
using System.Reflection;
using System.Windows.Forms;
using System.ComponentModel;

using static MagpyServerWindows.Constants;
using System.Threading.Tasks;
using System.Diagnostics;
using Serilog;

namespace MagpyServerWindows
{
    public class NotificationIcon
    {
        static private NotifyIcon notifyIcon;
        static private ContextMenu contextMenu;
        static private MenuItem menuItem1;
        static private MenuItem menuItem2;
        static private MenuItem menuItem3;

        public static void StartNotificationIcon()
        {
            menuItem1 = new MenuItem();
            menuItem1.Index = 0;
            menuItem1.Text = AppName + " v" + version;

            menuItem2 = new MenuItem();
            menuItem2.Index = 2;
            menuItem2.Text = "Exit";
            menuItem2.Click += new EventHandler(Exit_Clicked);

            menuItem3 = new MenuItem();
            menuItem3.Index = 1;
            menuItem3.Text = "Check for updates";
            menuItem3.Click += new EventHandler(CheckForUpdates_Clicked);


            contextMenu = new ContextMenu();
            contextMenu.MenuItems.AddRange(
                        new MenuItem[] { menuItem1, menuItem3, menuItem2 });
            
            notifyIcon = new NotifyIcon();
            notifyIcon.Icon = new System.Drawing.Icon(PathManager.RelativeExeToAbsolute(".\\appicon.ico"));
            notifyIcon.ContextMenu = contextMenu;
            notifyIcon.Text = AppName;
            notifyIcon.Visible = true;
            notifyIcon.DoubleClick += new EventHandler(MenuItem_DoubleClick);
        }

        static private async void Exit_Clicked(object Sender, EventArgs e)
        {
            try
            {
                NodeEvents.SendEventSystrayExit();
                await Task.Delay(500);
                Application.Exit();
            }
            catch (Exception ex)
            {
                Log.Debug(ex, "Error while trying to close server.");
            }
        }

        static private async void CheckForUpdates_Clicked(object Sender, EventArgs e)
        {
            try
            {
                await UpdateManager.UpdateMyApp();
            }
            catch (Exception ex)
            {
                Log.Debug(ex, "Error while trying to update server.");
            }
        }

        private static void MenuItem_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                ServerManager.OpenWebInterface();
            }
            catch (Exception ex)
            {
                Log.Debug(ex, "Error while trying to open server web interface.");
            }
        }
    }
}
