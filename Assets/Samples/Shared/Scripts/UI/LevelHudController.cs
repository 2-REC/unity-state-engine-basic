using StateEngine.Managers.Game;
using StateEngine.Runtime;
using UnityEngine;
using UnityEngine.UIElements;

namespace UI {
    [RequireComponent(typeof(UIDocument))]
    public class LevelHudController : MonoBehaviour {

        [Header("References")]
        [SerializeField] private UIDocument uiDocument;

        [Header("UI Element Names")]
        [SerializeField] private string levelLabelName = "level-label";
        [SerializeField] private string livesLabelName = "lives-label";
        //[SerializeField] private string healthLabelName = "HealthLabel";
        //[SerializeField] private string pointsLabelName = "PointsLabel";

        [Header("Display")]
        [SerializeField] private bool displayLevelAsOneBased = false;
        [SerializeField] private string missingValueText = "-";

        private GameDataManager gameDataManager;
        private Label levelLabel;
        private Label livesLabel;
        //private Label healthLabel;
        //private Label pointsLabel;

        private RuntimeGameStateAsset runtimeState;

        private void Reset() {
            uiDocument = GetComponent<UIDocument>();
        }

        private void Awake() {
            if (uiDocument == null) {
                uiDocument = GetComponent<UIDocument>();
            }

            if (gameDataManager == null) {
                gameDataManager = GameManager.Instance.Data;
            }
        }

        private void OnEnable() {
            Initialize();
            SubscribeToRuntimeState();
            RefreshAll();
        }

        private void OnDisable() {
            UnsubscribeFromRuntimeState();
        }

        private void Initialize() {
            if (uiDocument == null) {
                Debug.LogError($"{nameof(LevelHudController)}: UIDocument reference is missing.", this);
                return;
            }

            if (gameDataManager == null) {
                gameDataManager = GameManager.Instance.Data;
                if (gameDataManager == null) {
                    Debug.LogError($"{nameof(LevelHudController)}: GameDataManager could not be found.", this);
                    return;
                }
            }

            runtimeState = gameDataManager.RuntimeState;

            VisualElement root = uiDocument.rootVisualElement;
            if (root == null) {
                Debug.LogError($"{nameof(LevelHudController)}: UIDocument rootVisualElement is null.", this);
                return;
            }

            levelLabel = root.Q<Label>(levelLabelName);
            livesLabel = root.Q<Label>(livesLabelName);
            //healthLabel = root.Q<Label>(healthLabelName);
            //pointsLabel = root.Q<Label>(pointsLabelName);

            WarnIfMissing(levelLabel, levelLabelName);
            WarnIfMissing(livesLabel, livesLabelName);
            //WarnIfMissing(healthLabel, healthLabelName);
            //WarnIfMissing(pointsLabel, pointsLabelName);
        }

        private void SubscribeToRuntimeState() {
            if (runtimeState == null) {
                return;
            }

            runtimeState.LevelChanged += OnLevelChanged;
            runtimeState.LivesChanged += OnLivesChanged;
            runtimeState.FieldChanged += OnFieldChanged;
        }

        private void UnsubscribeFromRuntimeState() {
            if (runtimeState == null) {
                return;
            }

            runtimeState.LevelChanged -= OnLevelChanged;
            runtimeState.LivesChanged -= OnLivesChanged;
            runtimeState.FieldChanged -= OnFieldChanged;
        }

        private void RefreshAll() {
            if (runtimeState == null) {
                return;
            }

            SetLevelLabel(runtimeState.CurrentLevel);
            SetLivesLabel(runtimeState.Lives);
            //SetHealthLabel(runtimeState.GetField("HEALTH"));
            //SetPointsLabel(runtimeState.GetField("POINTS"));
        }

        private void OnLevelChanged(int level) {
            SetLevelLabel(level);
        }

        private void OnLivesChanged(int lives) {
            SetLivesLabel(lives);
        }

        private void OnFieldChanged(string fieldName, int value) {
            // TODO: add fields
            /*
            switch (fieldName) {
                case "HEALTH":
                    SetHealthLabel(value);
                    break;

                case "POINTS":
                    SetPointsLabel(value);
                    break;
            }
            */
        }

        private void SetLevelLabel(int level) {
            if (levelLabel == null) {
                return;
            }

            if (level < 0) {
                // TODO: make customizable
                //levelLabel.text = missingValueText;
                levelLabel.text = $"Level: {missingValueText}";
                return;
            }

            int displayValue = displayLevelAsOneBased ? level + 1 : level;
            // TODO: make customizable
            //levelLabel.text = displayValue.ToString();
            levelLabel.text = $"Level: {displayValue}";
        }

        private void SetLivesLabel(int lives) {
            if (livesLabel == null) {
                return;
            }

            // TODO: make customizable
            //livesLabel.text = lives.ToString();
            livesLabel.text = $"Lives: {lives}";
        }

        /*
        private void SetHealthLabel(int health) {
            if (healthLabel == null) {
                return;
            }

            healthLabel.text = health.ToString();
        }

        private void SetPointsLabel(int points) {
            if (pointsLabel == null) {
                return;
            }

            pointsLabel.text = points.ToString();
        }
        */

        private void WarnIfMissing(VisualElement element, string elementName) {
            if (element == null) {
                Debug.LogWarning($"{nameof(LevelHudController)}: Could not find UI element named '{elementName}'.", this);
            }
        }
    }
}
