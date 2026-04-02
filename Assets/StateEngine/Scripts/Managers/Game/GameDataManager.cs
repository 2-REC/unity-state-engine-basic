using StateEngine.Graphs;
using StateEngine.Managers.Shared;
using StateEngine.Runtime;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace StateEngine.Managers.Game {
    public class GameDataManager : IDataManager {

        // TODO: from previous impl => keep?
        //[SerializeField] private int _currentLevelIndex;
        //public int CurrentLevelIndex => _currentLevelIndex;
        /*
        public void SetCurrentLevelIndex(int levelIndex) {
            _currentLevelIndex = levelIndex;
        }
        */

        // TODO: or ISessionManager?
        [SerializeField] private GameSessionManager gameSessionManager;

        ////public static TextAsset xmlGameData;
        //public static TextAsset xmlGameLevels;
        [SerializeField] private TextAsset xmlGameLevels;

        [Header("Runtime State")]
        [SerializeField] private RuntimeGameStateAsset runtimeStateTemplate;
        [SerializeField] private GameStateScenarioAsset startupScenario;
        [SerializeField] private ScenarioLoadMode scenarioLoadMode = ScenarioLoadMode.None;

        //protected GameSessionManager gameSessionManager;
        protected RuntimeGameStateAsset runtimeState;

        private Dictionary<int, LevelNode> levels;
        private Dictionary<int, bool> availableLevels = new Dictionary<int, bool>();

        // public read-only access
        public RuntimeGameStateAsset RuntimeState => runtimeState;

        protected override void LoadData() {
            // TODO: could check that have a session manager
            //GameSessionManager.xmlGameData = xmlGameData;
            //gameSessionManager = GameSessionManager.Instance;

            // TODO: OK? (added)
            // make sure the session loaded first
            gameSessionManager.Init();

            runtimeState = CreateRuntimeStateInstance();

            levels = GameGraphLoader.LoadLevelGraph(xmlGameLevels);

            if (scenarioLoadMode == ScenarioLoadMode.Detached && startupScenario != null) {
                BuildRuntimeStateFromScenario(startupScenario);
            } else {
                PopulateRuntimeStateFromSession();
                if (startupScenario != null && scenarioLoadMode == ScenarioLoadMode.Overlay) {
                    ApplyScenarioOverlay(startupScenario);
                }
            }

            ApplyRuntimeCompletionStateToLevelGraph();
            InitializeAvailableLevels();
        }

        public override void CommitChanges() {
            if (ShouldSkipPersistence()) {
                return;
            }

            CommitRuntimeStateToSession();
            gameSessionManager.Save();
        }

        ////////
        // TODO: OK?
        public void NewGame(int difficulty) {
            gameSessionManager.NewGame(difficulty);
            LoadData();
        }
        ////////

        public void SaveGame(string filename) {
            if (ShouldSkipPersistence()) {
                return;
            }

            CommitRuntimeStateToSession();
            gameSessionManager.SaveGame(filename);
        }

        public bool LoadGame(string filename) {
            return GameManager.Instance.Session.LoadGame(filename);
        }

        public override void Leave() {
            // TODO: which?
            //if (runtimeState == null || runtimeState.CurrentLevel == -1) {
            if (runtimeState != null && runtimeState.CurrentLevel == -1) {
                gameSessionManager.Clear();
            } else if (!ShouldSkipPersistence()) {
                CommitChanges();
            }

            //loaded = false;
            availableLevels.Clear();
            runtimeState = null;
        }

        protected virtual RuntimeGameStateAsset CreateRuntimeStateInstance() {
            RuntimeGameStateAsset instance = null;
            if (runtimeStateTemplate != null) {
                instance = Instantiate(runtimeStateTemplate);
                instance.name = runtimeStateTemplate.name + "_Runtime";
                instance.ClearState();
            } else {
                instance = ScriptableObject.CreateInstance<RuntimeGameStateAsset>();
                instance.name = "RuntimeGameState_Runtime";
            }
            return instance;
        }

        protected virtual void PopulateRuntimeStateFromSession() {
            runtimeState.ClearState();
            runtimeState.Difficulty = gameSessionManager.GetDifficulty();
            runtimeState.CurrentLevel = gameSessionManager.GetLevel();
            runtimeState.LatestLevel = runtimeState.CurrentLevel;
            runtimeState.Lives = gameSessionManager.GetLives();
            runtimeState.Continues = gameSessionManager.GetContinues();
            runtimeState.SetFieldsFromDictionary(gameSessionManager.GetDynamicFields());
            runtimeState.SetInitialFieldsFromDictionary(gameSessionManager.GetInitialDynamicFields());
            runtimeState.SetCompletedLevels(gameSessionManager.GetCompletedLevels());
        }

        protected virtual void CommitRuntimeStateToSession() {
            gameSessionManager.SetLives(runtimeState.Lives);
            gameSessionManager.SetContinues(runtimeState.Continues);
            gameSessionManager.SetLevel(runtimeState.CurrentLevel);
            gameSessionManager.SetDynamicFields(runtimeState.GetFieldsDictionary(), true);
            gameSessionManager.SetCompletedLevels(runtimeState.GetCompletedLevels(), true);
        }

        protected virtual void ApplyScenarioOverlay(GameStateScenarioAsset scenario) {
            if (scenario == null) {
                return;
            }

            ApplyScenarioValues(runtimeState, scenario);
        }

        protected virtual void BuildRuntimeStateFromScenario(GameStateScenarioAsset scenario) {
            runtimeState.ClearState();

            if (scenario == null) {
                return;
            }

            if (scenario.FillMissingFromBaseState) {
                // TODO: make method in session manager reading+returning the data (?)
                int difficulty = scenario.OverrideDifficulty ? scenario.Difficulty : 0;
                // TODO: should get from session instead, or sync it? (avoid reading xml here - and duplicate code)
                DifficultyData difficultyData = new DifficultyData(gameSessionManager.XmlGameData);
                DifficultyData.DifficultyValues defaults = difficultyData.GetValues(difficulty);

                runtimeState.Difficulty = difficulty;
                runtimeState.CurrentLevel = -1;
                runtimeState.LatestLevel = -1;
                runtimeState.Lives = defaults.lives;
                runtimeState.Continues = defaults.continues;
                runtimeState.SetFieldsFromDictionary(defaults.fields);
                runtimeState.SetInitialFieldsFromDictionary(defaults.fields);
                runtimeState.SetCompletedLevels(new List<int>());
            }

            ApplyScenarioValues(runtimeState, scenario);
        }

        private void ApplyScenarioValues(RuntimeGameStateAsset target, GameStateScenarioAsset scenario) {
            if (scenario.ReplaceRuntimeState) {
                target.ClearState();
            }

            if (scenario.OverrideDifficulty) {
                target.Difficulty = scenario.Difficulty;
            }

            if (scenario.OverrideCurrentLevel) {
                target.CurrentLevel = scenario.CurrentLevel;
            }

            if (scenario.OverrideLatestLevel) {
                target.LatestLevel = scenario.LatestLevel;
            } else if (scenario.OverrideCurrentLevel && target.CurrentLevel != -1) {
                target.LatestLevel = target.CurrentLevel;
            }

            if (scenario.OverrideLives) {
                target.Lives = scenario.Lives;
            }

            if (scenario.OverrideContinues) {
                target.Continues = scenario.Continues;
            }

            Dictionary<string, int> fieldOverrides = RuntimeFieldListUtility.ToDictionary(scenario.FieldOverrides);
            foreach (KeyValuePair<string, int> pair in fieldOverrides) {
                target.SetField(pair.Key, pair.Value);
            }

            Dictionary<string, int> initialOverrides = RuntimeFieldListUtility.ToDictionary(scenario.InitialFieldOverrides);
            foreach (KeyValuePair<string, int> pair in initialOverrides) {
                target.SetInitialField(pair.Key, pair.Value);
            }

            if (scenario.OverrideCompletedLevels) {
                target.SetCompletedLevels(scenario.CompletedLevels);
            }
        }

        private bool ShouldSkipPersistence() {
            return startupScenario != null && startupScenario.DisableSave;
        }

        private void ApplyRuntimeCompletionStateToLevelGraph() {
            foreach (KeyValuePair<int, LevelNode> level in levels) {
                level.Value.Completed = runtimeState.IsLevelCompleted(level.Key);
            }
        }

        private void InitializeAvailableLevels() {
            availableLevels.Clear();

            foreach (KeyValuePair<int, LevelNode> level in levels) {
                if (level.Value.Startup) {
                    availableLevels[level.Key] = level.Value.Completed;
                    if (level.Value.Completed) {
                        UnlockNextLevelsFrom(level.Key);
                    }
                }
            }
        }

        private void UnlockNextLevelsFrom(int level) {
            List<int> next = levels[level].Next;
            if (next == null) {
                return;
            }

            for (int i = 0; i < next.Count; ++i) {
                int nextLevel = next[i];
                if (!availableLevels.ContainsKey(nextLevel)) {
                    availableLevels[nextLevel] = levels[nextLevel].Completed;
                    if (levels[nextLevel].Completed) {
                        UnlockNextLevelsFrom(nextLevel);
                    }
                }
            }
        }

        public bool IsLevelCompleted(int level) {
            if (availableLevels.ContainsKey(level)) {
                return availableLevels[level];
            }
            return false;
        }

        public void SetLevelCompleted() {
            if (runtimeState.CurrentLevel == -1) {
                return;
            }

            levels[runtimeState.CurrentLevel].Completed = true;
            availableLevels[runtimeState.CurrentLevel] = true;
            runtimeState.MarkLevelCompleted(runtimeState.CurrentLevel);
            UnlockNextLevelsFrom(runtimeState.CurrentLevel);
        }

        public int GetLives() {
            return runtimeState.Lives;
        }

        public int LoseLife() {
            runtimeState.Lives -= 1;
            ResetFieldsToInitial(GetLifeResetFields());

            if (runtimeState.Lives <= 0) {
                SetLevel(-1);
            }

            if (!ShouldSkipPersistence()) {
                CommitChanges();
            }

            return runtimeState.Lives;
        }

        public int GetContinues() {
            return runtimeState.Continues;
        }

        public int LoseContinue() {
            if (!CanContinue()) {
                return runtimeState.Continues;
            }

            SetLevel(GetLatestLevel());
            runtimeState.Continues -= 1;
            runtimeState.Lives = runtimeState.HasInitialField("LIVES")
                ? runtimeState.GetInitialField("LIVES")
                : gameSessionManager.GetInitialLives();

            ResetFieldsToInitial(GetContinueResetFields());

            if (!ShouldSkipPersistence()) {
                CommitChanges();
            }

            return runtimeState.Continues;
        }

        public int GetLevel() {
            return runtimeState.CurrentLevel;
        }

        public void SetLevel(int level) {
            runtimeState.CurrentLevel = level;
            if (level != -1) {
                runtimeState.LatestLevel = level;
            }
        }

        public int GetLatestLevel() {
            return runtimeState.LatestLevel;
        }

        public LevelNode GetLevelNode() {
            if (runtimeState.CurrentLevel == -1) {
                return null;
            }
            return levels[runtimeState.CurrentLevel];
        }

        public string GetSceneName() {
            if (runtimeState.CurrentLevel == -1) {
                return null;
            }
            return levels[runtimeState.CurrentLevel].Scene;
        }

        public string GetLevelName() {
            if (runtimeState.CurrentLevel == -1) {
                return null;
            }
            return levels[runtimeState.CurrentLevel].Name;
        }

        public List<int> GetNextLevels(int level) {
            if (level == -1 || !levels.ContainsKey(level)) {
                return new List<int>();
            }

            return levels[level].Next;
        }

        public Dictionary<int, bool> GetAvailableLevels() {
            return availableLevels;
        }

        public bool IsGameOver() {
            return (runtimeState.Lives <= 0);
        }

        public bool IsGameComplete() {
            foreach (KeyValuePair<int, LevelNode> level in levels) {
                if (!level.Value.Completed) {
                    return false;
                }
            }
            return true;
        }

        public bool CanContinue() {
            return (runtimeState.Continues > 0);
        }

        public bool HasField(string name) {
            ValidateFieldAccess(name);
            return runtimeState.HasField(name);
        }

        public int GetField(string name, int defaultValue = 0) {
            ValidateFieldAccess(name);
            return runtimeState.GetField(name, defaultValue);
        }

        public void SetField(string name, int value) {
            ValidateFieldAccess(name);
            runtimeState.SetField(name, value);
        }

        public Dictionary<string, int> GetFields() {
            return runtimeState.GetFieldsDictionary();
        }

        public IEnumerable<string> GetFieldNames() {
            return new List<string>(GetFields().Keys);
        }

        public int GetInitialField(string name, int defaultValue = 0) {
            ValidateFieldAccess(name);
            if (runtimeState.HasInitialField(name)) {
                return runtimeState.GetInitialField(name, defaultValue);
            }
            return gameSessionManager.GetInitialField(name, defaultValue);
        }

        public void ResetFieldToInitial(string name) {
            ValidateFieldAccess(name);
            int value = runtimeState.HasInitialField(name)
                ? runtimeState.GetInitialField(name)
                : gameSessionManager.GetInitialField(name);
            runtimeState.SetField(name, value);
        }

        protected void ResetFieldsToInitial(IEnumerable<string> names) {
            if (names == null) {
                return;
            }

            foreach (string name in names) {
                ResetFieldToInitial(name);
            }
        }

        protected virtual IEnumerable<string> GetLifeResetFields() {
            return new List<string>();
        }

        protected virtual IEnumerable<string> GetContinueResetFields() {
            return new List<string>();
        }

        protected virtual void ValidateFieldAccess(string name) {
            if (runtimeState == null || string.IsNullOrEmpty(name)) {
                throw new System.ArgumentException("Field name cannot be null or empty.", "name");
            }
            if (gameSessionManager != null && !gameSessionManager.IsDynamicGameField(name)) {
                throw new System.ArgumentException("Field is not a dynamic gameplay field: " + name, "name");
            }
        }
    }
}

