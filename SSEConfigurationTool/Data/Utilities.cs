using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using System.Reflection;
using SSECommon;
using SSECommon.Enums;
using SSECommon.Types;
using Newtonsoft.Json;
using System.Diagnostics;
using ICSharpCode.SharpZipLib.Core;
using ICSharpCode.SharpZipLib.Zip;
namespace SSEConfigurationTool.Data {
    public class Utilities {
        public static bool MatchesLogicalDistros(params LogicalDistro[] distros) {
            return distros.Contains(Globals.distro);
        }

        public static Stream GetEmbeddedFileAsStream(string filename) {
            Assembly assembly = Assembly.GetExecutingAssembly();
            string path = "SSEConfigurationTool.files." + filename.Replace("\\", "/").Replace("/", ".").Replace(" ", "_").Trim();
            Console.WriteLine(path);
            return assembly.GetManifestResourceStream(path);
        }

        public static byte[] GetEmbeddedFile(string filename) {
            Assembly assembly = Assembly.GetExecutingAssembly();
            string path = "SSEConfigurationTool.files." + filename.Replace("\\", "/").Replace("/", ".").Replace(" ", "_").Trim();
            Stream s = assembly.GetManifestResourceStream(path);
            byte[] b = new byte[s.Length];
            s.Read(b, 0, (int)s.Length);
            return b;
        }

        public static string GetEmbeddedTextFile(string filename) {
            Assembly assembly = Assembly.GetExecutingAssembly();
            string path = "SSEConfigurationTool.files." + filename.Replace("\\", "/").Replace("/", ".").Replace(" ", "_").Trim();
            using (StreamReader reader = new StreamReader(assembly.GetManifestResourceStream(path))) {
                return reader.ReadToEnd();
            }
        }

        public static void SaveDraftJson(string path) {
            Draft draft = new Draft() { Platform = Globals.TargetPlatform, ConfiguredScoringItems = Globals.ConfiguredScoringItems };
            string json = JsonConvert.SerializeObject(draft, Formatting.Indented);
            File.WriteAllText(path, json);
        }

        public static bool LoadDraftJson(string path) {
            if (!File.Exists(path)) return false;
            string json = File.ReadAllText(path);
            Draft draft = JsonConvert.DeserializeObject<Draft>(json);
            Globals.TargetPlatform = draft.Platform;
            Globals.ConfiguredScoringItems = draft.ConfiguredScoringItems;

            //Old drafts did not have properties trimmed of spaces. Trim them to fix exporting.
            if (draft.Version == 1) {
                foreach (ConfiguredScoringItem k in draft.ConfiguredScoringItems) {
                    k.Platform = k.Platform.Trim();
                    k.Category = k.Category.Trim();
                    k.Name = k.Name.Trim();
                }
                draft.Version = Draft.LatestVersion;
            }

            return true;
        }

        public static void ExportConfiguration(string path, bool zip = false) {
            string exportPath;
            if (zip) {
                exportPath = GetTempDirectory().FullName;
                if (Directory.Exists(exportPath)) Directory.Delete(exportPath, true);
                Directory.CreateDirectory(exportPath);
            } else exportPath = path;

            for (int i = 0; i < Globals.ConfiguredScoringItems.Count(); i++) {
                string dir = (exportPath + "/" + (i+1).ToString("000") + (!zip ? (" " + Globals.ConfiguredScoringItems[i].DescriptiveName) : "")).AsPath();
                if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);

                CompiledScoringItem item = ScoringItemUtilities.CompileScoringItem(Globals.ConfiguredScoringItems[i]);
                File.WriteAllText((dir + "/client.csx").AsPath(), item.ClientPayload);
                File.WriteAllText((dir + "/server.csx").AsPath(), item.ServerPayload);
                File.WriteAllText((dir + "/metadata.json").AsPath(), item.Metadata);
            }

            SaveDraftJson((exportPath + "/draft.json").AsPath());

            if (zip) {
                using (FileStream fsOut = File.Create(path))
                using (var zipStream = new ZipOutputStream(fsOut)) {
                    zipStream.SetLevel(0);
                    zipStream.Password = Constants.OFFLINE_SCORING_CONFIGURATION_PASSWORD;
                    int folderOffset = exportPath.Length;
                    CompressFolder(exportPath, zipStream, folderOffset);
                }
                Directory.Delete(exportPath, true);
            }
        }

        private static void CompressFolder(string path, ZipOutputStream zipStream, int folderOffset) {

            string[] files = Directory.GetFiles(path);

            foreach (string filename in files) {

                FileInfo fi = new FileInfo(filename);

                string entryName = filename.Substring(folderOffset);
                entryName = ZipEntry.CleanName(entryName);

                ZipEntry newEntry = new ZipEntry(entryName);

                newEntry.DateTime = fi.LastWriteTime;
                newEntry.AESKeySize = 256;
                newEntry.Size = fi.Length;

                zipStream.PutNextEntry(newEntry);

                byte[] buffer = new byte[4096];
                using (FileStream fsInput = File.OpenRead(filename)) {
                    StreamUtils.Copy(fsInput, zipStream, buffer);
                }
                zipStream.CloseEntry();
            }

            string[] folders = Directory.GetDirectories(path);
            foreach (string folder in folders) {
                CompressFolder(folder, zipStream, folderOffset);
            }
        }

        public static void InstallOffline(string platform, string desktopPath, string readmePath, bool clearLogs) {
            string path = "install/" + platform + "/offline/";

            DirectoryInfo tempDir = GetTempDirectory();
            DirectoryInfo desktopExtDir = new DirectoryInfo((tempDir.FullName + "/desktop").AsPath());
            DirectoryInfo rootExtDir = new DirectoryInfo((tempDir.FullName + "/root").AsPath());
            desktopExtDir.Create();
            rootExtDir.Create();

            ExtractZipFileFromStream(GetEmbeddedFileAsStream(path + "root.zip"), rootExtDir.FullName);
            ExtractZipFileFromStream(GetEmbeddedFileAsStream(path + "root-config.zip"), rootExtDir.FullName);
            ExtractZipFileFromStream(GetEmbeddedFileAsStream(path + "desktop.zip"), desktopExtDir.FullName);

            string args = String.Join('|', new string[] { 
                tempDir.FullName,
                desktopPath,
                "http://localhost:5000",
                readmePath,
                clearLogs.ToString()
            });

            string scoringPath = String.Join('\n', GetEmbeddedTextFile((path + "/install.csx").AsPath()).Replace("\r\n", "\n").Split('\n').Where(x => !x.Contains("$INTELLISENSE"))).ExecuteAsCode<string>(args);
            ExportConfiguration(scoringPath, true);
            tempDir.Delete(true);
        }

        public static void InstallOnline(string platform, string desktopPath, string scoringServerUrl, string runtimeId, bool clearLogs) {
            string path = "install/" + platform + "/online/";

            DirectoryInfo tempDir = GetTempDirectory();
            DirectoryInfo desktopExtDir = new DirectoryInfo((tempDir.FullName + "/desktop").AsPath());
            DirectoryInfo rootExtDir = new DirectoryInfo((tempDir.FullName + "/root").AsPath());
            desktopExtDir.Create();
            rootExtDir.Create();

            ExtractZipFileFromStream(GetEmbeddedFileAsStream(path + "root.zip"), rootExtDir.FullName);
            ExtractZipFileFromStream(GetEmbeddedFileAsStream(path + "root-config.zip"), rootExtDir.FullName);
            ExtractZipFileFromStream(GetEmbeddedFileAsStream(path + "desktop.zip"), desktopExtDir.FullName);

            string args = String.Join('|', new string[] {
                tempDir.FullName,
                desktopPath,
                scoringServerUrl,
                runtimeId,
                clearLogs.ToString()
            });

            String.Join('\n', GetEmbeddedTextFile((path + "/install.csx").AsPath()).Replace("\r\n", "\n").Split('\n').Where(x => !x.Contains("$INTELLISENSE"))).ExecuteAsCode<string>(args);
            tempDir.Delete(true);
        }

        public static DirectoryInfo ExportRuntime(string runtimeId, RuntimeType runtimeType, string readmePath, bool zip = false) {
            DirectoryInfo tmp = GetTempDirectory();

            Console.WriteLine(tmp.FullName);

            Runtime runtime = new Runtime() {
                ID = runtimeId,
                Type = runtimeType
            };
            File.WriteAllText((tmp.FullName + "/runtime.conf").AsPath(), JsonConvert.SerializeObject(runtime));

            File.Move(readmePath, (tmp.FullName + "/readme.bin").AsPath());

            string scoringReportPath = "install/" + Globals.TargetPlatform + "/scoringreport.bin";
            File.WriteAllText((tmp.FullName + "/scoringreport.bin").AsPath(), GetEmbeddedTextFile(scoringReportPath));



            ExportConfiguration((tmp.FullName + "/scoring" + (zip ? ".zip" : "")).AsPath(), zip);

            return tmp;
        }

        public static void ExtractZipFileFromStream(Stream stream, string outFolder) {
            ZipFile zf = null;
            try {
                zf = new ZipFile(stream);
                foreach (ZipEntry zipEntry in zf) {
                    String entryFileName = zipEntry.Name;
                    string fullZipToPath = Path.Combine(outFolder, entryFileName);

                    if (!zipEntry.IsFile) {
                        if (!Directory.Exists(fullZipToPath)) Directory.CreateDirectory(fullZipToPath);
                        continue;
                    }

                    byte[] buffer = new byte[4096];
                    Stream zipStream = zf.GetInputStream(zipEntry);

                    string directoryName = Path.GetDirectoryName(fullZipToPath);
                    if (directoryName.Length > 0) if(!Directory.Exists(directoryName)) Directory.CreateDirectory(directoryName);

                    using (FileStream streamWriter = File.Create(fullZipToPath)) {
                        StreamUtils.Copy(zipStream, streamWriter, buffer);
                    }
                }
            } finally {
                if (zf != null) {
                    zf.IsStreamOwner = true;
                    zf.Close();
                }
            }
        }

        public static string PickFile(string title, string startDirectory, bool save = false) {
            string file;
            if (Environment.OSVersion.Platform == PlatformID.Unix) {
                file = ("zenity --file-selection --title=\"" + title + "\" " + (save ? "--save --confirm-overwrite " : "")).ExecuteAsBash(false);
            } else if (Environment.OSVersion.Platform == PlatformID.Win32NT) {
                if (save) {
                    file = ("powershell \"Add-Type -AssemblyName System.windows.forms|Out-Null;$f=New-Object System.Windows.Forms.SaveFileDialog;$f.InitialDirectory='" + startDirectory + "';$f.Title='" + title +"';$f.ShowDialog()|Out-Null;$f.FileName\"").ExecuteAsCmd();
                } else {
                    file = ("powershell \"Add-Type -AssemblyName System.windows.forms|Out-Null;$f=New-Object System.Windows.Forms.OpenFileDialog;$f.InitialDirectory='" + startDirectory + "';$f.Title='" + title + "';$f.ShowDialog()|Out-Null;$f.FileName\"").ExecuteAsCmd();
                }
            } else throw new PlatformNotSupportedException();

            return file.Trim();
        }

        public static string PickDirectory(string title) {
            string file;
            if (Environment.OSVersion.Platform == PlatformID.Unix) {
                file = ("zenity --file-selection --directory --title=\"" + title + "\"").ExecuteAsBash(false);
            } else if (Environment.OSVersion.Platform == PlatformID.Win32NT) {
                file = ("powershell \"Add-Type -AssemblyName System.windows.forms|Out-Null;$f=New-Object System.Windows.Forms.FolderBrowserDialog;$f.Description='" + title + "';$f.ShowDialog()|Out-Null;$f.SelectedPath\"").ExecuteAsCmd();
            } else throw new PlatformNotSupportedException();

            return file.Trim();
        }

        public static string GetRandomString(int length) {
            Random random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public static DirectoryInfo GetTempDirectory() {
            DirectoryInfo dir = new DirectoryInfo((Path.GetTempPath() + "/" + GetRandomString(16)));
            if (dir.Exists) dir.Delete(true);
            dir.Create();
            return dir;
        }

        //compatible with both windows and linux!
        //vm needs notifu64.exe in path or running directory!
        public static void SendToastNotification(string title, string message) {
            if (Environment.OSVersion.Platform == PlatformID.Win32NT) {
                ("notifu64 /p \"" + title + "\" /m \"" + message + "\"").ExecuteAsCmd();
            } else {
                ("notify-send -u critical \"" + title + "\" \"" + message + "\"").ExecuteAsBash();
            }
        }
    }
}
