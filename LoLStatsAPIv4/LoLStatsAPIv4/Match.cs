using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoLStatsAPIv4 {
    class Match {

        public int MatchID { get; set; }
        public Team BlueTeam { get; set; }  // ID: 100
        public Team RedTeam { get; set; }   // ID: 200
        public int CompetitionID { get; set; }
        public int Minutes { get; set; }
        public int Seconds { get; set; }
        public string Patch { get; set; }
        public TeamSide TeamWin { get; set; }
        public List<Champ> BlueTeamBans { get; set; } // Pick order should be chronological
        public List<Champ> RedTeamBans { get; set; }

        // Ctor
        public Match() {

        }



    }
}
