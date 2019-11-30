using System;
using System.IO;
using System.Text;

using System.Threading;

using SSECommon.Types;
using SSEService.Net;

namespace SSEService {
    class Program {
        static void Main(string[] args) {

            
            //fancy format stuff
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine("==================================================");
            Console.WriteLine("=         Secure Scoring Engine (Service)        =");
            Console.WriteLine("=                 made by noahc3                 =");
            Console.WriteLine("==================================================");
            Console.WriteLine("");

#if (DEBUG)
            Console.ForegroundColor = ConsoleColor.Red;

            Console.WriteLine("--------------------------------------------------");
            Console.WriteLine("-                    WARNING!                    -");
            Console.WriteLine("-              THIS IS A DEBUG BUILD             -");
            Console.WriteLine("-  DO NOT RUN THIS BUILD IN A REAL COMPETITION!  -");
            Console.WriteLine("--------------------------------------------------");
            Console.WriteLine("");
#endif

            Console.ForegroundColor = ConsoleColor.DarkCyan;

            Console.WriteLine("--------------------------------------------------");
            Console.WriteLine("-                      NOTE                      -");
            Console.WriteLine("-        This console window is temporary        -");
            Console.WriteLine("-   and will not be present in the final build   -");
            Console.WriteLine("--------------------------------------------------");
            Console.WriteLine("");

            Console.ResetColor();

            //end fancy format stuff

            if (!Directory.Exists(Globals.CONFIG_DIRECTORY)) Directory.CreateDirectory(Globals.CONFIG_DIRECTORY);
            if (!Directory.Exists(Globals.FILES_DIRECTORY)) Directory.CreateDirectory(Globals.FILES_DIRECTORY);

            if (!File.Exists(Globals.CONFIG_SESSION)) {
                Globals.GenerateConfig();
            }

            Globals.LoadConfig();

            //main logic loop
            while (true) {
                if (!IsClientReadyForScoring()) {
                    Thread.Sleep(5000);
                    Globals.LoadConfig();
                    continue;
                }

                if (!File.Exists(Globals.README_LOCATION)) {
                    ClientServerComms.GetReadme();
                }

                if (ClientServerComms.RequestStartScoringProcess()) {
                    ClientServerComms.ScoringProcess();
                    ClientServerComms.GetScoringReport();
                    FileTransferWrapper reportTemplateWrapper = ClientServerComms.GetScoringReportTemplate();

                    string reportTemplate = Encoding.Default.GetString(reportTemplateWrapper.Blob);

                    ScoringReport r = Globals.ScoringReport;

                    int penaltyPoints = 0;
                    string penalties = "";

                    int rewardsPoints = 0;
                    string rewards = "";

                    if (r.penalties.Count > 0) {
                        foreach (ClientScoreMetadata meta in r.penalties) {
                            penaltyPoints += meta.ScoreValue;
                            penalties += meta.Description + " - " + meta.ScoreValue + "<br>";
                        }
                    }

                    if (r.rewards.Count > 0) {
                        foreach (ClientScoreMetadata meta in r.rewards) {
                            rewardsPoints += meta.ScoreValue;
                            rewards += meta.Description + " - " + meta.ScoreValue + "<br>";
                        }
                    }

                    TimeSpan teamRunningTime = DateTime.UtcNow.Subtract(new DateTime(r.teamStartTimestamp));
                    TimeSpan imageRunningTime = DateTime.UtcNow.Subtract(new DateTime(r.runtimeStartTimestamp));

                    reportTemplate = String.Format(reportTemplate, String.Format("{0:0}:{1:00}:{2:00}", Math.Floor(teamRunningTime.TotalHours), teamRunningTime.Minutes, teamRunningTime.Seconds), r.score, r.totalScore, r.penaltiesGained, penaltyPoints, r.rewardsFound, rewardsPoints, r.totalRewards, penalties, rewards, Globals.SessionConfig.TeamUUID, String.Format("{0:0}:{1:00}:{2:00}", Math.Floor(imageRunningTime.TotalHours), imageRunningTime.Minutes, imageRunningTime.Seconds));

                    File.WriteAllText(Globals.SCORING_REPORT_LOCATION, reportTemplate);

                    if (r.score > Globals.LastScore) {
                        Globals.SendToastNotification(Globals.SSESERVICE_NOTIFICATION_TITLE, Globals.SSESERVICE_NOTIFICATION_GAINED_POINTS);
                    } else if (r.score < Globals.LastScore) {
                        Globals.SendToastNotification(Globals.SSESERVICE_NOTIFICATION_TITLE, Globals.SSESERVICE_NOTIFICATION_LOST_POINTS);
                    }

                    Globals.LastScore = r.score;
                }
                Thread.Sleep(10000);

                if (File.Exists("C:\\killswitch.bin")) {
                    Console.Write("Killswitch detected");
                    break;
                }

                Console.WriteLine();
                    
            }

            Console.WriteLine("-- Exited logic loop --");
            Console.ReadKey();

            
        }


        public static bool IsClientReadyForScoring() {
            if (!ClientServerComms.IsInternetAvailable()) {
                Globals.WriteErrorScoringReport("SSEService Error", "red", "SSE Error", "The scoring engine failed to connect to the server. Scoring cannot continue.", "Failed to ping " + Globals.INTERNET_CHECK_ADDRESS, "Please check your Internet connection!");
                Globals.SendToastNotification("SSEService Error", "Cannot connect to scoring server, check the scoring report for more information.");
                return false;
            }

            if (String.IsNullOrWhiteSpace(Globals.SessionConfig.TeamUUID)) {
                Globals.WriteErrorScoringReport("SSEService Warning", "orange", "SSE WARNING", "The scoring engine failed to connect to the server. Scoring cannot continue.", "Your Team UUID has not been set!", "Please enter your Team UUID with the \"Set Team UUID\" desktop icon!");
                Globals.SendToastNotification("SSEService Warning", "Please enter your team UUID!");
                return false;
            }

            if (!ClientServerComms.CanReachScoringServer()) {
                Globals.WriteErrorScoringReport("SSEService Error", "red", "SSE ERROR", "The scoring engine failed to connect to the server. Scoring cannot continue.", "Cannot connect to the scoring server at " + Globals.ENDPOINT_BASE_ADDRESS, "Please check your Internet connection and ensure the scoring server can be reached through any firewalls!");
                Globals.SendToastNotification("SSEService Error", "Cannot connect to scoring server, check the scoring report for more information.");
                return false;
            }
            

            if (!ClientServerComms.VerifyTeamUUID(Globals.SessionConfig.TeamUUID, Globals.SessionConfig.RuntimeID)) {
                Globals.WriteErrorScoringReport("SSEService Error", "red", "SSE ERROR", "The scoring engine failed to connect to the server. Scoring cannot continue.", "Your Team UUID is invalid!", "Please correct your Team UUID with the \"Set Team UUID\" desktop icon!");
                Globals.SendToastNotification("SSEService Error", "Your Team UUID is invalid! Please correct it with the 'Set Team UUID' desktop icon!");
                return false;
            }

            if (!ClientServerComms.CiphertextPing()) {
                Globals.WriteErrorScoringReport("SSEService Error", "red", "SSE ERROR", "The scoring engine failed to connect to the server. Scoring cannot continue.", "Encrypted communications with the server failed.", "Please check your Internet connection and ensure the scoring server can be reached through any firewalls!");
                Globals.SendToastNotification("SSEService Error", "Cannot connect to scoring server, check the scoring report for more information.");
                return false;
            }

#if (DEBUG)
            ClientServerComms.CheckDebugSvcStatus();
#endif

            if (!File.Exists(Globals.SCORING_REPORT_LOCATION) || File.ReadAllText(Globals.SCORING_REPORT_LOCATION).Contains("<!--SSEERR-->")) {
                Globals.WriteErrorScoringReport("Scoring in Progress", "blue", "SCORING IN PROGRESS", "Please wait for scoring to complete before your scoring report is generated.", "", "");
            }

            return true;
        }


    }


}
