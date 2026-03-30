using StateEngine.IO;
using System.Collections.Generic;
using UnityEngine;

namespace StateEngine.Managers.Shared {
    public abstract class ISessionManager : MonoBehaviour {
        private string DATA_FILE;

        protected Dictionary<string, int> fields;


        protected ISessionManager() {
            fields = new Dictionary<string, int>();
        }

        protected void SetFile(string filename) {
            DATA_FILE = filename;
        }

        public void Clear() {
            fields.Clear();
            //TODO: should be saved somewhere else (crypted & inaccessible)!
            FileManager.Delete(DATA_FILE);
        }

        protected void ClearInMemory() {
            fields.Clear();
        }

        public bool HasField(string name) {
            return fields.ContainsKey(name);
        }

        public bool RemoveField(string name) {
            return fields.Remove(name);
        }

        public Dictionary<string, int> GetAllFields() {
            return new Dictionary<string, int>(fields);
        }

        public IEnumerable<string> GetAllFieldNames() {
            return new List<string>(fields.Keys);
        }

        public void SetFields(Dictionary<string, int> values, bool clearFirst = false) {
            if (clearFirst) {
                fields.Clear();
            }

            if (values == null) {
                return;
            }

            foreach (KeyValuePair<string, int> pair in values) {
                fields[pair.Key] = pair.Value;
            }
        }

        public int GetField(string name, int defaultValue) {
            if (fields.TryGetValue(name, out int value)) {
                return value;
            }
            return defaultValue;
        }

        public int GetField(string name) {
            return GetField(name, 0);
        }

        public void SetField(string name, int value) {
            fields[name] = value;
        }

        public void Save() {
            //TODO: should be saved somewhere else (crypted & inaccessible)!
            FileManager.Save(DATA_FILE, fields);
        }

        public bool Load() {
            fields.Clear();
            //TODO: should be saved somewhere else (crypted & inaccessible)!
            return FileManager.Load(DATA_FILE, fields);
        }
    }
}
