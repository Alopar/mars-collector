using GameApplication.Gameplay.Models;
using GameApplication.Gameplay.Models.Cargo;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GameApplication.UI.Cargo
{
    public class CargoShapeButton : MonoBehaviour
    {
        [Header("References")]
        public Button button;
        public Transform shapeGridContainer;
        public GameObject cellPrefab;
        public TextMeshProUGUI amountText;
        public GameObject selectionFrame;
        
        public CargoShapeData ShapeData { get; private set; }
        
        private CargoShapePanel _panel;
        
        public void Initialize(CargoShapeData shapeData, CargoShapePanel panel)
        {
            ShapeData = shapeData;
            _panel = panel;
            
            GenerateShapePreview(shapeData);
            
            if (amountText != null)
            {
                amountText.text = $"x{shapeData.resourceAmount}";
            }
            
            if (button != null)
            {
                button.onClick.AddListener(OnClick);
            }
            
            SetSelected(false);
        }
        
        private void GenerateShapePreview(CargoShapeData shapeData)
        {
            if (shapeGridContainer == null || cellPrefab == null)
                return;
            
            foreach (Transform child in shapeGridContainer)
            {
                Destroy(child.gameObject);
            }
            
            var gridLayout = shapeGridContainer.GetComponent<GridLayoutGroup>();
            if (gridLayout != null)
            {
                gridLayout.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
                gridLayout.constraintCount = shapeData.shapeWidth;
            }
            
            var shape = shapeData.GetShape();
            
            for (int y = 0; y < shapeData.shapeHeight; y++)
            {
                for (int x = 0; x < shapeData.shapeWidth; x++)
                {
                    GameObject cellObj = Instantiate(cellPrefab, shapeGridContainer);
                    var cellView = cellObj.GetComponent<CargoShapeCellView>();
                    
                    if (cellView != null)
                    {
                        if (shape[y, x] == 1)
                        {
                            if (shapeData.icon != null)
                            {
                                cellView.SetIcon(shapeData.icon);
                            }
                            else
                            {
                                cellView.SetColor(GetColorForType(shapeData.resourceType));
                            }
                        }
                        else
                        {
                            cellView.Hide();
                        }
                    }
                }
            }
        }
        
        private Color GetColorForType(ResourceType type)
        {
            switch (type)
            {
                case ResourceType.Weapons: return new Color(1f, 0.3f, 0.3f);
                case ResourceType.Supplies: return new Color(0.3f, 1f, 0.3f);
                case ResourceType.People: return new Color(0.3f, 0.3f, 1f);
                default: return Color.white;
            }
        }
        
        private void OnClick()
        {
            if (_panel != null)
            {
                _panel.OnShapeButtonClicked(this);
            }
        }
        
        public void SetSelected(bool selected)
        {
            if (selectionFrame != null)
            {
                selectionFrame.SetActive(selected);
            }
        }
    }
}

