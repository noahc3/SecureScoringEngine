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
        }

        private void CreateOutputFolderLayout() {
            Directory.CreateDirectory(Environment.CurrentDirectory + "\\output");
            Directory.CreateDirectory(Environment.CurrentDirectory + "\\output\\server");
            Directory.CreateDirectory(Environment.CurrentDirectory + "\\output\\client");
            Directory.CreateDirectory(Environment.CurrentDirectory + "\\output\\server\\config");
            Directory.CreateDirectory(Environment.CurrentDirectory + "\\output\\server\\config\\runtimes");
        }

        private void btnImportReadme_Click(object sender, EventArgs e) {
            OpenFileDialog ofd = new OpenFileDialog();
            DialogResult result = ofd.ShowDialog();
            if (result == DialogResult.OK) // Test result.
            {
                txtReadmeFile.Text = ofd.FileName;
            }
        }

        private void btnExportRuntime_Click(object sender, EventArgs e) {
            CreateOutputFolderLayout();

            //create runtime folder
            string outputFolder = Environment.CurrentDirectory + "\\output\\server\\config\\runtimes\\" + txtRuntimeId.Text + "\\";
            Directory.CreateDirectory(outputFolder);

            //save readme
            File.Copy(txtReadmeFile.Text, outputFolder +  "\\readme.bin");

            //save general runtime infos
            Runtime runtime = new Runtime();

            runtime.ID = txtRuntimeId.Text;

            if (rbWindows.Checked) runtime.Type = RuntimeType.Windows;
            else if (rbLinux.Checked) runtime.Type = RuntimeType.Linux;

            runtime.readmeLocation = txtReadmeVmLocation.Text;

            //save runtime general conf
            File.WriteAllText(Environment.CurrentDirectory + "\\output\\server\\config\\runtimes\\" + txtRuntimeId.Text + "\\runtime.conf", JsonConvert.SerializeObject(runtime, Formatting.Indented));

            //open output dir
            if (chkShowOutputDirectoryRuntime.Checked) Process.Start(Environment.CurrentDirectory + "\\output");

        }
    }
}
