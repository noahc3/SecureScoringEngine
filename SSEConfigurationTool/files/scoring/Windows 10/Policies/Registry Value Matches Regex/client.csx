//assembly System;
//assembly System.IO;
//assembly System.Linq;
//assembly Microsoft.Win32.Registry;
//assembly System.Text.RegularExpressions;
//assembly SSECommon;

#r "../../../../../../SSECommon/bin/Debug/netstandard2.0/SSECommon.dll" //$INTELLISENSE

using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using SSECommon;
using Microsoft.Win32;

string key; //$CONFIG | Registry Key | The full registry path of the key, beginning with a valid registry root, such as HKEY_CURRENT_USER.
string name; //$CONFIG | Registry Pair Name | The name of the name/value pair.

try {
    string value = Registry.GetValue(key, name, "NOT FOUND").ToString();
    return value;
} catch (Exception e) {
    return "{#} CLIENT PAYLOAD FAILED! {#} " + e.Message;
}