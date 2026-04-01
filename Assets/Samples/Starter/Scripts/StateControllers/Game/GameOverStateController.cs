using StateEngine.Graphs;
using StateEngine.Managers.Game;

namespace StateEngine.Samples.Starter {
    public class GameOverStateController : GameStateController {
        public bool CanContinue { get; private set; } = false;

        public override void HandleMainState() {
            CanContinue = GameManager.Instance.Data.CanContinue();
        }

        public void Continue() {
            if (CanContinue) {
                End();
            } else {
                LoadChildState("QUIT_GAME");
            }
        }
    }
}
