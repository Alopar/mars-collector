using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GameApplication.UI
{
    public class MissionButtonView : MonoBehaviour
    {
        [SerializeField] private Button _button;
        [SerializeField] private TextMeshProUGUI _missionText;
        [SerializeField] private MissionDescriptionView _missionDescriptionView;

        public void Show() =>
            gameObject.SetActive(true);

        public void Hide() =>
            gameObject.SetActive(false);

        public void SetMissionText(string text) =>
            _missionText.text = text;

        private void OnEnable() =>
            _button.onClick.AddListener(OnButtonClick);

        private void OnDisable() =>
            _button.onClick.RemoveListener(OnButtonClick);

        private void OnButtonClick() =>
            _missionDescriptionView.Show();
    }
}