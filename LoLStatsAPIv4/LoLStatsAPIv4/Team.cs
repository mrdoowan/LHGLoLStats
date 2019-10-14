using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;

namespace LoLStatsAPIv4 {
    public class Team {

        // Member Variables: SHOULD ONLY BE EXACTLY FROM API
        public int TeamID { get; set; }
        public List<Player> Players { get; set; }
        public List<Objective> Objectives { get; set; }


        // Ctor
        public Team() {

        }

        // Any non-API database columns needs to be in the form of a public function

    }
}
