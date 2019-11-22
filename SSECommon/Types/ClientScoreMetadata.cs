using System;
using System.Collections.Generic;
using System.Text;

using SSECommon.Enums;

namespace SSECommon.Types
{
    public class ClientScoreMetadata
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int ScoreValue { get; set; }
        public ScoreType Type { get; set; }
    }
}
