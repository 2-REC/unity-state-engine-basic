using StateEngine.Graphs;

namespace StateEngine.Samples.Starter {
    public class MenuStateController : GlobalStateController {
        public void NewGame() {
            LoadChildState("NEW_GAME");
        }
    }
}
