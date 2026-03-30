using System;

namespace StateEngine.Runtime {
    [Serializable]
    public class RuntimeIntFieldEntry {
        public string Key;
        public int Value;

        public RuntimeIntFieldEntry() {
        }

        public RuntimeIntFieldEntry(string key, int value) {
            Key = key;
            Value = value;
        }
    }
}
