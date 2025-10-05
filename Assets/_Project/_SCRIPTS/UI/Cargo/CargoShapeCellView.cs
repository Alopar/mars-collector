using UnityEngine;
using UnityEngine.UI;

namespace GameApplication.UI.Cargo
{
    public class CargoShapeCellView : MonoBehaviour
    {
        [Header("References")]
        public Image cellImage;
        
        public void SetIcon(Sprite icon)
        {
            if (cellImage != null)
            {
                cellImage.sprite = icon;
                cellImage.enabled = true;
            }
        }
        
        public void SetColor(Color color)
        {
            if (cellImage != null)
            {
                cellImage.color = color;
                cellImage.enabled = true;
            }
        }
        
        public void Hide()
        {
            if (cellImage != null)
            {
                cellImage.enabled = false;
            }
        }
    }
}
