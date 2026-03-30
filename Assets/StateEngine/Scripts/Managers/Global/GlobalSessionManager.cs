using StateEngine.Managers.Shared;
using StateEngine.Runtime;
using System.Collections.Generic;
using UnityEngine;

namespace StateEngine.Managers.Global {
    public sealed class GlobalSessionManager : ISessionManager {
        // TODO: from previous impl => keep? (could use in 'init')
        //[SerializeField] private bool _sessionOpen;
        //public bool SessionOpen => _sessionOpen;
        /*
        public void OpenSession() {
            _sessionOpen = true;
        }
        public void CloseSession() {
            _sessionOpen = false;
        }
        */

        // TODO: move to base class... (?)
        [field: SerializeField] public TextAsset XmlGlobalData { get; private set; }

        public void Init() {
            // TODO: OK?
            SetFile("OPTIONS");

            if (!Load()) {
                LoadDefaults();
                Save();
            }
        }

        public void LoadDefaults() {
            ClearInMemory();
            SetFields(DifficultyData.LoadGlobalDefaults(XmlGlobalData), true);
        }

        public Dictionary<string, int> GetGlobalFields() {
            return GetAllFields();
        }

        public IEnumerable<string> GetGlobalFieldNames() {
            return GetAllFieldNames();
        }

        public bool HasGlobalField(string name) {
            return HasField(name);
        }

        public int GetGlobalField(string name, int defaultValue = 0) {
            return GetField(name, defaultValue);
        }

        public void SetGlobalField(string name, int value) {
            SetField(name, value);
        }

        public void SetGlobalFields(Dictionary<string, int> values, bool replaceExisting = true) {
            SetFields(values, replaceExisting);
        }
    }
}
