﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Win32;
using SSECommon;
using SSEConfigurationTool.Types;

namespace SSEConfigurationTool.Data {
    public class Globals {

        public static string DebugTeamUUID = "";
        public static string CONFIG_TOOL_RUNTIME_ID = "DEBUG";

        public static string DRAFT_PATH = "out/draft.json";

        public static string ENDPOINT_BASE_ADDRESS;
        public static Uri ENDPOINT_PING_PLAINTEXT;
        public static Uri ENDPOINT_KEY_EXCHANGE;

        public static Uri ENDPOINT_DEBUG_CHECK_SVC_STATUS;
        public static Uri ENDPOINT_DEBUG_ADD_RUNTIME;
        public static Uri ENDPOINT_DEBUG_HOT_RELOAD;

        public static string TargetPlatform = "";
        public static Dictionary<string, Dictionary<string, Dictionary<string, TemplateScoringItem>>> ScoringItems = new Dictionary<string, Dictionary<string, Dictionary<string, TemplateScoringItem>>>();
        public static List<ConfiguredScoringItem> ConfiguredScoringItems = new List<ConfiguredScoringItem>();

        public static LogicalDistro distro;


        public static void SetScoringServerEndpoints(string serverUrl) {
            ENDPOINT_BASE_ADDRESS = serverUrl;
            ENDPOINT_PING_PLAINTEXT = new Uri(ENDPOINT_BASE_ADDRESS + "/api/generic/ping");
            ENDPOINT_KEY_EXCHANGE = new Uri(serverUrl + "/api/auth/keyexchange");

            ENDPOINT_DEBUG_CHECK_SVC_STATUS = new Uri(ENDPOINT_BASE_ADDRESS + "/api/debug/debugsvcstatus");
            ENDPOINT_DEBUG_ADD_RUNTIME = new Uri(ENDPOINT_BASE_ADDRESS + "/api/debug/addruntime");
            ENDPOINT_DEBUG_HOT_RELOAD = new Uri(ENDPOINT_BASE_ADDRESS + "/api/debug/hotreload");

        }

        //called on launch
        public static void Init() {
            foreach(String _path in Assembly.GetExecutingAssembly().GetManifestResourceNames()) {
                Console.WriteLine(_path);
                string path = _path;
                if (path.StartsWith("SSEConfigurationTool.files.scoring.")) {
                    path = path.Substring("SSEConfigurationTool.files.scoring.".Length);
                    string[] split = path.Split('.');
                    string platform = split[0].Replace("_", " ").Trim();
                    string category = split[1].Replace("_", " ").Trim();
                    string item = split[2].Replace("_", " ").Trim();

                    if (!ScoringItems.ContainsKey(platform)) ScoringItems[platform] = new Dictionary<string, Dictionary<string, TemplateScoringItem>>();
                    if (!ScoringItems[platform].ContainsKey(category)) ScoringItems[platform][category] = new Dictionary<string, TemplateScoringItem>();
                    if (!ScoringItems[platform][category].ContainsKey(item)) {
                        TemplateScoringItem scoringItem = new TemplateScoringItem(platform, category, item);
                        ScoringItems[platform][category][item] = scoringItem;
                    }
                }
            }

            foreach(string platform in ScoringItems.Keys) {
                Console.WriteLine(platform + ": ");
                foreach(string category in ScoringItems[platform].Keys) {
                    Console.WriteLine("  " + category);
                    foreach(TemplateScoringItem item in ScoringItems[platform][category].Values) {
                        Console.WriteLine("    " + item.Name);
                    }
                }
            }

        }

        //called after the target platform is chosen
        public static void PostInit() {

        }

        public static List<User> GetUsers() {
            List<User> users = new List<User>();

            if (Utilities.MatchesLogicalDistros(LogicalDistro.DebianBased)) {
                string[] usernames = "cut -d: -f1 /etc/passwd".ExecuteAsBash().Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries); //CONSOLIDATE: REMTCommon.Utilities.GetUsernames();

                foreach (string k in usernames) {
                    if (int.Parse(("id -u " + k).ExecuteAsBash().Trim()) >= 1000) {
                        User u = new User() {
                            Name = k
                        };
                        users.Add(u);
                    }
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
