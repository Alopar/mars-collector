# Mars Collector - Список задач

## ✅ Этап 1: Документация
- [x] Создать GAME_DESIGN_DOC.md
- [x] Создать TODO.md

## 📦 Этап 2: Модели данных
- [ ] ResourceType enum
- [ ] ShipCargo class
- [ ] MarsColonyState class
- [ ] TurnEvent и EventSequence classes
- [ ] GameState class
- [ ] GameConfig ScriptableObject

## 🎮 Этап 3: Менеджеры
- [ ] ShipManager
- [ ] MarsManager
- [ ] EventManager
- [ ] GameFlowManager

## ⚙️ Этап 4: Конфигурация
- [ ] Создать events.json с примерами событий
- [ ] Создать инструкцию по настройке Unity

## 🎨 Этап 5: UI Integration (Пользователь делает сам)
- [ ] Создать ShipView
  - [ ] Кнопки загрузки ресурсов (Weapons, Supplies, People)
  - [ ] Индикатор заполненности корабля
  - [ ] Кнопка отправки
- [ ] Создать MarsStatsView
  - [ ] 3 слайдера для шкал
  - [ ] Числовые значения
  - [ ] Предпросмотр изменений (стрелки +/-)
- [ ] Создать EventView
  - [ ] Панель с названием и описанием события
- [ ] Создать TurnCounterView
  - [ ] Отображение "Ход X из Y"
- [ ] Создать GameOverScreen
  - [ ] Сообщение победы/поражения
  - [ ] Кнопка рестарта

## 🚀 Этап 6: Финализация
- [ ] Тестирование полного цикла
- [ ] Балансировка событий
- [ ] Настройка конфига под джем
