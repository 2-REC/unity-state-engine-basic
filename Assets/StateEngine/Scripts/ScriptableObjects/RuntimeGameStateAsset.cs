using System.Collections.Generic;
using UnityEngine;

namespace StateEngine.Runtime {
    [CreateAssetMenu(fileName = "RuntimeGameState", menuName = "State Engine/Runtime Game State")]
    public class RuntimeGameStateAsset : ScriptableObject {
        public int Difficulty = 0;
        public int CurrentLevel = -1;
        public int LatestLevel = -1;
        public int Lives = 0;
        public int Continues = 0;

        [SerializeField] private List<RuntimeIntFieldEntry> fields = new List<RuntimeIntFieldEntry>();
        [SerializeField] private List<RuntimeIntFieldEntry> initialFields = new List<RuntimeIntFieldEntry>();
        [SerializeField] private List<int> completedLevels = new List<int>();

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
            RuntimeFieldListUtility.SetValue(fields, name, value);
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
