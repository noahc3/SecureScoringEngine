using System;
using System.Collections.Generic;
using System.Text;

namespace SSECommon.Types
{
    public class ScoringReport
    {
        public List<ClientScoreMetadata> rewards = new List<ClientScoreMetadata>();
        public List<ClientScoreMetadata> penalties = new List<ClientScoreMetadata>();
        public int rewardsFound;
        public int totalRewards;

        public int penaltiesGained;

        public int score;
        public int totalScore;
    }
}
