# Bastion UA — Week 1 Checkpoint

Статус: **DONE**  
Дата закриття: 2026-05-21  
Unity: **6000.4.4f1**  
Проєкт: `E:\BastionUA\game`

---

## 1. Definition of Done — результат

| Критерій | Статус |
|----------|--------|
| Сцена запускається без compile errors | ✅ |
| `GameBootstrap` ініціалізує гру | ✅ |
| Ресурс збільшується по таймеру | ✅ |
| Ресурс можна додати вручну (кнопка + hotkey) | ✅ |
| HUD показує актуальний ammo / morale / region | ✅ |
| Стан централізований у `GameState` + services | ✅ |
| Save/load працює | ✅ |
| Known issues зафіксовані | ✅ |

**Week 1 вважається завершеним.**

---

## 2. Changelog (Week 1)

### Core loop
- `GameBootstrap` — composition root, autosave кожні 15 сек
- `GameState` — ammo, morale, 3 регіони, selected region, completed events
- `ResourceService` — manual tap (+5) + auto tick (+1/sec)
- `MapService` — Kyiv, Chernihiv, Sumy + status (Safe/Danger/Occupied)
- `BattleService` — спрощена симуляція бою
- `SaveService` — JSON save/load з fallback

### UI (W1-04)
- Runtime HUD: ammo, morale, selected region
- Кнопки регіонів + Tap + Battle
- Dev hotkeys збережені (T, 1/2/3, Q/W/E, B, S)

### Events
- `EventService` + popup UI
- Перша подія: **«Гостомель»** (2 вибори, ефект на ammo/morale/Kyiv)
- Подія показується один раз, зберігається в `CompletedEventIds`

### Tooling
- Unity project у `game/` (не лише скрипти)
- `Boot.unity` + `UnityVerificationRunner` (batchmode перевірки)
- Автоматичні тести: compile, resources, map, battle, save, HUD, Hostomel

### Manual QA (підтверджено в Editor)
- Play Mode стабільний
- HUD оновлюється в realtime
- Регіон selection через кнопки і клавіші
- Save після Play / Stop
- Hostomel popup + choice effects

---

## 3. Smoke test checklist (повторити перед Week 2)

- [ ] Unity Hub → `E:\BastionUA\game` → Boot scene → Play
- [ ] HUD видно: Ammo, Morale, Selected, 3 регіони
- [ ] **+ Ammo (Tap)** → ammo +5
- [ ] Auto tick ~1/sec
- [ ] Клік регіону → Selected змінюється
- [ ] **Battle** → morale + status регіону змінюються
- [ ] Stop → Play → прогрес відновлюється
- [ ] (Новий save) Hostomel popup через ~2.5 сек → choice → ефект

---

## 4. Known issues (Week 2 backlog)

| # | Issue | Пріоритет | Нotes |
|---|-------|-----------|-------|
| 1 | Input Manager deprecation warning (Unity 6) | P2 | Поки hotkeys + uGUI buttons; Input System — пізніше |
| 2 | Save path змінився після rename (`BastionUA/BastionUA`) | P1 | Старий save: `%LOCALAPPDATA%/../LocalLow/DefaultCompany/_unity_bootstrap/` |
| 3 | `DateTime` у save через JsonUtility — не критично | P3 | Працює для прототипу |
| 4 | HUD runtime-only, без дизайн-системи | P2 | Week 2 polish |
| 5 | Hostomel trigger тільки on-start | P2 | Week 2: умовні тригери |
| 6 | Немає git repo | P1 | Ініціалізувати перед Week 2 |
| 7 | Product docs (`README.md`) — biz-focused, не dev | P3 | Dev guide: `game/README_START_TODAY.md` |

---

## 5. Save migration (після rename проєкту)

Новий шлях save:
```
%USERPROFILE%\AppData\LocalLow\BastionUA\BastionUA\bastionua_save.json
```

Щоб перенести старий прогрес — скопіюй `bastionua_save.json` з:
```
%USERPROFILE%\AppData\LocalLow\DefaultCompany\_unity_bootstrap\
```
у нову папку (створиться після першого Play).

---

## 6. Week 2 entry point (рекомендований порядок)

1. **Git init** + перший commit
2. **Cleanup** — Input System (опційно), HUD polish
3. **Event #2** — Чорнобаївка (той самий шаблон)
4. **Event triggers** — після battle / region / ammo threshold
5. **Map visualization** — проста карта замість списку кнопок

---

## 7. Команди для перевірки (batchmode)

```powershell
& "C:\Program Files\Unity\Hub\Editor\6000.4.4f1\Editor\Unity.exe" `
  -batchmode -nographics -quit `
  -projectPath "E:\BastionUA\game" `
  -executeMethod BastionUA.EditorTools.UnityVerificationRunner.RunAll `
  -logFile "E:\BastionUA\logs\unity_verify.log"
```

Очікування: `[UnityVerification] ALL CHECKS PASSED.`

---

**Week 1 closed. Ready for Week 2.**
