# Тетрис-система - Примеры кода

Этот документ содержит примеры ключевых классов для понимания структуры.

---

## 📦 1. CargoShapeData (ScriptableObject)

```csharp
using UnityEngine;

[CreateAssetMenu(fileName = "CargoShape", menuName = "Mars Collector/Cargo Shape")]
public class CargoShapeData : ScriptableObject
{
    [Header("Basic Info")]
    public string shapeName;
    public ResourceType resourceType;
    public Sprite icon;
    
    [Header("Shape Definition")]
    [Tooltip("2D массив: 1 = занято, 0 = пусто")]
    public int[] shapeFlat; // Плоский массив для редактора
    public int shapeWidth = 2;
    public int shapeHeight = 2;
    
    [Header("Gameplay")]
    public int resourceAmount = 1;
    public Color previewColor = new Color(0, 1, 0, 0.3f);
    
    // Преобразовать плоский массив в 2D
    public int[,] GetShape()
    {
        int[,] shape = new int[shapeHeight, shapeWidth];
        for (int y = 0; y < shapeHeight; y++)
        {
            for (int x = 0; x < shapeWidth; x++)
            {
                shape[y, x] = shapeFlat[y * shapeWidth + x];
            }
        }
        return shape;
    }
    
    // Проверить занята ли ячейка в фигуре
    public bool IsShapeCellOccupied(int x, int y)
    {
        if (x < 0 || x >= shapeWidth || y < 0 || y >= shapeHeight)
            return false;
        
        return shapeFlat[y * shapeWidth + x] == 1;
    }
    
    // Получить список занятых позиций относительно (0,0)
    public List<Vector2Int> GetOccupiedPositions()
    {
        List<Vector2Int> positions = new List<Vector2Int>();
        for (int y = 0; y < shapeHeight; y++)
        {
            for (int x = 0; x < shapeWidth; x++)
            {
                if (shapeFlat[y * shapeWidth + x] == 1)
                {
                    positions.Add(new Vector2Int(x, y));
                }
            }
        }
        return positions;
    }
}
```

---

## 📦 2. CargoGrid (Model)

```csharp
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct CargoCell
{
    public bool isOccupied;
    public int shapeInstanceId; // ID установленной фигуры
}

[Serializable]
public class PlacedShapeData
{
    public CargoShapeData shape;
    public Vector2Int position; // Левый верхний угол
    public int instanceId;
    
    public PlacedShapeData(CargoShapeData shape, Vector2Int position, int instanceId)
    {
        this.shape = shape;
        this.position = position;
        this.instanceId = instanceId;
    }
}

public class CargoGrid
{
    public int Width { get; private set; }
    public int Height { get; private set; }
    
    private CargoCell[,] _cells;
    private List<PlacedShapeData> _placedShapes;
    private int _nextInstanceId = 1;
    
    public CargoGrid(int width, int height)
    {
        Width = width;
        Height = height;
        _cells = new CargoCell[height, width];
        _placedShapes = new List<PlacedShapeData>();
    }
    
    // Проверить можно ли разместить фигуру
    public bool CanPlaceShape(CargoShapeData shape, int x, int y)
    {
        var positions = shape.GetOccupiedPositions();
        
        foreach (var offset in positions)
        {
            int cellX = x + offset.x;
            int cellY = y + offset.y;
            
            // Выход за границы
            if (cellX < 0 || cellX >= Width || cellY < 0 || cellY >= Height)
                return false;
            
            // Ячейка уже занята
            if (_cells[cellY, cellX].isOccupied)
                return false;
        }
        
        return true;
    }
    
    // Разместить фигуру
    public PlacedShapeData PlaceShape(CargoShapeData shape, int x, int y)
    {
        if (!CanPlaceShape(shape, x, y))
            return null;
        
        int instanceId = _nextInstanceId++;
        var positions = shape.GetOccupiedPositions();
        
        // Занять ячейки
        foreach (var offset in positions)
        {
            int cellX = x + offset.x;
            int cellY = y + offset.y;
            
            _cells[cellY, cellX].isOccupied = true;
            _cells[cellY, cellX].shapeInstanceId = instanceId;
        }
        
        // Сохранить информацию о размещенной фигуре
        var placedShape = new PlacedShapeData(shape, new Vector2Int(x, y), instanceId);
        _placedShapes.Add(placedShape);
        
        return placedShape;
    }
    
    // Удалить фигуру по позиции клика
    public PlacedShapeData RemoveShapeAt(int x, int y)
    {
        if (x < 0 || x >= Width || y < 0 || y >= Height)
            return null;
        
        if (!_cells[y, x].isOccupied)
            return null;
        
        int instanceId = _cells[y, x].shapeInstanceId;
        
        // Найти фигуру
        PlacedShapeData shapeToRemove = _placedShapes.Find(s => s.instanceId == instanceId);
        if (shapeToRemove == null)
            return null;
        
        // Освободить все ячейки этой фигуры
        var positions = shapeToRemove.shape.GetOccupiedPositions();
        foreach (var offset in positions)
        {
            int cellX = shapeToRemove.position.x + offset.x;
            int cellY = shapeToRemove.position.y + offset.y;
            
            _cells[cellY, cellX].isOccupied = false;
            _cells[cellY, cellX].shapeInstanceId = 0;
        }
        
        _placedShapes.Remove(shapeToRemove);
        return shapeToRemove;
    }
    
    // Подсчитать загруженные ресурсы
    public Dictionary<ResourceType, int> CalculateLoadedResources()
    {
        var resources = new Dictionary<ResourceType, int>
        {
            { ResourceType.Weapons, 0 },
            { ResourceType.Supplies, 0 },
            { ResourceType.People, 0 }
        };
        
        foreach (var placedShape in _placedShapes)
        {
            resources[placedShape.shape.resourceType] += placedShape.shape.resourceAmount;
        }
        
        return resources;
    }
    
    // Очистить всю сетку
    public void Clear()
    {
        _cells = new CargoCell[Height, Width];
        _placedShapes.Clear();
        _nextInstanceId = 1;
    }
    
    // Получить ячейку
    public CargoCell GetCell(int x, int y)
    {
        if (x < 0 || x >= Width || y < 0 || y >= Height)
            return new CargoCell();
        
        return _cells[y, x];
    }
    
    // Получить все размещенные фигуры
    public List<PlacedShapeData> GetPlacedShapes()
    {
        return new List<PlacedShapeData>(_placedShapes);
    }
}
```

---

## 📦 3. CargoManager (Singleton)

```csharp
using System;
using System.Collections.Generic;
using UnityEngine;

public class CargoManager : MonoBehaviour
{
    public static CargoManager Instance { get; private set; }
    
    public CargoGrid Grid { get; private set; }
    public CargoShapeDatabase Database { get; private set; }
    
    public event Action<ResourceType, int> OnResourcesChanged;
    public event Action<PlacedShapeData> OnShapePlaced;
    public event Action<PlacedShapeData> OnShapeRemoved;
    
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        
        Instance = this;
    }
    
    public void Initialize(int width, int height, CargoShapeDatabase database)
    {
        Grid = new CargoGrid(width, height);
        Database = database;
    }
    
    public bool PlaceShape(CargoShapeData shape, int x, int y)
    {
        var placed = Grid.PlaceShape(shape, x, y);
        
        if (placed != null)
        {
            OnShapePlaced?.Invoke(placed);
            OnResourcesChanged?.Invoke(shape.resourceType, shape.resourceAmount);
            return true;
        }
        
        return false;
    }
    
    public bool RemoveShapeAt(int x, int y)
    {
        var removed = Grid.RemoveShapeAt(x, y);
        
        if (removed != null)
        {
            OnShapeRemoved?.Invoke(removed);
            OnResourcesChanged?.Invoke(removed.shape.resourceType, -removed.shape.resourceAmount);
            return true;
        }
        
        return false;
    }
    
    public Dictionary<ResourceType, int> GetLoadedResources()
    {
        return Grid.CalculateLoadedResources();
    }
    
    public void ClearCargo()
    {
        Grid.Clear();
        OnResourcesChanged?.Invoke(ResourceType.Weapons, 0);
        OnResourcesChanged?.Invoke(ResourceType.Supplies, 0);
        OnResourcesChanged?.Invoke(ResourceType.People, 0);
    }
    
    // Конвертировать в старый формат для отправки
    public ShipCargo ConvertToShipCargo()
    {
        var resources = GetLoadedResources();
        var cargo = new ShipCargo(Grid.Width * Grid.Height); // Для совместимости
        
        // Заполнить cargo из словаря
        foreach (var resource in resources)
        {
            for (int i = 0; i < resource.Value; i++)
            {
                cargo.AddResource(resource.Key, 1);
            }
        }
        
        return cargo;
    }
}
```

---

## 📦 4. CargoPlacementController

```csharp
using UnityEngine;
using UnityEngine.EventSystems;

public class CargoPlacementController : MonoBehaviour
{
    [Header("References")]
    public CargoGridView gridView;
    public Camera mainCamera;
    
    private CargoShapeData _selectedShape;
    private Vector2Int _currentGridPos = new Vector2Int(-1, -1);
    private bool _isValidPlacement;
    
    private void Update()
    {
        if (_selectedShape == null)
            return;
        
        // Проверка что курсор не над UI
        if (EventSystem.current.IsPointerOverGameObject())
        {
            gridView.HideGhostPreview();
            return;
        }
        
        // Получить позицию мыши на сетке
        Vector2Int gridPos = gridView.GetGridPositionFromMouse(Input.mousePosition);
        
        if (gridPos.x == -1) // Вне сетки
        {
            gridView.HideGhostPreview();
            return;
        }
        
        // Обновить ghost только если позиция изменилась
        if (gridPos != _currentGridPos)
        {
            _currentGridPos = gridPos;
            UpdateGhostPreview();
        }
        
        // Обработка кликов
        if (Input.GetMouseButtonDown(0)) // ЛКМ
        {
            TryPlaceShape();
        }
        else if (Input.GetMouseButtonDown(1)) // ПКМ
        {
            DeselectShape();
        }
    }
    
    public void SelectShape(CargoShapeData shape)
    {
        _selectedShape = shape;
        Debug.Log($"Выбрана фигура: {shape.shapeName}");
    }
    
    public void DeselectShape()
    {
        _selectedShape = null;
        gridView.HideGhostPreview();
        Debug.Log("Выбор фигуры отменен");
    }
    
    private void UpdateGhostPreview()
    {
        if (_selectedShape == null)
            return;
        
        // Проверить валидность размещения
        _isValidPlacement = CargoManager.Instance.Grid.CanPlaceShape(
            _selectedShape, 
            _currentGridPos.x, 
            _currentGridPos.y
        );
        
        // Показать ghost
        gridView.ShowGhostPreview(
            _selectedShape, 
            _currentGridPos, 
            _isValidPlacement
        );
    }
    
    private void TryPlaceShape()
    {
        if (_selectedShape == null || !_isValidPlacement)
        {
            // Звук ошибки
            return;
        }
        
        bool placed = CargoManager.Instance.PlaceShape(
            _selectedShape, 
            _currentGridPos.x, 
            _currentGridPos.y
        );
        
        if (placed)
        {
            // Звук успеха
            DeselectShape();
        }
    }
    
    public void OnGridCellRightClicked(int x, int y)
    {
        if (_selectedShape != null)
        {
            DeselectShape();
        }
        else
        {
            // Попытка удалить фигуру
            bool removed = CargoManager.Instance.RemoveShapeAt(x, y);
            if (removed)
            {
                // Звук удаления
            }
        }
    }
}
```

---

## 📦 5. CargoGridView

```csharp
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CargoGridView : MonoBehaviour
{
    [Header("Settings")]
    public int gridWidth = 5;
    public int gridHeight = 4;
    public float cellSize = 64f;
    public float cellSpacing = 2f;
    
    [Header("Prefabs")]
    public GameObject cellPrefab;
    public GameObject ghostPreviewPrefab;
    
    [Header("Colors")]
    public Color validPlacementColor = new Color(0, 1, 0, 0.3f);
    public Color invalidPlacementColor = new Color(1, 0, 0, 0.3f);
    
    private CargoSlotView[,] _slots;
    private GameObject _ghostPreview;
    private List<Image> _ghostImages = new List<Image>();
    
    private void Start()
    {
        GenerateGrid();
        
        // Подписаться на события
        if (CargoManager.Instance != null)
        {
            CargoManager.Instance.OnShapePlaced += OnShapePlaced;
            CargoManager.Instance.OnShapeRemoved += OnShapeRemoved;
        }
    }
    
    private void GenerateGrid()
    {
        _slots = new CargoSlotView[gridHeight, gridWidth];
        
        // Настроить GridLayoutGroup
        var gridLayout = GetComponent<GridLayoutGroup>();
        if (gridLayout != null)
        {
            gridLayout.cellSize = new Vector2(cellSize, cellSize);
            gridLayout.spacing = new Vector2(cellSpacing, cellSpacing);
            gridLayout.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
            gridLayout.constraintCount = gridWidth;
        }
        
        // Создать ячейки
        for (int y = 0; y < gridHeight; y++)
        {
            for (int x = 0; x < gridWidth; x++)
            {
                GameObject slotObj = Instantiate(cellPrefab, transform);
                slotObj.name = $"Slot_{x}_{y}";
                
                var slotView = slotObj.GetComponent<CargoSlotView>();
                slotView.Initialize(x, y, this);
                
                _slots[y, x] = slotView;
            }
        }
    }
    
    public Vector2Int GetGridPositionFromMouse(Vector3 mousePosition)
    {
        // Простой вариант: raycast к слотам
        // Или математический расчет позиции
        
        // Здесь упрощенная версия
        foreach (var slot in _slots)
        {
            if (RectTransformUtility.RectangleContainsScreenPoint(
                slot.GetComponent<RectTransform>(), 
                mousePosition))
            {
                return new Vector2Int(slot.GridX, slot.GridY);
            }
        }
        
        return new Vector2Int(-1, -1); // Вне сетки
    }
    
    public void ShowGhostPreview(CargoShapeData shape, Vector2Int position, bool isValid)
    {
        HideGhostPreview();
        
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
        foreach (var slot in _slots)
        {
            slot.ClearHighlight();
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
    
    private void UpdateVisuals()
    {
        // Очистить все
        foreach (var slot in _slots)
        {
            slot.Clear();
        }
        
        // Отобразить размещенные фигуры
        var placedShapes = CargoManager.Instance.Grid.GetPlacedShapes();
        
        foreach (var placedShape in placedShapes)
        {
            var positions = placedShape.shape.GetOccupiedPositions();
            
            foreach (var offset in positions)
            {
                int x = placedShape.position.x + offset.x;
                int y = placedShape.position.y + offset.y;
                
                _slots[y, x].SetContent(placedShape.shape.icon);
            }
        }
    }
}
```

---

## 📝 Пример создания фигуры в Inspector

### Пистолет (2x1, 1 оружие):
```
Shape Name: "Пистолет"
Resource Type: Weapons
Shape Width: 2
Shape Height: 1
Shape Flat: [1, 1]  // Массив из 2 элементов
Resource Amount: 1
Preview Color: (0, 1, 0, 0.3)
```

### Автомат L-форма (3x2, 3 оружия):
```
Shape Name: "Автомат"
Resource Type: Weapons
Shape Width: 3
Shape Height: 2
Shape Flat: [1, 1, 1,  // Первая строка
             0, 1, 0]  // Вторая строка
Resource Amount: 3
```

---

Эти примеры дают представление о структуре кода. Полная реализация будет включать больше деталей, обработку ошибок и оптимизации.

