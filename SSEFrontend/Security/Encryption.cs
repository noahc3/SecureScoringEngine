using Newtonsoft.Json;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net.Http;
using System.Security.Cryptography;
using System.Windows.Forms;
using SSECommon;
using SSECommon.Types;

namespace SSEFrontend.Security {
    static class Encryption {
        
        private static byte[] EncKeyBlob;

        private static void RefreshEncryptionKey() {

            GenericEncryptedMessage resp;

            //begin key exchange
            using (ECDiffieHellmanCng exchange = new ECDiffieHellmanCng()) {
                exchange.KeyDerivationFunction = ECDiffieHellmanKeyDerivationFunction.Hash;
                exchange.HashAlgorithm = CngAlgorithm.Sha256;

                //communicate public key to server
                using (HttpClient http = new HttpClient()) {
                    http.DefaultRequestHeaders.Add("DHKE-PUBLIC-KEY", exchange.PublicKey.ToByteArray().ToHex());
                    http.DefaultRequestHeaders.Add("TEAM-UUID", Globals.SessionConfig.TeamUUID);
                    http.DefaultRequestHeaders.Add("RUNTIME-ID", Globals.SessionConfig.RuntimeID);
                    HttpResponseMessage response = http.GetAsync(Globals.ENDPOINT_KEY_EXCHANGE).Result;

                    //if successful split returned content into values[]
                    if (response.IsSuccessStatusCode) {
                        resp = JsonConvert.DeserializeObject<GenericEncryptedMessage>(response.Content.ReadAsStringAsync().Result); 
                    } else {
                        //if failed the server is likely not online or the key material send was invalid/not parsable
                        MessageBox.Show("HTTP key exchange failed! " + response.StatusCode);
                        Environment.Exit(0);
                        return;
                    }
                }

                //convert hex keyblob to byte[]
                byte[] keyblob = resp.Tag.FromHexToByteArray();

                //try to read server's public keyblob into DHKE and derive the key material
                try {
                    EncKeyBlob = exchange.DeriveKeyMaterial(CngKey.Import(keyblob, CngKeyBlobFormat.EccPublicBlob));
                } catch (Exception e) {
                    //if this fails the public key returned by the server was either invalid or could not be parsed.
                    MessageBox.Show("Public key invalid! \r\n\r\n" + e.Message);
                    Environment.Exit(0);
                    return;
                }

                //key exchange was likely successful, perform a sanity check.

                string sanityCheck = Cryptography.Decrypt(resp.Ciphertext, EncKeyBlob, resp.IV);

                if (sanityCheck != Constants.KEY_EXCHANGE_SANITY_CHECK) {
                    MessageBox.Show("Key exchange sanity check failed! " + EncKeyBlob.ToHex());
                    Environment.Exit(0);
                    return;
                }




            }

            return;
        }

        //in-between method to keep EncKeyblob and RefreshEncryptionKey private to prevent mitming external methods and generating valid comms keys
        //methods outside of this class should NEVER be able to generate valid comms keys or retrieve EncKeyBlob directly!
        public static byte[] EncryptMessage(string Plaintext, out byte[] IV) {
            RefreshEncryptionKey();
            return Cryptography.Encrypt(Plaintext, EncKeyBlob, out IV);
        }

        public static string DecryptMessage(byte[] Ciphertext, byte[] IV) {
            return Cryptography.Decrypt(Ciphertext, EncKeyBlob, IV);
        }
    }
}
