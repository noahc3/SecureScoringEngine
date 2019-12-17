//assembly System;
//assembly System.IO;
//assembly System.Linq;

using System;
using System.IO;
using System.Linq;


int minimumAcceptableValue = 7;
int maximumAcceptableValue = 21;
bool allowNotDefined = false;

int val;

if (allowNotDefined && passthrough.Contains("NOT FOUND")) return true;
else if (int.TryParse(passthrough, out val)) {
    if (val >= minimumAcceptableValue && val <= maximumAcceptableValue) return true;
    else return false;
} else return false;