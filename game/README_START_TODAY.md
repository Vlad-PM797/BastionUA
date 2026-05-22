# BastionUA — Unity Prototype (Week 2 ✅)

## Статус
- Week 1: [`docs/week1_checkpoint.md`](../docs/week1_checkpoint.md)
- Week 2: [`docs/week2_checkpoint.md`](../docs/week2_checkpoint.md)

## Вимоги
- **Unity 6000.4.4f1** (Unity 6)
- Проєкт: `E:\BastionUA\game`

## Швидкий старт
1. Unity Hub → **Open** → `E:\BastionUA\game`
2. Сцена: `Assets/Scenes/Boot.unity`
3. **Play**

HUD, карта і Event popup створюються автоматично.

## Що працює
- **Resources:** tap (+5) + auto tick (+1/sec)
- **Map HUD:** інтерактивна карта Kyiv / Chernihiv / Sumy
- **Battle:** симуляція для обраного регіону
- **Save/load:** JSON autosave (15 sec)
- **Events:** Гостомель (старт) → Чорнобаївка (після 1 battle)

## Керування

### UI
- **Клік на вузол карти** — вибір регіону
- **+ Ammo (Tap)** — додати ammo
- **Battle** — бій у обраному регіоні

### Dev hotkeys
| Key | Дія |
|-----|-----|
| T | +ammo |
| 1/Q | Kyiv |
| 2/W | Chernihiv |
| 3/E | Sumy |
| B | Battle |
| S | Save |

## Save file
```
%USERPROFILE%\AppData\LocalLow\BastionUA\BastionUA\bastionua_save.json
```

## Структура скриптів
```
Assets/Scripts/
  Bootstrap/   GameBootstrap
  Core/        GameState, events, MapUiConstants
  Services/    Resource, Map, Battle, Save, Event, EventTrigger
  UI/          HudController, RegionMapView, EventPopupController
  Editor/      UnityVerificationRunner
```

## Week 3 — наступні кроки
1. Git commit (map + docs)
2. Event #3 або map v2
3. Windows dev build
