using UnityEngine;
using UnityEngine.UIElements;

namespace StateEngine.Samples.LoadSave {
    public class LoadSaveMenuHudController : MonoBehaviour {
        [Header("State Controller")]
        [SerializeField] private MenuStateController stateController;

        [Header("UI")]
        [SerializeField] private UIDocument document;

        private Button continueButton;
        private Button newButton;
        private Button loadButton;
        private Button quitButton;

        private VisualElement difficultyContainer;
        private Button difficultyEasyButton;
        private Button difficultyNormalButton;
        private Button difficultyHardButton;
        private Button difficultyBackButton;

        private VisualElement loadContainer;
        private Button loadButton1;
        private Button loadButton2;
        private Button loadButton3;
        private Button loadBackButton;

        void OnEnable() {
            var root = document.rootVisualElement;

            continueButton = root.Q<Button>("continue-button");

            newButton = root.Q<Button>("new-button");

            difficultyContainer = root.Q<VisualElement>("difficulty-container");
            difficultyEasyButton = root.Q<Button>("easy-button");
            difficultyNormalButton = root.Q<Button>("normal-button");
            difficultyHardButton = root.Q<Button>("hard-button");
            difficultyBackButton = root.Q<Button>("back-button");

            loadButton = root.Q<Button>("load-button");

            loadContainer = root.Q<VisualElement>("load-container");
            loadButton1 = root.Q<Button>("load-button-1");
            loadButton2 = root.Q<Button>("load-button-2");
            loadButton3 = root.Q<Button>("load-button-3");
            loadBackButton = root.Q<Button>("load-back-button");

            ToggleVisibility(continueButton, stateController.CanContinue());
            // TODO: can optimize...
            ToggleVisibility(loadButton, stateController.CheckSavedGames());
            ToggleVisibility(loadButton1, stateController.CheckSavedGame(0));
            ToggleVisibility(loadButton2, stateController.CheckSavedGame(1));
            ToggleVisibility(loadButton3, stateController.CheckSavedGame(2));

            quitButton = root.Q<Button>("quit-button");

            continueButton.clicked += Continue;
            newButton.clicked += OpenDifficultyMenu;
            loadButton.clicked += OpenLoadMenu;
            quitButton.clicked += Quit;

            difficultyEasyButton.clicked += StartGameEasy;
            difficultyNormalButton.clicked += StartGameNormal;
            difficultyHardButton.clicked += StartGameHard;
            difficultyBackButton.clicked += CloseDifficultyMenu;

            loadButton1.clicked += LoadGame1;
            loadButton2.clicked += LoadGame2;
            loadButton3.clicked += LoadGame3;
            loadBackButton.clicked += CloseLoadMenu;
        }

        private void ToggleVisibility(VisualElement visualElement, bool visible) {
            if (visible) {
                visualElement.style.display = DisplayStyle.Flex;
            } else {
                visualElement.style.display = DisplayStyle.None;
            }
        }

        private void Continue() {
            stateController.Continue();
        }

        private void OpenDifficultyMenu() {
            ToggleVisibility(difficultyContainer, true);
        }

        private void CloseDifficultyMenu() {
            ToggleVisibility(difficultyContainer, false);
        }

        private void StartGameEasy() {
            stateController.StartGame(0);
        }

        private void StartGameNormal() {
            stateController.StartGame(1);
        }

        private void StartGameHard() {
            stateController.StartGame(2);
        }

        private void OpenLoadMenu() {
            ToggleVisibility(loadContainer, true);
        }

        private void CloseLoadMenu() {
            ToggleVisibility(loadContainer, false);
        }

        private void LoadGame1() {
            stateController.LoadGame(0);
        }

        private void LoadGame2() {
            stateController.LoadGame(1);
        }

        private void LoadGame3() {
            stateController.LoadGame(2);
        }

        private void Quit() {
            stateController.End();
        }
    }
}
