using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace SSECommon {
    public static class Extensions {
        public static string AsPath(this string path) {
            if (Environment.OSVersion.Platform == PlatformID.Win32NT) {
                return path.Replace("/", "\\").Replace("\\\\", "\\");
            } else {
                return path.Replace("\\", "/").Replace("//", "/");
            }
        }

        public static byte[] FromHexToByteArray(this string hex) {
            return Enumerable.Range(0, hex.Length)
                     .Where(x => x % 2 == 0)
                     .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
                     .ToArray();
        }

        public static string ToHex(this byte[] bytes) {
            return BitConverter.ToString(bytes).Replace("-", "");
        }
    }
}
