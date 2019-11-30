using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Newtonsoft.Json;

using SSECommon.Types;

namespace SSEConfigurationTool {
    public partial class Form1 : Form {
        public Form1() {
            InitializeComponent();

            if (Globals.platform == PlatformID.Unix) {
                lblReadmeHelp.Text = "This file will be copied from the server to /opt/SSEService/files/readme.html. You should create a desktop shortcut to this location to make it viewable.";
            }
        }

        private void CreateOutputFolderLayout() {
        }

        private void btnImportReadme_Click(object sender, EventArgs e) {
        }

        private void btnExportRuntime_Click(object sender, EventArgs e) {
        }
    }
}
