using StateEngine.Managers.Game;
using StateEngine.Managers.Global;
using UnityEngine;

namespace StateEngine.Managers.Core {
    public sealed class ManagerHost : MonoBehaviour {
        private static ManagerHost _instance;

        [Header("Manager Prefabs")]
        [SerializeField] private GlobalManager _globalManagerPrefab;
        [SerializeField] private GameManager _gameManagerPrefab;

        public static bool HasInstance => _instance != null;

        public static ManagerHost Instance {
            get {
                if (_instance != null) {
                    return _instance;
                }

                _instance = ManagerLookup.FindSingletonInLoadedWorld<ManagerHost>();
                if (_instance != null) {
                    return _instance;
                }

                throw new MissingReferenceException(
                    $"No {nameof(ManagerHost)} exists in the loaded world. " +
                    "Place a ManagerHost in your bootstrap scene or use ManagerHostBootstrap.");
            }
        }

        public bool HasGlobalManager => GlobalManager.HasInstance;
        public bool HasGameManager => GameManager.HasInstance;

        private void Awake() {
            if (_instance != null && _instance != this) {
                Destroy(gameObject);
                return;
            }

            _instance = this;
            gameObject.name = ManagerConstants.HostObjectName;
            DontDestroyOnLoad(gameObject);
        }

        private void OnDestroy() {
            if (_instance == this) {
                _instance = null;
            }
        }

        public GlobalManager GetOrCreateGlobalManager() {
            if (GlobalManager.HasInstance) {
                return GlobalManager.Current;
            }

            if (_globalManagerPrefab == null) {
                throw new MissingReferenceException(
                    $"{nameof(ManagerHost)} is missing the {nameof(GlobalManager)} prefab reference.");
            }

            GlobalManager created = Instantiate(_globalManagerPrefab);
            if (!GlobalManager.HasInstance) {
                throw new System.InvalidOperationException(
                    $"Instantiated {nameof(GlobalManager)} did not register itself as singleton.");
            }

            return created;
        }

        public GameManager GetOrCreateGameManager() {
            if (GameManager.HasInstance) {
                return GameManager.Current;
            }

            if (_gameManagerPrefab == null) {
                throw new MissingReferenceException(
                    $"{nameof(ManagerHost)} is missing the {nameof(GameManager)} prefab reference.");
            }

            GameManager created = Instantiate(_gameManagerPrefab);
            if (!GameManager.HasInstance) {
                throw new System.InvalidOperationException(
                    $"Instantiated {nameof(GameManager)} did not register itself as singleton.");
            }

            return created;
        }

        public bool TryGetGlobalManager(out GlobalManager manager) {
            manager = GlobalManager.Current;
            return manager != null;
        }

        public bool TryGetGameManager(out GameManager manager) {
            manager = GameManager.Current;
            return manager != null;
        }

        public void DestroyGlobalManager() {
            if (GlobalManager.Current != null) {
                Destroy(GlobalManager.Current.gameObject);
            }
        }

        public void DestroyGameManager() {
            if (GameManager.Current != null) {
                Destroy(GameManager.Current.gameObject);
            }
        }

        public void DestroyAllManagers() {
            DestroyGameManager();
            DestroyGlobalManager();
        }
    }
}
