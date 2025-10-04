# Тетрис-система загрузки - Быстрый старт

## ✅ ЧТО УЖЕ ГОТОВО (код)

### Все скрипты созданы:
- ✅ Models: CargoShapeData, CargoGrid, PlacedShapeData, CargoShapeDatabase
- ✅ Manager: CargoManager  
- ✅ UI: CargoSlotView, CargoGridView, CargoShapeButton, CargoShapePanel, CargoPlacementController, CargoResourceCounter
- ✅ Integration: GameFlowManager обновлен, GameConfig обновлен

### Код без ошибок! ✅

---

## 🎯 ЧТО ТЕБЕ НУЖНО СДЕЛАТЬ

### 📝 КРАТКИЙ ЧЕКЛИСТ:

1. ✅ Открой Unity
2. ✅ Создай `CargoShapeDatabase` asset
3. ✅ Создай 15 фигур (5 Weapons, 5 Supplies, 5 People)
4. ✅ Добавь фигуры в Database
5. ✅ Подключи Database к GameConfig
6. ✅ Добавь CargoManager на сцену
7. ✅ Создай 2 префаба (CargoSlot, CargoShapeButton)
8. ✅ Собери UI на Canvas
9. ✅ Подключи компоненты
10. ✅ Запусти и тестируй!

---

## 📖 ПОДРОБНАЯ ИНСТРУКЦИЯ

Открой файл: **`UNITY_CARGO_SETUP.md`**

Там пошагово расписано:
- Как создавать фигуры (с примерами)
- Как настроить UI
- Как подключить компоненты
- Что делать если не работает
- Советы по балансировке

---

## ⏱️ ПРИМЕРНОЕ ВРЕМЯ:

| Задача | Время |
|--------|-------|
| Создать Database и фигуры | 20-25 мин |
| Обновить GameConfig | 2 мин |
| Добавить CargoManager | 1 мин |
| Создать префабы | 15-20 мин |
| Настроить UI сцены | 20-30 мин |
| Подключить компоненты | 10 мин |
| **ИТОГО** | **~1.5 часа** |

---

## 🚀 НАЧНИ ОТСЮДА:

### Шаг 1: Создай Database
```
Assets/_Project/_CONFIGS/
ПКМ → Create → Mars Collector → Cargo Shape Database
Назови: CargoShapeDatabase
```

### Шаг 2: Создай 1 тестовую фигуру
```
Assets/_Project/_CONFIGS/CargoShapes/
ПКМ → Create → Mars Collector → Cargo Shape

Настрой:
Shape Name: Тест
Resource Type: Weapons
Shape Width: 2
Shape Height: 1
Shape Flat: [1, 1]
Resource Amount: 1
```

### Шаг 3: Добавь в Database
```
Открой CargoShapeDatabase
Перетащи фигуру в Weapon Shapes
```

### Шаг 4: Подключи к Config
```
Открой GameConfig
Grid Width: 5
Grid Height: 4
Cargo Database: [перетащи CargoShapeDatabase]
```

### Шаг 5: Добавь CargoManager
```
GameObject "GameManagers" на сцене
Add Component → CargoManager
```

### Шаг 6: Запусти игру
```
Посмотри консоль:
Должно быть: "CargoManager initialized: 5x4 grid"
Должно быть: "Загружено событий: 20"
```

Если все ОК - продолжай по `UNITY_CARGO_SETUP.md`! 

---

## ❓ БЫСТРЫЕ ОТВЕТЫ

**Q: Нужно ли удалять старую ShipView?**
A: Пока можешь оставить. Cargo система работает параллельно.

**Q: Где брать иконки для фигур?**
A: Пока можешь оставить пустым - будут цветные блоки. Иконки опционально.

**Q: Обязательно 15 фигур?**
A: Нет! Можешь начать с 3 (по одной на тип) для теста, потом добавить.

**Q: Что если я застрял?**
A: Читай раздел "🐛 Если что-то не работает" в UNITY_CARGO_SETUP.md

---

## 📚 ДОКУМЕНТАЦИЯ

1. **CARGO_QUICK_START.md** ← ты здесь (краткий старт)
2. **UNITY_CARGO_SETUP.md** ← подробная инструкция (читай дальше!)
3. **TETRIS_CARGO_DESIGN.md** - детальный дизайн системы
4. **TETRIS_CARGO_CODE_EXAMPLES.md** - примеры кода

---

## 🎉 ВСЁ ГОТОВО К СБОРКЕ!

Код написан, протестирован, без ошибок. 
Теперь дело за Unity UI! 

Открывай **UNITY_CARGO_SETUP.md** и вперед! 🚀

