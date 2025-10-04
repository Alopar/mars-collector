using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace GameApplication.UI.Cargo
{
    public class CargoSlotView : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
    {
        [Header("Components")]
        public Image background;
        public Image content;
        public GameObject highlight;
        
        [Header("Colors")]
        public Color normalColor = Color.white;
        public Color hoverColor = new Color(1f, 1f, 0.5f, 1f);
        
        public int GridX { get; private set; }
        public int GridY { get; private set; }
        
        private CargoGridView _gridView;
        
        public void Initialize(int x, int y, CargoGridView gridView)
        {
            GridX = x;
            GridY = y;
            _gridView = gridView;
            
            Clear();
        }
        
        public void SetContent(Sprite sprite, Color color = default)
        {
            if (content != null)
            {
                content.sprite = sprite;
                content.enabled = true;
                
                if (sprite == null && color != default)
                {
                    content.color = color;
                }
                else if (sprite != null)
                {
                    content.color = Color.white;
                }
            }
        }
        
        public void SetHighlight(Color color)
        {
            if (highlight != null)
            {
                highlight.SetActive(true);
                var img = highlight.GetComponent<Image>();
                if (img != null)
                {
                    img.color = color;
                }
            }
        }
        
        public void ClearHighlight()
        {
            if (highlight != null)
            {
                highlight.SetActive(false);
            }
        }
        
        public void Clear()
        {
            if (content != null)
            {
                content.sprite = null;
                content.enabled = false;
            }
            
            ClearHighlight();
            
            if (background != null)
            {
                background.color = normalColor;
            }
        }
        
        public void OnPointerEnter(PointerEventData eventData)
        {
            if (_gridView != null)
            {
                _gridView.OnSlotHover(GridX, GridY);
            }
        }
        
        public void OnPointerExit(PointerEventData eventData)
        {
            if (_gridView != null)
            {
                _gridView.OnSlotExitHover();
            }
        }
        
        public void OnPointerClick(PointerEventData eventData)
        {
            if (_gridView != null)
            {
                if (eventData.button == PointerEventData.InputButton.Left)
                {
                    _gridView.OnSlotLeftClick(GridX, GridY);
                }
                else if (eventData.button == PointerEventData.InputButton.Right)
                {
                    _gridView.OnSlotRightClick(GridX, GridY);
                }
            }
        }
    }
}

