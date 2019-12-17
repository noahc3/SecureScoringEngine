using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using System.Reflection;
using SSECommon;
using Newtonsoft.Json;
using System.Diagnostics;

namespace SSEConfigurationTool.Data {
    public class Utilities {
        public static bool MatchesLogicalDistros(params LogicalDistro[] distros) {
            return distros.Contains(Globals.distro);
        }

        public static string GetEmbeddedTextFile(string filename) {
            Assembly assembly = Assembly.GetExecutingAssembly();
            string path = "SSEConfigurationTool.files." + filename.Replace("//", "/").Replace("/", ".").Trim();
            using (StreamReader reader = new StreamReader(assembly.GetManifestResourceStream(path))) {
                return reader.ReadToEnd();
            }
        }

        public static void SaveDraftJson(string path) {
            FileInfo i = new FileInfo(path);
            if (!Directory.Exists(i.DirectoryName)) Directory.CreateDirectory(i.DirectoryName);
            Draft draft = new Draft() { Platform = Globals.TargetPlatform, ConfiguredScoringItems = Globals.ConfiguredScoringItems };
            string json = JsonConvert.SerializeObject(draft, Formatting.Indented);
            File.WriteAllText(path, json);
            new Process { StartInfo = new ProcessStartInfo(i.FullName) { UseShellExecute = true } }.Start();
        }

        public static bool LoadDraftJson(string path) {
            if (!File.Exists(path)) return false;
            string json = File.ReadAllText(path);
            Draft draft = JsonConvert.DeserializeObject<Draft>(json);
            Globals.TargetPlatform = draft.Platform;
            Globals.ConfiguredScoringItems = draft.ConfiguredScoringItems;
            return true;
        }

        public static void ExportConfiguration(string path) {

            for (int i = 0; i < Globals.ConfiguredScoringItems.Count(); i++) {
                string dir = (path + "/" + (i+1).ToString("000") + " " + Globals.ConfiguredScoringItems[i].DescriptiveName).AsPath();
                if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);

                CompiledScoringItem item = ScoringItemUtilities.CompileScoringItem(Globals.ConfiguredScoringItems[i]);
                File.WriteAllText((dir + "/client.csx").AsPath(), item.ClientPayload);
                File.WriteAllText((dir + "/server.csx").AsPath(), item.ServerPayload);
                File.WriteAllText((dir + "/metadata.json").AsPath(), item.Metadata);
            }
        }
    }
}
