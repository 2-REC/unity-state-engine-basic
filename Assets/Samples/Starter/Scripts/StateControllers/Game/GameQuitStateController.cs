using StateEngine.Graphs;

namespace StateEngine.Samples.Starter {
    public class GameQuitStateController : GameStateController {
        public string globalSceneName;

        public void Quit() {
            Leave(globalSceneName);
        }
    }
}
