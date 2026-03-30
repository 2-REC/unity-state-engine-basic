using System.Collections.Generic;
using UnityEngine;

namespace StateEngine.Runtime {
    [CreateAssetMenu(fileName = "GlobalStateScenario", menuName = "State Engine/Global State Scenario")]

    public class GlobalStateScenarioAsset : ScriptableObject {
        public string ScenarioId = "Scenario";
        public bool ReplaceRuntimeState = false;
        public bool DisableSave = false;
        public bool FillMissingFromBaseState = true;

        public List<RuntimeIntFieldEntry> FieldOverrides = new List<RuntimeIntFieldEntry>();
    }
}
