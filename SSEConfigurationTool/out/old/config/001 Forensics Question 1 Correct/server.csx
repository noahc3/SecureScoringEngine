//assembly System;
//assembly System.IO;
//assembly System.Linq;
//assembly System.Text.RegularExpressions;

using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;


string answer = "w1cK3d_1S_g00D_6uTnvdqDaO";
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