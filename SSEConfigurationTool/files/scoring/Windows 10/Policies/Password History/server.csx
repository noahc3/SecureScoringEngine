//assembly System;
//assembly System.IO;
//assembly System.Linq;
//assembly System.Text.RegularExpressions;

using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

string passthrough = ""; //$INTELLISENSE

int minimumAcceptableValue = 5; //$CONFIG | Minimum Acceptable Value | The lowest value permitted to pass the check.
int maximumAcceptableValue = 20; //$CONFIG | Maximum Acceptable Value | The highest value permitted to pass the check.

int value;
bool success = int.TryParse(Regex.Match(passthrough, "(?im)(?<=(Length of password history maintained:[\\s]*))[0-9]+").Value.Replace("None", "0"), out value);

if (success) {
    return value >= minimumAcceptableValue && value <= maximumAcceptableValue;
}

return false;