using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GameApplication.UI
{
    public class MissionButtonView : MonoBehaviour
    {
        [SerializeField] private Button _button;
        [SerializeField] private Image _mainImage;
        [SerializeField] private TextMeshProUGUI _missionText;
        [SerializeField] private MissionDescriptionView _missionDescriptionView;
        [SerializeField] private Color _newColor;
        [SerializeField] private Color _inactiveColor;

        public void Show()
        {
            SetDefault();
            _button.interactable = true;
        }

        public void Hide()
        {
            _button.interactable = false;
            _mainImage.color = _inactiveColor;
        }

        public void SetNew() =>
            _mainImage.color = _newColor;

        public void SetDefault() =>
            _mainImage.color = Color.white;

        public void SetMissionText(string text) =>
            _missionText.text = text;

        private void OnEnable() =>
            _button.onClick.AddListener(OnButtonClick);

        private void OnDisable() =>
            _button.onClick.RemoveListener(OnButtonClick);

        private void OnButtonClick()
        {
            _missionDescriptionView.Show();
            SetDefault();
        }
    }
}