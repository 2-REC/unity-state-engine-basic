using StateEngine.Managers.Shared;
using StateEngine.Runtime;
using UnityEngine;

namespace StateEngine.Managers.Global {
    public class GlobalDataManager : IDataManager {
        // TODO: move to base class and replace with 'ISessionManager'?
        [SerializeField] private GlobalSessionManager globalSessionManager;

        [Header("Runtime State")]
        [SerializeField] private RuntimeGlobalStateAsset runtimeStateTemplate;
        [SerializeField] private GlobalStateScenarioAsset startupScenario;
        [SerializeField] private ScenarioLoadMode scenarioLoadMode = ScenarioLoadMode.None;

        protected RuntimeGlobalStateAsset runtimeState;

        // TODO" from previous impl => keep?
        //[SerializeField] private int _profileId;
        //public int ProfileId => _profileId;
        /*
        public void SetProfileId(int profileId) {
            _profileId = profileId;
        }
        */

        protected override void LoadData() {
            // TODO: could check that have a session manager
            // make sure the session loaded first
            globalSessionManager.Init();

            runtimeState = CreateRuntimeStateInstance();

            if (scenarioLoadMode == ScenarioLoadMode.Detached && startupScenario != null) {
                BuildRuntimeStateFromScenario(startupScenario);
            } else {
                PopulateRuntimeStateFromSession();
                if (startupScenario != null && scenarioLoadMode == ScenarioLoadMode.Overlay) {
                    ApplyScenarioOverlay(startupScenario);
                }
            }
        }

        public override void CommitChanges() {
            if (ShouldSkipPersistence()) {
                return;
            }

            CommitRuntimeStateToSession();
            globalSessionManager.Save();
        }

        protected virtual RuntimeGlobalStateAsset CreateRuntimeStateInstance() {
            RuntimeGlobalStateAsset instance = null;
            if (runtimeStateTemplate != null) {
                instance = Instantiate(runtimeStateTemplate);
                instance.name = runtimeStateTemplate.name + "_Runtime";
                instance.ClearState();
            } else {
                instance = ScriptableObject.CreateInstance<RuntimeGlobalStateAsset>();
                instance.name = "RuntimeGlobalState_Runtime";
            }
            return instance;
        }

        protected virtual void PopulateRuntimeStateFromSession() {
            runtimeState.ClearState();
            runtimeState.SetFieldsFromDictionary(globalSessionManager.GetGlobalFields());
        }

        protected virtual void CommitRuntimeStateToSession() {
            globalSessionManager.SetGlobalFields(runtimeState.GetFieldsDictionary(), true);
        }

        protected virtual void ApplyScenarioOverlay(GlobalStateScenarioAsset scenario) {
            if (scenario == null) {
                return;
            }

            if (scenario.ReplaceRuntimeState) {
                runtimeState.ClearState();
            }

            var values = RuntimeFieldListUtility.ToDictionary(scenario.FieldOverrides);
            foreach (var pair in values) {
                runtimeState.SetField(pair.Key, pair.Value);
            }
        }

        protected virtual void BuildRuntimeStateFromScenario(GlobalStateScenarioAsset scenario) {
            runtimeState.ClearState();

            if (scenario == null) {
                return;
            }

            if (scenario.FillMissingFromBaseState) {
                runtimeState.SetFieldsFromDictionary(
                    // TODO: make method in session manager reading+returning the data (?)
                    DifficultyData.LoadGlobalDefaults(
                        //GlobalSessionManager.xmlGlobalData
                        globalSessionManager.XmlGlobalData
                    )
                );
            }

            ApplyScenarioOverlay(scenario);
        }

        private bool ShouldSkipPersistence() {
            return startupScenario != null && startupScenario.DisableSave;
        }

        public bool HasField(string name) {
            return runtimeState.HasField(name);
        }

        public int GetField(string name, int defaultValue = 0) {
            return runtimeState.GetField(name, defaultValue);
        }

        public void SetField(string name, int value) {
            runtimeState.SetField(name, value);
        }

        public System.Collections.Generic.Dictionary<string, int> GetFields() {
            return runtimeState.GetFieldsDictionary();
        }

        public System.Collections.Generic.IEnumerable<string> GetFieldNames() {
            return new System.Collections.Generic.List<string>(GetFields().Keys);
        }
    }
}
