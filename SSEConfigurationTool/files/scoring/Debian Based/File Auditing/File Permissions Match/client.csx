//assembly System;
//assembly System.IO;
//assembly System.Linq;
//assembly SSECommon;

#r "../../../../../../SSECommon/bin/Debug/netstandard2.0/SSECommon.dll" //$INTELLISENSE

using System;
using System.IO;
using SSECommon;

string path; //$CONFIG | File Path | Path to the file to check permissions of.

try {  
    if (!File.Exists(path)) return "FILE DOESNT EXIST";
    return ("stat -c %a " + path).ExecuteAsBash().Trim();

} catch (Exception e) {
    return "{#} CLIENT PAYLOAD FAILED! {#} " + e.Message;
}