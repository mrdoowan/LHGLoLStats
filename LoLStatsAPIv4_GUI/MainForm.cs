using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LoLStatsAPIv4_GUI {
    public partial class MainForm : Form {

        public MainForm() {
            InitializeComponent();

            // Add to List of Competition Names from Database Combobox
            comboBox_Competition.Items.AddRange(MasterWrapper.GetCompetitionNames().ToArray());
        }

        #region Helper Functions

        private OpenFileDialog OFD(string title) {
            using (OpenFileDialog ofd = new OpenFileDialog()) {
                ofd.Filter = "Text File (*.txt)|*.txt";
                ofd.Title = title;
                ofd.RestoreDirectory = true;

                if (ofd.ShowDialog() == DialogResult.OK) {
                    return ofd;
                }
                else {
                    return null;
                }
            }
        }

        private SaveFileDialog SFD(string title) {
            using (SaveFileDialog sfd = new SaveFileDialog()) {
                sfd.Filter = "Text File (*.txt)|*.txt";
                sfd.Title = title;
                sfd.OverwritePrompt = true;
                if (sfd.ShowDialog() == DialogResult.OK) {
                    return sfd;
                }
                else {
                    return null;
                }
            }
        }

        private bool isAPIKeyEmpty() {
            if (string.IsNullOrWhiteSpace(textBox_apiKey.Text)) {
                MessageBox.Show("API Key is empty.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return true;
            }
            return false;
        }

        #endregion

        private void button_LoadNames_Click(object sender, EventArgs e) {
            if (isAPIKeyEmpty()) { return; }

            var ofd_Txt = OFD("Open Summoner Names");
            if (ofd_Txt == null) { return; }
            var summonersList = new List<string>(File.ReadLines(ofd_Txt.FileName));
            if (string.IsNullOrWhiteSpace(textBox_NewCompName.Text)) {
                MessageBox.Show("Competition name is empty.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            LogClass.ClearLog();
            if (MasterWrapper.LoadSummonerNamesIntoDB(textBox_NewCompName.Text, summonersList)) {
                comboBox_Competition.Items.Add(textBox_NewCompName.Text);
                MessageBox.Show("Player Database updated!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            richTextBox_Log.Text = LogClass.GetReport();
        }

        private void button_LoadMatch_Click(object sender, EventArgs e) {
            if (isAPIKeyEmpty()) { return; }

            var matchForm = new LoadMatchForm();
            if (string.IsNullOrWhiteSpace(comboBox_Competition.Text)) {
                MessageBox.Show("Competition not selected.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            LogClass.ClearLog();
            if (matchForm.OpenWindow(comboBox_Competition.Text)) {
                MessageBox.Show("Match Stats updated!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            richTextBox_Log.Text = LogClass.GetReport();
        }

        private void button_AssignRoles_Click(object sender, EventArgs e) {

        }

        private void button_UpdateRanks_Click(object sender, EventArgs e) {
            if (isAPIKeyEmpty()) { return; }

            if (string.IsNullOrWhiteSpace(comboBox_Competition.Text)) {
                MessageBox.Show("Competition not selected", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            LogClass.ClearLog();
            if (MasterWrapper.CompetitionUpdateSummonerRanks(comboBox_Competition.Text)) {
                MessageBox.Show("Summoner ranks updated!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            richTextBox_Log.Text = LogClass.GetReport();
        }

        private void button_ExportExcel_Click(object sender, EventArgs e) {

        }

        private void textBox_apiKey_TextChanged(object sender, EventArgs e) {
            MasterWrapper.UpdateAPIDevInstance(textBox_apiKey.Text);
        }

        private void comboBox_Competition_SelectedIndexChanged(object sender, EventArgs e) {
            // Add to List of Match IDs from Database Combobox
            comboBox_MatchId.Items.Clear();
            if (!string.IsNullOrWhiteSpace(comboBox_Competition.Text)) {
                comboBox_MatchId.Items.AddRange(MasterWrapper.GetMatchIdList(comboBox_Competition.Text).ToArray());
            }
        }

        private void checkBox_NoDBLog_CheckedChanged(object sender, EventArgs e) {
            LogClass.SetEnableDBLogs(!checkBox_NoDBLog.Checked);
        }

        private void button_LoadTeamNames_Click(object sender, EventArgs e) {

        }
    }
}
