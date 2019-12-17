//assembly System;
//assembly System.IO;
//assembly System.Linq;
//assembly SSECommon;

#r "../../../../../../SSECommon/bin/Debug/netstandard2.0/SSECommon.dll" //$INTELLISENSE

using System;
using SSECommon;

string packageName; //$CONFIG | Package Name | Name of the package to check the install status of.

try {  
    string output = ("dpkg -s " + packageName).ExecuteAsBash();
    if (output.Contains("install ok installed")) return "true";
    else return "false";
} catch (Exception e) {
    return "{#} CLIENT PAYLOAD FAILED! {#} " + e.Message;
}