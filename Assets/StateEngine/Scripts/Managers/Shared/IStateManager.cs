using StateEngine.Graphs;
using StateEngine.Managers.Contracts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace StateEngine.Managers.Shared {
    public delegate bool OnStateChangeHandler();

    public abstract class IStateManager : MonoBehaviour {
        // TODO: or set by root/boot?
        [SerializeField] private IDataManager dataManager;

        private IManagerOwner _owner;
        protected IManagerOwner Owner => _owner;

        public event OnStateChangeHandler OnStateChange;
        public int CurrentStateId { get; private set; }

        private State[] states;
        private Stack<int> stack;

        private AsyncOperation async;
        private bool isAsync;

        protected IStateManager() {
            Debug.Log("NEW STATE MANAGER: " + GetType());
            isAsync = false;

            // TODO: make sure ok here (needed BEFORE loading new graph)
            StateIds.Reset();

            stack = new Stack<int>();
            stack.Push(StateIds.NONE);
            CurrentStateId = StateIds.NONE;
        }

        protected virtual void Awake() {
            _owner = GetComponent<IManagerOwner>();

            if (_owner == null) {
                Debug.LogError(
                    $"{GetType().Name} on '{name}' requires a component implementing {nameof(IManagerOwner)} on the same GameObject.",
                    this);
            }

            Load();
        }

        /*
        //void Load() {
        public void Load() {
            GraphLoader graphLoader = new GraphLoader(GRAPH_XML);
            states = graphLoader.LoadStateGraph();
        }
        */
        public void Load(IGraphLoader graphLoader) {
            states = graphLoader.LoadStateGraph();
            Debug.Log("IStateManager::Load states: " + states);
        }

        /*
        public static IStateManager Instance {
            get {
                return instance;
            }
        }
        public static bool IsInstance() {
            Debug.Log("IsInstance: " + instance);
            return (instance != null);
        }

        public static void SetInstance(IStateManager ins) {
            if (IsInstance()) {
                throw new Exception("INSTANCE ALREADY SET");
            }
            instance = ins;
        }
        */

        public State GetState(int stateId) {
            return states[stateId];
        }

        public int GetStateId(string sceneName) {
            Debug.Log("IStateManager::GetStateId: " + states);
            foreach (State state in states) {
                if (GetSceneName(state) == sceneName) {
                    return state.Id;
                }
            }
            return StateIds.NONE;
        }

        public void SetState(int stateId, bool push = true) {
            // TODO: does this cause issues? (when is this happening?)
            //if (CurrentStateId != stateId) {
            if (true) {
                CurrentStateId = stateId;
                Debug.Log($"PUSH (SetState): {CurrentStateId}");
                if (push)
                    stack.Push(stateId);
            }

            bool handled = OnStateChange();

            if (!handled) {
                LoadState(stateId);
            } else {
                State state = states[CurrentStateId];
                if (state.Children == null && !state.Leavable) {
                    state = states[state.Next];
                    if (StateIds.NONE != state.Id) {
                        AsyncLoadScene();
                    }
                }
            }
        }

        // TODO: REWRITE ENTIRE METHOD!
        public void NextState() {
            CurrentStateId = stack.Pop();
            Debug.Log($"POP (NextState): {CurrentStateId}");

            State state = states[CurrentStateId];
            int next = state.Next;
            if (next != StateIds.NONE) {
                LoadState(next);
                return;
            }

            if (stack.Count == 0) {
                Debug.Log("Stack is empty => Leaving graph");
                LeaveGraph();
                return;
            }

            //CurrentStateId = stack.Peek();
            CurrentStateId = stack.Pop();
            State currentState = states[CurrentStateId];

            if (!currentState.Restartable) {
                //                stack.Pop();

                int stateId = currentState.Next;
                // TODO: correct test "CurrentStateId != StateIds.NONE"? (seems useless)
                while ((CurrentStateId != StateIds.NONE) && (stateId == StateIds.NONE)) {
                    //while ((CurrentStateId != StateIds.NONE) && (stateId == StateIds.NONE || !states[stateId].Restartable)) {
                    if (stack.Count == 0) {
                        Debug.Log("Stack is empty => Leaving graph");
                        LeaveGraph();
                        return;
                    }

                    /*
                    CurrentStateId = stack.Peek();
                    currentState = states[CurrentStateId];
                    if (currentState.Restartable) {
                        LoadState(CurrentStateId);
                        return;
                    }
                    stack.Pop();
                    */
                    CurrentStateId = stack.Pop();
                    currentState = states[CurrentStateId];
                    if (currentState.Restartable) {
                        LoadState(CurrentStateId);
                        return;
                    }

                    stateId = currentState.Next;
                }
                LoadState(stateId);
            } else {
                LoadState(CurrentStateId);
            }
        }

        private void LoadState(int stateId) {
            // TODO: does this cause issues? (needed if want same state as 'next' state, eg: LEVEL)
            //if (CurrentStateId != stateId) {
            if (true) {
                CurrentStateId = stateId;
                //                stack.Push(stateId);
            }

            State state = states[CurrentStateId];
            string scene = GetSceneName(state);
            if (!string.IsNullOrEmpty(scene)) {
                if (isAsync) {
                    ActivateScene();
                } else {
                    SceneManager.LoadScene(scene);
                }
            } else {
                // TODO: should be an error (?)
                Terminate();
            }
        }

        /*
        public void OnApplicationQuit() {
            // TODO: do something?
            //instance = null;
        }
        */

        public void AsyncLoadScene() {
            StartCoroutine("LoadScene");
        }


        IEnumerator LoadScene() {
            //TODO: remove when finished developing?
            Debug.LogWarning("ASYNC LOAD STARTED - " + "DO NOT EXIT PLAY MODE UNTIL SCENE LOADS... UNITY WILL CRASH");

            State state = states[CurrentStateId];
            state = states[state.Next];

            async = SceneManager.LoadSceneAsync(GetSceneName(state));

            isAsync = true;
            async.allowSceneActivation = false;
            yield return async;
        }

        public void ActivateScene() {
            isAsync = false;
            async.allowSceneActivation = true;
        }

        public void LeaveGraph(string exitScene = "") {
            dataManager.Leave();

            if (!string.IsNullOrEmpty(exitScene)) {
                /*
                //TODO: should be done in 'parent' (galobalmanager/boot)
                //instance = null;
                Destroy(gameObject);
                SceneManager.LoadScene(exitScene);
                */
                RequestSceneTransition(exitScene);
            } else {
                Terminate();
            }
        }

        public void Terminate() {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.ExitPlaymode();
#else
            RequestApplicationQuit();
#endif
        }

        protected void RequestSceneTransition(string sceneName) {
            if (_owner == null) {
                Debug.LogError($"{GetType().Name} cannot request a scene transition because no owner was found.", this);
                return;
            }

            _owner.HandleTransitionRequest(TransitionIntent.LoadScene(sceneName));
        }

        protected void RequestApplicationQuit() {
            if (_owner == null) {
                Debug.LogError($"{GetType().Name} cannot request application quit because no owner was found.", this);
                return;
            }

            _owner.HandleTransitionRequest(TransitionIntent.QuitApplication());
        }

        protected void RequestOwnerTeardown() {
            if (_owner == null) {
                Debug.LogError($"{GetType().Name} cannot request owner teardown because no owner was found.", this);
                return;
            }

            _owner.RequestManagerTeardown();
        }

        // TODO: not really useful...?
        public void LeaveToScene(string sceneName) {
            RequestSceneTransition(sceneName);
        }
        public void QuitApplicationFromState() {
            RequestApplicationQuit();
        }
        public void DestroyOwnerOnly() {
            RequestOwnerTeardown();
        }

        /*
        public void SetDataManager(IDataManager dataManager) {
            if (this.dataManager == null) {
                this.dataManager = dataManager;
            }
        }
        */
        public IDataManager GetDataManager() {
            return dataManager;
        }

        protected virtual string GetSceneName(State state) {
            return state.Scene;
        }

        protected abstract void Load();
    }
}
