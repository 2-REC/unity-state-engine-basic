using StateEngine.Graphs;
using StateEngine.Managers.Game;

namespace StateEngine.Samples {
    public class MenuCtrl : GlobalStateController {
        public string gameSceneName;

        public int Difficulty { get; private set; }

        private GameManager gameManager;

        protected override void Awake() {
            base.Awake();
            // TODO: could be done in base class (common to all global state controllers)
            gameManager = GameManager.Instance;
        }
        public override void HandleMainState() {
            // TODO: could keep as private field to avoid casting every call (?)
            Difficulty = ((SampleGlobalDataManager)globalManager.Data).Difficulty;
        }

        public void NewGame() {
            // TODO: could keep as private field to avoid casting every call (?)
            gameManager.Session.NewGame(Difficulty);
            gameManager.Session.SetLevel(1);

            Leave(gameSceneName);
        }

        public void SetDifficulty(int value) {
            Difficulty = value;
            ((SampleGlobalDataManager)globalManager.Data).Difficulty = value;
        }
    }
}
