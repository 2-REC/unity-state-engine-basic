using StateEngine.Samples.Shared;
using UnityEngine;
using UnityEngine.UIElements;

namespace StateEngine.Samples.LoadSave {
    public class LoadSaveLevelHudController : LevelHudController {
        [Header("State Controller")]
        [SerializeField] private LevelStateController stateController;

        private Button healthSubButton;
        private Button pointsAddButton;
        private Button winButton;
        private Button loseButton;
        private Button saveButton;
        private Button quitButton;

        private VisualElement saveContainer;
        private Button saveButton1;
        private Button saveButton2;
        private Button saveButton3;
        private Button saveBackButton;


        protected override void OnEnable() {
            base.OnEnable();

            var root = uiDocument.rootVisualElement;

            healthSubButton = root.Q<Button>("health-sub-button");
            pointsAddButton = root.Q<Button>("points-add-button");
            winButton = root.Q<Button>("win-button");
            loseButton = root.Q<Button>("lose-button");
            saveButton = root.Q<Button>("save-button");
            quitButton = root.Q<Button>("quit-button");

            // TODO: handle overwrite check
            saveContainer = root.Q<VisualElement>("save-container");
            saveButton1 = root.Q<Button>("save-button-1");
            saveButton2 = root.Q<Button>("save-button-2");
            saveButton3 = root.Q<Button>("save-button-3");
            saveBackButton = root.Q<Button>("save-back-button");

            healthSubButton.clicked += stateController.SubHealth;
            pointsAddButton.clicked += stateController.AddPoints;
            winButton.clicked += stateController.EndLevelSuccess;
            loseButton.clicked += stateController.EndLevelFailure;
            saveButton.clicked += OpenSaveMenu;
            quitButton.clicked += stateController.Quit;

            saveButton1.clicked += SaveGame1;
            saveButton2.clicked += SaveGame2;
            saveButton3.clicked += SaveGame3;
            saveBackButton.clicked += CloseSaveMenu;

        }

        private void OpenSaveMenu() {
            saveContainer.style.display = DisplayStyle.Flex;
        }

        private void CloseSaveMenu() {
            saveContainer.style.display = DisplayStyle.None;
        }

        private void SaveGame1() {
            stateController.SaveSlot(0);
        }

        private void SaveGame2() {
            stateController.SaveSlot(1);
        }

        private void SaveGame3() {
            stateController.SaveSlot(2);
        }
    }
}
