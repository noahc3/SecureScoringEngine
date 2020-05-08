//assembly System;
//assembly System.IO;
//assembly System.Linq;
//assembly System.Text.RegularExpressions;

using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

string passthrough = ""; //$INTELLISENSE

bool antivirusShouldBeEnabled = true; //$CONFIG | Score if Realtime Monitoring Enabled | Score if the Windows Defender realtime monitoring is or is not enabled.

return (passthrough.Contains("True")) == antivirusShouldBeEnabled;