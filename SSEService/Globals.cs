using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

using SSECommon;
using SSEService.Net;
using SSEService.Types;

namespace SSEService {
    static class Globals {
        public static string CONFIG_DIRECTORY = (Environment.CurrentDirectory + "\\config\\").AsPath();
        public static string CONFIG_SESSION = (CONFIG_DIRECTORY + "\\session.json").AsPath();
        public static string README_LOCATION = (CONFIG_DIRECTORY + "\\readme.rtf").AsPath();

        //file locations

        //base address
        public static string ENDPOINT_BASE_ADDRESS = "http://192.168.0.10:59137";

        //plaintext endpoints
        public static Uri ENDPOINT_VERIFY_TEAM_UUID = new Uri(ENDPOINT_BASE_ADDRESS + "/api/auth/isteamuuidvalid");
        public static Uri ENDPOINT_KEY_EXCHANGE = new Uri(ENDPOINT_BASE_ADDRESS + "/api/auth/keyexchange");

        //ciphertext endpoints
        public static Uri ENDPOINT_PING_CIPHERTEXT = new Uri(ENDPOINT_BASE_ADDRESS + "/api/auth/ping");
        public static Uri ENDPOINT_README = new Uri(ENDPOINT_BASE_ADDRESS + "/api/fetch/readme");

        public static SessionConfig SessionConfig;


        public static void GenerateConfig() {
            SessionConfig sessionConfig = new SessionConfig();

            Console.WriteLine("Platform: " + RuntimeInformation.OSDescription);
            sessionConfig.RuntimeID = RuntimeInformation.OSDescription;

            //loop to require people to enter a valid team UUID.
            while (true) {
                Console.WriteLine("TEMPORARY INPUT: Please enter your Team UUID:");
                sessionConfig.TeamUUID = Console.ReadLine();
                if (ClientServerComms.VerifyTeamUUID(sessionConfig.TeamUUID, sessionConfig.RuntimeID)) {
                    Console.WriteLine("Team UUID authorized.");
                    break;
                } else {
                    Console.WriteLine("Invalid team UUID!");
                }
            }

            sessionConfig.Flush();
            Console.WriteLine("Session Config flushed to disk.");
        }

        public static void LoadConfig() {
            SessionConfig = SessionConfig.FromJson(File.ReadAllText(Globals.CONFIG_SESSION));

            //verify the team UUID and runtime ID parsed from the config
            Console.WriteLine("Verifying team UUID and runtime ID...");
            if (ClientServerComms.VerifyTeamUUID(SessionConfig.TeamUUID, SessionConfig.RuntimeID)) {
                Console.WriteLine("Team UUID authorized.");
            } else {
                //if the team UUID and runtime ID pair are invalid, wipe the session config and restart the authentication process.
                Console.WriteLine("Invalid team UUID!");
                File.Delete(Globals.CONFIG_SESSION);
                GenerateConfig();
                LoadConfig();
            }
        }
    }
}
