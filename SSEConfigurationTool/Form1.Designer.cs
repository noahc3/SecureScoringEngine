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
            this.rtxtReadme = new System.Windows.Forms.RichTextBox();
            this.tabControlGlobal = new System.Windows.Forms.TabControl();
            this.tabRuntime = new System.Windows.Forms.TabPage();
            this.tabControlRuntime = new System.Windows.Forms.TabControl();
            this.tabRuntimeInfo = new System.Windows.Forms.TabPage();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.rbLinux = new System.Windows.Forms.RadioButton();
            this.rbWindows = new System.Windows.Forms.RadioButton();
            this.label1 = new System.Windows.Forms.Label();
            this.txtRuntimeId = new System.Windows.Forms.TextBox();
            this.tabReadme = new System.Windows.Forms.TabPage();
            this.btnImportReadme = new System.Windows.Forms.Button();
            this.tabExportRuntime = new System.Windows.Forms.TabPage();
            this.lblRuntimeSummary = new System.Windows.Forms.Label();
            this.btnExportRuntime = new System.Windows.Forms.Button();
            this.tabServer = new System.Windows.Forms.TabPage();
            this.tabControlGlobal.SuspendLayout();
            this.tabRuntime.SuspendLayout();
            this.tabControlRuntime.SuspendLayout();
            this.tabRuntimeInfo.SuspendLayout();
            this.panel1.SuspendLayout();
            this.tabReadme.SuspendLayout();
            this.tabExportRuntime.SuspendLayout();
            this.SuspendLayout();
            // 
            // rtxtReadme
            // 
            this.rtxtReadme.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.rtxtReadme.BackColor = System.Drawing.SystemColors.Control;
            this.rtxtReadme.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.rtxtReadme.Location = new System.Drawing.Point(13, 13);
            this.rtxtReadme.Margin = new System.Windows.Forms.Padding(10);
            this.rtxtReadme.Name = "rtxtReadme";
            this.rtxtReadme.ReadOnly = true;
            this.rtxtReadme.Size = new System.Drawing.Size(914, 437);
            this.rtxtReadme.TabIndex = 0;
            this.rtxtReadme.Text = "";
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
            this.tabRuntimeInfo.Controls.Add(this.panel1);
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
            // panel1
            // 
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.rbLinux);
            this.panel1.Controls.Add(this.rbWindows);
            this.panel1.Location = new System.Drawing.Point(9, 45);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(177, 70);
            this.panel1.TabIndex = 5;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 5);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(45, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Platform";
            // 
            // rbLinux
            // 
            this.rbLinux.AutoSize = true;
            this.rbLinux.Location = new System.Drawing.Point(6, 44);
            this.rbLinux.Name = "rbLinux";
            this.rbLinux.Size = new System.Drawing.Size(50, 17);
            this.rbLinux.TabIndex = 4;
            this.rbLinux.TabStop = true;
            this.rbLinux.Text = "Linux";
            this.rbLinux.UseVisualStyleBackColor = true;
            // 
            // rbWindows
            // 
            this.rbWindows.AutoSize = true;
            this.rbWindows.Location = new System.Drawing.Point(6, 21);
            this.rbWindows.Name = "rbWindows";
            this.rbWindows.Size = new System.Drawing.Size(69, 17);
            this.rbWindows.TabIndex = 3;
            this.rbWindows.TabStop = true;
            this.rbWindows.Text = "Windows";
            this.rbWindows.UseVisualStyleBackColor = true;
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
            this.tabReadme.Controls.Add(this.btnImportReadme);
            this.tabReadme.Controls.Add(this.rtxtReadme);
            this.tabReadme.Location = new System.Drawing.Point(4, 22);
            this.tabReadme.Name = "tabReadme";
            this.tabReadme.Padding = new System.Windows.Forms.Padding(3);
            this.tabReadme.Size = new System.Drawing.Size(944, 527);
            this.tabReadme.TabIndex = 0;
            this.tabReadme.Text = "README";
            this.tabReadme.UseVisualStyleBackColor = true;
            // 
            // btnImportReadme
            // 
            this.btnImportReadme.Location = new System.Drawing.Point(701, 498);
            this.btnImportReadme.Name = "btnImportReadme";
            this.btnImportReadme.Size = new System.Drawing.Size(233, 23);
            this.btnImportReadme.TabIndex = 1;
            this.btnImportReadme.Text = "Import README (.rtf)";
            this.btnImportReadme.UseVisualStyleBackColor = true;
            this.btnImportReadme.Click += new System.EventHandler(this.btnImportReadme_Click);
            // 
            // tabExportRuntime
            // 
            this.tabExportRuntime.Controls.Add(this.lblRuntimeSummary);
            this.tabExportRuntime.Controls.Add(this.btnExportRuntime);
            this.tabExportRuntime.Location = new System.Drawing.Point(4, 22);
            this.tabExportRuntime.Name = "tabExportRuntime";
            this.tabExportRuntime.Size = new System.Drawing.Size(944, 527);
            this.tabExportRuntime.TabIndex = 2;
            this.tabExportRuntime.Text = "Export";
            this.tabExportRuntime.UseVisualStyleBackColor = true;
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
            this.btnExportRuntime.Size = new System.Drawing.Size(930, 23);
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
            this.tabServer.Size = new System.Drawing.Size(948, 549);
            this.tabServer.TabIndex = 1;
            this.tabServer.Text = "Server Config Generator";
            this.tabServer.UseVisualStyleBackColor = true;
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
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.tabReadme.ResumeLayout(false);
            this.tabExportRuntime.ResumeLayout(false);
            this.tabExportRuntime.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RichTextBox rtxtReadme;
        private System.Windows.Forms.TabControl tabControlGlobal;
        private System.Windows.Forms.TabPage tabRuntime;
        private System.Windows.Forms.TabControl tabControlRuntime;
        private System.Windows.Forms.TabPage tabReadme;
        private System.Windows.Forms.TabPage tabRuntimeInfo;
        private System.Windows.Forms.TabPage tabServer;
        private System.Windows.Forms.Button btnImportReadme;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.RadioButton rbLinux;
        private System.Windows.Forms.RadioButton rbWindows;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtRuntimeId;
        private System.Windows.Forms.TabPage tabExportRuntime;
        private System.Windows.Forms.Label lblRuntimeSummary;
        private System.Windows.Forms.Button btnExportRuntime;
    }
}

