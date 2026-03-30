using System.Collections.Generic;

namespace StateEngine.Runtime {
    public static class RuntimeFieldListUtility {

        public static Dictionary<string, int> ToDictionary(List<RuntimeIntFieldEntry> fields) {
            Dictionary<string, int> values = new Dictionary<string, int>();
            if (fields == null) {
                return values;
            }

            foreach (RuntimeIntFieldEntry entry in fields) {
                if (entry == null || string.IsNullOrEmpty(entry.Key)) {
                    continue;
                }
                values[entry.Key] = entry.Value;
            }

            return values;
        }

        public static List<RuntimeIntFieldEntry> FromDictionary(Dictionary<string, int> values) {
            List<RuntimeIntFieldEntry> entries = new List<RuntimeIntFieldEntry>();
            if (values == null) {
                return entries;
            }

            foreach (KeyValuePair<string, int> pair in values) {
                entries.Add(new RuntimeIntFieldEntry(pair.Key, pair.Value));
            }

            return entries;
        }

        public static bool HasKey(List<RuntimeIntFieldEntry> entries, string key) {
            return IndexOf(entries, key) != -1;
        }

        public static int GetValue(List<RuntimeIntFieldEntry> entries, string key, int defaultValue = 0) {
            int index = IndexOf(entries, key);
            if (index == -1) {
                return defaultValue;
            }
            return entries[index].Value;
        }

        public static void SetValue(List<RuntimeIntFieldEntry> entries, string key, int value) {
            int index = IndexOf(entries, key);
            if (index == -1) {
                entries.Add(new RuntimeIntFieldEntry(key, value));
                return;
            }

            entries[index].Value = value;
        }

        public static void RemoveKey(List<RuntimeIntFieldEntry> entries, string key) {
            int index = IndexOf(entries, key);
            if (index != -1) {
                entries.RemoveAt(index);
            }
        }

        public static void SetFromDictionary(List<RuntimeIntFieldEntry> entries, Dictionary<string, int> values, bool clearFirst = true) {
            if (entries == null) {
                return;
            }

            if (clearFirst) {
                entries.Clear();
            }

            if (values == null) {
                return;
            }

            foreach (KeyValuePair<string, int> pair in values) {
                entries.Add(new RuntimeIntFieldEntry(pair.Key, pair.Value));
            }
        }

        private static int IndexOf(List<RuntimeIntFieldEntry> entries, string key) {
            if (entries == null || string.IsNullOrEmpty(key)) {
                return -1;
            }

            for (int i = 0; i < entries.Count; ++i) {
                RuntimeIntFieldEntry entry = entries[i];
                if (entry != null && entry.Key == key) {
                    return i;
                }
            }

            return -1;
        }
    }
}
