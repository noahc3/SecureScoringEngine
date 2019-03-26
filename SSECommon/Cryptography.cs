using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;

using SSECommon.Types;

namespace SSECommon {
    public static class Cryptography {
        
        public static byte[] Encrypt(string plaintext, byte[] key, out byte[] iv) {
            using (Aes aes = new AesCryptoServiceProvider()) {
                aes.Key = key;
                iv = aes.IV;

                using (MemoryStream ciphertext = new MemoryStream())
                using (CryptoStream cs = new CryptoStream(ciphertext, aes.CreateEncryptor(), CryptoStreamMode.Write)) {
                    byte[] plaintextMessage = Encoding.UTF8.GetBytes(plaintext);
                    cs.Write(plaintextMessage, 0, plaintextMessage.Length);
                    cs.Close();
                    return ciphertext.ToArray();
                }
            }
        }

        public static string Decrypt(byte[] ciphertext, byte[] key, byte[] iv) {
            using (Aes aes = new AesCryptoServiceProvider()) {
                aes.Key = key;
                aes.IV = iv;
                
                using (MemoryStream plaintext = new MemoryStream()) {
                    using (CryptoStream cs = new CryptoStream(plaintext, aes.CreateDecryptor(), CryptoStreamMode.Write)) {
                        cs.Write(ciphertext, 0, ciphertext.Length);
                        cs.Close();
                        return Encoding.UTF8.GetString(plaintext.ToArray());
                    }
                }
            }
        }
    }
}
