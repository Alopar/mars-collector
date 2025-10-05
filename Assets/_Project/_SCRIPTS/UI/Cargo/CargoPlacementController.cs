using GameApplication.Gameplay.Managers;
using GameApplication.Gameplay.Models;
using GameApplication.Gameplay.Models.Cargo;
using UnityEngine;

namespace GameApplication.UI.Cargo
{
    public class CargoPlacementController : MonoBehaviour
    {
        [Header("References")]
        public CargoGridView gridView;
        public CargoShapePanel weaponsPanel;
        public CargoShapePanel suppliesPanel;
        public CargoShapePanel peoplePanel;
        
        private CargoShapeData _selectedShape;
        private Vector2Int _currentGridPos = new Vector2Int(-1, -1);
        private bool _isValidPlacement;
        private bool _initialized = false;
        
        private void Start()
        {
            if (gridView != null)
            {
                gridView.SetController(this);
            }
            
            TryInitializePanels();
        }
        
        private void Update()
        {
            if (!_initialized)
            {
                TryInitializePanels();
            }
        }
        
        private void TryInitializePanels()
        {
            if (_initialized)
                return;
            
            if (CargoManager.Instance == null || CargoManager.Instance.Database == null || CargoManager.Instance.Grid == null)
            {
                return;
            }
            
            var database = CargoManager.Instance.Database;
            
            if (weaponsPanel != null)
            {
                weaponsPanel.Initialize(database.GetShapesForType(ResourceType.Weapons), this);
                Debug.Log($"Weapons panel initialized with {database.GetShapesForType(ResourceType.Weapons).Count} shapes");
            }
            
            if (suppliesPanel != null)
            {
                suppliesPanel.Initialize(database.GetShapesForType(ResourceType.Supplies), this);
                Debug.Log($"Supplies panel initialized with {database.GetShapesForType(ResourceType.Supplies).Count} shapes");
            }
            
            if (peoplePanel != null)
            {
                peoplePanel.Initialize(database.GetShapesForType(ResourceType.People), this);
                Debug.Log($"People panel initialized with {database.GetShapesForType(ResourceType.People).Count} shapes");
            }
            
            _initialized = true;
            Debug.Log("CargoPlacementController fully initialized");
        }
        
        public void SelectShape(CargoShapeData shape)
        {
            if (weaponsPanel != null) weaponsPanel.DeselectAll();
            if (suppliesPanel != null) suppliesPanel.DeselectAll();
            if (peoplePanel != null) peoplePanel.DeselectAll();
            
            _selectedShape = shape;
            Debug.Log($"Selected shape: {shape.shapeName}");
        }
        
        public void DeselectShape()
        {
            _selectedShape = null;
            
            if (gridView != null)
            {
                gridView.HideGhostPreview();
            }
            
            if (weaponsPanel != null) weaponsPanel.DeselectAll();
            if (suppliesPanel != null) suppliesPanel.DeselectAll();
            if (peoplePanel != null) peoplePanel.DeselectAll();
            
            Debug.Log("Shape deselected");
        }
        
        public void OnGridHover(int x, int y)
        {
            if (_selectedShape == null)
                return;
            
            _currentGridPos = new Vector2Int(x, y);
            UpdateGhostPreview();
        }
        
        public void OnGridExitHover()
        {
            if (gridView != null)
            {
                gridView.HideGhostPreview();
            }
        }
        
        public void OnGridLeftClick(int x, int y)
        {
            if (_selectedShape == null)
                return;
            
            _currentGridPos = new Vector2Int(x, y);
            
            _isValidPlacement = CargoManager.Instance.Grid.CanPlaceShape(_selectedShape, x, y);
            
            if (_isValidPlacement)
            {
                CargoManager.Instance.PlaceShape(_selectedShape, x, y);
            }
        }
        
        public void OnGridRightClick(int x, int y)
        {
            if (_selectedShape != null)
            {
                DeselectShape();
            }
            else
            {
                bool removed = CargoManager.Instance.RemoveShapeAt(x, y);
                
                if (!removed)
                {
                    Debug.Log("Nothing to remove at this position");
                }
            }
        }
        
        private void UpdateGhostPreview()
        {
            if (_selectedShape == null || gridView == null)
                return;
            
            _isValidPlacement = CargoManager.Instance.Grid.CanPlaceShape(
                _selectedShape,
                _currentGridPos.x,
                _currentGridPos.y
            );
            
            gridView.ShowGhostPreview(_selectedShape, _currentGridPos, _isValidPlacement);
        }
    }
}

