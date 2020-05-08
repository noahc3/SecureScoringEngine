using System;
using System.Collections.Generic;
using System.Text;

namespace SSECommon.Types
{
    public class ScoringReport
    {
        public List<ClientScoreMetadata> rewards { get; set; } = new List<ClientScoreMetadata>();
        public List<ClientScoreMetadata> penalties { get; set; } = new List<ClientScoreMetadata>();
        public int rewardsFound { get; set; }
        public int totalRewards { get; set; }

        public int penaltiesGained { get; set; }

        public int score { get; set; }
        public int totalScore { get; set; }
        public int lastTotalScore { get; set; }

        public long runtimeStartTimestamp { get; set; }
        public long teamStartTimestamp { get; set; }
    }
}
