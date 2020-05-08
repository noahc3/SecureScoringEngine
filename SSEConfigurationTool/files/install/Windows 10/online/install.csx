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
string runtimeId = items[3];
bool clearLogs = Boolean.Parse(items[4]);

if (Directory.Exists("C:\\SSE\\SSEService")) Directory.Delete("C:\\SSE\\SSEService", true);

DirectoryInfo rootExt = new DirectoryInfo((filesDir.FullName + "\\root").AsPath());
DirectoryInfo desktopExt = new DirectoryInfo((filesDir.FullName + "\\desktop").AsPath());

//use cmd instead of c# because microsoft is too stupid to write a competent filesystem wrapper for c#
("xcopy /s /e /y " + rootExt.FullName + " C:\\").ExecuteAsCmd();
("xcopy /s /e /y " + desktopExt.FullName + " " + desktopDir.FullName).ExecuteAsCmd();

SessionConfig config = new SessionConfig();

config.Backend = backendUrl;
config.RuntimeID = runtimeId;
config.TeamUUID = "";

File.WriteAllText("C:\\SSE\\SSEService\\config\\session.json", config.ToJson());

"schtasks /Create /RU %username% /SC ONLOGON /TN SSEWatchdog /TR C:\\SSE\\SSEWatchdog\\SSEWatchdog.exe".ExecuteAsCmd();

if (clearLogs) {
    "rmdir /s /q C:\\$Recycle.Bin".ExecuteAsCmd();
}

return "OK";