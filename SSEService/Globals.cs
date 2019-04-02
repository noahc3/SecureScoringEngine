using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

using SSECommon;
using SSECommon.Types;
using SSEService.Net;
using SSEService.Types;

namespace SSEService {
    static class Globals {
        public static string CONFIG_DIRECTORY = (Environment.CurrentDirectory + "\\config\\").AsPath();
        public static string CONFIG_SESSION = (CONFIG_DIRECTORY + "\\session.json").AsPath();
        public static string README_LOCATION = (CONFIG_DIRECTORY + "\\readme.rtf").AsPath();

        //file locations

        //base address
        public static string ENDPOINT_BASE_ADDRESS = "http://192.168.0.10:5002";

        //-------------------------------//
        //----- plaintext endpoints -----//
        //-------------------------------//
        
        // various startup/initial config endpoints
        public static Uri ENDPOINT_VERIFY_TEAM_UUID = new Uri(ENDPOINT_BASE_ADDRESS + "/api/auth/isteamuuidvalid");
        public static Uri ENDPOINT_KEY_EXCHANGE = new Uri(ENDPOINT_BASE_ADDRESS + "/api/auth/keyexchange");

        //--------------------------------//
        //----- ciphertext endpoints -----//
        //--------------------------------//

        // various startup/initial config endpoints
        public static Uri ENDPOINT_PING_CIPHERTEXT = new Uri(ENDPOINT_BASE_ADDRESS + "/api/auth/ping");

        // general file request endpoints (readme, scoring report, forensics?)
        public static Uri ENDPOINT_README = new Uri(ENDPOINT_BASE_ADDRESS + "/api/fetch/readme");
        public static Uri ENDPOINT_SCORING_REPORT_TEMPLATE = new Uri(ENDPOINT_BASE_ADDRESS + "/api/fetch/scoringreporttemplate");

        // actual logic endpoints (scoring)
        public static Uri ENDPOINT_START_SCORING_PROCESS = new Uri(ENDPOINT_BASE_ADDRESS + "/api/scoring/start");
        public static Uri ENDPOINT_CONTINUE_SCORING_PROCESS = new Uri(ENDPOINT_BASE_ADDRESS + "/api/scoring/continuescoringprocess");

        // information endpoints (reports, notices, etc)
        public static Uri ENDPOINT_SCORING_REPORT = new Uri(ENDPOINT_BASE_ADDRESS + "/api/scoring/getscoringreport");

#if (DEBUG)

        //--------------------------------//
        //------- debug  endpoints -------//
        //--------------------------------//

        public static Uri ENDPOINT_DEBUG_CHECK_SVC_STATUS = new Uri(ENDPOINT_BASE_ADDRESS + "/api/debug/debugsvcstatus");

#endif

        public static SessionConfig SessionConfig;

        public static ScoringReport ScoringReport;
        public static int LastScore = 0;

        //constant strings

        public const string SSESERVICE_NOTIFICATION_TITLE = "SSEService Notification";
        public const string SSESERVICE_NOTIFICATION_GAINED_POINTS = "You gained points!";
        public const string SSESERVICE_NOTIFICATION_LOST_POINTS = "You lost points!";
        public const string SSESERVICE_NOTIFICATION_CONNECTION_FAILED = "Failed to connect to server, check your internet!";





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
        
        //compatible with both windows and linux!
        //vm needs notifu64.exe in path or running directory!
        public static void SendToastNotification(string title, string message) {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows)) {
                ("notifu64 /p \"" + title + "\" /m \"" + message + "\"").ExecuteAsCmd();
            } else {
                //string user = "echo $SUDO_USER".ExecuteAsBash();
                //Console.WriteLine(("su " + user + " -c \"notify-send -u critical \"" + title + "\" \"" + message + "\"\"").ExecuteAsBash());
                Console.WriteLine("Unable to show notification on Linux (for now)");
            }
        }
    }
}
