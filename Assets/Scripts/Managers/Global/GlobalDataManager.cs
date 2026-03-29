using StateEngine.Managers.Shared;
using UnityEngine;

namespace StateEngine.Managers.Global {
    public sealed class GlobalDataManager : IDataManager {
        [SerializeField] private int _profileId;

        public int ProfileId => _profileId;

        public void SetProfileId(int profileId) {
            _profileId = profileId;
        }
    }
}
