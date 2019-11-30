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
        public string Name;
        public List<string> ValidRuntimeIDs;
        public Dictionary<string, ScoringProgressTracker> ScoringProgressTrackers;
        public Dictionary<string, int> RuntimeLastScores = new Dictionary<string, int>();

        public Dictionary<Runtime, byte[]> EncKeys;

        public long TeamStartTimestamp; // ticks
        public long TeamLastTimestamp; // ticks
        public Dictionary<string, long> RuntimeStartTimestamps = new Dictionary<string, long>(); // runtimeid, ticks
        public Dictionary<string, long> RuntimeLastTimestamps = new Dictionary<string, long>(); // runtimeid, ticks

        public bool Debug = false;

        //creates a copy of this team object only for scoreboard purposes.
        public Team GetScoreboardShadowCopy() {
            Team team = new Team();

            team.UUID = this.UUID;
            team.Name = this.Name;

            team.ValidRuntimeIDs = new List<string>();
            foreach (string k in this.ValidRuntimeIDs) team.ValidRuntimeIDs.Add(k);

            team.RuntimeLastScores = new Dictionary<string, int>();
            foreach (string k in this.RuntimeLastScores.Keys) team.RuntimeLastScores[k] = this.RuntimeLastScores[k];

            team.TeamStartTimestamp = this.TeamStartTimestamp;
            team.TeamLastTimestamp = this.TeamLastTimestamp;

            team.RuntimeStartTimestamps = new Dictionary<string, long>();
            foreach (string k in this.RuntimeStartTimestamps.Keys) team.RuntimeStartTimestamps[k] = this.RuntimeStartTimestamps[k];

            team.RuntimeLastTimestamps = new Dictionary<string, long>();
            foreach (string k in this.RuntimeLastTimestamps.Keys) team.RuntimeLastTimestamps[k] = this.RuntimeLastTimestamps[k];

            team.Debug = this.Debug;

            return team;
        }
    }
}
