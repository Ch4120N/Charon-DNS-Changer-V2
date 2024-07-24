using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Telerik.WinControls.UI;
using Charon_DNS_Changer.Core;

namespace Charon_DNS_Changer
{
    public partial class About_Us : RadForm
    {
        private CharonPlayerSounds.CharonPlayer2 ch_player = new CharonPlayerSounds.CharonPlayer2();
        public About_Us()
        {
            InitializeComponent();
            richTextBox_about_us.Text = AboutUsConfig.about_us_content();
            ch_player.LoadSound(Properties.Resources.ab_us);
            ch_player.PlaySound();
        }

        private void About_Us_FormClosed(object sender, FormClosedEventArgs e)
        {
            ch_player.StopSound();
        }
    }
}
