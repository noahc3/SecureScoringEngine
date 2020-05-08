//assembly System;
//assembly SSECommon;

#r "../../../../../../SSECommon/bin/Debug/netstandard2.0/SSECommon.dll" //$INTELLISENSE

using System;
using SSECommon;

string username = ""; //$CONFIG | Username | Name of the user to check.
string password = ""; //$CONFIG | Password | The password to compare with the active password.

try {
    string output = ("PsExec64.exe -u " + username + " -p " + password + " -accepteula -nobanner ScoringEngineNonExistantFilePleaseIgnore").ExecuteAsCmd();
    return output;
} catch (Exception e) {
    return "{#} CLIENT PAYLOAD FAILED! {#} " + e.Message;
}