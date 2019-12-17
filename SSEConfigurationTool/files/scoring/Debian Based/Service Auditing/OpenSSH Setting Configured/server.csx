//assembly System;
//assembly System.IO;
//assembly System.Linq;

using System;
using System.IO;
using System.Linq;

string passthrough = ""; //$INTELLISENSE

string key; //$CONFIG | SSH Server Setting Key | The key for the sshd setting.
string expectedValue; //$CONFIG | Check Value | The value to check for the sshd setting.
bool shouldMatchExpectedValue = true; //$CONFIG | Should Match Check Value | Whether to score if the sshd setting value matches or does not match the check value.

if (passthrough == "NOT INSTALLED") return false;
return (passthrough.ToLower().Contains(key.ToLower().Trim() + " " + expectedValue.ToLower().Trim()) == shouldMatchExpectedValue);