using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using GameApplication.Gameplay.Managers;
using GameApplication.Gameplay.Models;

namespace GameApplication.UI
{
    public class MarsStatsView : MonoBehaviour
    {
        [Header("Bars")]
        public Slider weaponsBar;
        public Slider suppliesBar;
        public Slider peopleBar;

        [Header("Value Texts")]
        public TextMeshProUGUI weaponsText;
        public TextMeshProUGUI suppliesText;
        public TextMeshProUGUI peopleText;

        [Header("Preview Texts")]
        public TextMeshProUGUI weaponsPreview;
        public TextMeshProUGUI suppliesPreview;
        public TextMeshProUGUI peoplePreview;

        [Header("Colors")]
        public Color safeColor = Color.green;
        public Color warningColor = Color.yellow;
        public Color dangerColor = Color.red;

        private void Start()
        {
            if (MarsManager.Instance != null)
            {
                MarsManager.Instance.OnColonyStateChanged += UpdateBars;
                MarsManager.Instance.OnPreviewUpdated += UpdatePreview;
            }
        }

        private void UpdateBars(MarsColonyState state)
        {
            weaponsBar.value = state.Weapons / 100f;
            suppliesBar.value = state.Supplies / 100f;
            peopleBar.value = state.People / 100f;

            weaponsText.text = $"Оружие: {state.Weapons}/100";
            suppliesText.text = $"Припасы: {state.Supplies}/100";
            peopleText.text = $"Люди: {state.People}/100";

            UpdateBarColors(state);
        }

        private void UpdateBarColors(MarsColonyState state)
        {
            if (weaponsBar.fillRect != null)
            {
                var image = weaponsBar.fillRect.GetComponent<Image>();
                if (image != null) image.color = GetColorForValue(state.Weapons);
            }

            if (suppliesBar.fillRect != null)
            {
                var image = suppliesBar.fillRect.GetComponent<Image>();
                if (image != null) image.color = GetColorForValue(state.Supplies);
            }

            if (peopleBar.fillRect != null)
            {
                var image = peopleBar.fillRect.GetComponent<Image>();
                if (image != null) image.color = GetColorForValue(state.People);
            }
        }

        private Color GetColorForValue(int value)
        {
            if (value <= 20 || value >= 80) return dangerColor;
            if (value <= 40 || value >= 60) return warningColor;
            return safeColor;
        }

        private void UpdatePreview(Dictionary<ResourceType, int> preview)
        {
            if (preview.ContainsKey(ResourceType.Weapons))
            {
                int val = preview[ResourceType.Weapons];
                weaponsPreview.text = val >= 0 ? $"+{val}" : val.ToString();
                weaponsPreview.color = val >= 0 ? Color.green : Color.red;
            }
            else
            {
                weaponsPreview.text = "";
            }

            if (preview.ContainsKey(ResourceType.Supplies))
            {
                int val = preview[ResourceType.Supplies];
                suppliesPreview.text = val >= 0 ? $"+{val}" : val.ToString();
                suppliesPreview.color = val >= 0 ? Color.green : Color.red;
            }
            else
            {
                suppliesPreview.text = "";
            }

            if (preview.ContainsKey(ResourceType.People))
            {
                int val = preview[ResourceType.People];
                peoplePreview.text = val >= 0 ? $"+{val}" : val.ToString();
                peoplePreview.color = val >= 0 ? Color.green : Color.red;
            }
            else
            {
                peoplePreview.text = "";
            }
        }

        private void OnDestroy()
        {
            if (MarsManager.Instance != null)
            {
                MarsManager.Instance.OnColonyStateChanged -= UpdateBars;
                MarsManager.Instance.OnPreviewUpdated -= UpdatePreview;
            }
        }
    }
}

