# Тетрис-система загрузки - Инструкция для Unity

## ✅ Что уже готово (код)
- ✅ Все модели данных (CargoShapeData, CargoGrid, PlacedShapeData)
- ✅ CargoManager (менеджер)
- ✅ Все UI компоненты (SlotView, GridView, ShapeButton, ShapePanel, Controller)
- ✅ Интеграция с GameFlowManager
- ✅ Обновленный GameConfig

---

## 🎯 Что нужно сделать в Unity (шаг за шагом)

### ШАГ 1: Создать CargoShapeDatabase (5 минут)

1. **Создай базу данных:**
   ```
   Assets/_Project/_CONFIGS/
   ПКМ → Create → Mars Collector → Cargo Shape Database
   Назови: CargoShapeDatabase
   ```

2. **Пока оставь пустым** (фигуры создадим дальше)

---

### ШАГ 2: Создать фигуры (15-20 минут)

#### Создание фигуры:
```
Assets/_Project/_CONFIGS/CargoShapes/
ПКМ → Create → Mars Collector → Cargo Shape
```

#### Weapons (создай 5 штук):

**1. Пистолет**
```
Shape Name: Пистолет
Resource Type: Weapons
Shape Width: 2
Shape Height: 1
Shape Flat: [1, 1]
Resource Amount: 1
```

**2. Автомат**
```
Shape Name: Автомат
Resource Type: Weapons
Shape Width: 3
Shape Height: 2
Shape Flat: [1, 1, 1,
             0, 1, 0]
Resource Amount: 3
```

**3. Снайперка**
```
Shape Name: Снайперка
Resource Type: Weapons
Shape Width: 4
Shape Height: 1
Shape Flat: [1, 1, 1, 1]
Resource Amount: 2
```

**4. Гранаты**
```
Shape Name: Гранаты
Resource Type: Weapons
Shape Width: 2
Shape Height: 2
Shape Flat: [1, 1,
             1, 1]
Resource Amount: 2
```

**5. Ракетница**
```
Shape Name: Ракетница
Resource Type: Weapons
Shape Width: 3
Shape Height: 3
Shape Flat: [0, 1, 0,
             1, 1, 1,
             0, 1, 0]
Resource Amount: 5
```

#### Supplies (создай 5 штук):

**1. Малый ящик**
```
Shape Name: Малый ящик
Resource Type: Supplies
Shape Width: 2
Shape Height: 2
Shape Flat: [1, 1,
             1, 1]
Resource Amount: 2
```

**2. Средний ящик**
```
Shape Name: Средний ящик
Resource Type: Supplies
Shape Width: 3
Shape Height: 2
Shape Flat: [1, 1, 1,
             1, 1, 1]
Resource Amount: 4
```

**3. Большой ящик**
```
Shape Name: Большой ящик
Resource Type: Supplies
Shape Width: 3
Shape Height: 3
Shape Flat: [1, 1, 1,
             1, 1, 1,
             1, 1, 1]
Resource Amount: 6
```

**4. Бочка**
```
Shape Name: Бочка
Resource Type: Supplies
Shape Width: 3
Shape Height: 3
Shape Flat: [0, 1, 0,
             1, 1, 1,
             0, 1, 0]
Resource Amount: 4
```

**5. Паллета**
```
Shape Name: Паллета
Resource Type: Supplies
Shape Width: 3
Shape Height: 3
Shape Flat: [1, 1, 1,
             1, 1, 1,
             1, 1, 1]
Resource Amount: 8
```

#### People (создай 5 штук):

**1. Один человек**
```
Shape Name: Один человек
Resource Type: People
Shape Width: 1
Shape Height: 2
Shape Flat: [1,
             1]
Resource Amount: 1
```

**2. Пара**
```
Shape Name: Пара
Resource Type: People
Shape Width: 2
Shape Height: 2
Shape Flat: [1, 1,
             1, 1]
Resource Amount: 2
```

**3. Группа 3**
```
Shape Name: Группа 3
Resource Type: People
Shape Width: 3
Shape Height: 1
Shape Flat: [1, 1, 1]
Resource Amount: 3
```

**4. Группа 4**
```
Shape Name: Группа 4
Resource Type: People
Shape Width: 2
Shape Height: 2
Shape Flat: [1, 1,
             1, 1]
Resource Amount: 4
```

**5. Команда**
```
Shape Name: Команда
Resource Type: People
Shape Width: 3
Shape Height: 3
Shape Flat: [1, 1, 1,
             1, 0, 1,
             1, 1, 1]
Resource Amount: 5
```

3. **Добавь фигуры в базу данных:**
   - Открой `CargoShapeDatabase`
   - Перетащи фигуры Weapons в список `Weapon Shapes`
   - Перетащи фигуры Supplies в список `Supply Shapes`
   - Перетащи фигуры People в список `People Shapes`

---

### ШАГ 3: Обновить GameConfig (2 минуты)

1. Открой свой `GameConfig`
2. Настрой параметры:
   ```
   Grid Width: 5
   Grid Height: 4
   Cargo Database: [перетащи CargoShapeDatabase]
   ```

---

### ШАГ 4: Добавить CargoManager на сцену (1 минута)

1. Найди GameObject `GameManagers` на сцене
2. Add Component → `CargoManager`
3. Готово!

---

### ШАГ 5: Создать UI префабы (15-20 минут)

#### 5.1. CargoSlot (префаб ячейки)

```
Создай: GameObject "CargoSlot"

Структура:
CargoSlot (64x64)
├─ Background (Image) - белый квадрат с границей
├─ Content (Image) - прозрачная, сюда идет иконка груза
└─ Highlight (Image) - прозрачная, включается для подсветки

Компоненты на CargoSlot:
- Add Component → Cargo Slot View
- Подключи:
  background → Background
  content → Content
  highlight → Highlight
  
Сохрани как префаб: CargoSlot.prefab
```

#### 5.2. CargoShapeButton (префаб кнопки фигуры)

```
Создай: Button "CargoShapeButton"

Структура:
CargoShapeButton (Button)
├─ Icon (Image) - иконка фигуры
├─ AmountText (TextMeshPro) - "x3"
└─ SelectionFrame (Image) - рамка выделения (выключена по умолчанию)

Компоненты на CargoShapeButton:
- Add Component → Cargo Shape Button
- Подключи:
  button → сам Button
  icon → Icon
  amountText → AmountText
  selectionFrame → SelectionFrame

Сохрани как префаб: CargoShapeButton.prefab
```

---

### ШАГ 6: Настроить UI сцены (20-30 минут)

#### Создай Canvas структуру:

```
Canvas
└─ CargoLoadingScreen (Panel)
   ├─ Header
   │  ├─ Title: "Загрузка корабля" (TextMeshPro)
   │  └─ TurnCounter (TextMeshPro) - уже есть
   │
   ├─ ShapePanels (HorizontalLayoutGroup)
   │  ├─ WeaponsPanel
   │  │  ├─ Title: "Оружие" (TextMeshPro)
   │  │  └─ ButtonsContainer (VerticalLayoutGroup)
   │  │
   │  ├─ SuppliesPanel
   │  │  ├─ Title: "Припасы" (TextMeshPro)
   │  │  └─ ButtonsContainer (VerticalLayoutGroup)
   │  │
   │  └─ PeoplePanel
   │     ├─ Title: "Люди" (TextMeshPro)
   │     └─ ButtonsContainer (VerticalLayoutGroup)
   │
   ├─ CargoGridView (GridLayoutGroup)
   │  └─ [Слоты генерируются автоматически]
   │
   ├─ ResourceCounter
   │  ├─ WeaponsText: "Оружие: 0" (TextMeshPro)
   │  ├─ SuppliesText: "Припасы: 0" (TextMeshPro)
   │  └─ PeopleText: "Люди: 0" (TextMeshPro)
   │
   └─ LaunchButton (Button)
      └─ Text: "ОТПРАВИТЬ"
```

---

### ШАГ 7: Настроить компоненты (10 минут)

#### 7.1. CargoGridView

```
На GameObject CargoGridView:

Add Component → Grid Layout Group
- Cell Size: (64, 64)
- Spacing: (2, 2)
- Constraint: Fixed Column Count: 5

Add Component → Cargo Grid View
- Grid Width: 5
- Grid Height: 4
- Cell Size: 64
- Cell Spacing: 2
- Cell Prefab: [перетащи CargoSlot.prefab]
- Valid Placement Color: зеленый (0, 1, 0, 0.5)
- Invalid Placement Color: красный (1, 0, 0, 0.5)
```

#### 7.2. WeaponsPanel

```
Add Component → Cargo Shape Panel
- Resource Type: Weapons
- Button Prefab: [перетащи CargoShapeButton.prefab]
- Buttons Container: [перетащи ButtonsContainer]
```

#### 7.3. SuppliesPanel

```
Add Component → Cargo Shape Panel
- Resource Type: Supplies
- Button Prefab: [перетащи CargoShapeButton.prefab]
- Buttons Container: [перетащи ButtonsContainer]
```

#### 7.4. PeoplePanel

```
Add Component → Cargo Shape Panel
- Resource Type: People
- Button Prefab: [перетащи CargoShapeButton.prefab]
- Buttons Container: [перетащи ButtonsContainer]
```

#### 7.5. CargoPlacementController

```
На GameObject CargoLoadingScreen:

Add Component → Cargo Placement Controller
- Grid View: [перетащи CargoGridView]
- Weapons Panel: [перетащи WeaponsPanel]
- Supplies Panel: [перетащи SuppliesPanel]
- People Panel: [перетащи PeoplePanel]
```

#### 7.6. CargoResourceCounter

```
На GameObject ResourceCounter:

Add Component → Cargo Resource Counter
- Weapons Text: [перетащи WeaponsText]
- Supplies Text: [перетащи SuppliesText]
- People Text: [перетащи PeopleText]
```

#### 7.7. LaunchButton

```
На Button "LaunchButton":
- OnClick() → ShipManager → LaunchShip()
```

---

### ШАГ 8: Скрыть/удалить старую ShipView (1 минута)

Если у тебя еще есть старая ShipView с кнопками "+Weapons/Supplies/People":
- Либо удали её
- Либо выключи GameObject

---

## ✅ Проверка что всё работает

### Запусти игру и проверь:

1. ✅ **На старте видны 3 панели** с фигурами (Weapons, Supplies, People)
2. ✅ **Клик по фигуре** - она выделяется рамкой
3. ✅ **Наведение на сетку** - появляется ghost preview (зеленый/красный)
4. ✅ **ЛКМ на сетке** - фигура размещается
5. ✅ **Счетчик ресурсов** обновляется
6. ✅ **ПКМ** - отменяет выбор или удаляет фигуру
7. ✅ **Кнопка "Отправить"** - корабль улетает
8. ✅ **Ресурсы доставляются** на Марс
9. ✅ **Сетка очищается** после отправки

---

## 🐛 Если что-то не работает

### Проблема: Фигуры не появляются в панелях
**Решение:** 
- Проверь что CargoDatabase подключен в GameConfig
- Проверь что фигуры добавлены в списки в CargoDatabase

### Проблема: Ghost preview не показывается
**Решение:**
- Проверь что Highlight объект есть в CargoSlot префабе
- Проверь что цвета не полностью прозрачные (Alpha > 0)

### Проблема: Клик не размещает фигуру
**Решение:**
- Проверь что CargoPlacementController подключен
- Проверь консоль на ошибки
- Убедись что CargoManager есть на сцене

### Проблема: NullReferenceException
**Решение:**
- Убедись что все UI элементы подключены в компонентах
- Проверь что все префабы назначены
- Проверь что GameManagers с менеджерами на сцене

---

## 🎨 Опциональные улучшения

### Добавить иконки для фигур:
1. Найди/создай иконки 128x128
2. Import как Sprite
3. Назначь в `CargoShapeData → Icon`
4. Иконки будут отображаться вместо цветных блоков

### Улучшить визуал:
- Background сетки - добавь sprite корабля
- Разные цвета для разных типов ресурсов
- Анимации появления/исчезновения фигур
- Звуки при размещении

---

## 📊 Балансировка

Если игра слишком сложная/легкая:

### Изменить размер сетки:
```
GameConfig:
Grid Width: 6 (вместо 5)
Grid Height: 5 (вместо 4)
```

### Изменить эффективность фигур:
Открой CargoShapeData и измени `Resource Amount`

### Создать новые фигуры:
Можно добавлять любые формы, главное чтобы:
- Shape Flat содержал Width × Height элементов
- 1 = занято, 0 = пусто

---

## 🎉 Готово!

После выполнения всех шагов у тебя будет работающая тетрис-система загрузки корабля!

Если возникнут проблемы - пиши, разберемся! 🚀

