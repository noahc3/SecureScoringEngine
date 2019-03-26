namespace SSEFrontend {
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
            this.tabGlobal = new System.Windows.Forms.TabControl();
            this.tabReadme = new System.Windows.Forms.TabPage();
            this.tabScore = new System.Windows.Forms.TabPage();
            this.rtxtReadme = new System.Windows.Forms.RichTextBox();
            this.tabGlobal.SuspendLayout();
            this.tabReadme.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabGlobal
            // 
            this.tabGlobal.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabGlobal.Controls.Add(this.tabReadme);
            this.tabGlobal.Controls.Add(this.tabScore);
            this.tabGlobal.Location = new System.Drawing.Point(2, 1);
            this.tabGlobal.Name = "tabGlobal";
            this.tabGlobal.SelectedIndex = 0;
            this.tabGlobal.Size = new System.Drawing.Size(985, 580);
            this.tabGlobal.TabIndex = 0;
            // 
            // tabReadme
            // 
            this.tabReadme.Controls.Add(this.rtxtReadme);
            this.tabReadme.Location = new System.Drawing.Point(4, 22);
            this.tabReadme.Name = "tabReadme";
            this.tabReadme.Padding = new System.Windows.Forms.Padding(3);
            this.tabReadme.Size = new System.Drawing.Size(977, 554);
            this.tabReadme.TabIndex = 0;
            this.tabReadme.Text = "README";
            this.tabReadme.UseVisualStyleBackColor = true;
            // 
            // tabScore
            // 
            this.tabScore.Location = new System.Drawing.Point(4, 22);
            this.tabScore.Name = "tabScore";
            this.tabScore.Padding = new System.Windows.Forms.Padding(3);
            this.tabScore.Size = new System.Drawing.Size(790, 422);
            this.tabScore.TabIndex = 1;
            this.tabScore.Text = "Score";
            this.tabScore.UseVisualStyleBackColor = true;
            // 
            // rtxtReadme
            // 
            this.rtxtReadme.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.rtxtReadme.Location = new System.Drawing.Point(0, 0);
            this.rtxtReadme.Name = "rtxtReadme";
            this.rtxtReadme.ReadOnly = true;
            this.rtxtReadme.Size = new System.Drawing.Size(977, 554);
            this.rtxtReadme.TabIndex = 0;
            this.rtxtReadme.Text = "";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(984, 581);
            this.Controls.Add(this.tabGlobal);
            this.Name = "Form1";
            this.Text = "Form1";
            this.tabGlobal.ResumeLayout(false);
            this.tabReadme.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabGlobal;
        private System.Windows.Forms.TabPage tabReadme;
        private System.Windows.Forms.RichTextBox rtxtReadme;
        private System.Windows.Forms.TabPage tabScore;
    }
}

