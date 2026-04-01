using StateEngine.Graphs;

namespace StateEngine.Samples.Starter {
    public class QuitStateController : GlobalStateController {
        public void Quit() {
            Leave();
        }
    }
}
