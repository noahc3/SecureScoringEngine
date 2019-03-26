using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using SSECommon;
using SSEFrontend.Types;
using SSEFrontend.Forms.Input;

namespace SSEFrontend {
    static class Globals {
        public static string CONFIG_DIRECTORY = (Environment.CurrentDirectory + "\\config\\").AsPath();
        public static string CONFIG_SESSION = (CONFIG_DIRECTORY + "\\session.json").AsPath();
        public static string README_LOCATION = (CONFIG_DIRECTORY + "\\readme.rtf").AsPath();

        //base address
        public static string ENDPOINT_BASE_ADDRESS = "http://192.168.0.10:59137";

        //plaintext endpoints
        public static Uri ENDPOINT_VERIFY_TEAM_UUID = new Uri(ENDPOINT_BASE_ADDRESS + "/api/auth/isteamuuidvalid");
        public static Uri ENDPOINT_KEY_EXCHANGE = new Uri(ENDPOINT_BASE_ADDRESS + "/api/auth/keyexchange");

        //ciphertext endpoints
        public static Uri ENDPOINT_PING_CIPHERTEXT = new Uri(ENDPOINT_BASE_ADDRESS + "/api/auth/ping");
        public static Uri ENDPOINT_README = new Uri(ENDPOINT_BASE_ADDRESS + "/api/fetch/readme");

        public static SessionConfig SessionConfig;


        public static void GenerateConfig() {
            SessionConfig sessionConfig = new SessionConfig();

            TeamUUID form = new TeamUUID();
            form.ShowDialog();
            sessionConfig.TeamUUID = form.result;

            MessageBox.Show(Environment.OSVersion.VersionString);

            sessionConfig.RuntimeID = "Microsoft Windows 10";

            sessionConfig.Flush();
        }

        public static void LoadConfig() {
            SessionConfig = SessionConfig.FromJson(File.ReadAllText(Globals.CONFIG_SESSION));
        }
    }
}
