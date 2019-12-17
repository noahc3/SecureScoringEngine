//assembly System;
//assembly System.IO;
//assembly System.Linq;

using System;
using System.IO;
using System.Linq;

string passthrough = ""; //$INTELLISENSE

int minimumAcceptableValue = 7; //$CONFIG | Minimum Acceptable Value | The lowest value permitted to pass the check.
int maximumAcceptableValue = 21; //$CONFIG | Maximum Acceptable Value | The highest value permitted to pass the check.
bool allowNotDefined = false; //$CONFIG | Allow Undefined | Whether or not having no custom minimum password age passes the check. Enable this if the default system value is acceptable to pass the check.

int val;

if (allowNotDefined && passthrough.Contains("NOT FOUND")) return true;
else if (int.TryParse(passthrough, out val)) {
    if (val >= minimumAcceptableValue && val <= maximumAcceptableValue) return true;
    else return false;
} else return false;