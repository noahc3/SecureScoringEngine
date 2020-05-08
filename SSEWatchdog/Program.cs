using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;

namespace SSEWatchdog {
    class Program {
        static void Main(string[] args) {
            if (Process.GetProcessesByName(Process.GetCurrentProcess().ProcessName).Count() > 1) {
                Environment.Exit(0);
            }
            while (true) {
                try {
                    if (File.Exists("C:\\SSE\\SSEBackend\\SSEBackend.exe")) {
                        Process[] backends = Process.GetProcessesByName("SSEBackend");
                        if (backends.Count() <= 0) {
                            Process.Start(new ProcessStartInfo() { CreateNoWindow = true, WorkingDirectory = "C:\\SSE\\SSEBackend", FileName = "C:\\SSE\\SSEBackend\\SSEBackend.exe" });
                        }
                    }
                    if (File.Exists("C:\\SSE\\SSEService\\SSEService.exe")) {
                        Process[] services = Process.GetProcessesByName("SSEService");
                        if (services.Count() <= 0) {
                            Process.Start(new ProcessStartInfo() { CreateNoWindow = true, WorkingDirectory = "C:\\SSE\\SSEService", FileName = "C:\\SSE\\SSEService\\SSEService.exe" });
                        }
                    }
                } catch (Exception) {

                } finally {
                    Thread.Sleep(10000);
                }
            }
        }
    }
}
