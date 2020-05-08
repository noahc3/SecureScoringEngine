//assembly System;
//assembly System.IO;
//assembly System.Linq;

using System;
using System.IO;
using System.Linq;

string passthrough = ""; //$INTELLISENSE

bool directoryRemovedIsAcceptable = true; //$CONFIG | Directory Removed is Acceptable | Whether or not the check passes if the directory doesn't exist.
int minCountToScore = 0; //$CONFIG | Minimum Count to Score | The minimum number of files found to pass the check. Set to 0 if you want all files to be removed.
int maxCountToScore = 0; //$CONFIG | Maximum Count to Score | The maximum number of files found to pass the check. Set to 0 if you want all files to be removed.

if (passthrough == "DIRECTORY DOESNT EXIST" && directoryRemovedIsAcceptable) return true;
else {
    int count = int.Parse(passthrough);
    if (count <= maxCountToScore && count >= minCountToScore) return true;
    return false;
}