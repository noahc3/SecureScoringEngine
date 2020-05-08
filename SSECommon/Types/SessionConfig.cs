using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using Newtonsoft.Json;

namespace SSECommon.Types {
    public class SessionConfig {
        public string TeamUUID;
        public string RuntimeID;
        public string Backend;


        public void Flush(string path) {
            File.WriteAllText(path, this.ToJson());
        }


        public string ToJson() {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }

        public static SessionConfig FromJson(string json) {
            return JsonConvert.DeserializeObject<SessionConfig>(json);
        }
    }
}
