using System;
using System.Collections.Generic;
using StateEngine.Managers.Game;
using StateEngine.Runtime;
using UnityEngine;
using UnityEngine.UIElements;

namespace StateEngine.Samples.Shared {
    //[RequireComponent(typeof(UIDocument))]
    public class LevelHudController : MonoBehaviour {

        public enum RuntimeValueSource {
            CurrentLevel,
            LatestLevel,
            Lives,
            Continues,
            Difficulty,
            DynamicField
        }

        [Serializable]
        public class LabelBinding {
            [Tooltip("Name of the Label in the UIDocument.")]
            public string ElementName;

            [Tooltip("Which runtime value should drive this Label.")]
            public RuntimeValueSource Source = RuntimeValueSource.DynamicField;

            [Tooltip("Used only when Source = DynamicField.")]
            public string FieldName;

            [Tooltip("Optional prefix, e.g. 'Lives: '")]
            public string Prefix = "";

            [Tooltip("Optional suffix, e.g. ' pts'")]
            public string Suffix = "";

            [Tooltip("For level-like values, display +1.")]
            public bool OneBased = false;

            [NonSerialized] public Label ResolvedElement;
        }

        [Serializable]
        public class ProgressBarBinding {
            [Tooltip("Name of the ProgressBar in the UIDocument.")]
            public string ElementName;

            [Tooltip("Which runtime value should drive this ProgressBar.")]
            public RuntimeValueSource Source = RuntimeValueSource.DynamicField;

            [Tooltip("Used only when Source = DynamicField.")]
            public string FieldName;

            [Tooltip("Minimum value for the progress bar.")]
            public int Min = 0;

            [Tooltip("Maximum value for the progress bar.")]
            public int Max = 100;

            [Tooltip("Optional label shown inside the ProgressBar, e.g. '{value}/{max}' or '{value}%'.")]
            public string TitleFormat = "{value}/{max}";

            [NonSerialized] public ProgressBar ResolvedElement;
        }

        [Header("References")]
        [SerializeField] private UIDocument uiDocument;

        [Header("General")]
        [SerializeField] private string missingValueText = "-";

        [Header("Bindings")]
        [SerializeField] private List<LabelBinding> labelBindings = new List<LabelBinding>();
        [SerializeField] private List<ProgressBarBinding> progressBarBindings = new List<ProgressBarBinding>();

        private GameDataManager gameDataManager;
        protected RuntimeGameStateAsset runtimeState;

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
            RefreshAllBindings();
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

            if (runtimeState == null) {
                Debug.LogError($"{nameof(LevelHudController)}: RuntimeState is null on GameDataManager.", this);
                return;
            }

            ResolveUiElements();
        }

        private void ResolveUiElements() {
            VisualElement root = uiDocument.rootVisualElement;
            if (root == null) {
                Debug.LogError($"{nameof(LevelHudController)}: UIDocument rootVisualElement is null.", this);
                return;
            }

            foreach (LabelBinding binding in labelBindings) {
                binding.ResolvedElement = root.Q<Label>(binding.ElementName);
                if (binding.ResolvedElement == null) {
                    Debug.LogWarning($"{nameof(LevelHudController)}: Could not find Label '{binding.ElementName}'.", this);
                }
            }

            foreach (ProgressBarBinding binding in progressBarBindings) {
                binding.ResolvedElement = root.Q<ProgressBar>(binding.ElementName);
                if (binding.ResolvedElement == null) {
                    Debug.LogWarning($"{nameof(LevelHudController)}: Could not find ProgressBar '{binding.ElementName}'.", this);
                }
            }
        }

        private void SubscribeToRuntimeState() {
            if (runtimeState == null) {
                return;
            }

            runtimeState.LevelChanged += OnLevelChanged;
            runtimeState.LatestLevelChanged += OnLatestLevelChanged;
            runtimeState.LivesChanged += OnLivesChanged;
            runtimeState.ContinuesChanged += OnContinuesChanged;
            runtimeState.DifficultyChanged += OnDifficultyChanged;
            runtimeState.FieldChanged += OnFieldChanged;
        }

        private void UnsubscribeFromRuntimeState() {
            if (runtimeState == null) {
                return;
            }

            runtimeState.LevelChanged -= OnLevelChanged;
            runtimeState.LatestLevelChanged -= OnLatestLevelChanged;
            runtimeState.LivesChanged -= OnLivesChanged;
            runtimeState.ContinuesChanged -= OnContinuesChanged;
            runtimeState.DifficultyChanged -= OnDifficultyChanged;
            runtimeState.FieldChanged -= OnFieldChanged;
        }

        private void RefreshAllBindings() {
            if (runtimeState == null) {
                return;
            }

            foreach (LabelBinding binding in labelBindings) {
                RefreshLabelBinding(binding);
            }

            foreach (ProgressBarBinding binding in progressBarBindings) {
                RefreshProgressBarBinding(binding);
            }
        }

        private void OnLevelChanged(int _) {
            RefreshBindingsForSource(RuntimeValueSource.CurrentLevel);
        }

        private void OnLatestLevelChanged(int _) {
            RefreshBindingsForSource(RuntimeValueSource.LatestLevel);
        }

        private void OnLivesChanged(int _) {
            RefreshBindingsForSource(RuntimeValueSource.Lives);
        }

        private void OnContinuesChanged(int _) {
            RefreshBindingsForSource(RuntimeValueSource.Continues);
        }

        private void OnDifficultyChanged(int _) {
            RefreshBindingsForSource(RuntimeValueSource.Difficulty);
        }

        private void OnFieldChanged(string fieldName, int _) {
            RefreshBindingsForField(fieldName);
        }

        private void RefreshBindingsForSource(RuntimeValueSource source) {
            foreach (LabelBinding binding in labelBindings) {
                if (binding.Source == source) {
                    RefreshLabelBinding(binding);
                }
            }

            foreach (ProgressBarBinding binding in progressBarBindings) {
                if (binding.Source == source) {
                    RefreshProgressBarBinding(binding);
                }
            }
        }

        private void RefreshBindingsForField(string fieldName) {
            foreach (LabelBinding binding in labelBindings) {
                if (binding.Source == RuntimeValueSource.DynamicField && string.Equals(binding.FieldName, fieldName, StringComparison.Ordinal)) {
                    RefreshLabelBinding(binding);
                }
            }

            foreach (ProgressBarBinding binding in progressBarBindings) {
                if (binding.Source == RuntimeValueSource.DynamicField && string.Equals(binding.FieldName, fieldName, StringComparison.Ordinal)) {
                    RefreshProgressBarBinding(binding);
                }
            }
        }

        private void RefreshLabelBinding(LabelBinding binding) {
            if (binding == null || binding.ResolvedElement == null || runtimeState == null) {
                return;
            }

            bool hasValue = TryGetValue(binding.Source, binding.FieldName, out int value);

            if (!hasValue) {
                binding.ResolvedElement.text = missingValueText;
                return;
            }

            int displayValue = binding.OneBased ? value + 1 : value;
            binding.ResolvedElement.text = $"{binding.Prefix}{displayValue}{binding.Suffix}";
        }

        private void RefreshProgressBarBinding(ProgressBarBinding binding) {
            if (binding == null || binding.ResolvedElement == null || runtimeState == null) {
                return;
            }

            bool hasValue = TryGetValue(binding.Source, binding.FieldName, out int value);

            if (!hasValue) {
                binding.ResolvedElement.value = binding.Min;
                binding.ResolvedElement.title = missingValueText;
                return;
            }

            float clamped = Mathf.Clamp(value, binding.Min, binding.Max);
            binding.ResolvedElement.lowValue = binding.Min;
            binding.ResolvedElement.highValue = binding.Max;
            binding.ResolvedElement.value = clamped;
            binding.ResolvedElement.title = FormatProgressTitle(binding.TitleFormat, value, binding.Min, binding.Max);
        }

        private bool TryGetValue(RuntimeValueSource source, string fieldName, out int value) {
            value = 0;

            if (runtimeState == null) {
                return false;
            }

            switch (source) {
                case RuntimeValueSource.CurrentLevel:
                    value = runtimeState.CurrentLevel;
                    return value >= 0;

                case RuntimeValueSource.LatestLevel:
                    value = runtimeState.LatestLevel;
                    return value >= 0;

                case RuntimeValueSource.Lives:
                    value = runtimeState.Lives;
                    return true;

                case RuntimeValueSource.Continues:
                    value = runtimeState.Continues;
                    return true;

                case RuntimeValueSource.Difficulty:
                    value = runtimeState.Difficulty;
                    return true;

                case RuntimeValueSource.DynamicField:
                    if (string.IsNullOrEmpty(fieldName) || !runtimeState.HasField(fieldName)) {
                        return false;
                    }
                    value = runtimeState.GetField(fieldName);
                    return true;

                default:
                    return false;
            }
        }

        private string FormatProgressTitle(string format, int value, int min, int max) {
            if (string.IsNullOrEmpty(format)) {
                return value.ToString();
            }

            return format
                .Replace("{value}", value.ToString())
                .Replace("{min}", min.ToString())
                .Replace("{max}", max.ToString());
        }
    }
}
