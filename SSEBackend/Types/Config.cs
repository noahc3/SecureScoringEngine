using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SSEBackend.Types
{
    public class Config
    {
        public bool DebugSvcs = false;
        public bool GiveUpSecurityAndDontBotherDisablingDebugModeWhenNormalClientsConnect = false;
    }
}
