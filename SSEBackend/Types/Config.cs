﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SSEBackend.Types
{
    public class Config
    {
        public string BindIp = "127.0.0.1";
        public int BindPort = 5000;
        public bool OfflineMode = false;
        public bool DebugSvcs = true;
        public bool GiveUpSecurityAndDontBotherDisablingDebugModeWhenNormalClientsConnect = false;
        public bool DetailedScoringReports = false;
        public bool SaveScoringReportsOnServer = false;
    }
}
