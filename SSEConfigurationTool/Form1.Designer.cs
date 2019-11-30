namespace SSEConfigurationTool {
    partial class Form1 {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.tabControlGlobal = new System.Windows.Forms.TabControl();
            this.tabRuntime = new System.Windows.Forms.TabPage();
            this.tabControlRuntime = new System.Windows.Forms.TabControl();
            this.tabRuntimeInfo = new System.Windows.Forms.TabPage();
            this.label1 = new System.Windows.Forms.Label();
            this.txtRuntimeId = new System.Windows.Forms.TextBox();
            this.tabReadme = new System.Windows.Forms.TabPage();
            this.tabExportRuntime = new System.Windows.Forms.TabPage();
            this.chkShowOutputDirectoryRuntime = new System.Windows.Forms.CheckBox();
            this.lblRuntimeSummary = new System.Windows.Forms.Label();
            this.btnExportRuntime = new System.Windows.Forms.Button();
            this.tabServer = new System.Windows.Forms.TabPage();
            this.lblReadmeHelp = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.txtReadmeFile = new System.Windows.Forms.TextBox();
            this.btnImportReadme = new System.Windows.Forms.Button();
            this.tabControlGlobal.SuspendLayout();
            this.tabRuntime.SuspendLayout();
            this.tabControlRuntime.SuspendLayout();
            this.tabRuntimeInfo.SuspendLayout();
            this.tabExportRuntime.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControlGlobal
            // 
            this.tabControlGlobal.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControlGlobal.Controls.Add(this.tabRuntime);
            this.tabControlGlobal.Controls.Add(this.tabServer);
            this.tabControlGlobal.Location = new System.Drawing.Point(0, 0);
            this.tabControlGlobal.Name = "tabControlGlobal";
            this.tabControlGlobal.SelectedIndex = 0;
            this.tabControlGlobal.Size = new System.Drawing.Size(986, 582);
            this.tabControlGlobal.TabIndex = 1;
            // 
            // tabRuntime
            // 
            this.tabRuntime.Controls.Add(this.tabControlRuntime);
            this.tabRuntime.Location = new System.Drawing.Point(4, 22);
            this.tabRuntime.Name = "tabRuntime";
            this.tabRuntime.Padding = new System.Windows.Forms.Padding(3);
            this.tabRuntime.Size = new System.Drawing.Size(978, 556);
            this.tabRuntime.TabIndex = 0;
            this.tabRuntime.Text = "Runtime Config Generator";
            this.tabRuntime.UseVisualStyleBackColor = true;
            // 
            // tabControlRuntime
            // 
            this.tabControlRuntime.Controls.Add(this.tabRuntimeInfo);
            this.tabControlRuntime.Controls.Add(this.tabReadme);
            this.tabControlRuntime.Controls.Add(this.tabExportRuntime);
            this.tabControlRuntime.Location = new System.Drawing.Point(0, 0);
            this.tabControlRuntime.Name = "tabControlRuntime";
            this.tabControlRuntime.SelectedIndex = 0;
            this.tabControlRuntime.Size = new System.Drawing.Size(975, 553);
            this.tabControlRuntime.TabIndex = 1;
            // 
            // tabRuntimeInfo
            // 
            this.tabRuntimeInfo.Controls.Add(this.lblReadmeHelp);
            this.tabRuntimeInfo.Controls.Add(this.label3);
            this.tabRuntimeInfo.Controls.Add(this.txtReadmeFile);
            this.tabRuntimeInfo.Controls.Add(this.btnImportReadme);
            this.tabRuntimeInfo.Controls.Add(this.label1);
            this.tabRuntimeInfo.Controls.Add(this.txtRuntimeId);
            this.tabRuntimeInfo.Location = new System.Drawing.Point(4, 22);
            this.tabRuntimeInfo.Name = "tabRuntimeInfo";
            this.tabRuntimeInfo.Padding = new System.Windows.Forms.Padding(3);
            this.tabRuntimeInfo.Size = new System.Drawing.Size(967, 527);
            this.tabRuntimeInfo.TabIndex = 1;
            this.tabRuntimeInfo.Text = "Runtime Information";
            this.tabRuntimeInfo.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 3);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(60, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Runtime ID";
            // 
            // txtRuntimeId
            // 
            this.txtRuntimeId.Location = new System.Drawing.Point(9, 19);
            this.txtRuntimeId.Name = "txtRuntimeId";
            this.txtRuntimeId.Size = new System.Drawing.Size(178, 20);
            this.txtRuntimeId.TabIndex = 0;
            // 
            // tabReadme
            // 
            this.tabReadme.Location = new System.Drawing.Point(4, 22);
            this.tabReadme.Name = "tabReadme";
            this.tabReadme.Padding = new System.Windows.Forms.Padding(3);
            this.tabReadme.Size = new System.Drawing.Size(967, 527);
            this.tabReadme.TabIndex = 0;
            this.tabReadme.Text = "README";
            this.tabReadme.UseVisualStyleBackColor = true;
            // 
            // tabExportRuntime
            // 
            this.tabExportRuntime.Controls.Add(this.chkShowOutputDirectoryRuntime);
            this.tabExportRuntime.Controls.Add(this.lblRuntimeSummary);
            this.tabExportRuntime.Controls.Add(this.btnExportRuntime);
            this.tabExportRuntime.Location = new System.Drawing.Point(4, 22);
            this.tabExportRuntime.Name = "tabExportRuntime";
            this.tabExportRuntime.Size = new System.Drawing.Size(967, 527);
            this.tabExportRuntime.TabIndex = 2;
            this.tabExportRuntime.Text = "Export";
            this.tabExportRuntime.UseVisualStyleBackColor = true;
            // 
            // chkShowOutputDirectoryRuntime
            // 
            this.chkShowOutputDirectoryRuntime.AutoSize = true;
            this.chkShowOutputDirectoryRuntime.Location = new System.Drawing.Point(7, 472);
            this.chkShowOutputDirectoryRuntime.Name = "chkShowOutputDirectoryRuntime";
            this.chkShowOutputDirectoryRuntime.Size = new System.Drawing.Size(133, 17);
            this.chkShowOutputDirectoryRuntime.TabIndex = 2;
            this.chkShowOutputDirectoryRuntime.Text = "Show Output Directory";
            this.chkShowOutputDirectoryRuntime.UseVisualStyleBackColor = true;
            // 
            // lblRuntimeSummary
            // 
            this.lblRuntimeSummary.AutoSize = true;
            this.lblRuntimeSummary.Location = new System.Drawing.Point(4, 11);
            this.lblRuntimeSummary.Name = "lblRuntimeSummary";
            this.lblRuntimeSummary.Size = new System.Drawing.Size(62, 13);
            this.lblRuntimeSummary.TabIndex = 1;
            this.lblRuntimeSummary.Text = "SUMMARY";
            // 
            // btnExportRuntime
            // 
            this.btnExportRuntime.Location = new System.Drawing.Point(4, 495);
            this.btnExportRuntime.Name = "btnExportRuntime";
            this.btnExportRuntime.Size = new System.Drawing.Size(960, 23);
            this.btnExportRuntime.TabIndex = 0;
            this.btnExportRuntime.Text = "EXPORT";
            this.btnExportRuntime.UseVisualStyleBackColor = true;
            this.btnExportRuntime.Click += new System.EventHandler(this.btnExportRuntime_Click);
            // 
            // tabServer
            // 
            this.tabServer.Location = new System.Drawing.Point(4, 22);
            this.tabServer.Name = "tabServer";
            this.tabServer.Padding = new System.Windows.Forms.Padding(3);
            this.tabServer.Size = new System.Drawing.Size(978, 556);
            this.tabServer.TabIndex = 1;
            this.tabServer.Text = "Server Config Generator";
            this.tabServer.UseVisualStyleBackColor = true;
            // 
            // lblReadmeHelp
            // 
            this.lblReadmeHelp.Location = new System.Drawing.Point(196, 42);
            this.lblReadmeHelp.Name = "lblReadmeHelp";
            this.lblReadmeHelp.Size = new System.Drawing.Size(360, 52);
            this.lblReadmeHelp.TabIndex = 9;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(193, 3);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(69, 13);
            this.label3.TabIndex = 8;
            this.label3.Text = "README file";
            // 
            // txtReadmeFile
            // 
            this.txtReadmeFile.Location = new System.Drawing.Point(196, 19);
            this.txtReadmeFile.Name = "txtReadmeFile";
            this.txtReadmeFile.Size = new System.Drawing.Size(360, 20);
            this.txtReadmeFile.TabIndex = 7;
            // 
            // btnImportReadme
            // 
            this.btnImportReadme.Location = new System.Drawing.Point(562, 16);
            this.btnImportReadme.Name = "btnImportReadme";
            this.btnImportReadme.Size = new System.Drawing.Size(81, 23);
            this.btnImportReadme.TabIndex = 6;
            this.btnImportReadme.Text = "Browse...";
            this.btnImportReadme.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(984, 581);
            this.Controls.Add(this.tabControlGlobal);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.Text = "SSE Configuration Generator";
            this.tabControlGlobal.ResumeLayout(false);
            this.tabRuntime.ResumeLayout(false);
            this.tabControlRuntime.ResumeLayout(false);
            this.tabRuntimeInfo.ResumeLayout(false);
            this.tabRuntimeInfo.PerformLayout();
            this.tabExportRuntime.ResumeLayout(false);
            this.tabExportRuntime.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.TabControl tabControlGlobal;
        private System.Windows.Forms.TabPage tabRuntime;
        private System.Windows.Forms.TabControl tabControlRuntime;
        private System.Windows.Forms.TabPage tabReadme;
        private System.Windows.Forms.TabPage tabRuntimeInfo;
        private System.Windows.Forms.TabPage tabServer;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtRuntimeId;
        private System.Windows.Forms.TabPage tabExportRuntime;
        private System.Windows.Forms.Label lblRuntimeSummary;
        private System.Windows.Forms.Button btnExportRuntime;
        private System.Windows.Forms.CheckBox chkShowOutputDirectoryRuntime;
        private System.Windows.Forms.Label lblReadmeHelp;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtReadmeFile;
        private System.Windows.Forms.Button btnImportReadme;
    }
}

