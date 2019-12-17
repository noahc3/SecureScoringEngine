//assembly System;
//assembly System.IO;
//assembly System.Linq;
//assembly System.Collections;
//assembly SSECommon;

using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Collections.Generic;

List<string> files = new List<string>() { "/home/jdimon/.config/.openra/OpenRA-Red-Alert-x86_64.AppImage" };

try {
    string output = "";
    foreach(string k in files) {
        if (File.Exists(k)) output += "1";
        else output += "0";
    }
    return output;
} catch (Exception e) {
    return "{#} CLIENT PAYLOAD FAILED! {#} " + e.Message;
}