using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Charon_DNS_Changer.Core
{
    class Notif
    {
        //public string Message { get; set; }
        public static void NotificationCreator_INFO(string title, string message, int timesec)
        {
            NotifyIcon notification2 = new NotifyIcon();
            notification2.Visible = true;
            notification2.Icon = Properties.Resources.ICON;
            //notification2.Text = "Charon DNS Changer V2";
            notification2.BalloonTipIcon = ToolTipIcon.Info;
            notification2.BalloonTipTitle = title;
            notification2.BalloonTipText = message;
            notification2.ShowBalloonTip(timesec);
        }

        public static void NotificationCreator_ERROR(string title, string message, int timesec)
        {
            NotifyIcon notification2 = new NotifyIcon();
            notification2.Visible = true;
            notification2.Icon = Properties.Resources.ICON;
            notification2.BalloonTipIcon = ToolTipIcon.Error;
            notification2.BalloonTipTitle = title;
            notification2.BalloonTipText = message;
            notification2.ShowBalloonTip(timesec);
        }

    }
}
