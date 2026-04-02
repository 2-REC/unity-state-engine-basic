using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

namespace StateEngine.Samples.Shared {
    public class ButtonController : MonoBehaviour {
        [SerializeField] private UIDocument document;
        [SerializeField] private string buttonId;
        [SerializeField] private UnityEvent OnClick;

        private void OnEnable() {
            VisualElement root = document.rootVisualElement;

            Button myButton = root.Q<Button>(buttonId);
            if (myButton != null) {
                myButton.clicked += OnButtonClicked;
            }
        }

        private void OnButtonClicked() {
            OnClick.Invoke();
        }
    }
}
