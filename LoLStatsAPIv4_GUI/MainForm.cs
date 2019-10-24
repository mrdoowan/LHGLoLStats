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

            MasterWrapper.UpdateAPIDevInstance(textBox_apiKey.Text);
            MasterWrapper.UpdateConnectionString(textBox_ConnectionString.Text);

            // Initialize cache for champ json
            try { MasterWrapper.InitializeChampDict(); } 
            catch { }

            // Add to List of Competition Names from Database Combobox
            comboBox_Competition.Items.AddRange(MasterWrapper.GetCompetitionNames().ToArray());

            // Initialize LogClass logging DB queries based on checkbox
            LogClass.SetEnableDBLogs(!checkBox_NoDBLog.Checked);
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

        private bool areFieldsEmpty() {
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

        private void textBox_ConnectionString_TextChanged(object sender, EventArgs e) {
            MasterWrapper.UpdateConnectionString(textBox_ConnectionString.Text);
        }

        private void button_LoadNames_Click(object sender, EventArgs e) {
            if (areFieldsEmpty()) { return; }
            if (string.IsNullOrWhiteSpace(textBox_NewCompName.Text) || string.IsNullOrWhiteSpace(comboBox_CompetitionType.Text)) {
                MessageBox.Show("Competition Name or Type is empty.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var ofd_Txt = OFD("Load Summoner Names", "Text File (*.txt)|*.txt");
            if (ofd_Txt == null) { return; }
            var summonersList = new List<string>(File.ReadLines(ofd_Txt.FileName));

            LogClass.ClearLog();
            if (MasterWrapper.LoadSummonerNamesIntoDB(textBox_NewCompName.Text, comboBox_CompetitionType.Text, summonersList)) {
                comboBox_Competition.Items.Add(textBox_NewCompName.Text);
                MessageBox.Show("Player Database updated!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            richTextBox_Log.Text = LogClass.GetReport();
        }

        private void button_LoadMatch_Click(object sender, EventArgs e) {
            if (areFieldsEmpty()) { return; }

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
            if (areFieldsEmpty()) { return; }

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
                LogClass.ClearLog();
                comboBox_MatchId.Items.AddRange(MasterWrapper.GetMatchIdList(comboBox_Competition.Text).ToArray());
                richTextBox_Log.Text = LogClass.GetReport();
            }
        }

        private void checkBox_NoDBLog_CheckedChanged(object sender, EventArgs e) {
            LogClass.SetEnableDBLogs(!checkBox_NoDBLog.Checked);
        }

        private void button_LoadTeamNames_Click(object sender, EventArgs e) {
            if (!string.IsNullOrWhiteSpace(comboBox_Competition.Text)) {
                LogClass.ClearLog();
                var form = new EditTeamListForm();
                form.OpenWindow(comboBox_Competition.Text);
                richTextBox_Log.Text = LogClass.GetReport();
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
    }
}
