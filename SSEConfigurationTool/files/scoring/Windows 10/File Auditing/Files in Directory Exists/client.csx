//assembly System;
//assembly System.IO;
//assembly System.Linq;
//assembly SSECommon;

using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

string directory; //$CONFIG | Directory Path | Path to the directory with files to check for.
string extension; //$CONFIG | File Extension | Only count files with this extension. Leave blank to ignore file extension. Case insensitive.

try {  
    if (!Directory.Exists(directory)) return "DIRECTORY DOESNT EXIST";
    int count = Directory.EnumerateFiles(directory).Where((x) => x.EndsWith(extension.ToLower())).Count();
    return "" + count;

} catch (Exception e) {
    return "{#} CLIENT PAYLOAD FAILED! {#} " + e.Message;
}