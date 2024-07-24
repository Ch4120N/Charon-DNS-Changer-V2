using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Charon_DNS_Changer.Core
{
    class Config
    {
        public static Dictionary<string, string> selected = new Dictionary<string, string>()
        {
            //{ "1", "Ch4120N DNS" },
            //{ "2", "Charon Security Agency DNS1" },
            //{ "3", "Charon Security Agency DNS2" },
            //{ "4", "Shecan DNS" },
            //{ "5", "Electro DNS" },
            //{ "6", "403.online" },
            //{ "7", "Google Public DNS" },
            //{ "8", "Quad9" },
            //{ "9", "OpenDNS Home" },
            //{ "10", "Cloudflare DNS" },
            //{ "11", "Comodo Secure DNS" },
            //{ "12", "CleanBrowsing" },
            //{ "13", "Alternate DNS" },
            //{ "14", "AdGuard DNS" },
            //{ "15", "Verisign" },
            //{ "16", "OpenNIC" },
            //{ "17", "Yandex DNS" },
            //{ "18", "DNS.Watch" },
            //{ "19", "Level 3" },
            //{ "20", "Oracle Dyn" },
            //{ "21", "UncensoredDNS" },
            //{ "22", "Neustar Security Services" },
            //{ "23", "GreenTeamDNS" },
            //{ "24", "SafeDNS" }
            { "0", "Ch4120N DNS" },
            { "1", "Charon Security Agency DNS1" },
            { "2", "Charon Security Agency DNS2" },
            { "3", "Shecan DNS" },
            { "4", "Electro DNS" },
            { "5", "403.online" },
            { "6", "Google Public DNS" },
            { "7", "Quad9" },
            { "8", "OpenDNS Home" },
            { "9", "Cloudflare DNS" },
            { "10", "Comodo Secure DNS" },
            { "11", "CleanBrowsing" },
            { "12", "Alternate DNS" },
            { "13", "AdGuard DNS" },
            { "14", "Verisign" },
            { "15", "OpenNIC" },
            { "16", "Yandex DNS" },
            { "17", "DNS.Watch" },
            { "18", "Level 3" },
            { "19", "Oracle Dyn" },
            { "20", "UncensoredDNS" },
            { "21", "Neustar Security Services" },
            { "22", "GreenTeamDNS" },
            { "23", "SafeDNS" }
        };


        public static Dictionary<string, Dictionary<string, string>> DNS_DICTIONERY = new Dictionary<string, Dictionary<string, string>>()
        {
            {"Ch4120N DNS", new Dictionary<string, string>()
                {
                    {"index1", "78.157.42.100"},
                    {"index2", "10.202.10.11"}
                }
            },
            {"Charon Security Agency DNS1", new Dictionary<string, string>()
                {
                    {"index1", "178.22.122.100"},
                    {"index2", "1.1.1.1"}
                }
            },
            {"Charon Security Agency DNS2", new Dictionary<string, string>()
                {
                    {"index1", "78.157.42.101"},
                    {"index2", "1.1.1.1"}
                }
            },
            {"Shecan DNS", new Dictionary<string, string>()
                {
                    {"index1", "185.51.200.2"},
                    {"index2", "178.22.122.100"}
                }
            },
            {"Electro DNS", new Dictionary<string, string>()
                {
                    {"index1", "78.157.42.100"},
                    {"index2", "78.157.42.101"}
                }
            },
            {"403.online", new Dictionary<string, string>()
                {
                    {"index1", "10.202.10.202"},
                    {"index2", "10.202.10.102"}
                }
            },
            {"Google Public DNS", new Dictionary<string, string>()
                {
                    {"index1", "8.8.8.8"},
                    {"index2", "8.8.4.4"}
                }
            },
            {"Quad9", new Dictionary<string, string>()
                {
                    {"index1", "9.9.9.9"},
                    {"index2", "149.112.112.112"}
                }
            },
            {"OpenDNS Home", new Dictionary<string, string>()
                {
                    {"index1", "208.67.222.222"},
                    {"index2", "208.67.220.220"}
                }
            },
            {"Cloudflare DNS", new Dictionary<string, string>()
                {
                    {"index1", "1.1.1.1"},
                    {"index2", "1.0.0.1"}
                }
            },
            {"Comodo Secure DNS", new Dictionary<string, string>()
                {
                    {"index1", "8.26.56.26"},
                    {"index2", "8.20.247.20"}
                }
            },
            {"CleanBrowsing", new Dictionary<string, string>()
                {
                    {"index1", "185.225.168.168"},
                    {"index2", "185.228.169.168"}
                }
            },
            {"Alternate DNS", new Dictionary<string, string>()
                {
                    {"index1", "76.76.19.19"},
                    {"index2", "76.223.122.150"}
                }
            },
            {"AdGuard DNS", new Dictionary<string, string>()
                {
                    {"index1", "176.103.130.130"},
                    {"index2", "176.103.130.131"}
                }
            },
            {"Verisign", new Dictionary<string, string>()
                {
                    {"index1", "64.6.64.6"},
                    {"index2", "64.6.65.6"}
                }
            },
            {"OpenNIC", new Dictionary<string, string>()
                {
                    {"index1", "216.87.84.211"},
                    {"index2", "23.90.4.6"}
                }
            },
            {"Yandex DNS", new Dictionary<string, string>()
                {
                    {"index1", "77.88.8.8"},
                    {"index2", "77.88.8.1"}
                }
            },
            {"DNS.Watch", new Dictionary<string, string>()
                {
                    {"index1", "84.200.69.80"},
                    {"index2", "84.200.70.40"}
                }
            },
            {"Level 3", new Dictionary<string, string>()
                {
                    {"index1", "209.244.0.3"},
                    {"index2", "209.244.0.4"}
                }
            },
            {"Oracle Dyn", new Dictionary<string, string>()
                {
                    {"index1", "216.146.35.35"},
                    {"index2", "216.146.36.36"}
                }
            },
            {"UncensoredDNS", new Dictionary<string, string>()
                {
                    {"index1", "91.239.100.100"},
                    {"index2", "89.233.43.71"}
                }
            },
            {"Neustar Security Services", new Dictionary<string, string>()
                {
                    {"index1", "156.154.70.5"},
                    {"index2", "156.157.71.5"}
                }
            },
            {"GreenTeamDNS", new Dictionary<string, string>()
                {
                    {"index1", "81.218.119.11"},
                    {"index2", "209.88.198.133"}
                }
            },
            {"SafeDNS", new Dictionary<string, string>()
                {
                    {"index1", "195.46.39.39"},
                    {"index2", "195.46.39.40"}
                }
            }
        };
    }
}
