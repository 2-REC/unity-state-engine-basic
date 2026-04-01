using StateEngine.Managers.Global;

namespace StateEngine.Samples {
    public class SampleGlobalDataManager : GlobalDataManager {
        public int Difficulty {
            get { return GetField("DIFFICULTY"); }
            set { SetField("DIFFICULTY", value); }
        }
    }
}
