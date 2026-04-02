using StateEngine.Graphs;
using StateEngine.Managers.Game;
using System.Collections.Generic;
using UnityEngine;

namespace StateEngine.Samples.Values {
    public class LevelStateController : GameStateController {
        [SerializeField] private string globalSceneName;
        // TODO: rename
        [SerializeField] private int healthChange = 5;
        [SerializeField] private int pointsChange = 10;

        private ValuesGameDataManager gameDataManager;

        public override void HandleMainState() {
            gameDataManager = (ValuesGameDataManager)GameManager.Instance.Data;
        }

        public void AddHealth() {
            gameDataManager.Health += healthChange;
        }

        public void SubHealth() {
            gameDataManager.Health -= healthChange;
            if (gameDataManager.Health <= 0) {
                EndLevelFailure();
            }
        }

        public void AddPoints() {
            gameDataManager.Points += pointsChange;
        }

        public void SubPoints() {
            gameDataManager.Points -= pointsChange;
        }

        public void EndLevelSuccess() {
            GameDataManager gameDataManager = GameManager.Instance.Data;
            gameDataManager.SetLevelCompleted();
            gameDataManager.CommitChanges();

            if (gameDataManager.IsGameComplete()) {
                LoadChildState("GAME_END");
            } else {
                List<int> nextLevels = gameDataManager.GetNextLevels(gameDataManager.GetLevel());
                // TODO: IF NOT => ERROR!
                if (nextLevels.Count == 1) {
                    gameDataManager.SetLevel(nextLevels[0]);
                }

                End();
            }
        }

        public void EndLevelFailure() {
            GameDataManager gameDataManager = GameManager.Instance.Data;

            gameDataManager.LoseLife();

            if (gameDataManager.IsGameOver()) {
                if (!gameDataManager.CanContinue()) {
                    gameDataManager.CommitChanges();
                    LoadChildState("GAME_OVER");
                    return;
                }
                gameDataManager.LoseContinue();
            }

            gameDataManager.CommitChanges();
            End();
        }

        public void Quit() {
            Leave(globalSceneName);
        }
    }
}
