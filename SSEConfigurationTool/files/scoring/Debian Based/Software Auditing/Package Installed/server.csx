//assembly System;
//assembly System.IO;
//assembly System.Linq;

using System;
using System.IO;
using System.Linq;

string passthrough = ""; //$INTELLISENSE

bool shouldBeInstalled = false; //$CONFIG | Score if Package Installed | Whether to score if the package is or is not installed.

return (passthrough == "true") == shouldBeInstalled;