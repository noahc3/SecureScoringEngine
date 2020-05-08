//assembly System;
//assembly System.IO;
//assembly System.Linq;
//assembly System.Text.RegularExpressions;

using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

string passthrough = ""; //$INTELLISENSE

int minimumAcceptableValue = 21; //$CONFIG | Minimum Acceptable Value (minutes) | The lowest value permitted to pass the check.
int maximumAcceptableValue = 90; //$CONFIG | Maximum Acceptable Value (minutes) | The highest value permitted to pass the check.

int value;
bool success = int.TryParse(Regex.Match(passthrough, "(?im)(?<=(Lockout duration \\(minutes\\):[\\s]*))[0-9]+").Value, out value);

if (success) {
    return value >= minimumAcceptableValue && value <= maximumAcceptableValue;
}

return false;