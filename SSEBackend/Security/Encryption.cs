using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using SSECommon;
using SSECommon.Types;
using SSEBackend.Types;

namespace SSEBackend.Security
{
    public class Encryption
    {
        public static byte[] EncryptMessage(string plaintext, out byte[] iv, string teamUuid, string runtimeId) {
            byte[] key = GetRuntimeEncKey(teamUuid, runtimeId);
            return Cryptography.Encrypt(plaintext, key, out iv);
        }

        public static string DecryptMessage(byte[] ciphertext, byte[] iv, string teamUuid, string runtimeId) {
            byte[] key = GetRuntimeEncKey(teamUuid, runtimeId);
            return Cryptography.Decrypt(ciphertext, key, iv);
        }

        private static byte[] GetRuntimeEncKey(string teamUuid, string runtimeId) {
            Runtime runtime = Globals.GetRuntime(teamUuid, runtimeId);
            Team team = Globals.GetTeam(teamUuid);

            return team.EncKeys[runtime];
        }

        public static void SetRuntimeKeySlot(string teamUuid, string runtimeId, byte[] key) {
            Runtime runtime = Globals.GetRuntime(teamUuid, runtimeId);
            Team team = Globals.GetTeam(teamUuid);

            team.EncKeys[runtime] = key;
        }

        public static void ClearRuntimeKeySlot(string teamUuid, string runtimeId) {
            Runtime runtime = Globals.GetRuntime(teamUuid, runtimeId);
            Team team = Globals.GetTeam(teamUuid);

            team.EncKeys[runtime] = null;
        }

    }
}
