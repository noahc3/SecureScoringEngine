using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Win32;
using SSECommon;
using SSEConfigurationTool.Types;

namespace SSEConfigurationTool.Data {
    public class Globals {
        public static LogicalDistro distro;

        public static string DisplayName;
        public static List<User> users;

        public static void Init() {
            distro = DetermineLogicalDistro();

            if (Utilities.MatchesLogicalDistros(LogicalDistro.DebianBased)) {
                DisplayName = "Linux (Debian-based)";
                users = GetUsers();
            }
        }

        public static List<User> GetUsers() {
            List<User> users = new List<User>();

            if (Utilities.MatchesLogicalDistros(LogicalDistro.DebianBased)) {
                string[] usernames = "cut -d: -f1 /etc/passwd".ExecuteAsBash().Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries); //CONSOLIDATE: REMTCommon.Utilities.GetUsernames();

                foreach (string k in usernames) {
                    User u = new User() {
                        Name = k
                    };
                    users.Add(u);
                }
            }

            return users;
        }

        public static LogicalDistro DetermineLogicalDistro() {
            string OSSimple = "";

            if (Environment.OSVersion.Platform == PlatformID.Unix) {
                OSSimple = "lsb_release -s -d".ExecuteAsBash();
            } else if (Environment.OSVersion.Platform == PlatformID.Win32NT) {
                OSSimple = (string) Registry.GetValue("HKEY_LOCAL_MACHINE\\Software\\Microsoft\\Windows NT\\CurrentVersion", "ProductName", "");
            }

            if (OSSimple.Contains("Ubuntu 14")) return LogicalDistro.DebianBased;
            if (OSSimple.Contains("Ubuntu 16")) return LogicalDistro.DebianBased;
            if (OSSimple.Contains("Ubuntu 18")) return LogicalDistro.DebianBased;
            if (OSSimple.Contains("Ubuntu 19")) return LogicalDistro.DebianBased;
            if (OSSimple.Contains("Windows 10")) return LogicalDistro.Windows10;

            return LogicalDistro.Unknown;
        }

    }

    public enum LogicalDistro {
        Unknown,
        Windows10,
        WindowsServer2019,
        DebianBased
    }
}
