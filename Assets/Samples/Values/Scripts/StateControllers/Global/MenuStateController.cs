using StateEngine.Graphs;
using StateEngine.Managers.Game;

// TODO: replace buttons with combo => update data manager value from combo,
//       then start game with value in data manager

namespace StateEngine.Samples.Values {
    public class MenuStateController : GlobalStateController {
        public string gameSceneName;

        private void StartGame(int difficulty) {
            // NOTE: Must access Session Manager methods directly,
            //       => always go through Data Manager.
            GameManager.Instance.Data.NewGame(difficulty);

            // setting level here to start there immediately (no map/hub)
            GameManager.Instance.Data.SetLevel(1);

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
