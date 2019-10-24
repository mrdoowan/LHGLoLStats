using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Microsoft.VisualBasic;

namespace LoLStatsAPIv4_GUI {
    public partial class EditTeamListForm : Form {

        private bool ButtonPressed;
        private HashSet<string> OriginalTeamSet;
        private HashSet<string> RemoveTeamSet;      // Unique names being Removed
        private HashSet<string> AddTeamSet;         // Unique names newly Added
        private Dictionary<string, string> EditDict; // Key: New Name, Value: Former Name

        public EditTeamListForm() {
            InitializeComponent();
            ButtonPressed = false;
            OriginalTeamSet = new HashSet<string>();
            RemoveTeamSet = new HashSet<string>();
            AddTeamSet = new HashSet<string>();
            EditDict = new Dictionary<string, string>();
        }

        public void OpenWindow(string compName) {
            label_CompetitionName.Text = compName;
            OriginalTeamSet = MasterWrapper.GetTeamNames(compName).ToHashSet();
            foreach (string team in OriginalTeamSet) {
                listBox_Teams.Items.Add(team);
            }
            this.ShowDialog();
            if (ButtonPressed) {
                // Once confirmed, then do all the query commands
                foreach (string teamName in RemoveTeamSet) {
                    MasterWrapper.RemoveTeamInTable(teamName);
                }
                foreach (string teamName in AddTeamSet) {
                    MasterWrapper.AddTeamInTable(teamName, compName);
                }
                foreach (string newName in EditDict.Keys) {
                    MasterWrapper.UpdateTeamNameInTable(EditDict[newName], newName);
                }
            }
        }

        private void button_AddTeam_Click(object sender, EventArgs e) {
            string name = Interaction.InputBox("New Team Name:");
            if (OriginalTeamSet.Contains(name) && !RemoveTeamSet.Contains(name)) {
                MessageBox.Show("Team name already exists.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else {
                if (OriginalTeamSet.Contains(name)) {
                    // Implies re-adding the Team back to the original
                    AddTeamSet.Add(name);
                }
                RemoveTeamSet.Remove(name);
            }
        }

        private void button_Edit_Click(object sender, EventArgs e) {
            if (listBox_Teams.SelectedItems.Count > 0) {
                string newName = Interaction.InputBox("Edit Team Name to:");
                if (!string.IsNullOrWhiteSpace(newName)) {
                    string oldName = listBox_Teams.SelectedItem.ToString();
                    if (OriginalTeamSet.Contains(oldName)) {
                        EditDict.Add(newName, oldName);
                    }
                    else if (EditDict.ContainsKey(oldName)) {
                        // The same team was edited twice
                        string ogName = EditDict[oldName];
                        EditDict.Remove(oldName);
                        EditDict.Add(newName, ogName);
                    }
                    else {
                        // It's a newly added Team that suddenly wanted to change names
                        AddTeamSet.Remove(oldName);
                        AddTeamSet.Add(newName);
                    }
                    listBox_Teams.SelectedItem = newName;
                }
            }
        }

        // A team cannot be removed if they have Matches attached already, OR if name was edited
        private void button_RemoveTeam_Click(object sender, EventArgs e) {
            if (listBox_Teams.SelectedItems.Count > 0) {
                string name = listBox_Teams.SelectedItem.ToString();
                if (EditDict.ContainsKey(name)) {
                    MessageBox.Show("You cannot remove a team whose name has been Edited.",
                        "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else if (!MasterWrapper.IsTeamInMatchesTable(name)) {
                    RemoveTeamSet.Add(name);
                    AddTeamSet.Remove(name);
                    listBox_Teams.Items.RemoveAt(listBox_Teams.SelectedIndex);
                }
                else {
                    MessageBox.Show("This team already has stats attached to its name. Please edit the team name instead.",
                        "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void button_Confirm_Click(object sender, EventArgs e) {
            ButtonPressed = true;
            this.Close();
        }

    }
}
