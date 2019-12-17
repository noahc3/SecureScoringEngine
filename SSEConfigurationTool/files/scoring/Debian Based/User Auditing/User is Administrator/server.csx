//assembly System;
//assembly System.IO;
//assembly System.Linq;

using System;
using System.Linq;

string passthrough = ""; //$INTELLISENSE

bool shouldBeAdministrator = true; //$CONFIG | Score if User is Administrator | Score if the user is or is not an administrator.


return passthrough.Contains("sudo") == shouldBeAdministrator;