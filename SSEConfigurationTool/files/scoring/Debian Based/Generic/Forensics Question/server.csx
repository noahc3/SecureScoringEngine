//assembly System;
//assembly System.IO;
//assembly System.Linq;
//assembly System.Text.RegularExpressions;

using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

string passthrough = ""; //$INTELLISENSE

string answer; //$CONFIG | Correct Answer | The correct answer to the forensics question.
bool ignoreCapitalization = true; //$CONFIG | Ignore Capitalization | Whether to ignore capitalization in the answer.
bool ignoreWhitespace = false; //$CONFIG | Ignore Whitespace | Whether to ignore whitespace like spaces and tabs in the answer.

if (ignoreCapitalization) {
    answer = answer.ToLower();
    passthrough = passthrough.ToLower();
}

if (ignoreWhitespace) {
    answer = Regex.Replace(answer, @"\s+", "");
    passthrough = Regex.Replace(passthrough, @"\s+", "");
}

return passthrough.Trim() == answer.Trim();