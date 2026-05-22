# Week 5 Checkpoint — Map v2 & Battle Balance

Статус: **DONE** (2026-05-21)

## Deliverables

### 1. Map v2 — силует України
- Полігональний силует (`MapSilhouetteGraphic`) замість простого прямокутника
- Заголовок карти: **Карта України**
- 4 регіони з `RegionCatalog`: Kyiv, Chernihiv, Sumy, **Kharkiv**
- Нові з’єднання: Chernihiv–Kyiv–Sumy–Kharkiv + Kyiv–Kharkiv

### 2. 4-й регіон — Kharkiv
- Стартовий статус: **Occupied** (складніший східний фронт)
- Міграція старих сейвів: `GameState.Normalize()` додає Kharkiv автоматично
- Hotkeys: **4** / numpad **4** / **R**

### 3. Баланс бою
- `BattleBalanceService`: HP/damage залежать від регіону та статусу
- Kharkiv має найвищий `EnemyHpBonus` (+18 базово + occupied bonus)
- Morale: <30 → −10% damage; ≥70 → +2 damage
- Progression: +3 enemy HP кожні 4 бої (cap +15)

### 4. GitHub + build share
- Репозиторій на GitHub (див. README)
- Windows build: **BastionUA → Build Windows Dev** → `game/Builds/Windows/BastionUA.exe`
- `.gitignore` ігнорує `game/Builds/` — `.exe` для шерінгу збирається локально

## Manual QA
1. Play → на карті 4 маркери + силует UA
2. **R** / клік Kharkiv → Battle (важчий бій)
3. Старий сейв → Kharkiv з’являється після завантаження
4. Objective після Irpin → підказка про Kharkiv

## Next (Week 6+)
- Подія / контент для Kharkiv
- Release build (не тільки Development)
- Мобільний таргет / Telegram mini-app spike
