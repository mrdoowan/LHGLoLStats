using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;

namespace LoLStatsAPIv4_GUI {
    public class ObjectiveList {

        // Private member variables
        private Dictionary<ObjectiveEvent, int> EventTimes;
        private List<Objective> Objectives;

        #region DB Columns for Team

        public int TowersAt15 { get; set; }
        public int TowersDiff15 { get; set; }
        public int KillsAt15 { get; set; }
        public int KillsDiff15 { get; set; }
        public int TowersAt25 { get; set; }
        public int TowersDiff25 { get; set; }
        public int KillsAt25 { get; set; }
        public int KillsDiff25 { get; set; }

        #endregion

        // Ctor
        public ObjectiveList() {
            EventTimes = new Dictionary<ObjectiveEvent, int>() {
                { ObjectiveEvent.TOWER_DESTROYED, 0 },
                { ObjectiveEvent.DRAGON_KILL, 0 },
                { ObjectiveEvent.BARON_KILL, 0 },
                { ObjectiveEvent.RIFT_HERALD, 0 },
                { ObjectiveEvent.INHIBITOR_DESTROYED, 0 }
            };
            Objectives = new List<Objective>();

        }

        // Index overwrite
        public Objective this[int i] {
            get => Objectives[i];
            set => Objectives[i] = value;
        }

        public void AddObjective() {

        }

        public void SetDiffValues(ObjectiveList oppObjs) {
            TowersDiff15 = TowersAt15 - oppObjs.TowersAt15;
            KillsDiff15 = KillsAt15 - oppObjs.KillsAt15;
            TowersDiff25 = TowersAt25 - oppObjs.TowersAt25;
            KillsDiff25 = KillsAt25 - oppObjs.KillsAt25;
        }
    }
}
