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
    string output = ("sc query " + serviceName).ExecuteAsCmd();
    if (output.Contains("RUNNING")) return "running";
    else if (output.Contains("The specified service does not exist as an installed service")) return "not installed";
    else return "disabled";
} catch (Exception e) {
    return "{#} CLIENT PAYLOAD FAILED! {#} " + e.Message;
}