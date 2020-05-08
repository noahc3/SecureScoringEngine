//assembly System;
//assembly System.IO;
//assembly SSECommon;

#r "../../../../../../SSECommon/bin/Debug/netstandard2.0/SSECommon.dll" //$INTELLISENSE

using System;
using System.IO;
using SSECommon;

try {
    string outputPath = Path.GetRandomFileName();
    ("secedit /export /cfg \"" + outputPath + "\"").ExecuteAsCmd();
    string output = File.ReadAllText(outputPath);
    File.Delete(outputPath);
    return output;
} catch (Exception e) {
    return "{#} CLIENT PAYLOAD FAILED! {#} " + e.Message;
}