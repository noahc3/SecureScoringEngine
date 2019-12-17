//assembly System;
//assembly System.IO;
//assembly System.Linq;

using System;
using System.IO;
using System.Linq;


int minCountToScore = 0;
int maxCountToScore = 0;

int count = 0;

foreach (char k in passthrough) if (k == '1') count++;
return count >= minCountToScore && count <= maxCountToScore;