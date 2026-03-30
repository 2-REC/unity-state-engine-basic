
namespace StateEngine.Managers.Contracts {
    public interface ITransitionRequestHandler {
        void HandleTransitionRequest(TransitionIntent intent);
    }
}
