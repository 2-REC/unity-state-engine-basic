using UnityEngine;

namespace StateEngine.Managers.Core {
    /// <summary>
    /// Optional helper to ensure a ManagerHost exists when entering Play Mode from arbitrary scenes.
    ///
    /// Usage:
    /// - Add this to a tiny bootstrap prefab or scene object.
    /// - Assign the ManagerHost prefab in the inspector.
    /// - If a host already exists, this bootstrap destroys itself.
    /// </summary>
    public sealed class ManagerHostBootstrap : MonoBehaviour {
        [SerializeField] private ManagerHost _managerHostPrefab;

        private void Awake() {
            if (ManagerHost.HasInstance) {
                Destroy(gameObject);
                return;
            }

            if (_managerHostPrefab == null) {
                Debug.LogError($"{nameof(ManagerHostBootstrap)} on '{name}' has no ManagerHost prefab assigned.", this);
                return;
            }

            Instantiate(_managerHostPrefab);
            Destroy(gameObject);
        }
    }
}
