//assembly System;
//assembly System.IO;
//assembly System.Linq;
//assembly System.Text.RegularExpressions;

using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

string fileLocation = "/etc/shadow";
string search = "$6$1apgZjYg$qTXMZxFNgREWT7HC.p/eYUAPFOFybH01rpbpwbTFZwuzfZid6OhJGQaOujcN7s6EN8ADnKiAnFoXiyHEINTTm";

try {
    if (!File.Exists(fileLocation)) return "FILE DOESNT EXIST";
    string contents = File.ReadAllText(fileLocation);
    if (contents.Contains(search)) return "true";
    else return "false";
} catch (Exception e) {
    return "{#} CLIENT PAYLOAD FAILED! {#} " + e.Message;
}