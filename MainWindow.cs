using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Telerik.WinControls.UI;
using Charon_DNS_Changer.Core;
using System.Management;

namespace Charon_DNS_Changer
{
    public partial class MainWindow : RadForm
    {
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HTCAPTION = 0x2;

        [DllImport("User32.dll")]
        public static extern bool ReleaseCapture();

        [DllImport("User32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);


        private bool DefaultDNS = false;
        private bool CMMode = false;
        public string CMprimary, CMsecoundry;
        private string DNSChoose = null;
        private int DNSIndex;
        private CharonPlayerSounds.CharonPlayer ch_player = new CharonPlayerSounds.CharonPlayer();
        //private string Interface;
        public MainWindow()
        {
            InitializeComponent();
            SetPanelOpacity(0.7f);
            DNS_Lists.DropDownListElement.TextBox.TextAlign = HorizontalAlignment.Center;

            //this.ChangeDNSBTN.ButtonElement.EnableElementShadow = false;
            //this.ChangeDNSBTN.ButtonElement.EnableHighlight = false;
            //this.ChangeDNSBTN.ButtonElement.EnableRippleAnimation = false;
            //this.ChangeDNSBTN.ButtonElement.EnableFocusBorder = false;
            //this.ChangeDNSBTN.ButtonElement.EnableBorderHighlight = false;
            this.ChangeDNSBTN.ButtonElement.ButtonFillElement.GradientStyle = Telerik.WinControls.GradientStyles.Solid;
            this.ChangeDNSBTN.ButtonElement.BorderElement.ForeColor = Color.DimGray;
            this.ChangeDNSBTN.ButtonElement.BorderElement.Visibility = Telerik.WinControls.ElementVisibility.Visible;
            this.ChangeDNSBTN.ButtonElement.BorderElement.ForeColor2 = Color.DimGray;
            this.ChangeDNSBTN.ButtonElement.BorderElement.ForeColor3 = Color.DimGray;
            this.ChangeDNSBTN.ButtonElement.BorderElement.ForeColor4 = Color.DimGray;

            Logdata.ReadOnly = true;
            Logdata.ForeColor = Color.LimeGreen; // Default text color
            Logdata.TabStop = false;
            Logdata.KeyPress += (sender, e) => e.Handled = true;
            Logdata.Enter += (sender, e) => Logdata.Parent.Focus();
            Logdata.Cursor = Cursors.Default;

        }

        private void AddColoredText(List<object> parts)
        {
            parts.ForEach(part =>
            {
                if (part is Color)
                {
                    Logdata.SelectionColor = (Color)part;
                }
                else if (part is string)
                {
                    Logdata.AppendText((string)part);
                }
            });

            Logdata.SelectionColor = Logdata.ForeColor; // Reset color to default
        }


        private void MainWindow_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HTCAPTION, 0);
            }
        }

        private void MainWindow_Paint(object sender, PaintEventArgs e)
        {
            int cornerRadius = 20; // Adjust as needed
            System.Drawing.Drawing2D.GraphicsPath path = new System.Drawing.Drawing2D.GraphicsPath();
            path.AddArc(new System.Drawing.Rectangle(0, 0, cornerRadius * 2, cornerRadius * 2), 180, 90);
            path.AddArc(new System.Drawing.Rectangle(this.Width - cornerRadius * 2, 0, cornerRadius * 2, cornerRadius * 2), -90, 90);
            path.AddArc(new System.Drawing.Rectangle(this.Width - cornerRadius * 2, this.Height - cornerRadius * 2, cornerRadius * 2, cornerRadius * 2), 0, 90);
            path.AddArc(new System.Drawing.Rectangle(0, this.Height - cornerRadius * 2, cornerRadius * 2, cornerRadius * 2), 90, 90);
            path.CloseFigure();

            // Apply the rounded rectangle to the form's border
            this.Region = new System.Drawing.Region(path);
        }

        private void btn_close_Click(object sender, EventArgs e)
        {
            Process.GetCurrentProcess().Kill();
            Environment.Exit(0);
        }

        private void btn_minimize_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void SetPanelOpacity(float opacity)
        {
            if (opacity < 0 || opacity > 1)
                throw new ArgumentOutOfRangeException("opacity", "Opacity must be between 0 and 1.");

            Color color = MainPanel.BackColor;
            MainPanel.BackColor = Color.FromArgb((int)(opacity * 255), color.R, color.G, color.B);
        }

        private void radPanel1_Paint(object sender, PaintEventArgs e)
        {
            int cornerRadius = 20; // Adjust as needed
            System.Drawing.Drawing2D.GraphicsPath path = new System.Drawing.Drawing2D.GraphicsPath();
            path.AddArc(new System.Drawing.Rectangle(0, 0, cornerRadius * 2, cornerRadius * 2), 180, 90);
            path.AddArc(new System.Drawing.Rectangle(MainPanel.Width - cornerRadius * 2, 0, cornerRadius * 2, cornerRadius * 2), -90, 90);
            path.AddArc(new System.Drawing.Rectangle(MainPanel.Width - cornerRadius * 2, MainPanel.Height - cornerRadius * 2, cornerRadius * 2, cornerRadius * 2), 0, 90);
            path.AddArc(new System.Drawing.Rectangle(0, MainPanel.Height - cornerRadius * 2, cornerRadius * 2, cornerRadius * 2), 90, 90);
            path.CloseFigure();

            // Apply the rounded rectangle to the form's border
            MainPanel.Region = new System.Drawing.Region(path);
        }

        private void CustomMode_ValueChanged(object sender, EventArgs e)
        {
            if (CustomMode.Value)
            {
                CustomTB.Visible = true;
                CMMode = true;
                DefaultDNS = false;
                DHCPDefault.Value = false;
                DNS_Lists.Enabled = false;
                DHCPDefault.Enabled = false;
            }
            else
            {
                CMMode = false;
                DefaultDNS = false;
                DHCPDefault.Value = false;
                CustomTB.Visible = false;
                DNS_Lists.Enabled = true;
                DHCPDefault.Enabled = true;
            }
        }

        private void DHCPDefault_ValueChanged(object sender, EventArgs e)
        {
            if (DHCPDefault.Value)
            {
                DefaultDNS = true;
                CMMode = false;
                CustomMode.Value = false;
                DNS_Lists.Enabled = false;
                CustomMode.Enabled = false;
                CustomTB.Enabled = false;
            }
            else
            {
                DefaultDNS = false;
                CMMode = false;
                CustomMode.Value = false;
                DNS_Lists.Enabled = true;
                CustomMode.Enabled = true;
                CustomTB.Enabled = true;
            }
        }

        private void DNS_Lists_TextChanged(object sender, EventArgs e)
        {
            DNSChoose = DNS_Lists.Text;
            //int selectedIndex = DNS_Lists.SelectedIndex;
            //MessageBox.Show(DNSChoose);
        }

        private void DNS_Lists_VisualListItemFormatting(object sender, VisualItemFormattingEventArgs args)
        {
            args.VisualItem.TextAlignment = ContentAlignment.MiddleCenter;
        }

        private void DNS_Lists_SelectedIndexChanged(object sender, Telerik.WinControls.UI.Data.PositionChangedEventArgs e)
        {
            RadDropDownList radDropDownList = sender as RadDropDownList;
            int selectedIndex = radDropDownList.SelectedIndex;
            DNSIndex = selectedIndex;
        }

        private void ChangeDNSBTN_Click(object sender, EventArgs e)
        {
            //waitingBar.Visible = true;
            //waitingBar.StartWaiting();
            //waitingBar.Text = "Please wait...";
            DNS_Lists.Enabled = false;
            CustomMode.Enabled = false;
            CustomTB.Enabled = false;
            DHCPDefault.Enabled = false;

            Logdata.Clear();

            if (Utils.IsAdministrator())
            {
                ch_player.PlaySound(Properties.Resources.start);
                try
                {
                    string networkAdapterName = Utils.GetFirstConnectedNetworkAdapterName();
                    if (networkAdapterName == null)
                    {
                        AddColoredText(new List<object> { Color.Blue, "[", Color.Red, "-", Color.Blue, "]", Color.Red, " No connected network adapters found.\n" });
                        ch_player.PlaySound(Properties.Resources.error);
                        //Console.WriteLine("No connected network adapters found.");
                        DNS_Lists.Enabled = true;
                        CustomMode.Enabled = true;
                        CustomTB.Enabled = true;
                        DHCPDefault.Enabled = true;
                        CustomMode.Value = false;
                        DHCPDefault.Value = false;
                        return;
                    }

                    //Console.WriteLine($"Using network adapter: {networkAdapterName}");
                    AddColoredText(new List<object> { Color.Blue, "[", Color.LimeGreen, "+", Color.Blue, "]", Color.LimeGreen, $" Using network adapter: {networkAdapterName}\n" });
                    ManagementObject adapter;

                    if (Utils.IsConnectionValid(networkAdapterName, out adapter))
                    {
                        AddColoredText(new List<object> { Color.Blue, "[", Color.LimeGreen, "+", Color.Blue, "]", Color.LimeGreen, $" Connection is valid for adapter: {networkAdapterName}\n" });

                        AddColoredText(new List<object> { Color.Blue, "[", Color.LimeGreen, "+", Color.Blue, "]", Color.LimeGreen, $" Adapter Information:\n", Color.Cyan, $"{Utils.GetAdapterInfo(adapter)}\n" });
                        //;
                    }
                    else
                    {

                        AddColoredText(new List<object> { Color.Blue, "[", Color.Red, "-", Color.Blue, "]", Color.Red, $" Connection is not valid for adapter: {networkAdapterName}\n" });
                        ch_player.PlaySound(Properties.Resources.error);
                        DNS_Lists.Enabled = true;
                        CustomMode.Enabled = true;
                        CustomTB.Enabled = true;
                        DHCPDefault.Enabled = true;
                        CustomMode.Value = false;
                        DHCPDefault.Value = false;
                        return;
                        //Console.WriteLine($"Connection is not valid for adapter: {networkAdapterName}");
                    }



                    if (DefaultDNS)
                    {
                        Utils.SetDNSToAutomatic(networkAdapterName);
                        AddColoredText(new List<object> { Color.Blue, "[", Color.Yellow, "!", Color.Blue, "]", Color.LimeGreen, $" All DNS changed to default (DHCP).\n" });
                        Notif.NotificationCreator_INFO("", "All DNS changed to default (DHCP).", 2000);
                        ch_player.PlaySound(Properties.Resources.done);
                        DNS_Lists.Enabled = true;
                        CustomMode.Enabled = true;
                        CustomTB.Enabled = true;
                        DHCPDefault.Enabled = true;
                        CustomMode.Value = false;
                        DHCPDefault.Value = false;
                        return;
                    }
                    else if (CMMode)
                    {
                        string[] ipArray = CustomTB.Text.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                        List<string> ipList = new List<string>();
                        Parallel.ForEach(ipArray, ip =>
                        {
                            ipList.Add(ip.Trim());
                        });



                        if (ipList.Count > 2)
                        {
                            AddColoredText(new List<object> { Color.Blue, "[", Color.Red, "-", Color.Blue, "]", Color.Red, $" Please enter only two DNS IPs\n" });
                            ch_player.PlaySound(Properties.Resources.error);
                            DNS_Lists.Enabled = true;
                            CustomMode.Enabled = true;
                            CustomTB.Enabled = true;
                            DHCPDefault.Enabled = true;
                            CustomMode.Value = false;
                            DHCPDefault.Value = false;
                            return;
                        }
                        else
                        {
                            if (ipList.Count > 0)
                            {
                                CMprimary = ipList[0];
                            }
                            if (ipList.Count > 1)
                            {
                                CMsecoundry = ipList[1];
                            }

                            string[] dnsServers = { CMprimary, CMsecoundry };
                            Utils.SetDNSServers(networkAdapterName, dnsServers);
                            AddColoredText(new List<object> { Color.Blue, "[", Color.Yellow, "!", Color.Blue, "]", Color.LimeGreen, $" Custom DNS servers have been set successfully.\n" });
                            Notif.NotificationCreator_INFO("", "Custom DNS servers have been set successfully.", 2000);
                            ch_player.PlaySound(Properties.Resources.done);
                            DNS_Lists.Enabled = true;
                            CustomMode.Enabled = true;
                            CustomTB.Enabled = true;
                            DHCPDefault.Enabled = true;
                            CustomMode.Value = false;
                            DHCPDefault.Value = false;
                            return;
                        }
                    }
                    else
                    {
                        string DNSNameSelected = Utils.GetDns(DNSIndex.ToString());

                        var NameServer1 = Config.DNS_DICTIONERY[DNSNameSelected]["index1"];
                        var NameServer2 = Config.DNS_DICTIONERY[DNSNameSelected]["index2"];


                        //MessageBox.Show($"{DNSNameSelected} => {NameServer1}, {NameServer2}");
                        string[] dnsServersSelected = { NameServer1, NameServer2 };


                        Utils.SetDNSServers(networkAdapterName, dnsServersSelected);
                        AddColoredText(new List<object> { Color.Blue, "[", Color.Yellow, "!", Color.Blue, "]", Color.LimeGreen, $" DNS servers have been set successfully.\n" });
                        Notif.NotificationCreator_INFO("", "DNS servers have been set successfully.", 2000);
                        ch_player.PlaySound(Properties.Resources.done);
                        DNS_Lists.Enabled = true;
                        CustomMode.Enabled = true;
                        CustomTB.Enabled = true;
                        DHCPDefault.Enabled = true;
                        CustomMode.Value = false;
                        DHCPDefault.Value = false;
                        return;

                    }
                }
                catch (Exception ex)
                {
                    AddColoredText(new List<object> { Color.Blue, "[", Color.Red, "-", Color.Blue, "]", Color.Red, " Something is wrong\n" });
                    AddColoredText(new List<object> { Color.Blue, "[", Color.Red, "-", Color.Blue, "]", Color.Red, $" {ex.Message}\n" });
                    Notif.NotificationCreator_ERROR("", $"Something is wrong: {ex.Message}", 2000);
                    ch_player.PlaySound(Properties.Resources.error);
                    DNS_Lists.Enabled = true;
                    CustomMode.Enabled = true;
                    CustomTB.Enabled = true;
                    DHCPDefault.Enabled = true;
                    CustomMode.Value = false;
                    DHCPDefault.Value = false;
                    return;
                }

            }
            else
            {
                AddColoredText(new List<object> { Color.Blue, "[", Color.Red, "-", Color.Blue, "]", Color.Red, " Please run this program with Administrator user.\n" });
                Notif.NotificationCreator_ERROR("", "Please run this program with Administrator user", 2000);
                ch_player.PlaySound(Properties.Resources.error);
                DNS_Lists.Enabled = true;
                CustomMode.Enabled = true;
                CustomTB.Enabled = true;
                DHCPDefault.Enabled = true;
                CustomMode.Value = false;
                DHCPDefault.Value = false;
                return;
            }


        }

        private void radMenuItem1_Click(object sender, EventArgs e)
        {
            About_Us ab_us = new About_Us();
            ab_us.Show();
        }

        private void radMenuItem2_Click(object sender, EventArgs e)
        {
            string url = "https://github.com/Ch4120N/Charon-DNS-Changer-v2";

            try
            {
                // Start the default browser with the specified URL
                Process.Start(url);
            }
            catch (Exception ex)
            {
                // Handle any errors that occur
                MessageBox.Show("An error occurred: " + ex.Message);
            }
        }

        private void Logdata_TextChanged(object sender, EventArgs e)
        {
            Logdata.SelectionStart = Logdata.Text.Length;
            Logdata.ScrollToCaret();
        }

        //public void OptionSelected(string AdapterName)
        //{
        //    try
        //    {
        //        string DNSName = Utils.GetDns(DNSIndex.ToString());

        //        var NameServer1 = Config.DNS_DICTIONERY[DNSName]["index1"];
        //        var NameServer2 = Config.DNS_DICTIONERY[DNSName]["index2"];

        //        string[] dnsServers = { NameServer1, NameServer2 };

        //        Utils.SetDNSServers(AdapterName, dnsServers);

        //    }
        //    catch
        //    {

        //    }


            //MessageBox.Show(DNSName);
            //AddColoredText(new List<object> { Color.Green, "[", Color.White, "+", Color.Green, "] ", Color.LimeGreen, " Message" });


            //Interface = Utils.GetInterfaceName();

            //Utils.SetDns(DNSName, Interface, "", "");


            //MessageBox.Show($"{DNSName} => {NameServer1}, {NameServer2}");
            //Parallel.ForEach(Config.DNS_DICTIONERY, outerDic =>
            //{
            //    Parallel.ForEach(outerDic.Value, Inner =>
            //    {
            //        MessageBox.Show($"DNS Name: {outerDic.Key} => {Inner.Key} : {Inner.Value}");
            //    });
            //});

            //var charonDNS1Index1 = Config.DNS_DICTIONERY["Charon Security Agency DNS1"]["index1"];
            //var charonDNS1Index2 = Config.DNS_DICTIONERY["Charon Security Agency DNS1"]["index2"];

            //MessageBox.Show($"Charon Security Agency DNS1 => {charonDNS1Index1}, {charonDNS1Index2}");
            //if (Config.DNS_DICTIONERY.TryGetValue(select, out var indexs))
            //{
            //    int index1 = -1;
            //    int index2 = -1;


            //    return new string[] { index1, index2 };
            //}
            //else
            //{
            //    // Handle the case where the select key is not found
            //    return new string[] { "", "" };
            //}
        

    }
}
