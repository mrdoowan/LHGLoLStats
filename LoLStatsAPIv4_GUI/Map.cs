using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoLStatsAPIv4_GUI {
    // Class for Bidirectional Map
    // From https://stackoverflow.com/questions/10966331/two-way-bidirectional-dictionary-in-c/10966684

    public class Map<T1, T2> {
        private Dictionary<T1, T2> _forward = new Dictionary<T1, T2>();
        private Dictionary<T2, T1> _reverse = new Dictionary<T2, T1>();

        public Map() {
            this.Forward = new Indexer<T1, T2>(_forward);
            this.Reverse = new Indexer<T2, T1>(_reverse);
        }

        public class Indexer<T3, T4> {
            private Dictionary<T3, T4> _dictionary;
            public Indexer(Dictionary<T3, T4> dictionary) {
                _dictionary = dictionary;
            }
            public T4 this[T3 index] {
                get { return _dictionary[index]; }
            }

            public bool ContainsKey(T3 key) {
                return (key == null) ? false : _dictionary.ContainsKey(key);
            }

            public bool RemoveKey(T3 key) {
                return (key == null) ? false : _dictionary.Remove(key);
            }
        }

        public void Add(T1 t1, T2 t2) {
            _forward.Add(t1, t2);
            try { _reverse.Add(t2, t1); }
            catch { _forward.Remove(t1); }
        }

        public bool RemoveForward(T1 t1) {
            if (_forward.ContainsKey(t1)) {
                T2 val = _forward[t1];
                _forward.Remove(t1);
                try { _reverse.Remove(val); }
                catch { _forward.Add(t1, val); }
                return true;
            }
            else {
                return false;
            }
        }

        public bool RemoveBackward(T2 t2) {
            if (_reverse.ContainsKey(t2)) {
                T1 val = _reverse[t2];
                _reverse.Remove(t2);
                try { _forward.Remove(val); }
                catch { _reverse.Add(t2, val); }
                return true;
            }
            else {
                return false;
            }
        }

        public Indexer<T1, T2> Forward { get; private set; }
        public Indexer<T2, T1> Reverse { get; private set; }

    }
}
