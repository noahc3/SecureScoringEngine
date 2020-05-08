//assembly System;
//assembly SSECommon;

#r "../../../../../../SSECommon/bin/Debug/netstandard2.0/SSECommon.dll" //$INTELLISENSE

using System;
using SSECommon;

string username = ""; //$CONFIG | Username | Name of the user to check.

try {
    string output = ("PsExec64 -u " + username + " -p \"\" -accepteula -nobanner ScoringEngineNonExistantFilePleaseIgnore").ExecuteAsCmd();
    return output;
} catch (Exception e) {
    return "{#} CLIENT PAYLOAD FAILED! {#} " + e.Message;
}