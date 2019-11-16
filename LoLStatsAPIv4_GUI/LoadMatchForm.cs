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
    public partial class LoadMatchForm : Form {

        private bool pressed;
        private long matchId;

        public LoadMatchForm() {
            InitializeComponent();
            pressed = false;
            matchId = 0;
        }

        public string OpenWindow(string compName) {
            var teamNamesArr = MasterWrapper.GetTeamNames(compName).Keys.ToArray();
            comboBox_BlueTeamName.Items.AddRange(teamNamesArr);
            comboBox_RedTeamName.Items.AddRange(teamNamesArr);
            this.ShowDialog();
            if (pressed) {
                return MasterWrapper.LoadNewMatchStatsIntoDB(matchId, compName, comboBox_BlueTeamName.Text, comboBox_RedTeamName.Text);
            }
            return null;
        }

        private void button_OK_Click(object sender, EventArgs e) {
            // Validate inputs
            matchId = 0;
            if (!long.TryParse(textBox_MatchId.Text, out matchId)) {
                MessageBox.Show("Match ID is not a valid number!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (comboBox_BlueTeamName.Text == comboBox_RedTeamName.Text) {
                MessageBox.Show("Team names are the same!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (MasterWrapper.IsMatchIDInCache(textBox_MatchId.Text)) {
                MessageBox.Show("Match ID is already loaded in Competition", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else {
                pressed = true;
                this.Close();
            }
        }
    }
}
