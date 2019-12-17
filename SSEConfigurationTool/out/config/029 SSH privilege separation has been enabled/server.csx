//assembly System;
//assembly System.IO;
//assembly System.Linq;

using System;
using System.IO;
using System.Linq;


string key = "UsePrivilegeSeparation";
string expectedValue = "no";
bool shouldMatchExpectedValue = false;

if (passthrough == "NOT INSTALLED") return false;
return (passthrough.ToLower().Contains(key.ToLower().Trim() + " " + expectedValue.ToLower().Trim()) == shouldMatchExpectedValue);