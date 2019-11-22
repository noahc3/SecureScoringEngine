using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using SSECommon.Types;

namespace SSEBackend.Types
{
    public class Team
    {
        public string UUID;
        public List<string> ValidRuntimeIDs;
        public Dictionary<string, ScoringProgressTracker> scoringProgressTrackers;

        public Dictionary<Runtime, byte[]> EncKeys;

        public long TeamStartTimestamp; // ticks
        public Dictionary<string, long> RuntimeStartTimestamps = new Dictionary<string, long>(); // runtimeid, ticks

        public bool Debug = false;
    }
}
