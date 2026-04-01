using StateEngine.Graphs;
using StateEngine.Managers.Game;

namespace StateEngine.Samples.Starter {
    public class LevelStateController : GameStateController {
        public int Lives { get; private set; }

        public override void HandleMainState() {
            Lives = GameManager.Instance.Data.GetLives();
        }

        public void EndLevelSuccess() {
            GameManager.Instance.Data.SetLevelCompleted();
            GameManager.Instance.Data.CommitChanges();

            LoadChildState("SUCCESS");
        }

        public void EndLevelFailure() {
            GameManager.Instance.Data.LoseLife();
            GameManager.Instance.Data.CommitChanges();

StateIds.dumpStates();
            LoadChildState("FAILURE");
        }

        // TODO: OK?
        public void Quit() {
            LoadChildState("QUIT_GAME");
        }
    }
}
