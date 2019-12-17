using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using SSECommon.Types;
using SSECommon.Enums;
using Newtonsoft.Json;

namespace SSEConfigurationTool.Data {
    public static class ScoringItemUtilities {
        public static string[] GetPlatforms() {
            return Globals.ScoringItems.Keys.ToArray();
        }

        public static string[] GetScoringCategories(string platform) {
            return Globals.ScoringItems[platform].Keys.ToArray();
        }

        public static TemplateScoringItem[] GetScoringItems(string platform, string category) {
            return Globals.ScoringItems[platform][category].Values.ToArray();
        }

        public static int CountScoringCategories(string platform) {
            return GetScoringCategories(platform).Count();
        }

        public static int CountScoringItems(string platform) {
            int sum = 0;
            foreach (string k in GetScoringCategories(platform)) sum += GetScoringItems(platform, k).Count();
            return sum;
        }

        public static List<ScoringVariable> ParseVariables(string platform, string category, string name, bool server = false) {
            List<ScoringVariable> variables = new List<ScoringVariable>();

            string payload;

            if (server) payload = GetServerPayload(platform, category, name);
            else payload = GetClientPayload(platform, category, name);

            using (StringReader sr = new StringReader(payload)) {
                string line;
                while ((line = sr.ReadLine()) != null) {
                    if (line.Contains("//$CONFIG")) {
                        string[] split = line.Substring(0, line.IndexOf("//$CONFIG")).Split(' ');
                        string type = split[0].Trim('"', ' ', ';');
                        string var = split[1].Trim('"', ' ', ';');
                        string value = "";
                        if (split.Count() > 3 && type != "List<string>") {
                            value = String.Join(' ', split.Skip(3));
                            value = value.Trim('"', ' ', ';');
                        }

                        split = line.Split('|');
                        string prettyName = split[1];
                        string helpText = split[2];

                        ScoringVariable v = new ScoringVariable() { Name = var, Type = type, PrettyName = prettyName, HelpText = helpText, Value = value };
                        variables.Add(v);
                    }
                }
            }
            return variables;
        }

        public static string PopulatePayloadTemplate(string payload, List<ScoringVariable> scoringVariables) {
            List<string> finalPayload = new List<string>();
            using (StringReader sr = new StringReader(payload)) {
                string line;
                while ((line = sr.ReadLine()) != null) {
                    if (line.Contains("//$INTELLISENSE")) continue;
                    else if (line.Contains("//$CONFIG")) {
                        string[] split = line.Substring(0, line.IndexOf("//$CONFIG")).Split(' ');
                        string type = split[0].Trim('"', ' ', ';');
                        string var = split[1].Trim('"', ' ', ';');
                        Console.WriteLine(var + " | " + type);
                        ScoringVariable v = scoringVariables.Where((x) => x.Name == var && x.Type == type).First();
                        finalPayload.Add(v.GenerateCode());
                    } else {
                        finalPayload.Add(line);
                    }
                }
            }

            return String.Join('\n', finalPayload);
        }

        public static CompiledScoringItem CompileScoringItem(ConfiguredScoringItem item) {
            string clientPayload = GetClientPayload(item.Platform, item.Category, item.Name);
            string serverPayload = GetServerPayload(item.Platform, item.Category, item.Name);

            string finalClientPayload = PopulatePayloadTemplate(clientPayload, item.ClientScoringVariables);
            string finalServerPayload = PopulatePayloadTemplate(serverPayload, item.ServerScoringVariables);

            ScoringPayloadMetadata meta = JsonConvert.DeserializeObject<ScoringPayloadMetadata>(GetMetadata(item.Platform, item.Category, item.Name));
            meta.Name = item.ScoringReportName;
            meta.Description = item.DescriptiveName;
            meta.ScoreValue = item.ScoreAmount;

            if (item.ScoreType == "reward") meta.Type = ScoreType.Reward;
            else if (item.ScoreType == "penalty") meta.Type = ScoreType.Penalty;
            else meta.Type = ScoreType.DoNotScore;

            meta.BanOnInvalid = item.BanOnInvalid;
            meta.ScoreIfBool = true;

            CompiledScoringItem c = new CompiledScoringItem() {
                ClientPayload = finalClientPayload,
                ServerPayload = finalServerPayload,
                Metadata = JsonConvert.SerializeObject(meta, Formatting.Indented)
            };

            return c;

        }

        public static string GetScoringItemRootFolder(string platform, string category, string name) {
            return "scoring/" + platform.Replace(" ", "_") + "/" + category.Replace(" ", "_") + "/" + name.Replace(" ", "_");
        }

        public static string GetClientPayload(string platform, string category, string name) {
            return Utilities.GetEmbeddedTextFile(GetScoringItemRootFolder(platform, category, name) + "/client.csx");
        }

        public static string GetServerPayload(string platform, string category, string name) {
            return Utilities.GetEmbeddedTextFile(GetScoringItemRootFolder(platform, category, name) + "/server.csx");
        }

        public static string GetMetadata(string platform, string category, string name) {
            return Utilities.GetEmbeddedTextFile(GetScoringItemRootFolder(platform, category, name) + "/metadata.json");
        }
    }
}
