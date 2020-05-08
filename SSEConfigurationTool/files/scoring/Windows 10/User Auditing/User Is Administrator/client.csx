//assembly System;
//assembly SSECommon;

#r "../../../../../../SSECommon/bin/Debug/netstandard2.0/SSECommon.dll" //$INTELLISENSE

using System;
using SSECommon;

string username = ""; //$CONFIG | Username | Name of the user to check.

try {
    string output = ("net user " + username).ExecuteAsCmd();
    return output;
} catch (Exception e) {
    return "{#} CLIENT PAYLOAD FAILED! {#} " + e.Message;
}