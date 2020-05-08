//assembly System;
//assembly System.IO;
//assembly System.Linq;

using System;
using System.IO;
using System.Linq;

string passthrough = ""; //$INTELLISENSE

bool fileMissingIsOk = false; //$CONFIG | File Missing is Acceptable | Whether or not to score if the file is missing.
int minCountToScore = 0; //$CONFIG | Minimum Count to Score | The minimum number of matches found to pass the check. Set to 0 if you want no matches.
int maxCountToScore = 0; //$CONFIG | Maximum Count to Score | The maximum number of matches found to pass the check. Set to 0 if you want no matches, set to -1 if you want any number of matches.

if (passthrough == "FILE DOESNT EXIST") return fileMissingIsOk;
int matches = int.Parse(passthrough);
if (maxCountToScore < 0) return matches > 0;
else if (minCountToScore == 0 && maxCountToScore == 0) {
    return matches == 0;
} else return matches >= minCountToScore && matches <= maxCountToScore;