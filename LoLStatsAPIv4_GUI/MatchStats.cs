using System;
using System.Collections.Generic;
using RiotSharp.Endpoints.MatchEndpoint;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoLStatsAPIv4_GUI {
    class MatchStats {

        // Private Members
        private TimeSpan Duration; // Will be done in seconds
        private const int BLUEID = 100;
        private const int REDID = 200;

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
        public MatchStats(int compID) {
            CompetitionID = compID;
            BlueTeam = new Team(BLUEID);
            RedTeam = new Team(REDID); 
        }

        // Initialize
        public void InitializeClass(Match matchObj, MatchTimeline timelineObj) {
            MatchID = matchObj.GameId;
            Duration = matchObj.GameDuration;
            MatchCreation = matchObj.GameCreation;
            Patch = matchObj.GameVersion;

            foreach (var teamObj in matchObj.Teams) {
                if (teamObj.TeamId == BLUEID) { BlueTeam.InitializeClass(teamObj, GetDurationSeconds()); }
                else { RedTeam.InitializeClass(teamObj, GetDurationSeconds()); }
            }
            foreach (var playerObj in matchObj.Participants) {
                if (playerObj.TeamId == BLUEID) { BlueTeam.AddPlayer(playerObj, timelineObj.Frames); }
                else { RedTeam.AddPlayer(playerObj, timelineObj.Frames); }
            }
            foreach (var matchFrame in timelineObj.Frames) {
                foreach (var matchEvent in matchFrame.Events) {
                    if (matchEvent.TeamId == BLUEID) { BlueTeam.AddObjective(matchEvent); }
                    else { RedTeam.AddObjective(matchEvent); }
                }
            }
            // Open Separate Window to finalize the Roles

        }

    }
}
