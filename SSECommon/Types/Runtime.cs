using System;
using System.Collections.Generic;
using System.Text;

using Newtonsoft.Json;

namespace SSECommon.Types {
    public class Runtime {
        public string ID;
        public RuntimeType Type;
        public List<ScoringPayloadMetadata> scoredItems;

        public string ReadmeLocation;
        public string ScoringReportLocation;
        
        public static Runtime FromJson(string json) {
            return JsonConvert.DeserializeObject<Runtime>(json);
        }
    }
}
