using StateEngine.Graphs;
using StateEngine.Managers.Shared;
using UnityEngine;

namespace StateEngine.Managers.Game {
    public sealed class GameStateManager : IStateManager {

        // TODO: from previous impl => keep?
        //[SerializeField] private string _currentState = "Gameplay";
        //public string CurrentState => _currentState;
        /*
        public void SetState(string state) {
            _currentState = state;
        }
        */

        // TODO: could be moved to base class
        [SerializeField] private TextAsset xmlGraph;

        /*
        // TODO: add to check no other instance?
        protected override void Awake() {
            base.Awake();
            Load(new GameGraphLoader(xmlGraph));
        }
        */

        protected override void Load() {
            Load(new GlobalGraphLoader(xmlGraph));
        }

        // TODO: not really useful...?
        /*
        public void LeaveToScene(string sceneName) {
            RequestSceneTransition(sceneName);
        }

        public void QuitApplicationFromState() {
            RequestApplicationQuit();
        }

        public void DestroyOwnerOnly() {
            RequestOwnerTeardown();
        }
        */

        protected override string GetSceneName(State state) {
            GameState gameState = (GameState)state;
            if (gameState.IsLevel) {
                // TODO: or get from 'GameManager'?
                string effectiveScene = ((GameDataManager)GetDataManager()).GetSceneName();
                if ((effectiveScene != null) && !"".Equals(effectiveScene)) {
                    return effectiveScene;
                }
            }
            return state.Scene;
        }
    }
}
