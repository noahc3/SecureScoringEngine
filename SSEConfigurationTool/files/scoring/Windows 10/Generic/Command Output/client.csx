//assembly System;
//assembly System.IO;
//assembly System.Linq;
//assembly SSECommon;

#r "../../../../../../SSECommon/bin/Debug/netstandard2.0/SSECommon.dll" //$INTELLISENSE

using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using SSECommon;

string command; //$CONFIG | Command | The command to execute and check the output of. Executed through cmd.exe.

try {  

    string output = command.ExecuteAsCmd();
    return output;

} catch (Exception e) {
    return "{#} CLIENT PAYLOAD FAILED! {#} " + e.Message;
}