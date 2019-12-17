//assembly System;
//assembly System.IO;
//assembly System.Linq;
//assembly SSECommon;


using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using SSECommon;

string sysctlKey = "net.ipv4.ip_forward";

try {  

    string output = ("sysctl " + sysctlKey).ExecuteAsBash();
    return output;

} catch (Exception e) {
    return "{#} CLIENT PAYLOAD FAILED! {#} " + e.Message;
}