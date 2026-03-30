using StateEngine.IO;
using StateEngine.Managers.Shared;
using StateEngine.Runtime;
using System.Collections.Generic;
using System;
using UnityEngine;

namespace StateEngine.Managers.Game {
    public sealed class GameSessionManager : ISessionManager {

        // TODO: from previous impl => keep?
        //[SerializeField] private bool _runActive;
        //public bool RunActive => _runActive;
        /*
        public void BeginRun() {
            _runActive = true;
        }
        public void EndRun() {
            _runActive = false;
        }
        */

        // TODO: move to base class... (?)
        [field: SerializeField] public TextAsset XmlGameData { get; private set; }

        public void Init() {
            // TODO: OK?
            SetFile("INGAME");

            if (!Load()) {
                // TODO: needed? need anything? (defaults? save?)
                /*
                LoadDefaults();
                Save();
                */
            }
        }

        public void NewGame(int difficulty) {
            Clear();

            DifficultyData difficultyData = new DifficultyData(XmlGameData);
            DifficultyData.DifficultyValues diff = difficultyData.GetValues(difficulty);

            SetDifficulty(difficulty);

            SetInitialField("LIVES", diff.lives);
            SetField("LIVES", diff.lives);
            SetField("CONTINUES", diff.continues);

            foreach (KeyValuePair<string, int> pair in diff.fields) {
                SetInitialField(pair.Key, pair.Value);
                SetDynamicField(pair.Key, pair.Value);
            }

            SetLevel(-1);
            Save();
        }

        public bool IsReservedField(string name) {
            if (string.IsNullOrEmpty(name)) {
                return true;
            }

            return name.Equals("LEVEL")
                || name.Equals("DIFFICULTY")
                || name.Equals("LIVES")
                || name.Equals("CONTINUES")
                || name.StartsWith("INITIAL_", StringComparison.Ordinal)
                || name.StartsWith("LEVEL_", StringComparison.Ordinal);
        }

        public bool IsDynamicGameField(string name) {
            return !IsReservedField(name);
        }

        public Dictionary<string, int> GetDynamicFields() {
            Dictionary<string, int> dynamicFields = new Dictionary<string, int>();

            foreach (KeyValuePair<string, int> pair in fields) {
                if (IsDynamicGameField(pair.Key)) {
                    dynamicFields[pair.Key] = pair.Value;
                }
            }

            return dynamicFields;
        }

        public void SetDynamicFields(Dictionary<string, int> values, bool replaceExisting = true) {
            if (replaceExisting) {
                List<string> toRemove = new List<string>();
                foreach (KeyValuePair<string, int> pair in fields) {
                    if (IsDynamicGameField(pair.Key)) {
                        toRemove.Add(pair.Key);
                    }
                }
                for (int i = 0; i < toRemove.Count; ++i) {
                    RemoveField(toRemove[i]);
                }
            }

            if (values == null) {
                return;
            }

            foreach (KeyValuePair<string, int> pair in values) {
                SetDynamicField(pair.Key, pair.Value);
            }
        }

        public IEnumerable<string> GetDynamicFieldNames() {
            return new List<string>(GetDynamicFields().Keys);
        }

        public bool HasDynamicField(string name) {
            ValidateDynamicFieldName(name);
            return HasField(name);
        }

        public int GetDynamicField(string name, int defaultValue = 0) {
            ValidateDynamicFieldName(name);
            return GetField(name, defaultValue);
        }

        public void SetDynamicField(string name, int value) {
            ValidateDynamicFieldName(name);
            SetField(name, value);
        }

        public Dictionary<string, int> GetInitialDynamicFields() {
            Dictionary<string, int> initialFields = new Dictionary<string, int>();

            foreach (KeyValuePair<string, int> pair in fields) {
                if (!pair.Key.StartsWith("INITIAL_", StringComparison.Ordinal)) {
                    continue;
                }

                string originalName = pair.Key.Substring("INITIAL_".Length);
                if (IsDynamicGameField(originalName)) {
                    initialFields[originalName] = pair.Value;
                }
            }

            return initialFields;
        }

        public bool HasInitialField(string name) {
            ValidateDynamicFieldName(name);
            return HasField("INITIAL_" + name);
        }

        public void ResetDynamicFieldToInitial(string name) {
            ValidateDynamicFieldName(name);
            SetDynamicField(name, GetInitialField(name));
        }

        public void ResetDynamicFieldsToInitial(IEnumerable<string> names) {
            if (names == null) {
                return;
            }

            foreach (string name in names) {
                ResetDynamicFieldToInitial(name);
            }
        }

        public int GetInitialField(string name, int defaultValue) {
            return GetField("INITIAL_" + name, defaultValue);
        }

        public int GetInitialField(string name) {
            return GetInitialField(name, 0);
        }

        private void SetInitialField(string name, int value) {
            SetField("INITIAL_" + name, value);
        }

        public int GetLevel() {
            return GetField("LEVEL", -1);
        }

        public void SetLevel(int level) {
            SetField("LEVEL", level);
        }

        public int GetDifficulty() {
            return GetField("DIFFICULTY", -1);
        }

        private void SetDifficulty(int difficulty) {
            SetField("DIFFICULTY", difficulty);
        }

        public int GetInitialLives() {
            return GetField("INITIAL_LIVES", -1);
        }

        public int GetLives() {
            return GetField("LIVES", -1);
        }

        public void SetLives(int lives) {
            SetField("LIVES", lives);
        }

        public int GetContinues() {
            return GetField("CONTINUES", -1);
        }

        public void SetContinues(int continues) {
            SetField("CONTINUES", continues);
        }

        public bool IsLevelCompleted(int level) {
            return (GetField("LEVEL_" + level, -1) != -1);
        }

        public IEnumerable<int> GetCompletedLevels() {
            List<int> completed = new List<int>();
            foreach (KeyValuePair<string, int> pair in fields) {
                if (!pair.Key.StartsWith("LEVEL_", StringComparison.Ordinal)) {
                    continue;
                }

                string suffix = pair.Key.Substring("LEVEL_".Length);
                if (int.TryParse(suffix, out int level)) {
                    completed.Add(level);
                }
            }
            return completed;
        }

        public void ClearCompletedLevels() {
            List<string> keys = new List<string>();
            foreach (KeyValuePair<string, int> pair in fields) {
                if (pair.Key.StartsWith("LEVEL_", StringComparison.Ordinal)) {
                    keys.Add(pair.Key);
                }
            }
            for (int i = 0; i < keys.Count; ++i) {
                RemoveField(keys[i]);
            }
        }

        public void SetCompletedLevels(IEnumerable<int> levels, bool replaceExisting = true) {
            if (replaceExisting) {
                ClearCompletedLevels();
            }

            if (levels == null) {
                return;
            }

            foreach (int level in levels) {
                SetLevelCompleted(level);
            }
        }

        public void SetLevelCompleted(int level) {
            SetField("LEVEL_" + level, 1);
        }

        public void SaveGame(string filename) {
            FileManager.Save(filename, fields);
        }

        public bool LoadGame(string filename) {
            Dictionary<string, int> newFields = new Dictionary<string, int>();
            bool loaded = FileManager.Load(filename, newFields);
            if (loaded) {
                fields = newFields;
                Save();
            }
            return loaded;
        }

        public bool SaveExists(string filename) {
            return FileManager.Exists(filename);
        }

        public bool CheckExists(string filename) {
            return SaveExists(filename);
        }

        private void ValidateDynamicFieldName(string name) {
            if (string.IsNullOrEmpty(name)) {
                throw new ArgumentException("Field name cannot be null or empty.", "name");
            }

            if (IsReservedField(name)) {
                throw new ArgumentException("Reserved field name cannot be used as a dynamic game field: " + name, "name");
            }
        }
    }
}
