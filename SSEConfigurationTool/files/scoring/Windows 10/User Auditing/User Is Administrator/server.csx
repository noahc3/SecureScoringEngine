//assembly System;
//assembly System.IO;
//assembly System.Linq;
//assembly System.Text.RegularExpressions;

using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

string passthrough = ""; //$INTELLISENSE

bool shouldBeAdministrator = true; //$CONFIG | Score if User is Administrator | Score if the user is or is not an administrator.

return (Regex.Matches(passthrough, "(?im)Group Memberships[\\s\\S]*Administrators").Count > 0) == shouldBeAdministrator;