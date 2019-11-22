using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace SSECommon.Types {
    public class GenericEncryptedMessage {
        public byte[] Ciphertext { get; set; }
        public byte[] IV { get; set; }
        public string Tag { get; set; }
        public string TeamUUID { get; set; }
        public string RuntimeID { get; set; }

        public GenericEncryptedMessage() {

        }

        public GenericEncryptedMessage(byte[] ciphertext, byte[] iv, string tag, string teamUuid, string runtimeId) {
            Ciphertext = ciphertext;
            IV = iv;
            Tag = tag;
            TeamUUID = teamUuid;
            RuntimeID = runtimeId;
        }

        public string ToJson() {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }

        public static GenericEncryptedMessage FromJson(string json) {
            return JsonConvert.DeserializeObject<GenericEncryptedMessage>(json);
        }
    }
}
