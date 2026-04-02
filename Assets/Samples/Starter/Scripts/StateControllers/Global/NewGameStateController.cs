using StateEngine.Graphs;
using StateEngine.Managers.Game;

namespace StateEngine.Samples.Starter {
    public class NewGameStateController : GlobalStateController {
        public string gameSceneName;

        private void StartGame(int difficulty) {
            // NOTE: Must access Session Manager methods directly,
            //       => always go through Data Manager.
            GameManager.Instance.Data.NewGame(difficulty);

            // setting level here to start there immediately (no map/hub)
            GameManager.Instance.Data.SetLevel(1);

            // TODO: add method 'LeaveToGame' loading "first" scene of GameGraph
            Leave(gameSceneName);
        }

        public void StartGameEasy() {
            StartGame(0);
        }

        public void StartGameNormal() {
            StartGame(1);
        }

        public void StartGameHard() {
            StartGame(2);
        }
    }
}
