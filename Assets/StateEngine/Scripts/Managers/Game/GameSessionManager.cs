using StateEngine.Managers.Shared;
using UnityEngine;

namespace StateEngine.Managers.Game {
    public sealed class GameSessionManager : ISessionManager {
        [SerializeField] private bool _runActive;

        public bool RunActive => _runActive;

        public void BeginRun() {
            _runActive = true;
        }

        public void EndRun() {
            _runActive = false;
        }
    }
}
