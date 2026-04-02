using System.Collections.Generic;

namespace StateEngine.Graphs {
    public class StateIds {
        public int NONE { get; private set; } = 0;

        private List<string> states = new List<string>();


        public void Reset() {
            states.Clear();
            NONE = 0;
        }

        public void Add(string stateId) {
            states.Add(stateId);
            ++NONE; //or: NONE = states.Count;
        }

        public int Index(string stateId) {
            return states.IndexOf(stateId);
        }

        public string Name(int index) {
            return states[index];
        }
    }
}
