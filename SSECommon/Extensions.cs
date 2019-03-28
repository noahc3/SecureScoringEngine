using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Reflection;
using System.Text;
using System.Runtime.InteropServices;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Emit;

using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;

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

        public static ReturnType ExecuteAsCode<ReturnType>(this string code, string passthrough = null) {

            char lineSep = code.Split(';')[1][0];

            string[] lines = code.Split(lineSep);
            List<string> assembliesToLoad = new List<string>();
            foreach(string k in lines) {
                if (k.StartsWith("//assembly") && k.EndsWith(";")) {
                    assembliesToLoad.Add(k.Replace("//assembly", "").Replace(" ", "").Replace(";", ""));
                }
            }

            List<Assembly> assemblies = new List<Assembly>();

            foreach(string k in assembliesToLoad) {
                assemblies.Add(Assembly.Load(k));
            }
            
            if (String.IsNullOrWhiteSpace(passthrough)) {
                return (ReturnType)CSharpScript.EvaluateAsync(code, ScriptOptions.Default.WithReferences(assemblies)).Result;
            } else {
                Passthrough globals = new Passthrough { passthrough = passthrough };
                return (ReturnType) CSharpScript.EvaluateAsync(code, ScriptOptions.Default.WithReferences(assemblies), globals: globals).Result;
            }
        }
    }

    public class Passthrough
    {
        public string passthrough;
    };
}
