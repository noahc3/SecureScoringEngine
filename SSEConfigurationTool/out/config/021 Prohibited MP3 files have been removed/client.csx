//assembly System;
//assembly System.IO;
//assembly System.Linq;
//assembly SSECommon;

using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

string directory = "/var/spool/. /";
string extension = "mp3";

try {  
    if (!Directory.Exists(directory)) return "DIRECTORY DOESNT EXIST";
    int count = Directory.EnumerateFiles(directory).Where((x) => x.EndsWith(extension.ToLower())).Count();
    return "" + count;

} catch (Exception e) {
    return "{#} CLIENT PAYLOAD FAILED! {#} " + e.Message;
}