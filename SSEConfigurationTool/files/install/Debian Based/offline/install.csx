//assembly System;
//assembly System.IO;
//assembly System.Linq;
//assembly SSECommon;

#r "../../../../../SSECommon/bin/Debug/netstandard2.0/SSECommon.dll" //$INTELLISENSE
#r "netstandard.dll" //$INTELLISENSE

using System;
using System.IO;
using SSECommon;
using SSECommon.Types;

string passthrough = ""; //$INTELLISENSE

string[] items = passthrough.Split('|');

DirectoryInfo filesDir = new DirectoryInfo(items[0]);
DirectoryInfo desktopDir = new DirectoryInfo(items[1]);
string backendUrl = items[2];
string readmePath = items[3];
bool clearLogs = Boolean.Parse(items[4]);

if (Directory.Exists("/opt/SSEBackend")) Directory.Delete("/opt/SSEBackend", true);
if (Directory.Exists("/opt/SSEService")) Directory.Delete("/opt/SSEService", true);

DirectoryInfo rootExt = new DirectoryInfo((filesDir.FullName + "/root").AsPath());
DirectoryInfo desktopExt = new DirectoryInfo((filesDir.FullName + "/desktop").AsPath());

//use bash instead of c# because microsoft is too stupid to write a competent filesystem wrapper for c#
("cp -rf " + rootExt.FullName + "/* /").ExecuteAsBash();
("cp -rf " + desktopExt.FullName + "/* " + desktopDir.FullName).ExecuteAsBash();

File.Move(readmePath, "/opt/SSEBackend/config/runtimes/offline/readme.bin");

SessionConfig config = new SessionConfig();

config.Backend = backendUrl;
config.RuntimeID = "offline";
config.TeamUUID = "offline";

File.WriteAllText("/opt/SSEService/config/session.json", config.ToJson());

File.WriteAllText("/etc/systemd/system/sseservice.service", File.ReadAllText("/etc/systemd/system/sseservice.service").Replace("/home/bporter/.Xauthority", desktopDir.FullName.Replace("Desktop", ".Xauthority")));

("ln -s \"/opt/SSEService/assets/SSE README.desktop\" \"" + (desktopDir.FullName + "/SSE README.desktop").AsPath() + "\"").ExecuteAsBash();
("ln -s \"/opt/SSEService/assets/SSE Scoring Report.desktop\" \"" + (desktopDir.FullName + "/SSE Scoring Report.desktop").AsPath() + "\"").ExecuteAsBash();
"chmod -R 777 /opt/SSEService/*".ExecuteAsBash();
"chmod -R 777 /opt/SSEBackend/*".ExecuteAsBash();

"systemctl enable sseservice".ExecuteAsBash();
"systemctl enable ssebackend".ExecuteAsBash();

if (clearLogs) {
    ("rm -rf " + (desktopDir.FullName + "/.cache/vmware/*").AsPath()).ExecuteAsBash(true);
    ("rm -rf " + (desktopDir.FullName + "/.local/share/Trash/*").AsPath()).ExecuteAsBash(true);
    "find /var/log -type f -delete".ExecuteAsBash(true);
    "find /home/*/.bash_history -delete".ExecuteAsBash(true);
}

return "/opt/SSEBackend/config/runtimes/offline/scoring.zip";