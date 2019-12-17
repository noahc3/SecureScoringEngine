//assembly System;
//assembly System.IO;
//assembly System.Linq;
//assembly SSECommon;


using System;
using SSECommon;

string username = "brenda";

try {  

    string output = ("grep -c '^" + username + ":' /etc/passwd").ExecuteAsBash();
    return output;

} catch (Exception e) {
    return "{#} CLIENT PAYLOAD FAILED! {#} " + e.Message;
}