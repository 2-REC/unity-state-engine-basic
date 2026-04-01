using UnityEngine;

namespace StateEngine.Graphs {
    public class GlobalGraphLoader : IGraphLoader {
        /*
        public GlobalGraphLoader(string filename)
                : base(filename) {
        }
        */
        public GlobalGraphLoader(TextAsset xmlGraph)
                : base(xmlGraph) {
        }

        protected override bool CheckAttributes(StateData data) {
            if ((data.scene == null) || "".Equals(data.scene)) {
                // TODO: Exception
                Debug.LogError("Invalid state!");
                return false;
            }
            return true;
        }
    }
}
