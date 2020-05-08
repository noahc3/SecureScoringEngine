using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace SSEBackend
{
    public class Program
    {
        public static void Main(string[] args) {
            //On Windows, for offline images, we launch the server using Task Scheduler every minute.
            //We want to make sure this exits if another copy of the process is already running.
            Process current = Process.GetCurrentProcess();
            if (Process.GetProcessesByName(current.ProcessName).Count() > 1) {
                Console.WriteLine("Process already running, exiting.");
                Environment.Exit(0);
            }
            Globals.LoadData();
            Globals.StartPassiveTasks();
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder => {
                    webBuilder
                    .UseStartup<Startup>()
                    .UseUrls("http://" + Globals.config.BindIp + ":" + Globals.config.BindPort);
                });


        
    }
}
