//assembly System;
//assembly System.IO;
//assembly System.Linq;
//assembly SSECommon;


using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using SSECommon;

string command = "ufw status verbose";

try {  

    string output = command.ExecuteAsBash();
    return output;

} catch (Exception e) {
    return "{#} CLIENT PAYLOAD FAILED! {#} " + e.Message;
}