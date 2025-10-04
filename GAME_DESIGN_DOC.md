# Mars Collector - Техническая документация

## 🎮 Концепция
Пошаговая игра об управлении колонией на Марсе через отправку кораблей с ресурсами.

## 📊 Ресурсы = Шкалы
Три типа ресурсов, которые являются одновременно шкалами состояния колонии:

1. **Оружие (Weapons)** - защита от марсиан и поддержание порядка
2. **Припасы (Supplies)** - еда, медикаменты, расходники
3. **Люди (People)** - население колонии

**Диапазон шкал:** 0-100
- **<= 0** = ПРОИГРЫШ (критическая нехватка)
- **>= 100** = ПРОИГРЫШ (критический переизбыток)

## 🎯 Цель игры
Продержаться заданное количество ходов (настраивается в конфиге)

## 🔄 Игровой цикл

### Фаза 1: Загрузка (Loading)
- Игрок видит текущие шкалы колонии
- **ВАЖНО:** Показывается предпросмотр изменений на следующем ходу
- Игрок формирует груз корабля (общая вместимость, например 10)
- Может загружать любую комбинацию ресурсов

### Фаза 2: Отправка (Traveling)
- Корабль летит на Марс (анимация/таймер)
- UI заблокирован

### Фаза 3: Обработка (Processing)
1. Доставленные ресурсы добавляются к шкалам
2. Срабатывает текущее событие (из JSON-последовательности)
3. Из шкал вычитаются ресурсы согласно событию
4. **Люди потребляют ресурсы** (настраивается в конфиге)
   - Каждый человек тратит оружие и припасы
   - Количество зависит от `WeaponsPerPerson` и `SuppliesPerPerson`
5. Проверка условий проигрыша/победы
6. Переход к следующему ходу

### Фаза 4: Конец игры (GameOver)
- Победа: если продержались N ходов
- Поражение: если любая шкала вышла за пределы [0, 100]

## ⚙️ Структура кода

### Models (Модели данных)
```
ResourceType.cs - перечисление ресурсов
ShipCargo.cs - груз корабля с общей вместимостью
MarsColonyState.cs - состояние шкал колонии
TurnEvent.cs - событие хода из JSON
GameState.cs - текущее состояние игры
GameConfig.cs - ScriptableObject с настройками
```

### Managers (Синглтоны)
```
ShipManager - управление кораблем и грузом
MarsManager - управление шкалами + предпросмотр
EventManager - загрузка и воспроизведение событий из JSON
GameFlowManager - главный контроллер игрового цикла
```

## 📝 Формат JSON событий

```json
{
  "events": [
    {
      "id": "turn_1",
      "title": "Название события",
      "description": "Описание что происходит",
      "resourceChanges": {
        "weapons": -2,
        "supplies": -3,
        "people": 0
      }
    }
  ]
}
```

## 🎨 Интеграция с UI

### Кнопки загрузки ресурсов
```csharp
public void OnWeaponsButtonClick()
{
    ShipManager.Instance.LoadResource(ResourceType.Weapons, 1);
}
```

### Кнопка отправки
```csharp
public void OnLaunchButtonClick()
{
    ShipManager.Instance.LaunchShip();
}
```

### Отображение шкал
```csharp
void Start()
{
    MarsManager.Instance.OnColonyStateChanged += UpdateBars;
}

void UpdateBars(MarsColonyState state)
{
    weaponsBar.value = state.Weapons / 100f;
    suppliesBar.value = state.Supplies / 100f;
    peopleBar.value = state.People / 100f;
}
```

### Предпросмотр изменений
```csharp
void Start()
{
    MarsManager.Instance.OnPreviewUpdated += ShowPreview;
}

void ShowPreview(Dictionary<ResourceType, int> changes)
{
    weaponsPreview.text = changes[ResourceType.Weapons].ToString();
    // Показываем со знаком: +5 или -3
}
```

## 🔧 Настройка конфига

1. Create → Mars Collector → Game Config
2. Установите:
   - **Ship Capacity** - вместимость корабля (по умолчанию: 10)
   - **Starting Weapons/Supplies/People** - стартовые значения шкал (по умолчанию: 50)
   - **Weapons Per Person** - сколько оружия потребляет 1 человек за ход (по умолчанию: 0.1)
   - **Supplies Per Person** - сколько припасов потребляет 1 человек за ход (по умолчанию: 0.2)
   - **Turns To Win** - количество ходов для победы (по умолчанию: 20)
   - **Events Json** - ссылка на JSON файл с событиями
3. Назначьте конфиг в GameFlowManager на сцене

### Пример расчета потребления:
- У вас 50 человек
- WeaponsPerPerson = 0.1
- SuppliesPerPerson = 0.2
- **За ход потратится:** Оружие: 5, Припасы: 10

## 🎲 Условия проигрыша

| Шкала | <= 0 | >= 100 |
|-------|------|--------|
| Weapons | Марсиане уничтожили колонию | Милитаризация → Революция |
| Supplies | Голод → Смерть | Гедонизм → Коллапс |
| People | Вымирание населения | Перенаселение → Коллапс |

## 🎯 API менеджеров

### ShipManager
```csharp
LoadResource(ResourceType type, int amount = 1) -> bool
UnloadResource(ResourceType type, int amount = 1) -> bool
LaunchShip() -> void
GetResourceAmount(ResourceType type) -> int
GetAvailableSpace() -> int
CanAddResource(int amount = 1) -> bool

Events:
- OnResourceAdded(ResourceType, int)
- OnResourceRemoved(ResourceType, int)
- OnShipLaunched(ShipCargo)
- OnCargoChanged(int current, int max)
```

### MarsManager
```csharp
GetResourceValue(ResourceType type) -> int
GetPreview() -> Dictionary<ResourceType, int>
ApplyCargoDelivery(ShipCargo cargo) -> void
ApplyChanges(Dictionary<ResourceType, int> changes) -> void

Events:
- OnColonyStateChanged(MarsColonyState)
- OnPreviewUpdated(Dictionary<ResourceType, int>)
- OnGameOver(string reason)
```

### GameFlowManager
```csharp
RestartGame() -> void
EndGame(bool victory, string message) -> void

Properties:
- CurrentGameState (GameState)
- CurrentPhase (GamePhase)

Events:
- OnPhaseChanged(GamePhase)
- OnTurnChanged(int)
- OnGameEnded(bool victory, string message)
```

### EventManager
```csharp
LoadEvents(TextAsset jsonFile) -> void
GetNextEvent() -> TurnEvent
Reset() -> void
GetTotalEvents() -> int

Events:
- OnEventTriggered(TurnEvent)
```
