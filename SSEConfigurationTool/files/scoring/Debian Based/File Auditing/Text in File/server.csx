//assembly System;
//assembly System.IO;
//assembly System.Linq;

using System;
using System.IO;
using System.Linq;

string passthrough = ""; //$INTELLISENSE

bool fileMissingIsOk = false; //$CONFIG | File Missing is Acceptable | Whether or not to score if the file is missing.
bool scoreIfTextFound = false; //$CONFIG | Score if Text Found | Whether or not to score if the text is or is not found.

return (fileMissingIsOk && passthrough == "FILE DOESNT EXIST") || (passthrough == "true" == scoreIfTextFound) ;