using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SSEConfigurationTool.Data {
    public class Utilities {
        public static bool MatchesLogicalDistros(params LogicalDistro[] distros) {
            return distros.Contains(Globals.distro);
        }
    }
}
