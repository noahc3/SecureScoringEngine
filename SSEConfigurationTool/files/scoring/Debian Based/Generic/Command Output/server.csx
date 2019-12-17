//assembly System;
//assembly System.IO;
//assembly System.Linq;

using System;
using System.IO;
using System.Linq;

string passthrough = ""; //$INTELLISENSE

string expectedValue; //$CONFIG | Search Value | The value to search for in the command output.
bool shouldMatch = true; //$CONFIG | Score if Matches Expected Values | Score if the sysctl value does or does not match.


return passthrough.Trim().ToLower().Contains(expectedValue.Trim().ToLower()) == shouldMatch;