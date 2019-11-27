using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Microsoft.VisualBasic;

namespace LoLStatsAPIv4_GUI {
    public partial class EditTeamListForm : Form {

        private string competitionName;
        Dictionary<string, List<string>> teamList; // Key: Team Name, Value: List of Players in Team

        public EditTeamListForm() {
            InitializeComponent();
            teamList = new Dictionary<string, List<string>>();
        }

        public void OpenWindow(string compName) {
            label_CompetitionName.Text = compName;
            competitionName = compName;
            teamList = MasterWrapper.GetTeamNames(compName);
            foreach (string team in teamList.Keys) {
                listBox_Teams.Items.Add(team);
            }
            this.ShowDialog();
        }

        private void button_AddTeam_Click(object sender, EventArgs e) {
            string newName = Interaction.InputBox("New Team Name:");
            if (string.IsNullOrWhiteSpace(newName)) {
                return;
            }
            else if (teamList.ContainsKey(newName)) {
                MessageBox.Show("Team name already exists in Competition.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else {
                MasterWrapper.AddTeamInDBAndCache(newName);
                teamList.Add(newName, new List<string>());
                listBox_Teams.Items.Add(newName);
            }
        }

        private void button_Edit_Click(object sender, EventArgs e) {
            if (listBox_Teams.SelectedItems.Count > 0) {
                string editedName = Interaction.InputBox("Edit Team Name to:");
                if (string.IsNullOrWhiteSpace(editedName)) {
                    return;
                }
                else if (MasterWrapper.GetTeamID(editedName) != -1) {
                    MessageBox.Show("Team name already exists in Database.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else {
                    string oldName = listBox_Teams.SelectedItem.ToString();
                    MasterWrapper.UpdateTeamNameInDBAndCache(oldName, editedName);
                    var list = teamList[oldName];
                    teamList.Remove(oldName);
                    teamList.Add(editedName, list);
                    int index = listBox_Summoners.Items.IndexOf(oldName);
                    listBox_Teams.Items[index] = editedName;
                }
            }
        }

        // Essentially adds them into the Summoners database forever
        private void button_AddPlayer_Click(object sender, EventArgs e) {
            if (listBox_Teams.SelectedIndices.Count > 0) {
                string teamName = listBox_Teams.SelectedItem.ToString();
                string addSummoner = Interaction.InputBox("Add Summoner:");
                if (teamList[teamName].Contains(addSummoner)) {
                    MessageBox.Show("Summoner already exists in the team!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                string retSummName = MasterWrapper.AddSummonerIntoDBAndCache(competitionName, teamName, addSummoner);
                if (retSummName == null) { 
                    return; 
                }
                else if (addSummoner == retSummName) {
                    // Brand new summoner
                    teamList[teamName].Add(retSummName);
                    listBox_Summoners.Items.Add(retSummName);
                }
                else {
                    // Summoner with edited name
                    teamList[teamName].Remove(retSummName);
                    teamList[teamName].Add(addSummoner);
                    int index = listBox_Summoners.Items.IndexOf(retSummName);
                    listBox_Summoners.Items[index] = addSummoner;
                    MessageBox.Show("Summoner \"" + retSummName + "\" changed name to \"" + addSummoner + "\"", "Name Change", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else {
                MessageBox.Show("Select a Team.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button_RemovePlayer_Click(object sender, EventArgs e) {
            if (listBox_Teams.SelectedIndices.Count > 0 && listBox_Summoners.SelectedIndices.Count > 0) {
                string teamName = listBox_Teams.SelectedItem.ToString();
                string summName = listBox_Summoners.SelectedItem.ToString();
                if (!MasterWrapper.IsSummonerInPlayerStats(summName, teamName)) {
                    MasterWrapper.RemoveSummonerFromCompetition(summName, competitionName);
                    teamList[teamName].Remove(summName);
                    listBox_Summoners.Items.RemoveAt(listBox_Summoners.SelectedIndex);
                }
                else {
                    MessageBox.Show("Cannot remove. Summoner has a Stat attached to this Team.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else {
                MessageBox.Show("Select a Team and/or Summoner.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void listBox_Teams_SelectedIndexChanged(object sender, EventArgs e) {
            if (listBox_Teams.SelectedItems.Count > 0) {
                string name = listBox_Teams.SelectedItem.ToString();

                if (teamList.ContainsKey(name)) {
                    listBox_Summoners.Items.Clear();
                    var summList = teamList[name];
                    foreach (string summ in summList) {
                        listBox_Summoners.Items.Add(summ);
                    }
                }
            }
        }
    }
}
