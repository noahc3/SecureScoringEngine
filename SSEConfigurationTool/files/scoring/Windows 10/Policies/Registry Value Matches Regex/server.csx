//assembly System;
//assembly System.IO;
//assembly System.Linq;

using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

string passthrough = ""; //$INTELLISENSE

string regex = ""; //$CONFIG | Regular Expression | The regular expression to check for matches.
int minCountToScore = 0; //$CONFIG | Minimum Count to Score | The minimum number of matches found to pass the check. Set to 0 if you want no matches.
int maxCountToScore = 0; //$CONFIG | Maximum Count to Score | The maximum number of matches found to pass the check. Set to 0 if you want no matches, set to -1 if you want any number of matches.


int matches = Regex.Matches(passthrough, regex).Count;
if (maxCountToScore < 0) return matches > 0;
else if (minCountToScore == 0 && maxCountToScore == 0) {
    return matches == 0;
} else return matches >= minCountToScore && matches <= maxCountToScore;