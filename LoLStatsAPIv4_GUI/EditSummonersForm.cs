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
    public partial class EditSummonersForm : Form {

        private bool ButtonPressed;
        private Dictionary<string, Player> BluePlayerDict;  // Champ Name to Player object
        private Dictionary<string, Player> RedPlayerDict;   // Champ Name to Player object
        private Dictionary<Role, ComboBox> BlueChampDict;
        private Dictionary<Role, ComboBox> RedChampDict;
        private Dictionary<Role, ComboBox> BlueSummDict;
        private Dictionary<Role, ComboBox> RedSummDict;

        public EditSummonersForm() {
            InitializeComponent();
            ButtonPressed = false;
            BluePlayerDict = new Dictionary<string, Player>();
            RedPlayerDict = new Dictionary<string, Player>();
            BlueChampDict = new Dictionary<Role, ComboBox>() {
                { Role.TOP, comboBox_BlueChampTop },
                { Role.JUNGLE, comboBox_BlueChampJg },
                { Role.MIDDLE, comboBox_BlueChampMid },
                { Role.BOTTOM, comboBox_BlueChampBot },
                { Role.SUPPORT, comboBox_BlueChampSupp }
            };
            RedChampDict = new Dictionary<Role, ComboBox>() {
                { Role.TOP, comboBox_RedChampTop },
                { Role.JUNGLE, comboBox_RedChampJg },
                { Role.MIDDLE, comboBox_RedChampMid },
                { Role.BOTTOM, comboBox_RedChampBot },
                { Role.SUPPORT, comboBox_RedChampSupp }
            };
            BlueSummDict = new Dictionary<Role, ComboBox>() {
                { Role.TOP, comboBox_BlueSummTop },
                { Role.JUNGLE, comboBox_BlueSummJg },
                { Role.MIDDLE, comboBox_BlueSummMid },
                { Role.BOTTOM, comboBox_BlueSummBot },
                { Role.SUPPORT, comboBox_BlueSummSupp }
            };
            RedSummDict = new Dictionary<Role, ComboBox>() {
                { Role.TOP, comboBox_RedSummTop },
                { Role.JUNGLE, comboBox_RedSummJg },
                { Role.MIDDLE, comboBox_RedSummMid },
                { Role.BOTTOM, comboBox_RedSummBot },
                { Role.SUPPORT, comboBox_RedSummSupp }
            };
        }

        public Dictionary<int, PlayerList> OpenWindow(string compName, Team blueTeam, Team redTeam) {
            groupBox_BlueTeam.Text = "BLUE TEAM [" + MasterWrapper.GetTeamName(blueTeam.TeamId) + "]";
            groupBox_RedTeam.Text = "RED TEAM [" + MasterWrapper.GetTeamName(redTeam.TeamId) + "]";
            var blueTeamPlayers = blueTeam.Players;
            var redTeamPlayers = redTeam.Players;
            // 
            foreach (Player player in blueTeamPlayers) {
                string champName = MasterWrapper.GetChampName(player.ChampId);
                BluePlayerDict.Add(champName, player);
            }
            foreach (Player player in redTeamPlayers) {
                string champName = MasterWrapper.GetChampName(player.ChampId);
                RedPlayerDict.Add(champName, player);
            }

            var teamList = MasterWrapper.GetTeamNames(compName);
            foreach (ComboBox cb in BlueChampDict.Values) {
                cb.Items.AddRange(blueTeamPlayers.GetChampionsList().ToArray());
            }
            foreach (ComboBox cb in RedChampDict.Values) {
                cb.Items.AddRange(redTeamPlayers.GetChampionsList().ToArray());
            }
            string blueTeamName = MasterWrapper.GetTeamName(blueTeam.TeamId);
            foreach (ComboBox cb in BlueSummDict.Values) {
                cb.Items.Add("");
                cb.Items.AddRange(teamList[blueTeamName].ToArray());
            }
            string redTeamName = MasterWrapper.GetTeamName(redTeam.TeamId);
            foreach (ComboBox cb in RedSummDict.Values) {
                cb.Items.Add("");
                cb.Items.AddRange(teamList[redTeamName].ToArray());
            }

            foreach (Role role in BlueChampDict.Keys) {
                BlueChampDict[role].Text = MasterWrapper.GetChampName(blueTeamPlayers[role].ChampId);
            }
            foreach (Role role in RedChampDict.Keys) {
                RedChampDict[role].Text = MasterWrapper.GetChampName(redTeamPlayers[role].ChampId);
            }
            foreach (Role role in BlueSummDict.Keys) {
                BlueSummDict[role].Text = MasterWrapper.GetSummonerName(blueTeamPlayers[role].SummonerId);
            }
            foreach (Role role in RedSummDict.Keys) {
                RedSummDict[role].Text = MasterWrapper.GetSummonerName(redTeamPlayers[role].SummonerId);
            }

            richTextBox_BlueWarning.Text = WarningMessage(blueTeam.Players.GetUnassignedRoles());
            richTextBox_RedWarning.Text = WarningMessage(redTeam.Players.GetUnassignedRoles());

            this.ShowDialog();
            if (ButtonPressed) {
                var output = new Dictionary<int, PlayerList>();
                foreach (Role role in BlueChampDict.Keys) {
                    string champName = BlueChampDict[role].Text;
                    var bluePlayer = BluePlayerDict[champName];
                    bluePlayer.SummonerId = MasterWrapper.GetSummonerID(BlueSummDict[role].Text);
                    bluePlayer.Role = role;
                    blueTeamPlayers[role] = bluePlayer;
                }
                output.Add(MasterWrapper.BLUE_ID, blueTeamPlayers);
                foreach (Role role in RedChampDict.Keys) {
                    string champName = RedChampDict[role].Text;
                    var redPlayer = RedPlayerDict[champName];
                    redPlayer.SummonerId = MasterWrapper.GetSummonerID(RedSummDict[role].Text);
                    redPlayer.Role = role;
                    redTeamPlayers[role] = redPlayer;
                }
                output.Add(MasterWrapper.RED_ID, redTeamPlayers);
                return output;
            }
            return null;
        }

        private string WarningMessage(List<Role> roleList) {
            if (roleList.Count == 0) { return "Roles from API appear to be Correct!"; }
            var sb = new StringBuilder();
            sb.Append("WARNING: Check ");
            foreach (Role role in roleList) {
                sb.Append(role.ToString() + ", ");
            }
            return sb.ToString().TrimEnd(',', ' ');
        }

        private void button_Save_Click(object sender, EventArgs e) {
            var errorList = new List<string>();
            foreach (Role role in BlueSummDict.Keys) {
                ComboBox tb = BlueSummDict[role];
                if (string.IsNullOrWhiteSpace(MasterWrapper.GetSummonerID(tb.Text))) {
                    errorList.Add("BLUE " + role.ToString());
                }
            }
            foreach (Role role in RedSummDict.Keys) {
                ComboBox tb = RedSummDict[role];
                if (string.IsNullOrWhiteSpace(MasterWrapper.GetSummonerID(tb.Text))) {
                    errorList.Add("RED " + role.ToString());
                }
            }

            if (errorList.Count > 0) {
                var sb = new StringBuilder();
                sb.AppendLine("The following positions do not have summoner names in the Database:");
                foreach (string error in errorList) {
                    sb.AppendLine("- " + error);
                }
                sb.AppendLine("Still proceed?");
                if (MessageBox.Show(sb.ToString(), "Error", MessageBoxButtons.YesNo, MessageBoxIcon.Error) == DialogResult.No) {
                    return;
                }
            }
            ButtonPressed = true;
            this.Close();
        }
    }
}
