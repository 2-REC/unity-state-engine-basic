
namespace StateEngine.Managers.Contracts {
    public readonly struct TransitionIntent {
        public TransitionIntentType Type { get; }
        public string SceneName { get; }

        public TransitionIntent(TransitionIntentType type, string sceneName = null) {
            Type = type;
            SceneName = sceneName;
        }

        public static TransitionIntent LoadScene(string sceneName) {
            return new TransitionIntent(TransitionIntentType.LoadScene, sceneName);
        }

        public static TransitionIntent QuitApplication() {
            return new TransitionIntent(TransitionIntentType.QuitApplication);
        }
    }
}
