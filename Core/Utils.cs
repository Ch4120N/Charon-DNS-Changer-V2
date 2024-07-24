using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Management;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Telerik.WinControls.UI;

namespace Charon_DNS_Changer.Core
{
    class Utils
    {
        public static bool ExecuteCommand(string srcName, string args = null)
        {
            try
            {
                Process process = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = srcName,
                        Arguments = args,
                        UseShellExecute = true,
                        CreateNoWindow = true,
                        WindowStyle = ProcessWindowStyle.Hidden
                        //Verb = "runas"
                    }
                };
                process.Start();
                process.WaitForExit();
                int ReturnCode = process.ExitCode;
                if (ReturnCode == 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
                
            }
            catch
            {
                return false;
            }


        }


        private static string ExecuteCommand_RESAULT(string CommandName, string args)
        {
            var processInfo = new ProcessStartInfo(CommandName, args)
            {
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true,
                WindowStyle = ProcessWindowStyle.Hidden
            };

            using (var process = new Process())
            {
                process.StartInfo = processInfo;
                process.Start();
                string result = process.StandardOutput.ReadToEnd();
                process.WaitForExit();
                return result;
            }
        }

        public static string GetDns(string select)
        {
            List<string> otherChoice = new List<string>();
            Thread thread = new Thread(() =>
            {
                foreach (var key in Config.selected.Keys)
                {
                    otherChoice.Add(key);
                }
            });


            thread.Start();
            thread.Join();
            try
            {
                return Config.selected[otherChoice[int.Parse(select)]];
            }
            catch
            {
                return null;
            }
        }


        //public static void SetDns(string DNSName, string Interface, string primary, string secoundry)
        //{
        //    string Command = "netsh.exe";
        //    string ARGSNameServer1 = $"netsh interface ip set dns \"{Interface}\" static \"{primary}\"";
        //    string ARGSNameServer2 = $"netsh interface ip add dns \"{Interface}\" \"{secoundry}\" index=2";


        //    bool NameServer1 = ExecuteCommand(Command, ARGSNameServer1);
        //    bool NameServer2 = ExecuteCommand(Command, ARGSNameServer2);

        //    if (NameServer1 && NameServer2)
        //    {
        //        Notif.NotificationCreator_INFO("Charon DNS Changer", $"The DNS Has Been Changed To:\n{DNSName}", 2000);
        //    }
        //    else
        //    {
        //        Notif.NotificationCreator_ERROR("Charon DNS Changer", $"The DNS Not Changed To:\n{DNSName}\nSomething Happend.", 2000);
        //    }
        //}

        //public static string GetInterfaceName()
        //{
        //    if (IsLocalhost())
        //    {
        //        MessageBox.Show("Please connect to a network (LAN/Wi-Fi) !", "CONNECTION ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //        //Environment.Exit(1);
        //        return null;
        //    }
        //    else
        //    {
        //        string currentNetwork = ExecuteCommand_RESAULT("netsh.exe", "interface show interface");
        //        var currentNetworkLines = currentNetwork.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);

        //        foreach (var line in currentNetworkLines)
        //        {
        //            if (line.Contains("Enabled") && line.Contains("Connected"))
        //            {
        //                string lowerLine = line.ToLower();
        //                if (lowerLine.Contains("vm") || lowerLine.Contains("vir"))
        //                    continue;

        //                var interfaceList = line.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
        //                string connectedSsid = interfaceList.Last().Trim();
        //                //INTERFACE_SELECTED = connectedSsid;
        //                return connectedSsid;
        //            }
        //        }
        //    }
        //    return null;
        //}

        //private static bool IsLocalhost()
        //{
        //    string hostname = Dns.GetHostName();
        //    string ipAddress = Dns.GetHostAddresses(hostname)
        //                        .FirstOrDefault(ip => ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
        //                        ?.ToString();
        //    return ipAddress == "127.0.0.1";
        //}


        public static string GetFirstConnectedNetworkAdapterName()
        {
            string firstConnectedAdapterName = null;
            // Create a management object searcher to find all network adapters
            ManagementObjectSearcher searcher = new ManagementObjectSearcher(
                "SELECT * FROM Win32_NetworkAdapter WHERE NetConnectionId IS NOT NULL AND NetEnabled = TRUE");

            List<string> connectedAdapterNames = new List<string>();

            // Collecting all connected adapter names
            Parallel.ForEach(searcher.Get().Cast<ManagementObject>(), networkAdapter =>
            {
                int netConnectionStatus = Convert.ToInt32(networkAdapter["NetConnectionStatus"]);
                if (netConnectionStatus == 2 && !IsVirtualAdapter(networkAdapter)) // 2 means connected
                {
                    lock (connectedAdapterNames)
                    {
                        connectedAdapterNames.Add(networkAdapter["NetConnectionId"].ToString());
                    }
                }
            });

            // Get the first connected adapter name from the list
            firstConnectedAdapterName = connectedAdapterNames.FirstOrDefault();

            return firstConnectedAdapterName;
        }



        public static bool IsVirtualAdapter(ManagementObject adapter)
        {
            // Check if the adapter is a virtual adapter used by VMware or VirtualBox
            string description = adapter["Description"] as string;
            string name = adapter["Name"] as string;

            if (description != null && description.ToLower().Contains("vmware") ||
                name != null && (name.ToLower().Contains("virtualbox") || name.ToLower().Contains("vbox")))
            {
                return true;
            }

            // Additional checks can be added based on PNPDeviceID or other properties specific to virtual adapters

            return false;
        }


        public static bool IsConnectionValid(string networkAdapterName, out ManagementObject validAdapter)
        {
            validAdapter = null;
            ConcurrentDictionary<string, ManagementObject> validAdapters = new ConcurrentDictionary<string, ManagementObject>();

            // Create a management object searcher to find the network adapter
            ManagementObjectSearcher searcher = new ManagementObjectSearcher(
                "SELECT * FROM Win32_NetworkAdapter WHERE NetConnectionId IS NOT NULL AND NetEnabled = TRUE");

            bool isValidConnection = false;

            // Checking if the adapter is valid
            Parallel.ForEach(searcher.Get().Cast<ManagementObject>(), networkAdapter =>
            {
                if (networkAdapter["NetConnectionId"].ToString() == networkAdapterName)
                {
                    int netConnectionStatus = Convert.ToInt32(networkAdapter["NetConnectionStatus"]);
                    if (netConnectionStatus == 2) // 2 means connected
                    {
                        validAdapters.TryAdd(networkAdapterName, networkAdapter);
                        isValidConnection = true;
                    }
                }
            });

            // Retrieve the valid adapter from the dictionary
            validAdapters.TryGetValue(networkAdapterName, out validAdapter);

            return isValidConnection;
        }


        public static void GetNetworkAdapterNames()
        {
            // Create a management object searcher to find all network adapters
            ManagementObjectSearcher searcher = new ManagementObjectSearcher(
                "SELECT * FROM Win32_NetworkAdapter WHERE NetConnectionId != NULL");

            Console.WriteLine("Network Adapters:");
            foreach (ManagementObject networkAdapter in searcher.Get())
            {
                Console.WriteLine($"Name: {networkAdapter["NetConnectionId"]}");
            }
        }



        //public static void PrintAdapterInfo(ManagementObject adapter)
        //{
        //    // Print basic adapter information
        //    Console.WriteLine("Adapter Information:");
        //    Console.WriteLine($"Name: {adapter["Name"]}");
        //    Console.WriteLine($"Description: {adapter["Description"]}");
        //    Console.WriteLine($"MAC Address: {adapter["MACAddress"]}");
        //    Console.WriteLine($"Manufacturer: {adapter["Manufacturer"]}");
        //    Console.WriteLine($"NetConnectionID: {adapter["NetConnectionId"]}");

        //    // Get the associated network adapter configuration
        //    ManagementObjectSearcher configSearcher = new ManagementObjectSearcher(
        //        "SELECT * FROM Win32_NetworkAdapterConfiguration WHERE Index = " + adapter["Index"]);

        //    Parallel.ForEach(configSearcher.Get().Cast<ManagementObject>(), adapterConfig =>
        //    {
        //        if ((bool)adapterConfig["IPEnabled"])
        //        {
        //            Console.WriteLine("IP Address: " + string.Join(", ", (string[])adapterConfig["IPAddress"]));
        //            Console.WriteLine("DNS Servers: " + string.Join(", ", (string[])adapterConfig["DNSServerSearchOrder"] ?? new string[0]));
        //        }
        //    });
        //}
        public static string GetAdapterInfo(ManagementObject adapter)
        {
            StringBuilder info = new StringBuilder();

            // Append basic adapter information
            //info.AppendLine("Adapter Information:");
            info.AppendLine($"Name: {adapter["Name"]}");
            info.AppendLine($"Description: {adapter["Description"]}");
            info.AppendLine($"MAC Address: {adapter["MACAddress"]}");
            info.AppendLine($"Manufacturer: {adapter["Manufacturer"]}");
            info.AppendLine($"NetConnectionID: {adapter["NetConnectionId"]}");

            // Get the associated network adapter configuration
            ManagementObjectSearcher configSearcher = new ManagementObjectSearcher(
                "SELECT * FROM Win32_NetworkAdapterConfiguration WHERE Index = " + adapter["Index"]);

            Parallel.ForEach(configSearcher.Get().Cast<ManagementObject>(), adapterConfig =>
            {
                if ((bool)adapterConfig["IPEnabled"])
                {
                    lock (info)
                    {
                        info.AppendLine("IP Address: " + string.Join(", ", (string[])adapterConfig["IPAddress"]));
                        info.AppendLine("DNS Servers: " + string.Join(", ", (string[])adapterConfig["DNSServerSearchOrder"] ?? new string[0]));
                    }
                }
            });

            return info.ToString();
        }


        public static void SetDNSToAutomatic(string networkAdapterName)
        {
            // Create a management object searcher to find the network adapter
            ManagementObjectSearcher searcher = new ManagementObjectSearcher(
                "SELECT * FROM Win32_NetworkAdapter WHERE NetConnectionId != NULL");

            ManagementObjectCollection networkAdapters = searcher.Get();

            Parallel.ForEach(networkAdapters.Cast<ManagementObject>(), networkAdapter =>
            {
                if (networkAdapter["NetConnectionId"].ToString() == networkAdapterName)
                {
                    // Get the associated network adapter configuration
                    ManagementObjectSearcher configSearcher = new ManagementObjectSearcher(
                        "SELECT * FROM Win32_NetworkAdapterConfiguration WHERE Index = " + networkAdapter["Index"]);

                    foreach (ManagementObject adapterConfig in configSearcher.Get())
                    {
                        if ((bool)adapterConfig["IPEnabled"])
                        {
                            // Set DNS to obtain automatically via DHCP
                            ManagementBaseObject setDNS = adapterConfig.GetMethodParameters("SetDNSServerSearchOrder");
                            setDNS["DNSServerSearchOrder"] = null; // Setting to null sets DNS to automatic
                            ManagementBaseObject result = adapterConfig.InvokeMethod("SetDNSServerSearchOrder", setDNS, null);

                            if ((uint)result["ReturnValue"] != 0)
                            {
                                throw new Exception("Failed to set DNS to automatic.");
                            }
                        }
                    }
                }
            });
        }


        public static void SetDNSServers(string networkAdapterName, string[] dnsServers)
        {
            // Create a management object searcher to find the network adapter
            ManagementObjectSearcher searcher = new ManagementObjectSearcher(
                "SELECT * FROM Win32_NetworkAdapter WHERE NetConnectionId IS NOT NULL AND NetEnabled = TRUE");

            Parallel.ForEach(searcher.Get().Cast<ManagementObject>(), networkAdapter =>
            {
                if (networkAdapter["NetConnectionId"].ToString() == networkAdapterName)
                {
                    // Get the associated network adapter configuration
                    ManagementObjectSearcher configSearcher = new ManagementObjectSearcher(
                        "SELECT * FROM Win32_NetworkAdapterConfiguration WHERE Index = " + networkAdapter["Index"]);

                    Parallel.ForEach(configSearcher.Get().Cast<ManagementObject>(), adapterConfig =>
                    {
                        if ((bool)adapterConfig["IPEnabled"])
                        {
                            // Set the DNS servers
                            ManagementBaseObject setDNS = adapterConfig.GetMethodParameters("SetDNSServerSearchOrder");
                            setDNS["DNSServerSearchOrder"] = dnsServers;
                            ManagementBaseObject result = adapterConfig.InvokeMethod("SetDNSServerSearchOrder", setDNS, null);

                            if ((uint)result["ReturnValue"] != 0)
                            {
                                throw new Exception("Failed to set DNS servers.");
                            }
                        }
                    });
                }
            });
        }

        public static bool IsAdministrator()
        {
            var identity = System.Security.Principal.WindowsIdentity.GetCurrent();
            var principal = new System.Security.Principal.WindowsPrincipal(identity);
            return principal.IsInRole(System.Security.Principal.WindowsBuiltInRole.Administrator);
        }

    }
}
