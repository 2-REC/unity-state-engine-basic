using StateEngine.Managers.Core;
//using StateEngine.Managers.Shared;
using UnityEngine;

namespace StateEngine.Managers.Global {
    public sealed class GlobalManager : ManagerBase {
        private static GlobalManager _instance;

        [Header("Child Managers")]
        [SerializeField] private GlobalStateManager _state;
        [SerializeField] private GlobalSessionManager _session;
        [SerializeField] private GlobalDataManager _data;

        public static bool HasInstance => _instance != null;
        public static GlobalManager Current => _instance;

        public static GlobalManager Instance {
            get {
                if (_instance != null) {
                    return _instance;
                }

                return ManagerHost.Instance.GetOrCreateGlobalManager();
            }
        }

        public GlobalStateManager State => _state;
        public GlobalSessionManager Session => _session;
        public GlobalDataManager Data => _data;

        protected override bool TryRegisterAsSingleton() {
            if (_instance != null && _instance != this) {
                return false;
            }

            _instance = this;
            return true;
        }

        protected override void UnregisterAsSingleton() {
            if (_instance == this) {
                _instance = null;
            }
        }

        protected override void CacheChildComponents() {
            _state ??= GetComponent<GlobalStateManager>();
            _session ??= GetComponent<GlobalSessionManager>();
            _data ??= GetComponent<GlobalDataManager>();

            ValidateReferences();
        }

        private void ValidateReferences() {
            if (_state == null) {
                Debug.LogError($"{nameof(GlobalManager)} on '{name}' is missing {nameof(GlobalStateManager)}.", this);
            }

            if (_session == null) {
                Debug.LogError($"{nameof(GlobalManager)} on '{name}' is missing {nameof(GlobalSessionManager)}.", this);
            }

            if (_data == null) {
                Debug.LogError($"{nameof(GlobalManager)} on '{name}' is missing {nameof(GlobalDataManager)}.", this);
            }
        }
    }
}
