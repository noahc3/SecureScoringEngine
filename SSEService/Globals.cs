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
        public static string FILES_DIRECTORY = (Environment.CurrentDirectory + "\\files\\").AsPath();

        public static string CONFIG_SESSION = (CONFIG_DIRECTORY + "\\session.json").AsPath();

        public static string README_LOCATION = (FILES_DIRECTORY + "\\readme.html").AsPath();
        public static string SCORING_REPORT_LOCATION = (FILES_DIRECTORY + "\\report.html").AsPath();

        //file locations

        //base address
        public static string INTERNET_CHECK_ADDRESS = "http://www.google.com/";
#if (DEBUG)
        public static string ENDPOINT_BASE_ADDRESS = "http://192.168.0.10:5002";
#else
        public static string ENDPOINT_BASE_ADDRESS = "http://sse.noahc3.tech";
#endif

        //-------------------------------//
        //----- plaintext endpoints -----//
        //-------------------------------//

        // various startup/initial config endpoints
        public static Uri ENDPOINT_PING_PLAINTEXT;
        public static Uri ENDPOINT_VERIFY_TEAM_UUID;
        public static Uri ENDPOINT_KEY_EXCHANGE;

        //--------------------------------//
        //----- ciphertext endpoints -----//
        //--------------------------------//

        // various startup/initial config endpoints
        public static Uri ENDPOINT_PING_CIPHERTEXT;

        // general file request endpoints (readme, scoring report, forensics?)
        public static Uri ENDPOINT_README;
        public static Uri ENDPOINT_SCORING_REPORT_TEMPLATE;

        // actual logic endpoints (scoring)
        public static Uri ENDPOINT_START_SCORING_PROCESS;
        public static Uri ENDPOINT_CONTINUE_SCORING_PROCESS;

        // information endpoints (reports, notices, etc)
        public static Uri ENDPOINT_SCORING_REPORT;

#if (DEBUG)

        //--------------------------------//
        //------- debug  endpoints -------//
        //--------------------------------//

        public static Uri ENDPOINT_DEBUG_CHECK_SVC_STATUS;

#endif

        public static SessionConfig SessionConfig;

        public static ScoringReport ScoringReport;
        public static int LastScore = 0;

        //constant strings

        public const string SSESERVICE_NOTIFICATION_TITLE = "SSEService Notification";
        public const string SSESERVICE_NOTIFICATION_GAINED_POINTS = "You gained points!";
        public const string SSESERVICE_NOTIFICATION_LOST_POINTS = "You lost points!";
        public const string SSESERVICE_NOTIFICATION_CONNECTION_FAILED = "Failed to connect to server, check your internet!";

        public const string SSESERVICE_ERROR_HTML = "<html><!--SSEERR--><head><meta http-equiv=\"refresh\" content=\"5\"/><title>{0}</title><style type=\"text/css\"> .centerDiv{{left:50%; top:50%; transform: translate(-50%, -50%); position: fixed; border-style: solid; border-color: {1}; border-width: 3px; text-align:center; padding:40px; font-family: Arial, Verdana, sans-serif; font-size: 16px;}}h1{{color: {1}; font-family: Helvetica,Arial,sans-serif;}}h3{{font-family: 'Roboto', sans-serif;}}</style></head><body><div class=\"centerDiv\"> <h1>{2}</h1> <p>{3}</p><h3>{4}</h3> <h3>{5}</h3></div></body></html>";



        public static void SetEndpoints() {
            ENDPOINT_PING_PLAINTEXT = new Uri(ENDPOINT_BASE_ADDRESS + "/api/generic/ping");
            ENDPOINT_VERIFY_TEAM_UUID = new Uri(ENDPOINT_BASE_ADDRESS + "/api/auth/isteamuuidvalid");
            ENDPOINT_KEY_EXCHANGE = new Uri(ENDPOINT_BASE_ADDRESS + "/api/auth/keyexchange");

            ENDPOINT_PING_CIPHERTEXT = new Uri(ENDPOINT_BASE_ADDRESS + "/api/auth/ping");
            ENDPOINT_README = new Uri(ENDPOINT_BASE_ADDRESS + "/api/fetch/readme");
            ENDPOINT_SCORING_REPORT_TEMPLATE = new Uri(ENDPOINT_BASE_ADDRESS + "/api/fetch/scoringreporttemplate");
            ENDPOINT_START_SCORING_PROCESS = new Uri(ENDPOINT_BASE_ADDRESS + "/api/scoring/start");
            ENDPOINT_CONTINUE_SCORING_PROCESS = new Uri(ENDPOINT_BASE_ADDRESS + "/api/scoring/continuescoringprocess");
            ENDPOINT_SCORING_REPORT = new Uri(ENDPOINT_BASE_ADDRESS + "/api/scoring/getscoringreport");

            #if (DEBUG)
            ENDPOINT_DEBUG_CHECK_SVC_STATUS = new Uri(ENDPOINT_BASE_ADDRESS + "/api/debug/debugsvcstatus");
            #endif
        }

        public static void GenerateConfig() {
            SessionConfig sessionConfig = new SessionConfig();

            Console.WriteLine("Platform: " + RuntimeInformation.OSDescription);
            sessionConfig.RuntimeID = RuntimeInformation.OSDescription;
            sessionConfig.TeamUUID = "";

            //loop to require people to enter a valid team UUID.
            //while (true) {
            //    Console.WriteLine("TEMPORARY INPUT: Please enter your Team UUID:");
            //    sessionConfig.TeamUUID = Console.ReadLine();
            //    if (ClientServerComms.VerifyTeamUUID(sessionConfig.TeamUUID, sessionConfig.RuntimeID)) {
            //        Console.WriteLine("Team UUID authorized.");
            //        break;
            //    } else {
            //        Console.WriteLine("Invalid team UUID!");
            //    }
            //}

            sessionConfig.Flush();
            Console.WriteLine("Session Config flushed to disk.");
        }

        public static void LoadConfig() {
            SessionConfig = SessionConfig.FromJson(File.ReadAllText(Globals.CONFIG_SESSION));
            ENDPOINT_BASE_ADDRESS = SessionConfig.Backend;
            Console.WriteLine(SessionConfig.Backend);
            Console.WriteLine(ENDPOINT_BASE_ADDRESS);
            SetEndpoints();
        }

        public static void WriteErrorScoringReport(string windowTitle, string color, string headerText, string smallText, string errorText, string solutionText) {
            File.WriteAllText(Globals.SCORING_REPORT_LOCATION, String.Format(SSESERVICE_ERROR_HTML, windowTitle, color, headerText, smallText, errorText, solutionText));
        }
        
        //compatible with both windows and linux!
        //vm needs notifu64.exe in path or running directory!
        public static void SendToastNotification(string title, string message) {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows)) {
                ("notifu64 /p \"" + title + "\" /m \"" + message + "\"").ExecuteAsCmd();
            } else {
                ("notify-send -u critical -i \"/opt/SSEService/assets/icon.png\" \"" + title + "\" \"" + message + "\"").ExecuteAsBash();
            }
        }
    }
}
