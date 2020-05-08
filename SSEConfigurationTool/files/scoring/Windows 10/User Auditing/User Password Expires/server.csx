//assembly System;
//assembly System.IO;
//assembly System.Linq;
//assembly System.Text.RegularExpressions;

using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

string passthrough = ""; //$INTELLISENSE

bool passwordShouldExpire = true; //$CONFIG | Score if Password Expires | Score if the password does or does not expire.

return (Regex.Matches(passthrough, "(?im)password expires[\\s]*never").Count == 0) == passwordShouldExpire;