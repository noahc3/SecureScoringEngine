//assembly System;
//assembly System.IO;
//assembly System.Linq;
//assembly System.Text.RegularExpressions;

using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

string fileLocation; //$CONFIG | File Path | The path to the file to check for text.
string regex; //$CONFIG | Regular Expression | The regex pattern to match.

try {
    if (!File.Exists(fileLocation)) return "FILE DOESNT EXIST";
    string contents = File.ReadAllText(fileLocation);
    return Regex.Matches(contents, regex).Count.ToString();
} catch (Exception e) {
    return "{#} CLIENT PAYLOAD FAILED! {#} " + e.Message;
}