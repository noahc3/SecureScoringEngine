//assembly System;
//assembly System.IO;
//assembly System.Linq;

using System;
using System.IO;
using System.Linq;


bool directoryRemovedIsAcceptable = true;
int minCountToScore = 0;
int maxCountToScore = 0;

if (passthrough == "DIRECTORY DOESNT EXIST" && directoryRemovedIsAcceptable) return true;
else {
    int count = int.Parse(passthrough);
    if (count <= maxCountToScore && count >= minCountToScore) return true;
    return false;
}