using StateEngine.Managers.Game;
using StateEngine.Managers.Shared;
using UnityEngine;

namespace StateEngine.Graphs {
    public class GameStateController : IStateController {
        protected GameManager gameManager;

        protected override void Awake() {
            Debug.Log("GameStateController::Awake");
            gameManager = GameManager.Instance;
            Debug.Log("GameStateController::Awake - INSTANCE: " + GameManager.Instance);
            base.Awake();
        }
        protected override IStateManager GetStateManager() {
            Debug.Log("GameStateController::GetStateManager - INSTANCE: " + gameManager);
            return gameManager.State;
        }
    }
}
