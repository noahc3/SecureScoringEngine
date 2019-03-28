using System;
using System.IO;

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
                }
                break;
            }

            Console.WriteLine("-- Exited logic loop --");
            Console.ReadKey();
        }
    }
}
