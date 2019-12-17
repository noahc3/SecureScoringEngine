//assembly System;
//assembly System.IO;
//assembly System.Linq;

using System;
using System.IO;
using System.Linq;

string passthrough = ""; //$INTELLISENSE

string checkGroup; //$CONFIG | Group Name | Name of the group to check if the user is part of.
bool userShouldBeInGroup = false;  //$CONFIG | Score if User is in Group | Score if the user is or is not part of the group.

return passthrough.Contains(checkGroup) == userShouldBeInGroup;