//assembly System;
//assembly System.IO;
//assembly System.Linq;
//assembly System.Text.RegularExpressions;

using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

string passthrough = ""; //$INTELLISENSE

int minimumAcceptableValue = 3; //$CONFIG | Minimum Acceptable Value | The lowest value permitted to pass the check. Set to -1 to allow never.
int maximumAcceptableValue = 6; //$CONFIG | Maximum Acceptable Value | The highest value permitted to pass the check. Set to -1 to only allow never.

int value;
bool success = int.TryParse(Regex.Match(passthrough, "(?im)(?<=(Lockout threshold:[\\s]*))[0-9]+").Value.Replace("Never", "-1"), out value);

if (success) {
    return value >= minimumAcceptableValue && value <= maximumAcceptableValue;
}

return false;