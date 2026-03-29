using StateEngine.Managers.Shared;
using UnityEngine;

namespace StateEngine.Managers.Global {
    public sealed class GlobalSessionManager : ISessionManager {
        [SerializeField] private bool _sessionOpen;

        public bool SessionOpen => _sessionOpen;

        public void OpenSession() {
            _sessionOpen = true;
        }

        public void CloseSession() {
            _sessionOpen = false;
        }
    }
}
