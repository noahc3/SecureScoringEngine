//assembly System;
//assembly System.IO;
//assembly System.Linq;

using System;
using System.IO;
using System.Linq;

string passthrough = ""; //$INTELLISENSE

bool allowServiceNotFound = true; //$CONFIG | Score if Service Not Found | Whether to score if the service was not found (ex. uninstalled).
bool serviceShouldBeEnabled = false; //$CONFIG | Score if Service Enabled | Whether to score if the service is or is not enabled.



return (allowServiceNotFound && passthrough == "not installed") || (passthrough == "running" == serviceShouldBeEnabled);