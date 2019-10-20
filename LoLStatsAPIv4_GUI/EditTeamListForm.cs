using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LoLStatsAPIv4_GUI {
    public partial class EditTeamListForm : Form {

        private bool ButtonPressed;
        private HashSet<string> OriginalTeamSet;
        private HashSet<string> RemoveTeamSet;
        private HashSet<string> AddTeamSet;

        public EditTeamListForm() {
            InitializeComponent();
            ButtonPressed = false;
            OriginalTeamSet = new HashSet<string>();
            RemoveTeamSet = new HashSet<string>();
            AddTeamSet = new HashSet<string>();
        }

        public void OpenWindow(string compName) {
            OriginalTeamSet = MasterWrapper.GetTeamNames(compName).ToHashSet();
            this.ShowDialog();
            if (ButtonPressed) {

            }
        }

        private void button_AddTeam_Click(object sender, EventArgs e) {
            string name = textBox_Team.Text;
            if (!string.IsNullOrWhiteSpace(name) && !OriginalTeamSet.Contains(name)) {
                AddTeamSet.Add(name);
                RemoveTeamSet.Remove(name);
            }
        }

        private void button_Edit_Click(object sender, EventArgs e) {

        }

        // A team cannot be removed if they have MatchStats attached
        private void button_RemoveTeam_Click(object sender, EventArgs e) {
            if (listBox_Teams.SelectedItems.Count > 0) {
                // CHECK IF STATEMENT HERE IF MATCH STATS HAS THE TEAM ID
                string name = listBox_Teams.SelectedItem.ToString();
                RemoveTeamSet.Add(name);
                AddTeamSet.Remove(name);
                listBox_Teams.Items.RemoveAt(listBox_Teams.SelectedIndex);
            }
        }

        private void button_Confirm_Click(object sender, EventArgs e) {
            ButtonPressed = true;
            this.Close();
        }

    }
}
