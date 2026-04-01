using StateEngine.Graphs;
using StateEngine.Managers.Game;
using System.Collections.Generic;
using UnityEngine;

namespace StateEngine.Samples.Starter {
    public class SuccessStateController : GameStateController {
        public bool GameEnd { get; private set; } = false;

        public override void HandleMainState() {
            GameEnd = GameManager.Instance.Data.IsGameComplete();
            Debug.Log($"Game end: {GameEnd}");
        }

        public void Continue() {
            if (GameEnd) {
                LoadChildState("GAME_END");
            } else {
                // TODO: store data manager as field
                int level = GameManager.Instance.Data.GetLevel();
                List<int> nextLevels = GameManager.Instance.Data.GetNextLevels(level);

                // TODO: IF NOT => ERROR!
                if (nextLevels.Count == 1) {
                    GameManager.Instance.Data.SetLevel(nextLevels[0]);
                }

                End();
            }
        }
    }
}
