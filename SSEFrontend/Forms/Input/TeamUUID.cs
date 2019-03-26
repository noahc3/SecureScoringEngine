using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Http;

namespace SSEFrontend.Forms.Input {
    public partial class TeamUUID : Form {
        public string result = "";

        public TeamUUID() {
            InitializeComponent();
        }

        private void btnConfirm_Click(object sender, EventArgs e) {
            using (HttpClient http = new HttpClient()) {
                http.DefaultRequestHeaders.Add("TEAM-UUID", txtUuid.Text);
                HttpResponseMessage response = http.GetAsync(Globals.ENDPOINT_VERIFY_TEAM_UUID).Result;

                if (response.IsSuccessStatusCode) {
                    MessageBox.Show("Team UUID verified and saved successfully!");
                    this.result = txtUuid.Text;
                    this.Close();
                    return;
                } else MessageBox.Show("Team UUID invalid! " + response.StatusCode);

                Environment.Exit(0);
                return;
            }
        }
    }
}
