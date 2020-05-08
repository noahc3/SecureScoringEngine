using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using SSECommon.Types;
using SSECommon;
using Newtonsoft.Json;

namespace SSEConfigurationTool.Data {
    static class ServerComms {
        public static bool CanReachScoringServer() {
            using (HttpClient http = new HttpClient()) {
                try {
                    HttpResponseMessage response = http.GetAsync(Globals.ENDPOINT_PING_PLAINTEXT).Result;
                    if (response.IsSuccessStatusCode && response.Content.ReadAsStringAsync().Result.Contains("PONG!")) {
                        return true;
                    } else {
                        return false;
                    }
                } catch (Exception) {
                    return false;
                }
            }
        }
        public static bool CheckDebugSvcStatus() {
            try {
                byte[] iv;
                byte[] ciphertext = Encryption.EncryptMessage(Constants.KEY_EXCHANGE_SANITY_CHECK, out iv);

                GenericEncryptedMessage message = new GenericEncryptedMessage(ciphertext, iv, "", Globals.DebugTeamUUID, Globals.CONFIG_TOOL_RUNTIME_ID);
                GenericEncryptedMessage resp;

                using (HttpClient http = new HttpClient()) {
                    StringContent content = new StringContent(message.ToJson());
                    content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                    HttpResponseMessage response = http.PostAsync(Globals.ENDPOINT_DEBUG_CHECK_SVC_STATUS, content).Result;

                    if (response.IsSuccessStatusCode) {
                        resp = GenericEncryptedMessage.FromJson(response.Content.ReadAsStringAsync().Result);
                    } else {
                        //if failed the server is likely not online or the key material send was invalid/not parsable
                        Console.WriteLine("Server sent invalid response " + response.StatusCode);
                        return false;
                    }
                }

                ciphertext = resp.Ciphertext;
                iv = resp.IV;

                string status = Encryption.DecryptMessage(ciphertext, iv);

                return status == "true";
            } catch (Exception) {
                return false;
            }
        }
        public static bool AddRuntime(string runtimeId, byte[] zip) {
            try {
                FileTransferWrapper wrapper = new FileTransferWrapper { Path = runtimeId, Blob = zip };

                byte[] iv;
                byte[] ciphertext = Encryption.EncryptMessage(JsonConvert.SerializeObject(wrapper), out iv);

                GenericEncryptedMessage message = new GenericEncryptedMessage(ciphertext, iv, "", Globals.DebugTeamUUID, Globals.CONFIG_TOOL_RUNTIME_ID);

                using (HttpClient http = new HttpClient()) {
                    StringContent content = new StringContent(message.ToJson());
                    content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                    HttpResponseMessage response = http.PostAsync(Globals.ENDPOINT_DEBUG_ADD_RUNTIME, content).Result;

                    if (response.IsSuccessStatusCode) {
                        return true;
                    } else {
                        //if failed the server is likely not online or the key material send was invalid/not parsable
                        Console.WriteLine("Server sent invalid response " + response.StatusCode);
                        return false;
                    }
                }
            } catch (Exception e) {
                Console.WriteLine("Failed to add runtime to scoring server, caught exception " + e.Message + "\n" + e.StackTrace);
                return false;
            }
        }

        public static bool HotReload() {
            try {

                GenericEncryptedMessage message = new GenericEncryptedMessage(null, null, "", Globals.DebugTeamUUID, Globals.CONFIG_TOOL_RUNTIME_ID);

                using (HttpClient http = new HttpClient()) {
                    StringContent content = new StringContent(message.ToJson());
                    content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                    HttpResponseMessage response = http.PostAsync(Globals.ENDPOINT_DEBUG_HOT_RELOAD, content).Result;

                    if (response.IsSuccessStatusCode) {
                        return true;
                    } else {
                        //if failed the server is likely not online
                        Console.WriteLine("Server sent invalid response " + response.StatusCode);
                        return false;
                    }
                }
            } catch (Exception e) {
                Console.WriteLine("Failed to hotreload scoring server, caught exception " + e.Message + "\n" + e.StackTrace);
                return false;
            }
        }
    }
}
