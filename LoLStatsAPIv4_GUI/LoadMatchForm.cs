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

        public bool OpenWindow(string compName) {
            this.ShowDialog();
            if (pressed) {
                return MasterWrapper.LoadMatchStatsIntoDB(compName, textBox_BlueTeamName.Text, textBox_RedTeamName.Text, matchId);
            }
            return false;
        }

        private void button_OK_Click(object sender, EventArgs e) {
            // Validate inputs
            matchId = 0;
            if (long.TryParse(textBox_MatchId.Text, out matchId)) {
                pressed = true;
                this.Close();
            }
            else {
                MessageBox.Show("Match ID is not a valid number!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
