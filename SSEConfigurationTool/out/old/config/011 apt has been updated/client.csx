//assembly System;
//assembly System.IO;
//assembly System.Linq;
//assembly SSECommon;


using System;
using System.Linq;
using SSECommon;

string packageName = "apt";
string oldVersion = "1.2.29ubuntu0.1";
bool requireNoUpdatesAvailable = true;

try {  
    string output = ("dpkg -s " + packageName).ExecuteAsBash();
    if (output.Contains("Version: " + oldVersion.Trim())) return "outdated";
    else return "updated";
} catch (Exception e) {
    return "{#} CLIENT PAYLOAD FAILED! {#} " + e.Message;
}