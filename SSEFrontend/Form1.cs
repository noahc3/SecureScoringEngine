using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using SSEFrontend.Net;
using SSEFrontend.Security;

namespace SSEFrontend {
    public partial class Form1 : Form {
        public Form1() {
            InitializeComponent();

            if (!Directory.Exists(Globals.CONFIG_DIRECTORY)) Directory.CreateDirectory(Globals.CONFIG_DIRECTORY);

            if (!File.Exists(Globals.CONFIG_SESSION)) {
                Globals.GenerateConfig();
            }

            Globals.LoadConfig();

            ClientServerComms.CiphertextPing();

            byte[] readmeBlob = ClientServerComms.GetReadme();
            File.WriteAllBytes(Globals.README_LOCATION, readmeBlob);
            rtxtReadme.LoadFile(Globals.README_LOCATION);
            File.Delete(Globals.README_LOCATION);
        }
    }
}
