# BastionUA — Unity Prototype (Week 1 ✅)

## Статус
Week 1 закрито (2026-05-21). Деталі: [`docs/week1_checkpoint.md`](../docs/week1_checkpoint.md)

## Вимоги
- **Unity 6000.4.4f1** (Unity 6)
- Проєкт: `E:\BastionUA\game`

## Швидкий старт
1. Unity Hub → **Open** → `E:\BastionUA\game`
2. Сцена: `Assets/Scenes/Boot.unity`
3. **Play**

HUD і Event popup створюються автоматично — нічого вручну на сцені додавати не треba.

## Що працює
- **Resources:** tap (+5) + auto tick (+1/sec)
- **Map:** Kyiv, Chernihiv, Sumy (Safe / Danger / Occupied)
- **Battle:** симуляція для обраного регіону
- **HUD:** ammo, morale, selected, кнопки регіонів, Tap, Battle
- **Save/load:** JSON autosave (15 sec) + manual
- **Event:** «Гостомель» popup (2 choices, один раз за save)

## Керування

### UI
- **+ Ammo (Tap)** — додати ammo
- **Kyiv / Chernihiv / Sumy** — вибір регіону
- **Battle** — бій у обраному регіоні

### Dev hotkeys (тимчасово)
| Key | Дія |
|-----|-----|
| T | +ammo |
| 1 / Q | Kyiv |
| 2 / W | Chernihiv |
| 3 / E | Sumy |
| B | Battle |
| S | Manual save |

## Save file
```
%USERPROFILE%\AppData\LocalLow\BastionUA\BastionUA\bastionua_save.json
```

Старий save (до rename): `DefaultCompany\_unity_bootstrap\` — див. migration у checkpoint doc.

## Структура скриптів
```
Assets/Scripts/
  Bootstrap/   GameBootstrap
  Core/        GameState, GameConstants, events
  Services/    Resource, Map, Battle, Save, Event
  UI/          HudController, EventPopupController
  Editor/      UnityVerificationRunner
```

## Автоперевірка (batchmode)
```powershell
& "C:\Program Files\Unity\Hub\Editor\6000.4.4f1\Editor\Unity.exe" `
  -batchmode -nographics -quit `
  -projectPath "E:\BastionUA\game" `
  -executeMethod BastionUA.EditorTools.UnityVerificationRunner.RunAll `
  -logFile "E:\BastionUA\logs\unity_verify.log"
```

## Week 2 — наступні кроки
1. ~~Git init + commit~~ ✅
2. ~~Event #2 (Чорнобаївка) + triggers~~ ✅
3. HUD polish / проста карта
4. Week 2 checkpoint
