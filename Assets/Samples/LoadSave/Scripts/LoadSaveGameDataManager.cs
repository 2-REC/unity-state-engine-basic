using StateEngine.Managers.Game;
using System.Collections.Generic;

namespace StateEngine.Samples.LoadSave {
    public class LoadSaveGameDataManager : GameDataManager {
        public int Points {
            get => GetField("POINTS");
            set => SetField("POINTS", value);
        }

        public int Health {
            get => GetField("HEALTH");
            set => SetField("HEALTH", value);
        }

        protected override IEnumerable<string> GetLifeResetFields() {
            return new List<string> { "HEALTH" };
        }

        protected override IEnumerable<string> GetContinueResetFields() {
            return new List<string> { "POINTS" };
        }
    }
}
