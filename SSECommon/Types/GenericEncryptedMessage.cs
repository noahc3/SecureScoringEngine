using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace SSECommon.Types {
    public class GenericEncryptedMessage {
        public byte[] Ciphertext;
        public byte[] IV;
        public string Tag;
        public string TeamUUID;
        public string RuntimeID;

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
