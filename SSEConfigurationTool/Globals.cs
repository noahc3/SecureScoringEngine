using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSEConfigurationTool {
    public class Globals {
        public static PlatformID platform;
        public static void InitializeGlobals() {
            platform = Environment.OSVersion.Platform;
        }
    }
}
