using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ConsoleApp1
{
    public class NotificationIcon
    {
        static private System.Windows.Forms.NotifyIcon notifyIcon1;
        static private System.Windows.Forms.ContextMenu contextMenu1;
        static private System.Windows.Forms.MenuItem menuItem1;
        static private System.ComponentModel.IContainer components;

        public static void StartNotificationIcon()
        {
            components = new System.ComponentModel.Container();
            contextMenu1 = new System.Windows.Forms.ContextMenu();
            menuItem1 = new System.Windows.Forms.MenuItem();

            // Initialize contextMenu1
            contextMenu1.MenuItems.AddRange(
                        new System.Windows.Forms.MenuItem[] { menuItem1 });

            // Initialize menuItem1
            menuItem1.Index = 0;
            menuItem1.Text = "Exit";
            menuItem1.Click += new System.EventHandler(menuItem1_Click);

            // Create the NotifyIcon.
            notifyIcon1 = new System.Windows.Forms.NotifyIcon(components);

            // The Icon property sets the icon that will appear
            // in the systray for this application.
            notifyIcon1.Icon = new System.Drawing.Icon("appicon.ico");

            // The ContextMenu property sets the menu that will
            // appear when the systray icon is right clicked.
            notifyIcon1.ContextMenu = contextMenu1;

            // The Text property sets the text that will be displayed,
            // in a tooltip, when the mouse hovers over the systray icon.
            notifyIcon1.Text = "Magpy";
            notifyIcon1.Visible = true;

            // Handle the DoubleClick event to activate the form.
            notifyIcon1.DoubleClick += new System.EventHandler(menuItem1_Click);
        }

        static private void menuItem1_Click(object Sender, EventArgs e)
        {
            Program.child.StandardInput.WriteLine("[Event] exit clicked");
        }
    }
}
