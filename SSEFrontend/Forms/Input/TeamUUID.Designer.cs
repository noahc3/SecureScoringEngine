namespace SSEFrontend.Forms.Input {
    partial class TeamUUID {
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
            this.txtUuid = new System.Windows.Forms.TextBox();
            this.lblUUID = new System.Windows.Forms.Label();
            this.btnConfirm = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // txtUuid
            // 
            this.txtUuid.Location = new System.Drawing.Point(15, 25);
            this.txtUuid.Name = "txtUuid";
            this.txtUuid.Size = new System.Drawing.Size(266, 20);
            this.txtUuid.TabIndex = 0;
            // 
            // lblUUID
            // 
            this.lblUUID.AutoSize = true;
            this.lblUUID.Location = new System.Drawing.Point(12, 9);
            this.lblUUID.Name = "lblUUID";
            this.lblUUID.Size = new System.Drawing.Size(117, 13);
            this.lblUUID.TabIndex = 1;
            this.lblUUID.Text = "Enter Your Team UUID";
            // 
            // btnConfirm
            // 
            this.btnConfirm.Location = new System.Drawing.Point(206, 51);
            this.btnConfirm.Name = "btnConfirm";
            this.btnConfirm.Size = new System.Drawing.Size(75, 23);
            this.btnConfirm.TabIndex = 2;
            this.btnConfirm.Text = "Confirm";
            this.btnConfirm.UseVisualStyleBackColor = true;
            this.btnConfirm.Click += new System.EventHandler(this.btnConfirm_Click);
            // 
            // TeamUUID
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(297, 87);
            this.ControlBox = false;
            this.Controls.Add(this.btnConfirm);
            this.Controls.Add(this.lblUUID);
            this.Controls.Add(this.txtUuid);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "TeamUUID";
            this.Text = "TeamUUID";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtUuid;
        private System.Windows.Forms.Label lblUUID;
        private System.Windows.Forms.Button btnConfirm;
    }
}