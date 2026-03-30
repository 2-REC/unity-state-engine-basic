using StateEngine.Managers.Contracts;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace StateEngine.Managers.Core {
    public abstract class ManagerBase : MonoBehaviour, IManagerOwner {
        private bool _initialized;
        private bool _isTransitionInProgress;

        protected abstract bool TryRegisterAsSingleton();
        protected abstract void UnregisterAsSingleton();
        protected abstract void CacheChildComponents();
        protected abstract void DestroyViaHost();

        protected virtual void Awake() {
            if (!TryRegisterAsSingleton()) {
                Destroy(gameObject);
                return;
            }

            DontDestroyOnLoad(gameObject);
            CacheChildComponents();
            _initialized = true;
        }

        protected virtual void OnDestroy() {
            if (_initialized) {
                UnregisterAsSingleton();
            }
        }

        public void HandleTransitionRequest(Contracts.TransitionIntent intent) {
            if (_isTransitionInProgress) {
                Debug.LogWarning($"{GetType().Name} ignored a transition request because a transition is already in progress.", this);
                return;
            }

            _isTransitionInProgress = true;

            OnBeforeTransition(intent);
            PerformTeardownForTransition(intent);
            ExecuteTransition(intent);
            OnAfterTransitionTriggered(intent);
        }

        public void RequestManagerTeardown() {
            OnBeforeTeardown();
            DestroyViaHost();
            OnAfterTeardownRequested();
        }

        protected virtual void OnBeforeTransition(Contracts.TransitionIntent intent) { }

        protected virtual void OnAfterTransitionTriggered(Contracts.TransitionIntent intent) { }

        protected virtual void OnBeforeTeardown() { }

        protected virtual void OnAfterTeardownRequested() { }

        protected virtual void PerformTeardownForTransition(Contracts.TransitionIntent intent) {
            RequestManagerTeardown();
        }

        protected virtual void ExecuteTransition(Contracts.TransitionIntent intent) {
            switch (intent.Type) {
                case Contracts.TransitionIntentType.LoadScene:
                    if (string.IsNullOrWhiteSpace(intent.SceneName)) {
                        Debug.LogError($"{GetType().Name} received a LoadScene transition intent with an empty scene name.", this);
                        return;
                    }

                    SceneManager.LoadScene(intent.SceneName);
                    break;

                case Contracts.TransitionIntentType.QuitApplication:
                    Application.Quit();
                    break;

                default:
                    Debug.LogError($"{GetType().Name} received an unsupported transition type: {intent.Type}.", this);
                    break;
            }
        }
    }
}
