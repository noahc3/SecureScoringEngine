using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SSEConfigurationTool.Data {
    public class Draft {
        public int Version = 1;
        public string Platform;
        public List<ConfiguredScoringItem> ConfiguredScoringItems;
    }
}
