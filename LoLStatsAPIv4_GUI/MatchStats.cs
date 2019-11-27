using System;
using System.Collections.Generic;
using RiotSharp.Endpoints.MatchEndpoint;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoLStatsAPIv4_GUI {
    public class MatchStats {

        // Private Members
        private TimeSpan Duration; // Will be done in seconds
        private string CompetitionName;

        #region Database Columns

        public int CompetitionID { get; private set; }
        public long MatchID { get; private set; }
        public int GetDurationSeconds() {
            return (int)Duration.TotalSeconds;
        }
        public DateTime MatchCreation { get; private set; }
        private string Patch;
        public string GetPatch() {
            var patchArr = Patch.Split('.');
            return patchArr[0] + "." + patchArr[1];
        }
        public Team BlueTeam { get; private set; }  // ID: 100
        public Team RedTeam { get; private set; }   // ID: 200

        #endregion

        // Ctor
        public MatchStats(int compID, string compName) {
            CompetitionID = compID;
            CompetitionName = compName;
            BlueTeam = new Team(MasterWrapper.BLUE_ID);
            RedTeam = new Team(MasterWrapper.RED_ID);
        }

        // Initialize New Match
        public bool InitializeClassWithWindow(Match matchObj, MatchTimeline timelineObj, int blueTeamId, int redTeamId,
            Dictionary<Role, Tuple<string, int>> blueTeamDict = null, Dictionary<Role, Tuple<string, int>> redTeamDict = null) {

            // First initialize Match Instance
            InitializeMatchInstance(matchObj, timelineObj, blueTeamId, redTeamId, blueTeamDict, redTeamDict);

            // Open Separate Window to finalize the Roles
            using (var editSummForm = new EditSummonersForm()) {
                var resultTeam = editSummForm.OpenWindow(CompetitionName, BlueTeam, RedTeam);
                if (resultTeam == null) { return false; }
                BlueTeam.Players = resultTeam[MasterWrapper.BLUE_ID];
                RedTeam.Players = resultTeam[MasterWrapper.RED_ID];
                // After roles finalized, now set the Player Diffs
                BlueTeam.SetPlayerDiffs(RedTeam.Players);
                RedTeam.SetPlayerDiffs(BlueTeam.Players);
            }
            return true;
        }

        // Primarily for bugfixes and if we do not care about Summoner assignments
        // WARNING: Diffs will NOT be accurate. This is mostly used for bugfixing
        public void InitializeClassWithoutWindow(Match matchObj, MatchTimeline timelineObj, int blueTeamId, int redTeamId,
            Dictionary<Role, Tuple<string, int>> blueTeamDict = null, Dictionary<Role, Tuple<string, int>> redTeamDict = null) {

            InitializeMatchInstance(matchObj, timelineObj, blueTeamId, redTeamId, blueTeamDict, redTeamDict);
        }

        // Helper function for both InitializeClassWith or WithoutWindow
        private void InitializeMatchInstance(Match matchObject, MatchTimeline timelineObject, int blueTeamId, int redTeamId,
            Dictionary<Role, Tuple<string, int>> blueTeamDict, Dictionary<Role, Tuple<string, int>> redTeamDict) {
            MatchID = matchObject.GameId;
            Duration = matchObject.GameDuration;
            MatchCreation = matchObject.GameCreation;
            Patch = matchObject.GameVersion;

            foreach (var teamObject in matchObject.Teams) {
                if (teamObject.TeamId == MasterWrapper.BLUE_ID) {
                    BlueTeam.InitializeClass(teamObject, blueTeamId, GetDurationSeconds(), GetPatch());
                }
                else {
                    RedTeam.InitializeClass(teamObject, redTeamId, GetDurationSeconds(), GetPatch());
                }
            }
            foreach (var playerObject in matchObject.Participants) {
                if (playerObject.TeamId == MasterWrapper.BLUE_ID) {
                    BlueTeam.AddPlayer(playerObject, timelineObject.Frames);
                }
                else {
                    RedTeam.AddPlayer(playerObject, timelineObject.Frames);
                }
            }
            foreach (var matchFrame in timelineObject.Frames) {
                foreach (var matchEvent in matchFrame.Events) {
                    if (BlueTeam.ParticipantIds.Contains(matchEvent.KillerId.ToString())) {
                        BlueTeam.AddObjective(matchEvent);
                    }
                    else {
                        RedTeam.AddObjective(matchEvent);
                    }
                }
            }
            // Set Team Diff specifics
            BlueTeam.SetObjectiveDiffs(RedTeam.Objectives);
            RedTeam.SetObjectiveDiffs(BlueTeam.Objectives);
            // Set Baron Power Play
            BlueTeam.UpdateBaronPowerPlay(timelineObject.Frames, RedTeam.ParticipantIds);
            RedTeam.UpdateBaronPowerPlay(timelineObject.Frames, BlueTeam.ParticipantIds);
            // Lastly, if editing a Match, initialize with champID and summonerID
            BlueTeam.SetPlayerIDs(blueTeamDict);
            RedTeam.SetPlayerIDs(redTeamDict);
        }
    }
}
