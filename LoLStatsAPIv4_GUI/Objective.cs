using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoLStatsAPIv4_GUI {
    public class Objective {

        public ObjectiveEvent Event { get; set; }
        public int Order { get; set; }
        public ObjectiveType Type { get; set; }
        public int Timestamp { get; set; }
        public LaneMap Lane { get; set; } // Only applicable to Tower/Inhibitor, null on everything else
        public int BaronPowerPlay { get; set; } // Only applicable to Baron, null on everything else

        public Objective() { }

    }
}
