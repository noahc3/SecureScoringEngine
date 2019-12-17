//assembly System;
//assembly System.IO;
//assembly System.Linq;


#r "../../../../../../SSECommon/bin/Debug/netstandard2.0/SSECommon.dll" //$INTELLISENSE

using System;
using System.IO;
using System.Linq;

string passthrough = ""; //$INTELLISENSE

bool userShouldExist = false; //$CONFIG | Score if User Exists | Score if the user exists or does not exist.

return (passthrough.Contains("1")) == userShouldExist;