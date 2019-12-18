using System;
using System.Collections.Generic;
using System.Net.Http;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;

using System.Text;

using SSECommon;
using SSECommon.Types;

using Newtonsoft.Json;



namespace SSEService.Security {
    static class Encryption {

        static byte[] EncKeyBlob;

        //This method supports both Linux (OpenSSL) and Windows (CNG)
        private static void RefreshEncryptionKey() {
            
            GenericEncryptedMessage resp;

            //WINDOWS implementation using System.Security.Cryptography.Cng via the Windows Compatibility Pack
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows)) {
                using (ECDiffieHellmanCng exchange = new ECDiffieHellmanCng()) {
                    exchange.KeyDerivationFunction = ECDiffieHellmanKeyDerivationFunction.Hash;
                    exchange.HashAlgorithm = CngAlgorithm.Sha256;

                    //communicate public key to server
                    using (HttpClient http = new HttpClient()) {
                        http.DefaultRequestHeaders.Add("DHKE-PUBLIC-KEY", exchange.ExportSubjectPublicKeyInfo().ToHex());
                        http.DefaultRequestHeaders.Add("TEAM-UUID", Globals.SessionConfig.TeamUUID);
                        http.DefaultRequestHeaders.Add("RUNTIME-ID", Globals.SessionConfig.RuntimeID);
                        http.DefaultRequestHeaders.Add("CRYPTO-API", Constants.CRYPTO_API_WINDOWS);
                        HttpResponseMessage response = http.GetAsync(Globals.ENDPOINT_KEY_EXCHANGE).Result;

                        //if successful split returned content into values[]
                        if (response.IsSuccessStatusCode) {
                            resp = JsonConvert.DeserializeObject<GenericEncryptedMessage>(response.Content.ReadAsStringAsync().Result);
                        } else {
                            //if failed the server is likely not online or the key material send was invalid/not parsable
                            Console.WriteLine("HTTP key exchange failed! " + response.StatusCode);
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
                        Console.WriteLine("Public key invalid! \r\n\r\n" + e.Message);
                        Environment.Exit(0);
                        return;
                    }

                    //key exchange was likely successful, perform a sanity check.

                    string sanityCheck = Cryptography.Decrypt(resp.Ciphertext, EncKeyBlob, resp.IV);

                    if (sanityCheck != Constants.KEY_EXCHANGE_SANITY_CHECK) {
#if (DEBUG)
                        Console.WriteLine("Key exchange sanity check failed! " + EncKeyBlob.ToHex());
#endif
                        Environment.Exit(0);
                        return;
                    }
                }
            }
            //LINUX implementation using System.Security.Cryptography.OpenSsl
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux)) {
                //begin key exchange
                using (ECDiffieHellmanOpenSsl exchange = new ECDiffieHellmanOpenSsl()) {
                    
                    //communicate public key to server
                    using (HttpClient http = new HttpClient()) {
                        byte[] publicKeyBlob = exchange.ExportSubjectPublicKeyInfo();

                        http.DefaultRequestHeaders.Add("DHKE-PUBLIC-KEY", publicKeyBlob.ToHex());
                        http.DefaultRequestHeaders.Add("TEAM-UUID", Globals.SessionConfig.TeamUUID);
                        http.DefaultRequestHeaders.Add("RUNTIME-ID", Globals.SessionConfig.RuntimeID);
                        http.DefaultRequestHeaders.Add("CRYPTO-API", Constants.CRYPTO_API_UNIX);
                        HttpResponseMessage response = http.GetAsync(Globals.ENDPOINT_KEY_EXCHANGE).Result;

                        //if successful split returned content into values[]
                        if (response.IsSuccessStatusCode) {
                            resp = JsonConvert.DeserializeObject<GenericEncryptedMessage>(response.Content.ReadAsStringAsync().Result);
                        } else {
                            //if failed the server is likely not online or the key material send was invalid/not parsable
                            Console.WriteLine("HTTP key exchange failed! " + response.StatusCode);
                            Environment.Exit(0);
                            return;
                        }
                    }

                    //convert hex keyblob to byte[]
                    int read;
                    byte[] keyblob = resp.Tag.FromHexToByteArray();
                    ECDiffieHellman ecdh = ECDiffieHellman.Create();
                    ecdh.ImportSubjectPublicKeyInfo(keyblob, out read);


                    //try to read server's public keyblob into DHKE and derive the key material
                    try {
                        EncKeyBlob = exchange.DeriveKeyFromHash(ecdh.PublicKey, HashAlgorithmName.SHA256);
                    } catch (Exception e) {
                        //if this fails the public key returned by the server was either invalid or could not be parsed.
                        Console.WriteLine("Public key invalid! \r\n\r\n" + e.Message);
                        Environment.Exit(0);
                        return;
                    }

                    //key exchange was likely successful, perform a sanity check.

                    string sanityCheck = Cryptography.Decrypt(resp.Ciphertext, EncKeyBlob, resp.IV);

                    if (sanityCheck != Constants.KEY_EXCHANGE_SANITY_CHECK) {
#if (DEBUG)
                        Console.WriteLine("Key exchange sanity check failed! " + EncKeyBlob.ToHex());
#endif
                        Environment.Exit(0);
                        return;
                    }
                }
            }
#if (DEBUG)
            Console.WriteLine("Key Exchange Success! " + EncKeyBlob.ToHex());
#endif
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
