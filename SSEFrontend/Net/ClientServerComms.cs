using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using SSECommon;
using SSECommon.Types;
using SSEFrontend.Security;

namespace SSEFrontend.Net {
    class ClientServerComms {

        public static string CiphertextPing() {

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
                    MessageBox.Show("Server sent invalid response " + response.StatusCode);
                    Environment.Exit(0);
                    return "";
                }
            }

            ciphertext = resp.Ciphertext;
            iv = resp.IV;

            string pong = Encryption.DecryptMessage(ciphertext, iv);

            if (pong != "PONG!") {
                MessageBox.Show("Ping failed! Invalid response: " + pong);
                Environment.Exit(0);
                return "";
            }

            return "";
        }

        public static byte[] GetReadme() {

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
                    MessageBox.Show("Server sent invalid response " + response.StatusCode);
                    Environment.Exit(0);
                    return null;
                }
            }

            ciphertext = resp.Ciphertext;
            iv = resp.IV;

            byte[] readme = Encryption.DecryptMessage(ciphertext, iv).FromHexToByteArray();

            return readme;
        }
    }
}
