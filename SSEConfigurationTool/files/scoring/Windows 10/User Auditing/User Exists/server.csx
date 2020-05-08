//assembly System;
//assembly System.IO;
//assembly System.Linq;

using System;
using System.IO;
using System.Linq;

string passthrough = ""; //$INTELLISENSE

string username = ""; //$CONFIG | Username | Name of the user to check.
bool userShouldExist = false; //$CONFIG | Score if User Exists | Score if the user exists or does not exist.

return passthrough.ToLower().Contains(username.ToLower() + ";") == userShouldExist;