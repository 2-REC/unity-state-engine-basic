using StateEngine.Managers.Game;
using System.Collections.Generic;

namespace StateEngine.Samples {
    public class SampleGameDataManager : GameDataManager {
        public int Points {
            get { return GetField("POINTS"); }
            set { SetField("POINTS", value); }
        }

        public int Health {
            get { return GetField("HEALTH"); }
            set { SetField("HEALTH", value); }
        }

        protected override IEnumerable<string> GetLifeResetFields() {
            return new List<string> { "HEALTH" };
        }

        protected override IEnumerable<string> GetContinueResetFields() {
            return new List<string> { "POINTS" };
        }
    }
}
