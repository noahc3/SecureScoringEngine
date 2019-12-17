//assembly System;
//assembly System.IO;
//assembly System.Linq;

using System;
using System.IO;
using System.Linq;


string key = "PasswordAuthentication";
string expectedValue = "no";
bool shouldMatchExpectedValue = true;

if (passthrough == "NOT INSTALLED") return false;
return (passthrough.ToLower().Contains(key.ToLower().Trim() + " " + expectedValue.ToLower().Trim()) == shouldMatchExpectedValue);