using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoLStatsAPIv4_GUI {
    public class Champ {

        public string Name { get; set; }
        public int ID { get; set; }

        // Ctor
        public Champ(int _id) {
            ID = _id;
            Name = MasterWrapper.GetChampName(_id);
        }

    }
}
