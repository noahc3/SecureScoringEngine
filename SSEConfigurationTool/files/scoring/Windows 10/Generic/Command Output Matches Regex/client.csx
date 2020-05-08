//assembly System;
//assembly System.IO;
//assembly System.Linq;
//assembly System.Text.RegularExpressions;
//assembly SSECommon;

#r "../../../../../../SSECommon/bin/Debug/netstandard2.0/SSECommon.dll" //$INTELLISENSE

using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using SSECommon;

string command; //$CONFIG | Command | The command to check the output of. Executed through cmd.exe.
string regex; //$CONFIG | Regular Expression | The regex pattern to match.

try {
    string contents = command.ExecuteAsCmd();
    return Regex.Matches(contents, regex).Count.ToString();
} catch (Exception e) {
    return "{#} CLIENT PAYLOAD FAILED! {#} " + e.Message;
}