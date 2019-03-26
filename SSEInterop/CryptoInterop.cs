using System;
using System.Security.Cryptography;
using System.Net.Http;
using System.Collections.Generic;
using System.Text;

using Newtonsoft.Json;

using SSECommon;
using SSECommon.Types;

namespace SSEInterop {
    public static class CryptoInterop {

        private static byte[] EncKeyBlob;

        //Linux compatible implementation of the client-side portion of the key exchange.
        private static void RefreshEncryptionKey(Uri KeyExchangeEndpoint, string TeamUUID, string RuntimeID) {

            GenericEncryptedMessage resp;

            //begin key exchange
            using (ECDiffieHellmanOpenSsl exchange = new ECDiffieHellmanOpenSsl()) {
                
                //communicate public key to server
                using (HttpClient http = new HttpClient()) {
                    http.DefaultRequestHeaders.Add("DHKE-PUBLIC-KEY", exchange.PublicKey.ToByteArray().ToHex());
                    http.DefaultRequestHeaders.Add("TEAM-UUID", TeamUUID);
                    http.DefaultRequestHeaders.Add("RUNTIME-ID", RuntimeID);
                    HttpResponseMessage response = http.GetAsync(KeyExchangeEndpoint).Result;

                    //if successful split returned content into values[]
                    if (response.IsSuccessStatusCode) {
                        resp = JsonConvert.DeserializeObject<GenericEncryptedMessage>(response.Content.ReadAsStringAsync().Result);
                    } else {
                        //if failed the server is likely not online or the key material send was invalid/not parsable
                        //MessageBox.Show("HTTP key exchange failed! " + response.StatusCode);
                        Environment.Exit(0);
                        return;
                    }
                }

                //convert hex keyblob to byte[]
                ECDiffieHellmanPublicKey keyblob = JsonConvert.DeserializeObject<ECDiffieHellmanPublicKey>(resp.Tag);

                //try to read server's public keyblob into DHKE and derive the key material
                try {
                    EncKeyBlob = exchange.DeriveKeyFromHash(keyblob, HashAlgorithmName.SHA256);
                } catch (Exception e) {
                    //if this fails the public key returned by the server was either invalid or could not be parsed.
                    //MessageBox.Show("Public key invalid! \r\n\r\n" + e.Message);
                    Environment.Exit(0);
                    return;
                }

                //key exchange was likely successful, perform a sanity check.

                string sanityCheck = Cryptography.Decrypt(resp.Ciphertext, EncKeyBlob, resp.IV);

                if (sanityCheck != Constants.KEY_EXCHANGE_SANITY_CHECK) {
                    //MessageBox.Show("Key exchange sanity check failed! " + EncKeyBlob.ToHex());
                    Environment.Exit(0);
                    return;
                }
            }
            return;
        }
    }
}
