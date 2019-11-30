using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using SSECommon;
using Newtonsoft.Json;

namespace SSESetTeamUUID {
    class Program {
        private static string CONFIG_DIRECTORY = (Environment.CurrentDirectory + "\\..\\config\\").AsPath();
        private static string CONFIG_SESSION = (CONFIG_DIRECTORY + "\\session.json").AsPath();

        static void Main(string[] args) {
            SessionConfig config = SessionConfig.FromJson(File.ReadAllText(CONFIG_SESSION));

            if (args.Length > 0 && args.Contains("--if-unset")) {
                if (!String.IsNullOrWhiteSpace(config.TeamUUID)) Environment.Exit(0);
            }

            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine("==================================================");
            Console.WriteLine("=              Secure Scoring Engine             =");
            Console.WriteLine("=                  Set Team UUID                 =");
            Console.WriteLine("==================================================");
            Console.WriteLine("");

            Console.ResetColor();

            Console.Write("Enter your Team UUID (ex. XXXX-XXXX-XXXX): ");

            config.TeamUUID = Console.ReadLine();

            File.WriteAllText(CONFIG_SESSION, config.ToJson());
        }
    }

    class SessionConfig {
        public string TeamUUID;
        public string RuntimeID;

        public string ToJson() {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }

        public static SessionConfig FromJson(string json) {
            return JsonConvert.DeserializeObject<SessionConfig>(json);
        }
    }
}
