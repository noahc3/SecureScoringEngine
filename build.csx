#r "System.dll"
#r "System.IO.dll"
#r "System.Linq.dll"
#r "System.IO.Compression.dll"
#r "System.IO.Compression.ZipFile.dll"

using System;
using System.IO;
using System.IO.Compression;
using System.Linq;

const string TARGET_FRAMEWORK = "netcoreapp3.1";

Console.WriteLine("SSE Build Script");

bool buildWindows = Args[0].Contains("win") || Args[0].Contains("all");
bool buildLinux = Args[0].Contains("linux") || Args[0].Contains("all");

if (!(buildLinux || buildWindows)) {
    Console.WriteLine("Usage: build.csx <platform>");
    Console.WriteLine("  platform: win, linux, all");
    Environment.Exit(1);
}

if (Directory.Exists("build")) Directory.Delete("build", true);
if (Directory.Exists("temp")) Directory.Delete("temp", true);
Directory.CreateDirectory("build");
Directory.CreateDirectory("temp");

foreach(string k in Directory.GetDirectories("SSEConfigurationTool/files/install/")) {
    foreach(string j in Directory.GetDirectories(k)) {
        string rootZip = Path.Combine(j, "root.zip");
        if (File.Exists(rootZip)) File.Delete(rootZip);
    }
}

if (buildWindows) {
    RunDotnetCommand("publish -c Release -r win-x64 SSEBackend");
    RunDotnetCommand("publish -c Release -r win-x64 SSEService");
    RunDotnetCommand("publish -c Release -r win-x64 SSESetTeamUUID");

    CleanPublish("SSEBackend");
    CleanPublish("SSEService");
    CleanPublish("SSESetTeamUUID");

    Directory.CreateDirectory("temp/online/SSE/SSEService/setteamuuid/");
    Directory.CreateDirectory("temp/offline/SSE/SSEService/");
    Directory.CreateDirectory("temp/offline/SSE/SSEBackend/");

    DirectoryCopy("SSEBackend/bin/Release/" + TARGET_FRAMEWORK + "/win-x64/publish/", "temp/offline/SSE/SSEBackend/", true);
    DirectoryCopy("SSEService/bin/Release/" + TARGET_FRAMEWORK + "/win-x64/publish/", "temp/offline/SSE/SSEService/", true);
    DirectoryCopy("SSEService/bin/Release/" + TARGET_FRAMEWORK + "/win-x64/publish/", "temp/online/SSE/SSEService/", true);
    DirectoryCopy("SSESetTeamUUID/bin/Release/" + TARGET_FRAMEWORK + "/win-x64/publish/", "temp/online/SSE/SSEService/setteamuuid/", true);

    
}

public static void CleanPublish(string project) {
    foreach(string a in Directory.GetDirectories("./" + project + "/bin/")) {
        foreach(string b in Directory.GetDirectories(a)) {
            foreach(string c in Directory.GetDirectories(b)) {
                foreach(string d in Directory.GetDirectories(c + "/publish/")) {
                    if (wipeFolders.Contains(d.Replace("\\", "/").Split('/').Last())) {
                        Directory.Delete(d, true);
                    }
                }
            }
        }
    }
}

public static void RunDotnetCommand(string command) {
    if (Environment.OSVersion.Platform == PlatformID.Unix) {
        ("dotnet " + command).ExecuteAsBash();
    } else {
        ("dotnet " + command).ExecuteAsCmd();
    }
}

public static string ExecuteAsBash(this string cmd, bool includeErrors = true) {
    var escapedArgs = cmd.Replace("\"", "\\\"");

    var process = new Process() {
        StartInfo = new ProcessStartInfo {
            FileName = "/bin/bash",
            Arguments = $"-c \"{escapedArgs}\"",
            RedirectStandardOutput = true,
            RedirectStandardError = includeErrors,
            UseShellExecute = false,
            CreateNoWindow = true,
        }
    };
    process.Start();
    string result = process.StandardOutput.ReadToEnd();
    process.WaitForExit();
    return result;
}

public static string ExecuteAsCmd(this string cmd) {
    Process p = new Process();
    p.StartInfo.FileName = "cmd.exe";
    p.StartInfo.Arguments = "/c " + cmd;
    p.StartInfo.UseShellExecute = false;
    p.StartInfo.RedirectStandardOutput = true;
    p.StartInfo.RedirectStandardError = true;
    p.StartInfo.CreateNoWindow = true;

    p.Start();

    string result = p.StandardOutput.ReadToEnd();
    result += p.StandardError.ReadToEnd();
    p.WaitForExit();

    return result;
}

private static void DirectoryCopy(string sourceDirName, string destDirName, bool copySubDirs)
    {
        // Get the subdirectories for the specified directory.
        DirectoryInfo dir = new DirectoryInfo(sourceDirName);

        if (!dir.Exists)
        {
            throw new DirectoryNotFoundException(
                "Source directory does not exist or could not be found: "
                + sourceDirName);
        }

        DirectoryInfo[] dirs = dir.GetDirectories();
        // If the destination directory doesn't exist, create it.
        if (!Directory.Exists(destDirName))
        {
            Directory.CreateDirectory(destDirName);
        }

        // Get the files in the directory and copy them to the new location.
        FileInfo[] files = dir.GetFiles();
        foreach (FileInfo file in files)
        {
            string temppath = Path.Combine(destDirName, file.Name);
            file.CopyTo(temppath, false);
        }

        // If copying subdirectories, copy them and their contents to new location.
        if (copySubDirs)
        {
            foreach (DirectoryInfo subdir in dirs)
            {
                string temppath = Path.Combine(destDirName, subdir.Name);
                DirectoryCopy(subdir.FullName, temppath, copySubDirs);
            }
        }
    }

static string[] wipeFolders = new string[] { "cs", "de", "es", "fr", "it", "ja", "ko", "pl", "pt-BR", "ru", "tr", "zh-Hans", "zh-Hant" };