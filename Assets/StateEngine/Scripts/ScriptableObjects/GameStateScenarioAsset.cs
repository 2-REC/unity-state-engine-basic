using System.Collections.Generic;
using UnityEngine;

namespace StateEngine.Runtime {
    [CreateAssetMenu(fileName = "GameStateScenario", menuName = "State Engine/Game State Scenario")]
    public class GameStateScenarioAsset : ScriptableObject {
        public string ScenarioId = "Scenario";
        public bool ReplaceRuntimeState = false;
        public bool DisableSave = false;
        public bool FillMissingFromBaseState = true;

        public bool OverrideDifficulty = false;
        public int Difficulty = 0;

        public bool OverrideCurrentLevel = false;
        public int CurrentLevel = -1;

        public bool OverrideLatestLevel = false;
        public int LatestLevel = -1;

        public bool OverrideLives = false;
        public int Lives = 0;

        public bool OverrideContinues = false;
        public int Continues = 0;

        public bool OverrideCompletedLevels = false;
        public List<int> CompletedLevels = new List<int>();

        public List<RuntimeIntFieldEntry> FieldOverrides = new List<RuntimeIntFieldEntry>();
        public List<RuntimeIntFieldEntry> InitialFieldOverrides = new List<RuntimeIntFieldEntry>();
    }
}
