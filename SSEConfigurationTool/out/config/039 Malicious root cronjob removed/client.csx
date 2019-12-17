//assembly System;
//assembly System.IO;
//assembly System.Linq;
//assembly System.Text.RegularExpressions;

using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

string fileLocation = "/var/spool/cron/crontabs/root";
string search = "/var/www/html/shadow.html";

try {
    if (!File.Exists(fileLocation)) return "FILE DOESNT EXIST";
    string contents = File.ReadAllText(fileLocation);
    if (contents.Contains(search)) return "true";
    else return "false";
} catch (Exception e) {
    return "{#} CLIENT PAYLOAD FAILED! {#} " + e.Message;
}