//assembly System;
//assembly System.IO;
//assembly System.Linq;
//assembly SSECommon;


using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using SSECommon;

string command = "getent passwd www-data | cut -d: -f7";

try {  

    string output = command.ExecuteAsBash();
    return output;

} catch (Exception e) {
    return "{#} CLIENT PAYLOAD FAILED! {#} " + e.Message;
}