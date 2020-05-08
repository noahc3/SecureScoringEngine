//assembly System;
//assembly System.IO;
//assembly System.Linq;
//assembly System.Text.RegularExpressions;

using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

string passthrough = ""; //$INTELLISENSE

string eventToAudit = ""; //$CONFIG | Event to Audit | Accepted Values: AuditSystemEvents, AuditLogonEvents, AuditObjectAccess, AuditPrivilegeUse, AuditPolicyChange, AuditAccountManage, AuditProcessTracking, AuditDSAccess, AuditAccountLogon
int expectedBitwiseValue = 3; //$CONFIG | Bitwise Value to Check | 0 = None, 1 = Success, 2 = Failure, 3 = Success and Failure. Note this checks for equality, and does not perform a bitwise AND operation.

try {
    int output;
    bool success = int.TryParse(Regex.Match(passthrough, "(?im)(?<=" + eventToAudit.Trim() + "\\s+=\\s+)[0-9]+").Value, out output);
    return output == expectedBitwiseValue;
} catch (Exception) { }

return false;