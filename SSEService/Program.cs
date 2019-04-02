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
            Console.WriteLine("=================================================");
            Console.WriteLine("=        Secure Scoring Engine (Service)        =");
            Console.WriteLine("=                made by noahc3                 =");
            Console.WriteLine("=================================================");
            Console.WriteLine("");

            Console.ForegroundColor = ConsoleColor.Red;

            Console.WriteLine("--------------------------------------------------");
            Console.WriteLine("-                    WARNING!                    -");
            Console.WriteLine("-              THIS IS A DEBUG BUILD             -");
            Console.WriteLine("-  DO NOT RUN THIS BUILD IN A REAL COMPETITION!  -");
            Console.WriteLine("--------------------------------------------------");
            Console.WriteLine("");

            Console.ResetColor();

            //end fancy format stuff

            if (!Directory.Exists(Globals.CONFIG_DIRECTORY)) Directory.CreateDirectory(Globals.CONFIG_DIRECTORY);

            if (!File.Exists(Globals.CONFIG_SESSION)) {
                Globals.GenerateConfig();
            }

            Globals.LoadConfig();

            ClientServerComms.CiphertextPing();

            ClientServerComms.GetReadme();

            //main logic loop
            while (true) {
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

                    //TODO: implement running time (from server i guess)
                    reportTemplate = String.Format(reportTemplate, "Not yet implemented", r.score, r.totalScore, r.penaltiesGained, penaltyPoints, r.rewardsFound, rewardsPoints, r.totalRewards, penalties, rewards, Globals.SessionConfig.TeamUUID);

                    File.WriteAllText(reportTemplateWrapper.Path, reportTemplate);

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
                    
            }

            Console.WriteLine("-- Exited logic loop --");
            Console.ReadKey();
        }
    }
}
