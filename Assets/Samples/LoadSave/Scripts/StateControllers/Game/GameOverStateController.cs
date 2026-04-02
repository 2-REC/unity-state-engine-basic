using StateEngine.Graphs;
using UnityEngine;

namespace StateEngine.Samples.LoadSave {
    public class GameOverStateController : GameStateController {
        [SerializeField] private string globalSceneName;

        public void Quit() {
            Leave(globalSceneName);
        }
    }
}