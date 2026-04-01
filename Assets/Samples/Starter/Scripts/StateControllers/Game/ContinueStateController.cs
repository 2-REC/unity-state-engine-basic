using StateEngine.Graphs;
using StateEngine.Managers.Game;
using UnityEngine;

namespace StateEngine.Samples.Starter {
    public class ContinueStateController : GameStateController {
        public int NbContinues { get; private set; } = 0;

        public override void HandleMainState() {
            NbContinues = GameManager.Instance.Data.GetContinues();
            Debug.Log($"Nb Continues: {NbContinues}");
        }

        public void Continue() {
            NbContinues = GameManager.Instance.Data.LoseContinue();
            End();
        }

        public void Stop() {
            LoadChildState("QUIT_GAME");
        }
    }
}
