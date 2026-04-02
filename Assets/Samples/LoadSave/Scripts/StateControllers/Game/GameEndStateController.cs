using StateEngine.Graphs;
using UnityEngine;

namespace StateEngine.Samples.LoadSave {
    public class GameEndStateController : GameStateController {
        [SerializeField] private string globalSceneName;

        public void Quit() {
            Leave(globalSceneName);
        }
    }
}