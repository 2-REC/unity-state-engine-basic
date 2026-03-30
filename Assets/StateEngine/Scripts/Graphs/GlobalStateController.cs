using StateEngine.Managers.Global;
using StateEngine.Managers.Shared;
using UnityEngine;

namespace StateEngine.Graphs {
    public class GlobalStateController : IStateController {
        protected GlobalManager globalManager;

        protected override void Awake() {
Debug.Log("GlobalStateController::Awake");
            globalManager = GlobalManager.Instance;
Debug.Log("GlobalStateController::Awake - INSTANCE: " + GlobalManager.Instance);
            base.Awake();
        }
        protected override IStateManager GetStateManager() {
Debug.Log("GlobalStateController::GetStateManager - INSTANCE: " + globalManager);
            return globalManager.State;
        }
    }
}
