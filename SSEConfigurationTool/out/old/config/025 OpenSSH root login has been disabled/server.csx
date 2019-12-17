//assembly System;
//assembly System.IO;
//assembly System.Linq;

using System;
using System.IO;
using System.Linq;


string key = "PermitRootLogin";
string expectedValue = "no";
bool shouldMatchExpectedValue = true;


return (passthrough.ToLower().Contains(key.ToLower().Trim() + " " + expectedValue.ToLower().Trim()) == shouldMatchExpectedValue);