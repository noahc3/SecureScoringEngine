//assembly System;
//assembly System.IO;
//assembly System.Linq;
//assembly System.Text.RegularExpressions;

using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;


string answer = "ed8cfc1fc1a486ef86af53151f0cbdc0";
bool ignoreCapitalization = true;
bool ignoreWhitespace = true;

if (ignoreCapitalization) {
    answer = answer.ToLower();
    passthrough = passthrough.ToLower();
}

if (ignoreWhitespace) {
    answer = Regex.Replace(answer, @"\s+", "");
    passthrough = Regex.Replace(passthrough, @"\s+", "");
}

return passthrough.Trim() == answer.Trim();