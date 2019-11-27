using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace LoLStatsAPIv4_GUI {
    public partial class MainForm : Form {

        public MainForm() {
            InitializeComponent();

            textBox_ConnectionString.Text = Properties.Settings.Default.ConnectionString;
            textBox_apiKey.Text = Properties.Settings.Default.APIKey;

            try { MasterWrapper.UpdateAPIDevInstance(textBox_apiKey.Text); } catch { }
            MasterWrapper.UpdateConnectionString(textBox_ConnectionString.Text);

            // Initialize Cache
            try { MasterWrapper.InitializeChampCache(); } catch { }
            try { comboBox_Competition.Items.AddRange(MasterWrapper.InitializeCompetitionCache().ToArray()); } catch { }
            try { MasterWrapper.InitializeTeamCache(); } catch { }
            try { MasterWrapper.InitializeSummonerCache(); } catch { }
        }

        #region Helper Functions

        private OpenFileDialog OFD(string title, string filter) {
            using (OpenFileDialog ofd = new OpenFileDialog()) {
                ofd.Filter = filter;
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

        private SaveFileDialog SFD(string title, string filter) {
            using (SaveFileDialog sfd = new SaveFileDialog()) {
                sfd.Filter = filter;
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

        private bool AreMainFieldsEmpty() {
            if (string.IsNullOrWhiteSpace(textBox_apiKey.Text)) {
                MessageBox.Show("API Key is empty.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return true;
            }
            else if (string.IsNullOrWhiteSpace(textBox_ConnectionString.Text)) {
                MessageBox.Show("Connection string is empty.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return true;
            }
            return false;
        }

        #endregion

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e) {
            Properties.Settings.Default.ConnectionString = textBox_ConnectionString.Text;
            Properties.Settings.Default.APIKey = textBox_apiKey.Text;
            Properties.Settings.Default.Save();
        }

        private void textBox_ConnectionString_TextChanged(object sender, EventArgs e) {
            MasterWrapper.UpdateConnectionString(textBox_ConnectionString.Text);
        }

        private void button_LoadMatch_Click(object sender, EventArgs e) {
            if (AreMainFieldsEmpty()) { return; }

            if (string.IsNullOrWhiteSpace(comboBox_Competition.Text)) {
                MessageBox.Show("Competition not selected.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            using (var matchForm = new LoadMatchForm()) {
                LogClass.ClearLog();
                string idStr = matchForm.OpenWindow(comboBox_Competition.Text);
                if (!string.IsNullOrWhiteSpace(idStr)) {
                    if (!comboBox_MatchId.Items.Contains(idStr)) {
                        comboBox_MatchId.Items.Add(idStr);
                        MessageBox.Show("Match Stats updated!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                richTextBox_Log.Text = LogClass.GetReport();
            }
        }

        private void button_EditMatchRoles_Click(object sender, EventArgs e) {
            // Currently does nothing heehee xd
            if (AreMainFieldsEmpty()) { return; }

            if (string.IsNullOrWhiteSpace(comboBox_MatchId.Text)) {
                MessageBox.Show("Match ID not selected.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            LogClass.ClearLog();
            string idStr = MasterWrapper.LoadEditedMatchStatsIntoDB(long.Parse(comboBox_MatchId.Text));
            if (!string.IsNullOrWhiteSpace(idStr)) {
                MessageBox.Show("Match Stat " + idStr + " updated!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            richTextBox_Log.Text = LogClass.GetReport();
        }

        private void button_UpdateRanks_Click(object sender, EventArgs e) {
            if (AreMainFieldsEmpty()) { return; }

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

        private void textBox_apiKey_TextChanged(object sender, EventArgs e) {
            MasterWrapper.UpdateAPIDevInstance(textBox_apiKey.Text);
        }

        private void comboBox_Competition_SelectedIndexChanged(object sender, EventArgs e) {
            // Add to List of Match IDs from Database Combobox
            comboBox_MatchId.Items.Clear();
            if (!string.IsNullOrWhiteSpace(comboBox_Competition.Text)) {
                LogClass.ClearLog();
                comboBox_MatchId.Items.AddRange(MasterWrapper.GetMatchIdList(comboBox_Competition.Text).ToArray());
                richTextBox_Log.Text = LogClass.GetReport();
            }
        }

        private void button_EditTeamNames_Click(object sender, EventArgs e) {
            if (!string.IsNullOrWhiteSpace(comboBox_Competition.Text)) {
                using (var form = new EditTeamListForm()) {
                    LogClass.ClearLog();
                    form.OpenWindow(comboBox_Competition.Text);
                    richTextBox_Log.Text = LogClass.GetReport();
                }
            }
            else {
                MessageBox.Show("No Competition Name selected.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button_LoadChampJSON_Click(object sender, EventArgs e) {
            var ofd_JSON = OFD("Open Champ JSON", "JSON File (*.json)|*.json");
            if (ofd_JSON == null) { return; }

            LogClass.ClearLog();
            MasterWrapper.LoadChampionJSON(ofd_JSON.FileName);
            MessageBox.Show("Champions DB updated by JSON file", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            richTextBox_Log.Text = LogClass.GetReport();
        }

        private void button_FixDBEntries_Click(object sender, EventArgs e) {
            if (AreMainFieldsEmpty()) { return; }

            MessageBox.Show("No bugs to fix!", "Nothing", MessageBoxButtons.OK, MessageBoxIcon.Information); return;

            LogClass.ClearLog();
            if (!MasterWrapper.ArbitraryBugFix()) {
                MessageBox.Show("Error happened :(", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            richTextBox_Log.Text = LogClass.GetReport();
            MessageBox.Show("Gasp. Bug fixed!", "Yay", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

    }
}
