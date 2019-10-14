using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;

namespace LoLStatsAPIv4 {
    public class Objective {

        public ObjectiveEvent Event { get; set; }
        public int Order { get; set; } // What order of Event is this in the game? i.e. 1st Dragon, 2nd Dragon, etc.
        public ObjectiveType Type { get; set; }
        public int Minute { get; set; }
        public int Second { get; set; }
        public LaneMap Lane { get; set; } // Only applicable to Tower/Inhibitor, null on everything else
        public int BaronPowerPlay { get; set; } // Only applicable to Baron, null on everything else

        // Ctor
        public Objective() {

        }


    }
}
