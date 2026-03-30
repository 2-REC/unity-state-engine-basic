using System.Collections.Generic;
using UnityEngine;

// TODO: namespace?
namespace StateEngine.Runtime {

    [CreateAssetMenu(fileName = "RuntimeGlobalState", menuName = "State Engine/Runtime Global State")]
    public class RuntimeGlobalStateAsset : ScriptableObject {

        [SerializeField] private List<RuntimeIntFieldEntry> fields = new List<RuntimeIntFieldEntry>();

        public void ClearState() {
            fields.Clear();
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

    }

}
