using System;
using System.Collections.Generic;
using System.Text;

using SSECommon.Enums;

using Newtonsoft.Json;

namespace SSECommon.Types
{
    public class ScoringPayloadMetadata
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int ScoreValue { get; set; }
        public ScoreType Type { get; set; }
        public bool ScoreIfBool { get; set; }
        public bool BanOnInvalid { get; set; }
        public string ClientPayload { get; set; }
        public string ServerPayload { get; set; }

        public static ScoringPayloadMetadata FromJson(string json) {
            return JsonConvert.DeserializeObject<ScoringPayloadMetadata>(json);
        }
    }
}
