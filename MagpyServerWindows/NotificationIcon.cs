using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MagpyServerWindows
{
    public class NotificationIcon
    {
        static private System.Windows.Forms.NotifyIcon notifyIcon1;
        static private System.Windows.Forms.ContextMenu contextMenu1;
        static private System.Windows.Forms.MenuItem menuItem1;
        static private System.Windows.Forms.MenuItem menuItem2;
        static private System.ComponentModel.IContainer components;

        static string AppName = System.Reflection.Assembly.GetExecutingAssembly().GetName().Name;

        public static void StartNotificationIcon()
        {
            components = new System.ComponentModel.Container();
            contextMenu1 = new System.Windows.Forms.ContextMenu();
            menuItem1 = new System.Windows.Forms.MenuItem();
            menuItem2 = new System.Windows.Forms.MenuItem();

            // Initialize contextMenu1
            contextMenu1.MenuItems.AddRange(
                        new System.Windows.Forms.MenuItem[] { menuItem1, menuItem2 });

            // Initialize menuItem1
            menuItem1.Index = 0;
            menuItem1.Text = "Exit";
            menuItem1.Click += new System.EventHandler(menuItem1_Click);


            Assembly assembly = Assembly.GetExecutingAssembly();

            string version = assembly.GetName().Version.ToString(3);

            // Initialize menuItem1
            menuItem2.Index = 1;
            menuItem2.Text = AppName + " v" + version;

            // Create the NotifyIcon.
            notifyIcon1 = new NotifyIcon(components);

            // The Icon property sets the icon that will appear
            // in the systray for this application.
            
            notifyIcon1.Icon = new System.Drawing.Icon(PathManager.RelativeExeToAbsolute(".\\appicon.ico"));

            // The ContextMenu property sets the menu that will
            // appear when the systray icon is right clicked.
            notifyIcon1.ContextMenu = contextMenu1;

            // The Text property sets the text that will be displayed,
            // in a tooltip, when the mouse hovers over the systray icon.

            notifyIcon1.Text = AppName;
            notifyIcon1.Visible = true;

            // Handle the DoubleClick event to activate the form.
            notifyIcon1.DoubleClick += new System.EventHandler(MenuItem2_Click);
        }

        private static void MenuItem2_Click(object sender, EventArgs e)
        {
            ServerManager.OpenWebInterface();
        }

        static private void menuItem1_Click(object Sender, EventArgs e)
        {
            if(Program.child != null)
            {
                Program.child.StandardInput.WriteLine(NodeEvents.FormatEventSystrayExit());
            }
        }
    }
}
