using System.Collections.Generic;
using GameApplication.Gameplay.Managers;
using GameApplication.Gameplay.Models;
using GameApplication.Gameplay.Models.Cargo;
using UnityEngine;
using UnityEngine.UI;

namespace GameApplication.UI.Cargo
{
    public class CargoGridView : MonoBehaviour
    {
        [Header("Settings")]
        public float cellSize = 64f;
        public float cellSpacing = 2f;
        
        private int gridWidth;
        private int gridHeight;
        
        [Header("Prefabs")]
        public GameObject cellPrefab;
        
        [Header("Colors")]
        public Color validPlacementColor = new Color(0, 1, 0, 0.5f);
        public Color invalidPlacementColor = new Color(1, 0, 0, 0.5f);
        public Color hoverShapeColor = new Color(1f, 1f, 0f, 0.5f);
        
        private CargoSlotView[,] _slots;
        private CargoPlacementController _controller;
        private bool _initialized = false;
        
        private void Start()
        {
            TryInitialize();
        }
        
        private void Update()
        {
            if (!_initialized)
            {
                TryInitialize();
            }
        }
        
        private void TryInitialize()
        {
            if (_initialized)
                return;
            
            if (CargoManager.Instance == null || CargoManager.Instance.Grid == null)
                return;
            
            gridWidth = CargoManager.Instance.Grid.Width;
            gridHeight = CargoManager.Instance.Grid.Height;
            
            GenerateGrid();
            
            CargoManager.Instance.OnShapePlaced += OnShapePlaced;
            CargoManager.Instance.OnShapeRemoved += OnShapeRemoved;
            CargoManager.Instance.OnCargoCleared += OnCargoCleared;
            
            _initialized = true;
            Debug.Log($"CargoGridView initialized with size {gridWidth}x{gridHeight}");
        }
        
        public void SetController(CargoPlacementController controller)
        {
            _controller = controller;
        }
        
        private void GenerateGrid()
        {
            _slots = new CargoSlotView[gridHeight, gridWidth];
            
            var gridLayout = GetComponent<GridLayoutGroup>();
            if (gridLayout != null)
            {
                gridLayout.cellSize = new Vector2(cellSize, cellSize);
                gridLayout.spacing = new Vector2(cellSpacing, cellSpacing);
                gridLayout.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
                gridLayout.constraintCount = gridWidth;
            }
            
            for (int y = 0; y < gridHeight; y++)
            {
                for (int x = 0; x < gridWidth; x++)
                {
                    GameObject slotObj = Instantiate(cellPrefab, transform);
                    slotObj.name = $"Slot_{x}_{y}";
                    
                    var slotView = slotObj.GetComponent<CargoSlotView>();
                    if (slotView != null)
                    {
                        slotView.Initialize(x, y, this);
                    }
                    
                    _slots[y, x] = slotView;
                }
            }
        }
        
        public void OnSlotHover(int x, int y)
        {
            if (_controller != null)
            {
                _controller.OnGridHover(x, y);
            }
        }
        
        public void OnSlotExitHover()
        {
            if (_controller != null)
            {
                _controller.OnGridExitHover();
            }
        }
        
        public void OnSlotLeftClick(int x, int y)
        {
            if (_controller != null)
            {
                _controller.OnGridLeftClick(x, y);
            }
        }
        
        public void OnSlotRightClick(int x, int y)
        {
            if (_controller != null)
            {
                _controller.OnGridRightClick(x, y);
            }
        }
        
        public void ShowGhostPreview(CargoShapeData shape, Vector2Int position, bool isValid)
        {
            HideGhostPreview();
            
            if (shape == null)
                return;
            
            var positions = shape.GetOccupiedPositions();
            Color color = isValid ? validPlacementColor : invalidPlacementColor;
            
            foreach (var offset in positions)
            {
                int x = position.x + offset.x;
                int y = position.y + offset.y;
                
                if (x >= 0 && x < gridWidth && y >= 0 && y < gridHeight)
                {
                    _slots[y, x].SetHighlight(color);
                }
            }
        }
        
        public void HideGhostPreview()
        {
            if (_slots == null)
                return;
            
            foreach (var slot in _slots)
            {
                if (slot != null)
                {
                    slot.ClearHighlight();
                }
            }
        }
        
        public void HighlightPlacedShape(PlacedShapeData placedShape)
        {
            HideGhostPreview();
            
            if (placedShape == null || placedShape.shape == null)
                return;
            
            var positions = placedShape.shape.GetOccupiedPositions();
            
            foreach (var offset in positions)
            {
                int x = placedShape.position.x + offset.x;
                int y = placedShape.position.y + offset.y;
                
                if (x >= 0 && x < gridWidth && y >= 0 && y < gridHeight)
                {
                    _slots[y, x].SetHighlight(hoverShapeColor);
                }
            }
        }
        
        private void OnShapePlaced(PlacedShapeData placedShape)
        {
            UpdateVisuals();
        }
        
        private void OnShapeRemoved(PlacedShapeData removedShape)
        {
            UpdateVisuals();
        }
        
        private void OnCargoCleared()
        {
            UpdateVisuals();
        }
        
        private void UpdateVisuals()
        {
            if (_slots == null || CargoManager.Instance == null || CargoManager.Instance.Grid == null)
                return;
            
            foreach (var slot in _slots)
            {
                if (slot != null)
                {
                    slot.Clear();
                }
            }
            
            var placedShapes = CargoManager.Instance.Grid.GetPlacedShapes();
            
            foreach (var placedShape in placedShapes)
            {
                if (placedShape.shape == null)
                    continue;
                
                var positions = placedShape.shape.GetOccupiedPositions();
                Color fillColor = GetColorForResourceType(placedShape.shape.resourceType);
                
                foreach (var offset in positions)
                {
                    int x = placedShape.position.x + offset.x;
                    int y = placedShape.position.y + offset.y;
                    
                    if (x >= 0 && x < gridWidth && y >= 0 && y < gridHeight)
                    {
                        _slots[y, x].SetContent(placedShape.shape.icon, fillColor);
                    }
                }
            }
        }
        
        private Color GetColorForResourceType(ResourceType type)
        {
            switch (type)
            {
                case ResourceType.Weapons: return new Color(1f, 0.3f, 0.3f);
                case ResourceType.Supplies: return new Color(0.3f, 1f, 0.3f);
                case ResourceType.People: return new Color(0.3f, 0.3f, 1f);
                default: return Color.white;
            }
        }
        
        private void OnDestroy()
        {
            if (_initialized && CargoManager.Instance != null)
            {
                CargoManager.Instance.OnShapePlaced -= OnShapePlaced;
                CargoManager.Instance.OnShapeRemoved -= OnShapeRemoved;
                CargoManager.Instance.OnCargoCleared -= OnCargoCleared;
            }
        }
    }
}

