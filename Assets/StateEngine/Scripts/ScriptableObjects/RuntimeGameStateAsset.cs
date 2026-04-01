using System;
using System.Collections.Generic;
using UnityEngine;

namespace StateEngine.Runtime {
    [CreateAssetMenu(fileName = "RuntimeGameState", menuName = "State Engine/Runtime Game State")]
    public class RuntimeGameStateAsset : ScriptableObject {
        [SerializeField] private int difficulty = 0;
        [SerializeField] private int currentLevel = -1;
        [SerializeField] private int latestLevel = -1;
        [SerializeField] private int lives = 0;
        [SerializeField] private int continues = 0;

        [SerializeField] private List<RuntimeIntFieldEntry> fields = new List<RuntimeIntFieldEntry>();
        [SerializeField] private List<RuntimeIntFieldEntry> initialFields = new List<RuntimeIntFieldEntry>();
        [SerializeField] private List<int> completedLevels = new List<int>();

        // runtime events
        public event Action<int> DifficultyChanged;
        public event Action<int> LevelChanged;
        public event Action<int> LatestLevelChanged;
        public event Action<int> LivesChanged;
        public event Action<int> ContinuesChanged;
        public event Action<string, int> FieldChanged;

        public int Difficulty {
            get => difficulty;
            set {
                if (difficulty == value) {
                    return;
                }

                difficulty = value;
                DifficultyChanged?.Invoke(difficulty);
            }
        }

        public int CurrentLevel {
            get => currentLevel;
            set {
                if (currentLevel == value) {
                    return;
                }

                currentLevel = value;
                LevelChanged?.Invoke(currentLevel);
            }
        }

        public int LatestLevel {
            get => latestLevel;
            set {
                if (latestLevel == value) {
                    return;
                }

                latestLevel = value;
                LatestLevelChanged?.Invoke(latestLevel);
            }
        }

        public int Lives {
            get => lives;
            set {
                if (lives == value) {
                    return;
                }

                lives = value;
                LivesChanged?.Invoke(lives);
            }
        }

        public int Continues {
            get => continues;
            set {
                if (continues == value) {
                    return;
                }

                continues = value;
                ContinuesChanged?.Invoke(continues);
            }
        }

        public void ClearState() {
            Difficulty = 0;
            CurrentLevel = -1;
            LatestLevel = -1;
            Lives = 0;
            Continues = 0;

            fields.Clear();
            initialFields.Clear();
            completedLevels.Clear();
        }

        public Dictionary<string, int> GetFieldsDictionary() {
            return RuntimeFieldListUtility.ToDictionary(fields);
        }

        public void SetFieldsFromDictionary(Dictionary<string, int> values) {
            RuntimeFieldListUtility.SetFromDictionary(fields, values, true);
        }

        public bool HasField(string name) {
            return RuntimeFieldListUtility.HasKey(fields, name);
        }

        public int GetField(string name, int defaultValue = 0) {
            return RuntimeFieldListUtility.GetValue(fields, name, defaultValue);
        }

        public void SetField(string name, int value) {
            int oldValue = GetField(name, int.MinValue);
            if (oldValue == value) {
                return;
            }

            RuntimeFieldListUtility.SetValue(fields, name, value);
            FieldChanged?.Invoke(name, value);
        }

        public Dictionary<string, int> GetInitialFieldsDictionary() {
            return RuntimeFieldListUtility.ToDictionary(initialFields);
        }

        public void SetInitialFieldsFromDictionary(Dictionary<string, int> values) {
            RuntimeFieldListUtility.SetFromDictionary(initialFields, values, true);
        }

        public bool HasInitialField(string name) {
            return RuntimeFieldListUtility.HasKey(initialFields, name);
        }

        public int GetInitialField(string name, int defaultValue = 0) {
            return RuntimeFieldListUtility.GetValue(initialFields, name, defaultValue);
        }

        public void SetInitialField(string name, int value) {
            RuntimeFieldListUtility.SetValue(initialFields, name, value);
        }

        public List<int> GetCompletedLevels() {
            return new List<int>(completedLevels);
        }

        public void SetCompletedLevels(IEnumerable<int> levels) {
            completedLevels.Clear();
            if (levels == null) {
                return;
            }

            foreach (int level in levels) {
                if (!completedLevels.Contains(level)) {
                    completedLevels.Add(level);
                }
            }
        }

        public bool IsLevelCompleted(int level) {
            return completedLevels.Contains(level);
        }

        public void MarkLevelCompleted(int level) {
            if (!completedLevels.Contains(level)) {
                completedLevels.Add(level);
            }
        }
    }
}
