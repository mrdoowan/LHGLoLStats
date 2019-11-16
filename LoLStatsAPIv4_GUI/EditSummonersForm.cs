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
        private Dictionary<Role, ComboBox> BlueCBChampDict;
        private Dictionary<Role, ComboBox> RedCBChampDict;
        private Dictionary<Role, ComboBox> BlueCBSummDict;
        private Dictionary<Role, ComboBox> RedCBSummDict;
        private string competitionName;
        private string blueTeamName;
        private string redTeamName;

        public EditSummonersForm() {
            InitializeComponent();
            ButtonPressed = false;
            BluePlayerDict = new Dictionary<string, Player>();
            RedPlayerDict = new Dictionary<string, Player>();
            BlueCBChampDict = new Dictionary<Role, ComboBox>() {
                { Role.TOP, comboBox_BlueChampTop },
                { Role.JUNGLE, comboBox_BlueChampJg },
                { Role.MIDDLE, comboBox_BlueChampMid },
                { Role.BOTTOM, comboBox_BlueChampBot },
                { Role.SUPPORT, comboBox_BlueChampSupp }
            };
            RedCBChampDict = new Dictionary<Role, ComboBox>() {
                { Role.TOP, comboBox_RedChampTop },
                { Role.JUNGLE, comboBox_RedChampJg },
                { Role.MIDDLE, comboBox_RedChampMid },
                { Role.BOTTOM, comboBox_RedChampBot },
                { Role.SUPPORT, comboBox_RedChampSupp }
            };
            BlueCBSummDict = new Dictionary<Role, ComboBox>() {
                { Role.TOP, comboBox_BlueSummTop },
                { Role.JUNGLE, comboBox_BlueSummJg },
                { Role.MIDDLE, comboBox_BlueSummMid },
                { Role.BOTTOM, comboBox_BlueSummBot },
                { Role.SUPPORT, comboBox_BlueSummSupp }
            };
            RedCBSummDict = new Dictionary<Role, ComboBox>() {
                { Role.TOP, comboBox_RedSummTop },
                { Role.JUNGLE, comboBox_RedSummJg },
                { Role.MIDDLE, comboBox_RedSummMid },
                { Role.BOTTOM, comboBox_RedSummBot },
                { Role.SUPPORT, comboBox_RedSummSupp }
            };
        }

        public Dictionary<int, PlayerList> OpenWindow(string compName, Team blueTeam, Team redTeam) {
            competitionName = compName;
            blueTeamName = MasterWrapper.GetTeamName(blueTeam.TeamId);
            redTeamName = MasterWrapper.GetTeamName(redTeam.TeamId);
            groupBox_BlueTeam.Text = "BLUE TEAM [" + blueTeamName + "]";
            groupBox_RedTeam.Text = "RED TEAM [" + redTeamName + "]";
            var blueTeamPlayers = blueTeam.Players;
            var redTeamPlayers = redTeam.Players;
            
            // Initializes Player dictionary from ChampName -> Player Object
            foreach (Player player in blueTeamPlayers) {
                string champName = MasterWrapper.GetChampName(player.ChampId);
                BluePlayerDict.Add(champName, player);
            }
            foreach (Player player in redTeamPlayers) {
                string champName = MasterWrapper.GetChampName(player.ChampId);
                RedPlayerDict.Add(champName, player);
            }
            // Add all champ names into Combobox Items
            foreach (ComboBox cb in BlueCBChampDict.Values) {
                cb.Items.AddRange(blueTeamPlayers.GetChampionsList().ToArray());
            }
            foreach (ComboBox cb in RedCBChampDict.Values) {
                cb.Items.AddRange(redTeamPlayers.GetChampionsList().ToArray());
            }
            FillSummonerNames();

            foreach (Role role in BlueCBChampDict.Keys) {
                BlueCBChampDict[role].Text = MasterWrapper.GetChampName(blueTeamPlayers[role].ChampId);
            }
            foreach (Role role in RedCBChampDict.Keys) {
                RedCBChampDict[role].Text = MasterWrapper.GetChampName(redTeamPlayers[role].ChampId);
            }
            foreach (Role role in BlueCBSummDict.Keys) {
                BlueCBSummDict[role].Text = MasterWrapper.GetSummonerName(blueTeamPlayers[role].SummonerId);
            }
            foreach (Role role in RedCBSummDict.Keys) {
                RedCBSummDict[role].Text = MasterWrapper.GetSummonerName(redTeamPlayers[role].SummonerId);
            }

            richTextBox_BlueWarning.Text = WarningMessage(blueTeam.Players.GetUnassignedRoles());
            richTextBox_RedWarning.Text = WarningMessage(redTeam.Players.GetUnassignedRoles());

            this.ShowDialog();
            if (ButtonPressed) {
                var output = new Dictionary<int, PlayerList>();
                foreach (Role role in BlueCBChampDict.Keys) {
                    string champName = BlueCBChampDict[role].Text;
                    var bluePlayer = BluePlayerDict[champName];
                    bluePlayer.SummonerId = MasterWrapper.GetSummonerID(BlueCBSummDict[role].Text);
                    bluePlayer.Role = role;
                    blueTeamPlayers[role] = bluePlayer;
                }
                output.Add(MasterWrapper.BLUE_ID, blueTeamPlayers);
                foreach (Role role in RedCBChampDict.Keys) {
                    string champName = RedCBChampDict[role].Text;
                    var redPlayer = RedPlayerDict[champName];
                    redPlayer.SummonerId = MasterWrapper.GetSummonerID(RedCBSummDict[role].Text);
                    redPlayer.Role = role;
                    redTeamPlayers[role] = redPlayer;
                }
                output.Add(MasterWrapper.RED_ID, redTeamPlayers);
                return output;
            }
            return null;
        }

        private void FillSummonerNames() {
            var teamList = MasterWrapper.GetTeamNames(competitionName);
            foreach (ComboBox cb in BlueCBSummDict.Values) {
                string currText = cb.Text;
                cb.Items.Clear();
                cb.Items.Add("");
                cb.Items.AddRange(teamList[blueTeamName].ToArray());
                cb.Text = currText;
            }
            foreach (ComboBox cb in RedCBSummDict.Values) {
                string currText = cb.Text;
                cb.Items.Clear();
                cb.Items.Add("");
                cb.Items.AddRange(teamList[redTeamName].ToArray());
                cb.Text = currText;
            }
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
            foreach (Role role in BlueCBSummDict.Keys) {
                ComboBox cbSumm = BlueCBSummDict[role];
                ComboBox cbChamp = BlueCBChampDict[role];
                if (string.IsNullOrWhiteSpace(MasterWrapper.GetSummonerID(cbSumm.Text))) {
                    errorList.Add("BLUE " + role.ToString());
                }
            }
            foreach (Role role in RedCBSummDict.Keys) {
                ComboBox cbSumm = RedCBSummDict[role];
                if (string.IsNullOrWhiteSpace(MasterWrapper.GetSummonerID(cbSumm.Text))) {
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

        private void button_EditNames_Click(object sender, EventArgs e) {
            using (var form = new EditTeamListForm()) {
                form.OpenWindow(competitionName);
            }
            FillSummonerNames();
        }
    }
}
