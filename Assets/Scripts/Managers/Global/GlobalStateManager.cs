using StateEngine.Managers.Shared;
using UnityEngine;

namespace StateEngine.Managers.Global {
    public sealed class GlobalStateManager : IStateManager {
        [SerializeField] private string _currentState = "Boot";

        public string CurrentState => _currentState;

        public void SetState(string state) {
            _currentState = state;
        }
    }
}
