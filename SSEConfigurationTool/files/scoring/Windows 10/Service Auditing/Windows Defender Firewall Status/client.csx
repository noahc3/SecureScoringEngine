//assembly System;
//assembly SSECommon;

#r "../../../../../../SSECommon/bin/Debug/netstandard2.0/SSECommon.dll" //$INTELLISENSE

using System;
using SSECommon;

try {
    string output = "netsh advfirewall show allprofiles state".ExecuteAsCmd();
    return output;
} catch (Exception e) {
    return "{#} CLIENT PAYLOAD FAILED! {#} " + e.Message;
}