using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

using SSECommon;
using SSECommon.Types;

using SSEService.Security;
using SSEService.Types;

using Newtonsoft.Json;

namespace SSEService.Net {
    class ClientServerComms {

        //asks the server if the teamUuid and runtimeId combo is valid. Since the global SessionConfig may not be defined at this point, the id's are passed in as arguments.
        public static bool VerifyTeamUUID(string teamUuid, string runtimeId) {
            using (HttpClient http = new HttpClient()) {
                http.DefaultRequestHeaders.Add("TEAM-UUID", teamUuid);
                http.DefaultRequestHeaders.Add("RUNTIME-ID", runtimeId);
                HttpResponseMessage response = http.GetAsync(Globals.ENDPOINT_VERIFY_TEAM_UUID).Result;

                //if successful split returned content into values[]
                if (response.IsSuccessStatusCode) {
                    return true;
                } else {
                    return false;
                }
            }
        }
        
        public static void CiphertextPing() {

            byte[] iv;
            byte[] ciphertext = Encryption.EncryptMessage("PING!", out iv);

            GenericEncryptedMessage message = new GenericEncryptedMessage(ciphertext, iv, "", Globals.SessionConfig.TeamUUID, Globals.SessionConfig.RuntimeID);
            GenericEncryptedMessage resp;

            

            using (HttpClient http = new HttpClient()) {
                StringContent content = new StringContent(message.ToJson());
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                HttpResponseMessage response = http.PostAsync(Globals.ENDPOINT_PING_CIPHERTEXT, content).Result;

                if (response.IsSuccessStatusCode) {
                    resp = GenericEncryptedMessage.FromJson(response.Content.ReadAsStringAsync().Result);
                } else {
                    //if failed the server is likely not online or the key material send was invalid/not parsable
                    Console.WriteLine("Server sent invalid response " + response.StatusCode);
                    Environment.Exit(0);
                    return;
                }
            }

            ciphertext = resp.Ciphertext;
            iv = resp.IV;

            string pong = Encryption.DecryptMessage(ciphertext, iv);

            if (pong != "PONG!") {
                Console.WriteLine("Ping failed! Invalid response: " + pong);
                Environment.Exit(0);
                return;
            }

            Console.WriteLine(pong);

            return;
        }
        
        public static void GetReadme() {

            byte[] iv;
            byte[] ciphertext = Encryption.EncryptMessage(Constants.KEY_EXCHANGE_SANITY_CHECK, out iv);

            GenericEncryptedMessage message = new GenericEncryptedMessage(ciphertext, iv, "", Globals.SessionConfig.TeamUUID, Globals.SessionConfig.RuntimeID);
            GenericEncryptedMessage resp;

            using (HttpClient http = new HttpClient()) {
                StringContent content = new StringContent(message.ToJson());
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                HttpResponseMessage response = http.PostAsync(Globals.ENDPOINT_README, content).Result;

                if (response.IsSuccessStatusCode) {
                    resp = GenericEncryptedMessage.FromJson(response.Content.ReadAsStringAsync().Result);
                } else {
                    //if failed the server is likely not online or the key material send was invalid/not parsable
                    Console.WriteLine("Server sent invalid response " + response.StatusCode);
                    Environment.Exit(0);
                    return;
                }
            }

            ciphertext = resp.Ciphertext;
            iv = resp.IV;

            string plaintext = Encryption.DecryptMessage(ciphertext, iv);

            FileTransferWrapper ftw = JsonConvert.DeserializeObject<FileTransferWrapper>(plaintext);

            File.WriteAllBytes(ftw.Path, ftw.Blob);

            return;
        }
        
    }
}
