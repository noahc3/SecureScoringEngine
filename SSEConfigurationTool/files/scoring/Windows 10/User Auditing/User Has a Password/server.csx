//assembly System;
//assembly System.IO;
//assembly System.Linq;
//assembly System.Text.RegularExpressions;

using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

string passthrough = ""; //$INTELLISENSE

bool shouldHavePassword = true; //$CONFIG | Score if User Has Passowrd | Score if the user does or does not have a password.

return passthrough.Contains("The user name or password is incorrect.") == shouldHavePassword;