//assembly System;
//assembly System.IO;
//assembly System.Linq;
//assembly SSECommon;


using System;
using System.IO;
using SSECommon;

string path = "/etc/ssh/sshd_config";

try {  
    if (!File.Exists(path)) return "FILE DOESNT EXIST";
    return ("stat -c %a " + path).ExecuteAsBash().Trim();

} catch (Exception e) {
    return "{#} CLIENT PAYLOAD FAILED! {#} " + e.Message;
}