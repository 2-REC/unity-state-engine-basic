using StateEngine.Graphs;
using StateEngine.Managers.Game;
using UnityEngine;

namespace StateEngine.Samples.LoadSave {
    public class MenuStateController : GlobalStateController {
        [SerializeField] private string gameSceneName = "Level";

        public void StartGame(int difficulty) {
            GameManager.Instance.Data.NewGame(difficulty);
            GameManager.Instance.Data.SetLevel(1);
            Leave(gameSceneName);
        }

        public bool CanContinue() {
            return GameManager.Instance.Session.GetLevel() != -1;
        }

        public void Continue() {
            if (!CanContinue())
                return;

            Leave(gameSceneName);
        }

        // TODO: handle dynamic number...
        public bool CheckSavedGames() {
            for (int i = 0; i < 3; ++i) {
                if (CheckSavedGame(i)) {
                    return true;
                }
            }
            return false;
        }

        public bool CheckSavedGame(int slotNb) {
            return GameManager.Instance.Session.CheckExists($"_save{slotNb}");
        }

        public void LoadGame(int slotNb) {
            // must call from data manager, not session manager!
            if (GameManager.Instance.Data.LoadGame($"_save{slotNb}"))
                Leave(gameSceneName);
        }
    }
}
