//assembly System;
//assembly System.IO;
//assembly System.Linq;

using System;
using System.IO;
using System.Linq;

string passthrough = ""; //$INTELLISENSE

bool fileNotFoundAcceptable = true; //$CONFIG | File Not Found is Acceptable | Whether or not the check passes if the file doesn't exist.
string octalBitmask; //$CONFIG | Octal Bitmask | Three digit octal bitmask to bitwise AND with the files permissions before checking. Use this to ignore certain permissions.
string checkBits; //$CONFIG | Octal Permission Check | Three digit octal permissions to check.
bool scoreIfMatch = true; //$CONFIG | Score if Permissions Match Check | Whether to score if the file permissions match or do not match the permission check.

if (passthrough == "DIRECTORY DOESNT EXIST" && fileNotFoundAcceptable) return true;
else {
    bool match = true;
    for (int i = 0; i < 3; i++) {
        int filePerm;
        int bitmask;
        int checkPerm;

        if (!int.TryParse(passthrough[i].ToString(), out filePerm)) return false;
        if (!int.TryParse(octalBitmask[i].ToString(), out bitmask)) return false;
        if (!int.TryParse(checkBits[i].ToString(), out checkPerm)) return false;

        filePerm = filePerm & bitmask;

        if (filePerm != checkPerm) {
            match = false;
            break;
        }
    }
    return match == scoreIfMatch;
}