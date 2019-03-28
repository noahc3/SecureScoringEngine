using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SSEBackend.Types
{
    public class ScoringProgressTracker
    {
        //used to track in-progress scoring by the client
        public int scoringProgress;
        public List<bool> scored;

        public List<bool> safeScored;
        public bool safeToGenerateReport;
    }
}
