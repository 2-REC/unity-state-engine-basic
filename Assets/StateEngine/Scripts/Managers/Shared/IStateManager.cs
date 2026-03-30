using StateEngine.Managers.Contracts;
using UnityEngine;

namespace StateEngine.Managers.Shared {
    public abstract class IStateManager : MonoBehaviour {
        private IManagerOwner _owner;

        protected IManagerOwner Owner => _owner;

        protected virtual void Awake() {
            _owner = GetComponent<IManagerOwner>();

            if (_owner == null) {
                Debug.LogError(
                    $"{GetType().Name} on '{name}' requires a component implementing {nameof(IManagerOwner)} on the same GameObject.",
                    this);
            }
        }

        protected void RequestSceneTransition(string sceneName) {
            if (_owner == null) {
                Debug.LogError($"{GetType().Name} cannot request a scene transition because no owner was found.", this);
                return;
            }

            _owner.HandleTransitionRequest(TransitionIntent.LoadScene(sceneName));
        }

        protected void RequestApplicationQuit() {
            if (_owner == null) {
                Debug.LogError($"{GetType().Name} cannot request application quit because no owner was found.", this);
                return;
            }

            _owner.HandleTransitionRequest(TransitionIntent.QuitApplication());
        }

        protected void RequestOwnerTeardown() {
            if (_owner == null) {
                Debug.LogError($"{GetType().Name} cannot request owner teardown because no owner was found.", this);
                return;
            }

            _owner.RequestManagerTeardown();
        }
    }
}
