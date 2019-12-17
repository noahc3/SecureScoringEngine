//assembly System;
//assembly System.IO;
//assembly System.Linq;
//assembly SSECommon


using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using SSECommon;

try {
    return ("sshd -T").ExecuteAsBash();
} catch (Exception e) {
    return "{#} CLIENT PAYLOAD FAILED! {#} " + e.Message;
}