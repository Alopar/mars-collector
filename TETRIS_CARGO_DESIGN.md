# Тетрис-система загрузки корабля - Дизайн документ

## 🎯 Концепция

Вместо простых кнопок "+1 ресурс" игрок размещает **фигуры груза** на **сетке корабля** в стиле тетриса/инвентаря из Resident Evil.

## 📐 Архитектура системы

### 1. Данные фигур (Cargo Shapes)

#### CargoShapeData (ScriptableObject)
```
Свойства:
- ResourceType type           // Weapons/Supplies/People
- string shapeName            // "Пистолет", "Ящик припасов"
- Sprite icon                 // Иконка для UI
- int[,] shape                // 2D массив формы (1 = занято, 0 = пусто)
- int resourceAmount          // Сколько ресурсов даст эта фигура
- Color previewColor          // Цвет предпросмотра

Примеры форм:
Пистолет (2x1):     Автомат (3x2):      Ящик (2x2):
[1][1]              [1][1][1]           [1][1]
                    [1][1][1]           [1][1]
```

#### CargoShapeDatabase (ScriptableObject)
```
- List<CargoShapeData> weaponShapes
- List<CargoShapeData> supplyShapes
- List<CargoShapeData> peopleShapes
```

### 2. Сетка корабля (Cargo Grid)

#### CargoGrid (Model)
```
Свойства:
- int width                   // Ширина сетки (например 5)
- int height                  // Высота сетки (например 4)
- CargoCell[,] cells          // Массив ячеек
- int totalCapacity           // Общая вместимость

Методы:
- bool CanPlaceShape(shape, x, y)
- void PlaceShape(shape, x, y)
- void RemoveShape(x, y)
- List<Vector2Int> GetOccupiedCells(shape, x, y)
- bool IsCellOccupied(x, y)
```

#### CargoCell (Struct)
```
- bool isOccupied
- CargoShapeData placedShape
- int shapeInstanceId         // Чтобы различать разные экземпляры
```

### 3. Визуальная часть (Cargo Grid View)

#### CargoGridView (MonoBehaviour)
```
Компоненты:
- Grid/GridLayoutGroup        // Визуальная сетка
- List<CargoSlotView>         // Визуальные ячейки
- GameObject ghostPreview     // Блеклый предпросмотр

Методы:
- void ShowGhostPreview(shape, x, y, isValid)
- void HideGhostPreview()
- void HighlightSlots(positions, color)
- void UpdateSlotVisuals()
```

#### CargoSlotView (MonoBehaviour на каждую ячейку)
```
- Image background
- Image content               // Иконка установленного груза
- GameObject highlight        // Подсветка (зеленая/красная)

События:
- OnPointerEnter()
- OnPointerExit()
- OnPointerClick()
```

### 4. Панели выбора фигур

#### CargoShapePanel (MonoBehaviour)
```
Свойства:
- ResourceType resourceType
- List<CargoShapeButton> shapeButtons

Методы:
- void Initialize(List<CargoShapeData> shapes)
- void SelectShape(CargoShapeData shape)
- void DeselectAll()
```

#### CargoShapeButton (MonoBehaviour)
```
- CargoShapeData shapeData
- Image icon
- TextMeshPro amountText      // "x3" количество ресурсов
- GameObject selectionFrame   // Рамка выделения

События:
- OnClick() → выбрать фигуру
```

### 5. Управление (Cargo Placement Controller)

#### CargoPlacementController (MonoBehaviour)
```
Состояния:
- None                        // Ничего не выбрано
- ShapeSelected               // Фигура выбрана, показываем ghost
- PlacingShape                // В процессе размещения

Свойства:
- CargoShapeData selectedShape
- Vector2Int currentGridPos
- bool isValidPlacement

Методы:
- void SelectShape(CargoShapeData shape)
- void DeselectShape()
- void UpdateGhostPreview()
- void TryPlaceShape()
- void TryRemoveShape(Vector2Int pos)

События ввода:
- Update() → Ray от мыши к сетке
- LeftClick → PlaceShape()
- RightClick → Deselect() или RemoveShape()
```

### 6. Менеджер груза (Cargo Manager)

#### CargoManager (Singleton)
```
Свойства:
- CargoGrid grid
- Dictionary<ResourceType, int> loadedResources
- CargoShapeDatabase database

Методы:
- void InitializeGrid(width, height)
- bool PlaceShape(CargoShapeData shape, x, y)
- bool RemoveShape(x, y)
- Dictionary<ResourceType, int> GetLoadedResources()
- void ClearCargo()
- ShipCargo ConvertToShipCargo() // для отправки

События:
- OnCargoChanged(ResourceType, int delta)
- OnGridUpdated()
```

## 🎨 UI Структура

```
Canvas
├─ CargoLoadingScreen (Panel)
│  ├─ Header
│  │  ├─ Title: "Загрузка корабля"
│  │  └─ DayCounter: "День 1 из 20"
│  │
│  ├─ ShapePanels (HorizontalLayout)
│  │  ├─ WeaponsPanel
│  │  │  ├─ Title: "Оружие"
│  │  │  └─ ShapeButtons (3-5 вариантов)
│  │  ├─ SuppliesPanel
│  │  │  ├─ Title: "Припасы"
│  │  │  └─ ShapeButtons (3-5 вариантов)
│  │  └─ PeoplePanel
│  │     ├─ Title: "Люди"
│  │     └─ ShapeButtons (3-5 вариантов)
│  │
│  ├─ CargoGridView (центр)
│  │  ├─ GridBackground (Image корабля)
│  │  ├─ Grid (GridLayoutGroup 5x4)
│  │  │  └─ CargoSlots x20
│  │  └─ GhostPreview (Image overlay)
│  │
│  ├─ ResourceCounter (правый нижний угол)
│  │  ├─ WeaponsCount: "Оружие: 5"
│  │  ├─ SuppliesCount: "Припасы: 8"
│  │  └─ PeopleCount: "Люди: 3"
│  │
│  └─ LaunchButton
│     └─ "ОТПРАВИТЬ" (активна если загружено > 0)
```

## 📋 Пример фигур для каждого типа

### Оружие (Weapons)
```
Пистолет (1 оружие):     Автомат (3 оружия):     Снайперка (2 оружия):
[1][1]                   [1][1][1]               [1][1][1][1]
                         [0][1][0]               

Гранаты (2 оружия):      Ракетница (5 оружий):
[1][1]                   [0][1][0]
[1][1]                   [1][1][1]
                         [0][1][0]
```

### Припасы (Supplies)
```
Маленький ящик (2):      Большой ящик (5):       Бочка (4):
[1][1]                   [1][1][1]               [0][1][0]
[1][1]                   [1][1][1]               [1][1][1]
                                                 [0][1][0]

Паллета (8):             Контейнер (10):
[1][1][1]                [1][1][1][1]
[1][1][1]                [1][1][1][1]
[1][1][1]                [1][1][1][1]
```

### Люди (People)
```
1 человек (1):           Группа 2 (2):           Группа 3 (3):
[1]                      [1][1]                  [1][1][1]
[1]                      [1][1]                  
                         
Команда (5):             
[1][1][1]
[1][0][1]
[1][1][1]
```

## 🔧 Технические детали

### Определение формы в коде
```csharp
// Пример: Автомат 3x2
new int[2,3] {
    {1, 1, 1},
    {0, 1, 0}
}
```

### Алгоритм проверки размещения
```
1. Получить позицию мыши на сетке (x, y)
2. Для каждой занятой ячейки в shape:
   a. Проверить что (x + offsetX, y + offsetY) в границах сетки
   b. Проверить что ячейка не занята
3. Если все проверки прошли → isValid = true
4. Показать ghost с цветом (зеленый/красный)
```

### Визуальная подсветка
```
Зеленый (0, 255, 0, 100)  → можно установить
Красный (255, 0, 0, 100)  → нельзя установить
Желтый (255, 255, 0, 150) → hover над пустой ячейкой
```

## 📝 Список задач (порядок реализации)

### Этап 1: Модели данных
- [ ] Создать CargoShapeData (ScriptableObject)
- [ ] Создать CargoShapeDatabase (ScriptableObject)
- [ ] Создать структуру CargoCell
- [ ] Создать модель CargoGrid
- [ ] Обновить ShipCargo для совместимости

### Этап 2: Менеджер
- [ ] Создать CargoManager (Singleton)
- [ ] Реализовать логику размещения/удаления
- [ ] Реализовать валидацию
- [ ] Реализовать подсчет ресурсов
- [ ] Интеграция с ShipManager

### Этап 3: UI компоненты
- [ ] Создать CargoSlotView (префаб ячейки)
- [ ] Создать CargoGridView (сетка)
- [ ] Создать CargoShapeButton (кнопка фигуры)
- [ ] Создать CargoShapePanel (панель типа ресурса)
- [ ] Создать ResourceCounterView

### Этап 4: Контроллер размещения
- [ ] Создать CargoPlacementController
- [ ] Реализовать raycasting от мыши
- [ ] Реализовать ghost preview
- [ ] Реализовать подсветку ячеек
- [ ] Обработка ЛКМ/ПКМ

### Этап 5: Контент
- [ ] Создать 5-7 фигур для Weapons
- [ ] Создать 5-7 фигур для Supplies
- [ ] Создать 5-7 фигур для People
- [ ] Найти/создать иконки для фигур
- [ ] Настроить цвета и визуал

### Этап 6: Интеграция
- [ ] Заменить старую ShipView на новую систему
- [ ] Обновить GameFlowManager
- [ ] Тестирование полного цикла
- [ ] Балансировка (размер сетки, формы фигур)

### Этап 7: Polish
- [ ] Анимация размещения фигуры
- [ ] Звуки (выбор, размещение, ошибка)
- [ ] Частицы при размещении
- [ ] Инструкция для игрока (tutorial hints)

## 🎮 Особенности реализации

### Префаб отсека корабля
Можно создать визуально:
```
Prefab: CargoGridBackground
- Sprite корабля (фон)
- Overlay сетка (линии)
- Декоративные элементы (заклепки, панели)
```

### Вращение фигур (опционально)
Если добавить вращение (кнопка R):
```csharp
int[,] RotateShape(int[,] shape) {
    // Транспонировать и отразить
}
```

### Сохранение состояния
```csharp
[Serializable]
class CargoSaveData {
    List<PlacedShapeData> placedShapes;
}
```

## 🔄 Переход от старой системы

### Что меняется:
- ❌ Кнопки "+Weapons/Supplies/People"
- ❌ Простой счетчик "X/10"
- ✅ Сетка с тетрис-размещением
- ✅ Разные фигуры с разной эффективностью
- ✅ Стратегический выбор (большие фигуры = больше ресурсов, но занимают много места)

### Совместимость:
- ShipCargo остается (конвертируем CargoGrid → ShipCargo при отправке)
- Остальные менеджеры не меняются
- События OnCargoChanged остаются

## 💡 Геймплейные последствия

### Стратегия:
- Игрок должен **планировать** что везти
- Большие фигуры эффективнее, но менее гибкие
- Маленькие фигуры заполняют пробелы
- Тетрис-скилл = больше ресурсов в корабле

### Баланс:
```
Эффективность = resourceAmount / площадь_фигуры

Пример:
Пистолет: 1 ресурс / 2 клетки = 0.5
Автомат: 3 ресурса / 5 клеток = 0.6 (лучше!)
Паллета: 8 ресурсов / 9 клеток = 0.89 (очень хорошо!)
```

## 📐 Рекомендуемые размеры

### Сетка корабля:
- **5x4 = 20 клеток** (как было Ship Capacity = 10)
- Можно настраивать в Config

### Размер ячейки UI:
- 64x64 пикселя (удобно для кликов)
- Отступы: 2px между ячейками

### Иконки фигур:
- 128x128 или 256x256 (для четкости)
- Прозрачный фон

