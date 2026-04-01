using StateEngine.Graphs;
using StateEngine.Managers.Game;
using UnityEngine;

namespace StateEngine.Samples.Starter {
    public class FailureStateController : GameStateController {
        public bool GameOver { get; private set; } = false;

        public override void HandleMainState() {
            GameOver = GameManager.Instance.Data.IsGameOver();
            Debug.Log($"Game over: {GameOver}");
        }

        public void Continue() {
            if (GameOver) {
                LoadChildState("GAME_OVER");
            } else {
                End();
            }
        }
    }
}
