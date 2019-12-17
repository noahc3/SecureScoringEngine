//assembly System;
//assembly System.IO;
//assembly System.Linq;

using System;
using System.IO;
using System.Linq;

string passthrough = ""; //$INTELLISENSE

bool shouldBeRemembered = true; //$CONFIG | Score if Passwords Remembered | Score if passwords are or are not remembered.
bool fileMissingIsOk = false; //$CONFIG | File Missing is OK | Whether or not the check should pass if the common-password file is not found. Enable this if the system default is acceptable.

return fileMissingIsOk && (passthrough == "FILE DOESNT EXIST") || (passthrough == "true") == shouldBeRemembered;