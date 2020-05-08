//assembly System;
//assembly System.IO;
//assembly System.Linq;
//assembly System.Text.RegularExpressions;

using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

string passthrough = ""; //$INTELLISENSE

bool firewallShouldBeEnabled = true; //$CONFIG | Score if Firewall Enabled | Score if the firewall is or is not enabled.

return (Regex.Matches(passthrough, "(?im)State[\\s]*OFF").Count == 0) == firewallShouldBeEnabled;