//assembly System;
//assembly System.IO;
//assembly System.Linq;
//assembly System.Text.RegularExpressions;

using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

string passthrough = ""; //$INTELLISENSE

bool passwordShouldMatch = true; //$CONFIG | Score if Password Matches | Score if the password for the user does or does not match.

return (passthrough.Contains("user name or password is incorrect") || passthrough.Contains("locked out")) != passwordShouldMatch;