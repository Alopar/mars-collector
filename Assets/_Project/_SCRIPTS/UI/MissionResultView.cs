using TMPro;
using UnityEngine;

namespace GameApplication.UI
{
    public class MissionResultView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _resultText;

        public void Show() =>
            gameObject.SetActive(true);

        public void Hide() =>
            gameObject.SetActive(false);

        public void SetText(string text) =>
            _resultText.text = text;
    }
}