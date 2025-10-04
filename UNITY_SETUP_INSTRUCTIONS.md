# Mars Collector - Инструкция по настройке в Unity

## ✅ Что уже готово (код)
- ✅ Все модели данных
- ✅ Все менеджеры-синглтоны
- ✅ JSON файл с событиями
- ✅ ScriptableObject для конфигурации

## 🔧 Что нужно сделать в Unity

### Шаг 1: Создать GameConfig
1. В Unity в папке `Assets/_Project/_CONFIGS/`
2. ПКМ → Create → Mars Collector → Game Config
3. Назовите: `GameConfig`
4. Настройте параметры в инспекторе:
   - **Ship Capacity:** 10 (вместимость корабля)
   - **Starting Weapons:** 50
   - **Starting Supplies:** 50
   - **Starting People:** 50
   - **Turns To Win:** 20 (количество ходов для победы)
   - **Events Json:** перетащите файл `events.json` из той же папки

### Шаг 2: Настроить сцену Gameplay
1. Откройте игровую сцену
2. Создайте пустой GameObject, назовите: `GameManagers`
3. Добавьте на него компоненты (Add Component):
   - `ShipManager`
   - `MarsManager`
   - `EventManager`
   - `GameFlowManager`
4. На компоненте `GameFlowManager` в инспекторе:
   - Перетащите созданный `GameConfig` в поле **Config**
   - Настройте **Ship Travel Duration** (например 2 секунды)

### Шаг 3: Создать UI элементы

#### A. Панель корабля (ShipView)
Создайте UI элементы:
- **3 кнопки** для загрузки ресурсов:
  - Кнопка "Оружие" (Weapons)
  - Кнопка "Припасы" (Supplies)
  - Кнопка "Люди" (People)
- **Текст** для отображения: "Загружено: X/10"
- **Кнопка "Отправить"** (Launch)

Создайте скрипт `ShipView.cs`:
```csharp
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using GameApplication.Gameplay.Managers;
using GameApplication.Gameplay.Models;

public class ShipView : MonoBehaviour
{
    [Header("Buttons")]
    public Button weaponsButton;
    public Button suppliesButton;
    public Button peopleButton;
    public Button launchButton;
    
    [Header("Display")]
    public TextMeshProUGUI cargoText;

    private void Start()
    {
        weaponsButton.onClick.AddListener(() => LoadWeapons());
        suppliesButton.onClick.AddListener(() => LoadSupplies());
        peopleButton.onClick.AddListener(() => LoadPeople());
        launchButton.onClick.AddListener(() => LaunchShip());

        ShipManager.Instance.OnCargoChanged += UpdateCargoDisplay;
        UpdateCargoDisplay(0, ShipManager.Instance.CurrentCargo.MaxCapacity);
    }

    private void LoadWeapons()
    {
        ShipManager.Instance.LoadResource(ResourceType.Weapons, 1);
    }

    private void LoadSupplies()
    {
        ShipManager.Instance.LoadResource(ResourceType.Supplies, 1);
    }

    private void LoadPeople()
    {
        ShipManager.Instance.LoadResource(ResourceType.People, 1);
    }

    private void LaunchShip()
    {
        ShipManager.Instance.LaunchShip();
    }

    private void UpdateCargoDisplay(int current, int max)
    {
        cargoText.text = $"Загружено: {current}/{max}";
        launchButton.interactable = current > 0;
    }

    private void OnDestroy()
    {
        if (ShipManager.Instance != null)
            ShipManager.Instance.OnCargoChanged -= UpdateCargoDisplay;
    }
}
```

#### B. Панель состояния Марса (MarsStatsView)
Создайте UI элементы:
- **3 слайдера** (Slider) для шкал:
  - Weapons Bar
  - Supplies Bar
  - People Bar
- **3 текста** для значений: "Оружие: 50/100"
- **3 текста** для предпросмотра: "+5" или "-3"

Создайте скрипт `MarsStatsView.cs`:
```csharp
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using GameApplication.Gameplay.Managers;
using GameApplication.Gameplay.Models;

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

    private void Start()
    {
        MarsManager.Instance.OnColonyStateChanged += UpdateBars;
        MarsManager.Instance.OnPreviewUpdated += UpdatePreview;
    }

    private void UpdateBars(MarsColonyState state)
    {
        weaponsBar.value = state.Weapons / 100f;
        suppliesBar.value = state.Supplies / 100f;
        peopleBar.value = state.People / 100f;

        weaponsText.text = $"Оружие: {state.Weapons}/100";
        suppliesText.text = $"Припасы: {state.Supplies}/100";
        peopleText.text = $"Люди: {state.People}/100";
    }

    private void UpdatePreview(Dictionary<ResourceType, int> preview)
    {
        if (preview.ContainsKey(ResourceType.Weapons))
        {
            int val = preview[ResourceType.Weapons];
            weaponsPreview.text = val >= 0 ? $"+{val}" : val.ToString();
        }
        else
        {
            weaponsPreview.text = "";
        }

        if (preview.ContainsKey(ResourceType.Supplies))
        {
            int val = preview[ResourceType.Supplies];
            suppliesPreview.text = val >= 0 ? $"+{val}" : val.ToString();
        }
        else
        {
            suppliesPreview.text = "";
        }

        if (preview.ContainsKey(ResourceType.People))
        {
            int val = preview[ResourceType.People];
            peoplePreview.text = val >= 0 ? $"+{val}" : val.ToString();
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
```

#### C. Панель события (EventView)
Создайте UI:
- **Текст** для названия события
- **Текст** для описания

Создайте скрипт `EventView.cs`:
```csharp
using UnityEngine;
using TMPro;
using GameApplication.Gameplay.Managers;
using GameApplication.Gameplay.Models;

public class EventView : MonoBehaviour
{
    public TextMeshProUGUI titleText;
    public TextMeshProUGUI descriptionText;
    public GameObject eventPanel;

    private void Start()
    {
        EventManager.Instance.OnEventTriggered += ShowEvent;
        eventPanel.SetActive(false);
    }

    private void ShowEvent(TurnEvent turnEvent)
    {
        titleText.text = turnEvent.title;
        descriptionText.text = turnEvent.description;
        eventPanel.SetActive(true);
    }

    private void OnDestroy()
    {
        if (EventManager.Instance != null)
            EventManager.Instance.OnEventTriggered -= ShowEvent;
    }
}
```

#### D. Счетчик ходов (TurnCounterView)
Создайте UI:
- **Текст**: "Ход X из Y"

Создайте скрипт `TurnCounterView.cs`:
```csharp
using UnityEngine;
using TMPro;
using GameApplication.Gameplay.Managers;

public class TurnCounterView : MonoBehaviour
{
    public TextMeshProUGUI turnText;

    private void Start()
    {
        GameFlowManager.Instance.OnTurnChanged += UpdateTurn;
        UpdateTurn(0);
    }

    private void UpdateTurn(int currentTurn)
    {
        int totalTurns = GameFlowManager.Instance.CurrentGameState.TurnsToWin;
        turnText.text = $"Ход {currentTurn} из {totalTurns}";
    }

    private void OnDestroy()
    {
        if (GameFlowManager.Instance != null)
            GameFlowManager.Instance.OnTurnChanged -= UpdateTurn;
    }
}
```

#### E. Экран конца игры (GameOverScreen)
Создайте UI:
- **Панель** (по умолчанию выключена)
- **Текст** для сообщения (победа/поражение)
- **Кнопка "Рестарт"**

Создайте скрипт `GameOverScreen.cs`:
```csharp
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using GameApplication.Gameplay.Managers;

public class GameOverScreen : MonoBehaviour
{
    public GameObject gameOverPanel;
    public TextMeshProUGUI messageText;
    public Button restartButton;

    private void Start()
    {
        GameFlowManager.Instance.OnGameEnded += ShowGameOver;
        restartButton.onClick.AddListener(Restart);
        gameOverPanel.SetActive(false);
    }

    private void ShowGameOver(bool victory, string message)
    {
        messageText.text = message;
        gameOverPanel.SetActive(true);
    }

    private void Restart()
    {
        gameOverPanel.SetActive(false);
        GameFlowManager.Instance.RestartGame();
    }

    private void OnDestroy()
    {
        if (GameFlowManager.Instance != null)
            GameFlowManager.Instance.OnGameEnded -= ShowGameOver;
    }
}
```

### Шаг 4: Подключить UI скрипты
1. Создайте все UI скрипты в папке `Assets/_Project/_SCRIPTS/UI/`
2. Добавьте каждый скрипт на соответствующий GameObject в Canvas
3. В инспекторе перетащите UI элементы в нужные поля скриптов

### Шаг 5: Тестирование
1. Запустите сцену
2. Проверьте:
   - ✅ Загрузка ресурсов работает
   - ✅ Счетчик груза обновляется
   - ✅ Кнопка "Отправить" активна когда груз > 0
   - ✅ Корабль летит (задержка)
   - ✅ Шкалы обновляются
   - ✅ Предпросмотр показывает изменения следующего хода
   - ✅ События срабатывают
   - ✅ Победа/поражение работают

## 🎨 Дополнительные улучшения (опционально)

### Блокировка UI во время полета
Добавьте в `ShipView`:
```csharp
void Start()
{
    // ... существующий код
    GameFlowManager.Instance.OnPhaseChanged += HandlePhaseChange;
}

void HandlePhaseChange(GamePhase phase)
{
    bool canInteract = phase == GamePhase.Loading;
    weaponsButton.interactable = canInteract;
    suppliesButton.interactable = canInteract;
    peopleButton.interactable = canInteract;
    launchButton.interactable = canInteract;
}
```

### Цветовая индикация опасности шкал
Добавьте в `MarsStatsView`:
```csharp
void UpdateBars(MarsColonyState state)
{
    // ... существующий код
    
    weaponsBar.fillRect.GetComponent<Image>().color = GetColorForValue(state.Weapons);
    suppliesBar.fillRect.GetComponent<Image>().color = GetColorForValue(state.Supplies);
    peopleBar.fillRect.GetComponent<Image>().color = GetColorForValue(state.People);
}

Color GetColorForValue(int value)
{
    if (value <= 20 || value >= 80) return Color.red;
    if (value <= 40 || value >= 60) return Color.yellow;
    return Color.green;
}
```

### Анимация полета корабля
Используйте событие `OnPhaseChanged` для запуска анимаций:
```csharp
void Start()
{
    GameFlowManager.Instance.OnPhaseChanged += AnimatePhase;
}

void AnimatePhase(GamePhase phase)
{
    switch (phase)
    {
        case GamePhase.Traveling:
            // Запустить анимацию полета
            break;
        case GamePhase.Processing:
            // Эффекты обработки
            break;
    }
}
```

## 📊 Балансировка
Если игра слишком сложная/легкая, отредактируйте `events.json`:
- Измените значения `resourceChanges`
- Добавьте/удалите события
- Настройте `TurnsToWin` в GameConfig

## 🐛 Отладка
Если что-то не работает:
1. Проверьте консоль Unity на ошибки
2. Убедитесь что GameConfig назначен в GameFlowManager
3. Проверьте что events.json загружается (смотрите лог "Loaded X events")
4. Убедитесь что все менеджеры на сцене

## 🎉 Готово!
После выполнения всех шагов игра должна работать полностью!

