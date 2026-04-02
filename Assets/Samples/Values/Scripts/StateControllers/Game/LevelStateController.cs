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

        /*
        public override void HandleMainState() {
        }
        */

        public void AddHealth() {
            int health = gameManager.Data.GetField("HEALTH") + healthChange;
            gameManager.Data.SetField("HEALTH", health);
        }

        public void SubHealth() {
            int health = gameManager.Data.GetField("HEALTH") - healthChange;
            gameManager.Data.SetField("HEALTH", health);

            if (health <= 0) {
                EndLevelFailure();
            }
        }

        public void AddPoints() {
            int points = gameManager.Data.GetField("POINTS");
            gameManager.Data.SetField("POINTS", points + pointsChange);
        }

        public void SubPoints() {
            int points = gameManager.Data.GetField("POINTS");
            gameManager.Data.SetField("POINTS", points - pointsChange);
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
