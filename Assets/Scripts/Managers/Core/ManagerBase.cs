using UnityEngine;

namespace StateEngine.Managers.Core {
    public abstract class ManagerBase : MonoBehaviour {
        private bool _initialized;

        protected abstract bool TryRegisterAsSingleton();
        protected abstract void UnregisterAsSingleton();
        protected abstract void CacheChildComponents();

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
    }
}
