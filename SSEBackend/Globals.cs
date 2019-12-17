using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;

using SSEBackend.Types;
using SSECommon;
using SSECommon.Types;

using Newtonsoft.Json;

using ICSharpCode.SharpZipLib.Core;
using ICSharpCode.SharpZipLib.Zip;

namespace SSEBackend
{
    public static class Globals
    {

        public static string CONFIG_DIRECTORY = (AppContext.BaseDirectory + "\\config").AsPath();
        public static string RUNTIME_CONFIG_DIRECTORY = (CONFIG_DIRECTORY + "\\runtimes\\").AsPath();

        public static Config config; //general data for configuring the server software itself

        public static Data data; //dynamic data related to the current competition

        public static Team[] scoreboard = new Team[0]; //list of teams sorted by score/time for use with the scoreboard.

        public static void LoadData() {
            config = JsonConvert.DeserializeObject<Config>(File.ReadAllText((CONFIG_DIRECTORY + "\\config.json")).AsPath());

            data = new Data();

            List<Team> _teams;

            //checks if saved teams data exists (with scores and stuff) and loads it, otherwise loads template teams file.
            if (File.Exists((CONFIG_DIRECTORY + "\\teams_saved.json").AsPath())) {
                _teams = JsonConvert.DeserializeObject<List<Team>>(File.ReadAllText((CONFIG_DIRECTORY + "\\teams_saved.json").AsPath()));

            } else {
                _teams = JsonConvert.DeserializeObject<List<Team>>(File.ReadAllText((CONFIG_DIRECTORY + "\\teams.json")).AsPath());
            }

            foreach (Team t in _teams) {
                t.EncKeys = new Dictionary<Runtime, byte[]>();
                data.teams[t.UUID] = t;
            }

            //load all of the runtimes
            foreach (string runtimeDirectory in Directory.EnumerateDirectories(RUNTIME_CONFIG_DIRECTORY)) {
                string runtimeJson = File.ReadAllText((runtimeDirectory + "\\runtime.conf").AsPath());
                Runtime runtime = Runtime.FromJson(runtimeJson);

                runtime.scoredItems = new List<ScoringPayloadMetadata>();

                if (!config.OfflineMode) {
                    //if the server is running in online mode, the scoring data will be in a subdirectory.
                    //load all of the scored item payloads into the runtime object
                    foreach (string p in Directory.EnumerateDirectories((runtimeDirectory + "\\scoring\\").AsPath())) {
                        ScoringPayloadMetadata meta = ScoringPayloadMetadata.FromJson(File.ReadAllText((p + "\\metadata.json").AsPath()));
                        meta.ClientPayload = File.ReadAllText((p + "\\client.csx").AsPath());
                        meta.ServerPayload = File.ReadAllText((p + "\\server.csx").AsPath());
                        runtime.scoredItems.Add(meta);
                    }
                } else {
                    //if the server is running in offline mode, runtime scoring data will be located in encrypted zip files.
                    //the zip file is encrypted with a STATIC password which is defined in SSECommon constants. the encryption is
                    //to prevent accidentally viewing the scoring configuration data (ex. running a grep on the whole fs), NOT for
                    //security! If you want security, host an online scoring server!
                    Dictionary<string, Dictionary<string, byte[]>> zipEntries = new Dictionary<string, Dictionary<string, byte[]>>(); //<runtimeid, <filename, file bytes>>

                    using (Stream fsInput = File.OpenRead((runtimeDirectory + "\\scoring.zip").AsPath()))
                    using (ZipFile zf = new ZipFile(fsInput)) {
                        zf.Password = Constants.OFFLINE_SCORING_CONFIGURATION_PASSWORD;

                        foreach (ZipEntry zipEntry in zf) {
                            if (!zipEntry.IsFile) continue;

                            string fileName = zipEntry.Name.Replace("\\", "/");
                            string[] _split = fileName.Split('/');
                            string directoryName = String.Join('/', _split.Take(_split.Length - 1));
                            fileName = _split.Last();

                            byte[] file = new byte[zipEntry.Size];

                            using (Stream fileStream = zf.GetInputStream(zipEntry)) {
                                StreamUtils.ReadFully(fileStream, file);
                            }

                            if (!zipEntries.ContainsKey(directoryName)) zipEntries[directoryName] = new Dictionary<string, byte[]>();
                            zipEntries[directoryName][fileName] = file;
                        }



                        foreach (string dir in zipEntries.Keys) {
                            if (zipEntries[dir].ContainsKey("client.csx") && zipEntries[dir].ContainsKey("server.csx") && zipEntries[dir].ContainsKey("metadata.json")) {
                                ScoringPayloadMetadata meta = ScoringPayloadMetadata.FromJson(System.Text.Encoding.Default.GetString(zipEntries[dir]["metadata.json"]));
                                meta.ClientPayload = System.Text.Encoding.Default.GetString(zipEntries[dir]["client.csx"]);
                                meta.ServerPayload = System.Text.Encoding.Default.GetString(zipEntries[dir]["server.csx"]);
                                runtime.scoredItems.Add(meta);
                            }
                        }

                        foreach(ScoringPayloadMetadata k in runtime.scoredItems) {
                            Console.WriteLine(k.ClientPayload);
                        }
                    }
                }
                data.runtimes[runtime.ID] = runtime;
            }
        }

        public static void SaveData() {
            List<Team> teams = new List<Team>();
            foreach(Team k in data.teams.Values) {
                teams.Add(k.GetScoreboardShadowCopy());
            }
            File.WriteAllText((CONFIG_DIRECTORY + "\\teams_saved.json").AsPath(), JsonConvert.SerializeObject(teams, Formatting.Indented));
        }

        public static bool VerifyTeamExists(string teamUuid) {
            if (data.teams.ContainsKey(teamUuid)) {
                return true;
            } else {
                return false;
            }
        }

        public static bool VerifyTeamAuthenticity(string teamUuid, string runtimeId) {
            Team team;

            //if there is no team with the correct team id, reject the client
            if (data.teams.ContainsKey(teamUuid)) {
                team = data.teams[teamUuid];
            } else {
                return false;
            }

            //if the client is using a debug team id but the server does not have debug mode enable, reject the client
            if (team.Debug && !config.DebugSvcs) {
                return false;
            }

            //if the organizer forgets to turn off debug mode and did not disable this check, disable debug mode when non-debug clients try to connect.
            if (!team.Debug && config.DebugSvcs) {
                if (!config.GiveUpSecurityAndDontBotherDisablingDebugModeWhenNormalClientsConnect) {
                    config.DebugSvcs = false;
                }
            }

            bool validRuntimeId = false;

            string sanitizedRuntimeId = null;

            foreach (string k in team.ValidRuntimeIDs) {
                if (runtimeId.Contains(k)) {
                    sanitizedRuntimeId = k;
                    validRuntimeId = true;
                    break;
                }
            }

            //if this team is not registered for the runtime they are using, reject the client
            if (!validRuntimeId)
                return false;

            //accept the client, set start timestamps if necessary

            if (team.TeamStartTimestamp == default) {
                team.TeamStartTimestamp = DateTime.UtcNow.Ticks;
                team.RuntimeStartTimestamps[sanitizedRuntimeId] = DateTime.UtcNow.Ticks;
            } else if (!team.RuntimeStartTimestamps.ContainsKey(runtimeId)) {
                team.RuntimeStartTimestamps[runtimeId] = DateTime.UtcNow.Ticks;
            }

            return true;
        }

        public static bool VerifyRuntimeHasValidCommsKey(string teamUuid, string runtimeId) {
            Runtime runtime = GetRuntime(teamUuid, runtimeId);
            Team team = GetTeam(teamUuid);

            return team.EncKeys[runtime] != null;
        }

        public static bool IsTeamDebug(string teamUuid) {
            if (!VerifyTeamExists(teamUuid)) return false;
            Team t = GetTeam(teamUuid);
            return t.Debug;
        }

        public static Runtime GetRuntime(string teamUuid, string runtimeId) {

            //TODO: since runtimes can share id's, verify which one is intended using teamUuid
            Runtime runtime = null;
            foreach (string k in data.runtimes.Keys) {
                if (runtimeId.Contains(k)) {
                    runtime = data.runtimes[k];
                    break;
                }
            }
            return runtime;
        }

        public static Team GetTeam(string teamUuid) {
            return data.teams[teamUuid];
        }

        public static string GetRuntimeConfigDirectory(Runtime runtime) {
            return (RUNTIME_CONFIG_DIRECTORY + "\\" + runtime.ID + "\\").AsPath();
        }

        public static FileTransferWrapper GetReadme(string teamUuid, string runtimeId) {
            Runtime runtime = GetRuntime(teamUuid, runtimeId);
            string confdir = GetRuntimeConfigDirectory(runtime);
            FileTransferWrapper ftw = new FileTransferWrapper();

            ftw.Blob = File.ReadAllBytes((confdir + "\\readme.bin").AsPath());
            ftw.Path = runtime.ReadmeLocation;

            return ftw;
        }

        public static FileTransferWrapper GetScoringReportTemplate(string teamUuid, string runtimeId) {
            Runtime runtime = GetRuntime(teamUuid, runtimeId);
            string confdir = GetRuntimeConfigDirectory(runtime);
            FileTransferWrapper ftw = new FileTransferWrapper();
            
            ftw.Blob = File.ReadAllBytes((confdir + "\\scoringreport.bin").AsPath());
            ftw.Path = runtime.ScoringReportLocation;

            return ftw;
        }

        public static void StartPassiveTasks() {
            Task.Factory.StartNew(new Action(() => {
                while (true) {
                    Thread.Sleep(10000);
                    List<Team> teams = new List<Team>();

                    foreach (Team k in Globals.data.teams.Values) {
                        if (k.TeamLastTimestamp != default) {
                            teams.Add(k.GetScoreboardShadowCopy());
                        }
                    }

                    teams.OrderBy(c => c.RuntimeLastScores.Values.Sum()).ThenBy(c => c.TeamLastTimestamp - c.TeamStartTimestamp);

                    Globals.scoreboard = teams.ToArray();

                    SaveData();
                }
            }));
        }
    }
}
