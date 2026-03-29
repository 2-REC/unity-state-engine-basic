using StateEngine.Managers.Core;
//using StateEngine.Managers.Shared;
using UnityEngine;

namespace StateEngine.Managers.Game {
    public sealed class GameManager : ManagerBase {
        private static GameManager _instance;

        [Header("Child Managers")]
        [SerializeField] private GameStateManager _state;
        [SerializeField] private GameSessionManager _session;
        [SerializeField] private GameDataManager _data;

        public static bool HasInstance => _instance != null;
        public static GameManager Current => _instance;

        public static GameManager Instance {
            get {
                if (_instance != null) {
                    return _instance;
                }

                return ManagerHost.Instance.GetOrCreateGameManager();
            }
        }

        public GameStateManager State => _state;
        public GameSessionManager Session => _session;
        public GameDataManager Data => _data;

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
            _state ??= GetComponent<GameStateManager>();
            _session ??= GetComponent<GameSessionManager>();
            _data ??= GetComponent<GameDataManager>();

            ValidateReferences();
        }

        private void ValidateReferences() {
            if (_state == null) {
                Debug.LogError($"{nameof(GameManager)} on '{name}' is missing {nameof(GameStateManager)}.", this);
            }

            if (_session == null) {
                Debug.LogError($"{nameof(GameManager)} on '{name}' is missing {nameof(GameSessionManager)}.", this);
            }

            if (_data == null) {
                Debug.LogError($"{nameof(GameManager)} on '{name}' is missing {nameof(GameDataManager)}.", this);
            }
        }
    }
}
