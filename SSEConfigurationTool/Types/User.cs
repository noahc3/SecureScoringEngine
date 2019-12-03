using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SSEConfigurationTool.Types {
    public class User {
        public string Name = "";
        public VulnInfo RewardIfExists = new VulnInfo();
        public VulnInfo RewardIfRemoved = new VulnInfo();
        public VulnInfo RewardIfAdmin = new VulnInfo();
        public VulnInfo RewardIfNotAdmin = new VulnInfo();
        public VulnInfo RewardIfPasswordChanged = new VulnInfo();
        public VulnInfo RewardIfLocked = new VulnInfo();
    }
}
