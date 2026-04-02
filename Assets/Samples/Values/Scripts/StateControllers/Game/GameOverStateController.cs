using StateEngine.Graphs;
using UnityEngine;

namespace StateEngine.Samples.Values {
    public class GameOverStateController : GameStateController {
        [SerializeField] private string globalSceneName;

        public void Quit() {
            Leave(globalSceneName);
        }
    }
}