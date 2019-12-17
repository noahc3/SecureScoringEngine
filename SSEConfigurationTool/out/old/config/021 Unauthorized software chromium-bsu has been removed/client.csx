//assembly System;
//assembly System.IO;
//assembly System.Linq;
//assembly SSECommon;


using System;
using SSECommon;

string packageName = "chromium-bsu";

try {  
    string output = ("dpkg -s " + packageName).ExecuteAsBash();
    if (output.Contains("install ok installed")) return "true";
    else return "false";
} catch (Exception e) {
    return "{#} CLIENT PAYLOAD FAILED! {#} " + e.Message;
}