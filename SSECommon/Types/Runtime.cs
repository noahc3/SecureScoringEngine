using System;
using System.Collections.Generic;
using System.Text;

using Newtonsoft.Json;

namespace SSECommon.Types {
    public class Runtime {
        public string ID;
        public RuntimeType Type;
        
        public static Runtime FromJson(string json) {
            return JsonConvert.DeserializeObject<Runtime>(json);
        }
    }
}
