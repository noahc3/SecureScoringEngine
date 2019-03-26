using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using SSECommon.Types;

namespace SSEBackend.Types {
    public class Team {
        public string UUID;
        public List<string> ValidRuntimeIDs;
        public Dictionary<Runtime, byte[]> EncKeys;
    }
}
