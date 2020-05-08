using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SSEConfigurationTool.Data {
    public class Draft {
        public static int LatestVersion = 2;
        public int Version = LatestVersion;
        public string Platform;
        public List<ConfiguredScoringItem> ConfiguredScoringItems;
    }
}
