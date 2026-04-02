using StateEngine.Graphs;
using StateEngine.Managers.Game;
using System.Collections.Generic;
using UnityEngine;

namespace StateEngine.Samples.LoadSave {
    public class LevelStateController : GameStateController {
        [SerializeField] private string globalSceneName;
        // TODO: rename
        [SerializeField] private int healthChange = 5;
        [SerializeField] private int pointsChange = 10;

        private LoadSaveGameDataManager gameDataManager;

        protected override void Awake() {
            base.Awake();
        }
        public override void HandleMainState() {
            gameDataManager = (LoadSaveGameDataManager)GameManager.Instance.Data;
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

        public void SaveSlot(int slotNb) {
            SaveGame($"_save{slotNb}");

            // TODO: add feedback
            //savedGameText.text = $"Game saved to slot {slotNb}";
            //savedGameText.gameObject.SetActive(true);
        }

        private void SaveGame(string filename) {
            gameDataManager.SaveGame(filename);
        }

        public void Quit() {
            Leave(globalSceneName);
        }
    }
}
