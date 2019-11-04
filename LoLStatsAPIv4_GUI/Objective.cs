using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RiotSharp.Endpoints.MatchEndpoint;
using RiotSharp.Endpoints.MatchEndpoint.Enums;

namespace LoLStatsAPIv4_GUI {
    public class Objective {

        public ObjectiveEvent Event { get; set; }
        public ObjectiveType Type { get; set; }
        public TimeSpan Timestamp { get; set; }
        public LaneType? Lane { get; set; } // Only applicable to Tower/Inhibitor, null on everything else
        public int BaronPowerPlay { get; set; } // Only applicable to Baron
        // Baron Buff lasts for 3.5 minutes
        // If game ends with Baron buff, there is no Power Play.
        // 0 will indicate do not Insert value into Database

        // Default Ctor
        public Objective() { }

        // Init Ctor
        public Objective(ObjectiveEvent _event, ObjectiveType _type, TimeSpan _ts, LaneType? _lane, int _bpp) {
            Event = _event;
            Type = _type;
            Timestamp = _ts;
            Lane = _lane;
            BaronPowerPlay = _bpp;
        }
    }
}
