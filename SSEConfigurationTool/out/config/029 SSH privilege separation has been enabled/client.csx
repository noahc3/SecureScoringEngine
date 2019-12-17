//assembly System;
//assembly System.IO;
//assembly System.Linq;
//assembly SSECommon;


using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using SSECommon;

try {
    string output = "sshd -T".ExecuteAsBash();
    if (String.IsNullOrWhiteSpace(output)) return "NOT INSTALLED";
    return output;
} catch (Exception e) {
    return "{#} CLIENT PAYLOAD FAILED! {#} " + e.Message;
}