//assembly System;
//assembly System.IO;
//assembly System.Linq;

using System;
using System.IO;
using System.Linq;

string passthrough = ""; //$INTELLISENSE

int minCountToScore = 0; //$CONFIG | Minimum Count to Score | The minimum number of files found to pass the check. Set to 0 if you want all files to be removed.
int maxCountToScore = 0; //$CONFIG | Maximum Count to Score | The maximum number of files found to pass the check. Set to 0 if you want all files to be removed.

int count = 0;

foreach (char k in passthrough) if (k == '1') count++;
return count >= minCountToScore && count <= maxCountToScore;