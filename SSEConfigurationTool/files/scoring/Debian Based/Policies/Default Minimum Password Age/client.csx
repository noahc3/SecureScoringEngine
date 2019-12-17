//assembly System;
//assembly System.IO;
//assembly System.Linq;
//assembly System.Text.RegularExpressions;

using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

try {  
    string[] contents = File.ReadAllLines("/etc/login.defs");
    for (int i = contents.Length - 1; i >=0; i--) {
        string line = contents[i].Trim();
        if (line.StartsWith("#")) continue;
        else if (line.Contains("PASS_MIN_DAYS")) {
            return Regex.Split(line, "PASS_MIN_DAYS")[1].Trim();
        }
    }
    return "NOT FOUND";
} catch (Exception e) {
    return "{#} CLIENT PAYLOAD FAILED! {#} " + e.Message;
}