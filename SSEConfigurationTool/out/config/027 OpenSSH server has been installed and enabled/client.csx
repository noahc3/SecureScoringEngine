//assembly System;
//assembly System.IO;
//assembly System.Linq;
//assembly SSECommon;


using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using SSECommon;

string serviceName = "ssh";

try {  
    string output = ("systemctl status " + serviceName).ExecuteAsBash();
    if (output.Contains("active (running)")) return "running";
    else if (output.Contains("Loaded: not-found")) return "not installed";
    else return "disabled";
} catch (Exception e) {
    return "{#} CLIENT PAYLOAD FAILED! {#} " + e.Message;
}