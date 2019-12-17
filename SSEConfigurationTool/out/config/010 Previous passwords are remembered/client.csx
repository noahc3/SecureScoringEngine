//assembly System;
//assembly System.IO;
//assembly System.Linq;
//assembly System.Text.RegularExpressions;

using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

string path = "/etc/pam.d/common-password";
try {  
    if (!File.Exists(path)) return "FILE DOESNT EXIST";
    string contents = File.ReadAllText(path);
    if (contents.Contains("pam_pwhistory.so")) return "true";
    else return "false";
} catch (Exception e) {
    return "{#} CLIENT PAYLOAD FAILED! {#} " + e.Message;
}