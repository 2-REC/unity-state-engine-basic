using StateEngine.Managers.Shared;
using UnityEngine;

namespace StateEngine.Managers.Game {
    public sealed class GameDataManager : IDataManager {
        [SerializeField] private int _currentLevelIndex;

        public int CurrentLevelIndex => _currentLevelIndex;

        public void SetCurrentLevelIndex(int levelIndex) {
            _currentLevelIndex = levelIndex;
        }
    }
}
