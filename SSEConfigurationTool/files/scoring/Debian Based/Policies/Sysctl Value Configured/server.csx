//assembly System;
//assembly System.IO;
//assembly System.Linq;

using System;
using System.IO;
using System.Linq;

string passthrough = ""; //$INTELLISENSE

string expectedValue; //$CONFIG | Expected Value | The value the setting should be to pass the check.
bool shouldMatch = true; //$CONFIG | Score if Matches Expected Values | Score if the sysctl value does or does not match.


return passthrough.Split('=').Last().Trim().ToLower() == expectedValue.ToLower() == shouldMatch;