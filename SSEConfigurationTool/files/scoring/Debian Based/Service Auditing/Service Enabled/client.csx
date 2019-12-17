//assembly System;
//assembly System.IO;
//assembly System.Linq;
//assembly SSECommon;

#r "../../../../../../SSECommon/bin/Debug/netstandard2.0/SSECommon.dll" //$INTELLISENSE

using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using SSECommon;

string serviceName; //$CONFIG | Service Name | Name of the service to check the status of.

try {  
    string output = ("systemctl status " + serviceName).ExecuteAsBash();
    if (output.Contains("active (running)")) return "running";
    else if (output.Contains("Loaded: not-found")) return "not installed";
    else return "disabled";
} catch (Exception e) {
    return "{#} CLIENT PAYLOAD FAILED! {#} " + e.Message;
}