using UnityEngine;
using UnityEngine.UI;

namespace GameApplication.UI
{
    public class TutorialView : MonoBehaviour
    {
        [SerializeField] private GameObject _tutorialPanel;
        [SerializeField] private Button _closeButton;
        [SerializeField] private Button _openButton;

        private void Awake()
        {
            _closeButton.onClick.AddListener(Hide);
            _openButton.onClick.AddListener(Show);
        }

        public void Show() =>
            _tutorialPanel.SetActive(true);

        public void Hide() =>
            _tutorialPanel.SetActive(false);
    }
}

