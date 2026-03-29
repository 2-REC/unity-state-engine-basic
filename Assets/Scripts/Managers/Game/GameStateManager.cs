using StateEngine.Managers.Shared;
using UnityEngine;

namespace StateEngine.Managers.Game {
    public sealed class GameStateManager : IStateManager {
        [SerializeField] private string _currentState = "Gameplay";

        public string CurrentState => _currentState;

        public void SetState(string state) {
            _currentState = state;
        }
    }
}
