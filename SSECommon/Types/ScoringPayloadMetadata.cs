using System;
using System.Collections.Generic;
using System.Text;

using SSECommon.Enums;

using Newtonsoft.Json;

namespace SSECommon.Types
{
    public class ScoringPayloadMetadata
    {
        public string Name;
        public string Description;
        public int ScoreValue;
        public ScoreType Type;
        public bool ScoreIfBool;
        public bool BanOnInvalid;
        public string ClientPayload;
        public string ServerPayload;

        public static ScoringPayloadMetadata FromJson(string json) {
            return JsonConvert.DeserializeObject<ScoringPayloadMetadata>(json);
        }
    }
}
