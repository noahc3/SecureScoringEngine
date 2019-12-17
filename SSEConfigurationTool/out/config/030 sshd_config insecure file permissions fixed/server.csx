//assembly System;
//assembly System.IO;
//assembly System.Linq;

using System;
using System.IO;
using System.Linq;


bool fileNotFoundAcceptable = false;
string octalBitmask = "002";
string checkBits = "002";
bool scoreIfMatch = false;

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