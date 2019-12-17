using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using Newtonsoft.Json;

namespace SSEService.Types {
    class SessionConfig {
        public string TeamUUID;
        public string RuntimeID;
        public string Backend;


        public void Flush() {
            File.WriteAllText(Globals.CONFIG_SESSION, this.ToJson());
        }


        public string ToJson() {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }

        public static SessionConfig FromJson(string json) {
            return JsonConvert.DeserializeObject<SessionConfig>(json);
        }
    }
}
