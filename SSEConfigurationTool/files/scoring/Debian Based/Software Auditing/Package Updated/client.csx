//assembly System;
//assembly System.IO;
//assembly System.Linq;
//assembly SSECommon;

#r "../../../../../../SSECommon/bin/Debug/netstandard2.0/SSECommon.dll" //$INTELLISENSE

using System;
using System.Linq;
using SSECommon;

string packageName; //$CONFIG | Package Name | Name of the package to check.
string oldVersion; //$CONFIG | Old Version | The version of the package that will be initially installed on the system.
bool requireNoUpdatesAvailable = true; //$CONFIG | Require No Updates Available | With this enabled, the check will only pass if the package version is different and if apt reports there are no newer versions to install. Otherwise, only the former is checked.

try {  
    string output = ("dpkg -s " + packageName).ExecuteAsBash();
    if (output.Contains("Version: " + oldVersion.Trim())) return "outdated";
    else return "updated";
} catch (Exception e) {
    return "{#} CLIENT PAYLOAD FAILED! {#} " + e.Message;
}