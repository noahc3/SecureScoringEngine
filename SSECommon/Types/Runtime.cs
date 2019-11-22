using System;
using System.Collections.Generic;
using System.Text;

using Newtonsoft.Json;

namespace SSECommon.Types {
    public class Runtime {
        public string ID { get; set; }
        public RuntimeType Type { get; set; }
        public List<ScoringPayloadMetadata> scoredItems { get; set; }

        public string ReadmeLocation { get; set; }
        public string ScoringReportLocation { get; set; }

        public static Runtime FromJson(string json) {
            return JsonConvert.DeserializeObject<Runtime>(json);
        }
    }
}
