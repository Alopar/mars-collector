using DG.Tweening;
using GameApplication.Utility;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GameApplication.UI
{
    public class MissionDescriptionView : MonoBehaviour
    {
        [SerializeField] private Transform _mainPanel;
        [SerializeField] private Image _background;
        [SerializeField] private TextMeshProUGUI _mainText;
        [SerializeField] private Button _closeButton;
        [SerializeField] private Button _backgroundButton;

        public void Show()
        {
            gameObject.SetActive(true);
            _background.SetAlpha(0);
            _mainPanel.localScale = Vector3.zero;
            _mainPanel.DOScale(Vector3.one, 0.3f).SetEase(Ease.OutBack);
            _background.DOFade(0.75f, 0.3f);
        }

        public void SetMissionText(string text) =>
            _mainText.text = text;

        private void OnEnable()
        {
            _closeButton.onClick.AddListener(OnCloseClick);
            _backgroundButton.onClick.AddListener(OnCloseClick);
        }

        private void OnDisable()
        {
            _closeButton.onClick.RemoveListener(OnCloseClick);
            _backgroundButton.onClick.RemoveListener(OnCloseClick);
        }

        private void Close()
        {
            gameObject.SetActive(false);
            _mainPanel.DOScale(Vector3.zero, 0.3f).SetEase(Ease.InBack);
            _background.DOFade(0, 0.3f);
        }

        private void OnCloseClick() =>
            Close();
    }
}