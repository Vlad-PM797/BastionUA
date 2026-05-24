# Week 9 Checkpoint — Prestige, SFX, Event Log

Статус: **DONE**

## Deliverables

### 1. Prestige cycle
- `PrestigeService` — reset кампанії з permanent bonus
- Умова: 4 events пройдено + жоден регіон не `Occupied`
- Бонус за рівень: +3 damage, +20 ammo, +5 morale старту
- Max prestige: 5
- UI: кнопка **Prestige** (bottom bar) + лічильник у top bar
- Hotkey: `P`

### 2. Procedural SFX stub
- `AudioFeedbackController` + `ProceduralToneGenerator`
- Звуки: tap, battle win/lose, event, upgrade, prestige
- Без audio assets — sine tones runtime

### 3. Event log
- `EventLogService` — останні 5 подій, 3 рядки в HUD
- Лог: battles, events, upgrades, prestige, reset

### 4. Verification
- `VerifyPrestigeFlow` у `UnityVerificationRunner`

## Manual QA
1. Stop → Play на Boot
2. Пройти 4 events + звільнити всі регіони (без Occupied)
3. Objective → «Натисни Prestige…»
4. **Prestige** або `P` → новий цикл, `Prestige: 1`, більше damage
5. Tap / Battle / Upgrade — короткі звуки

## Next (Week 10 / demo polish)
- Android dev build
- Playtest feedback fixes
- Optional: real SFX assets replace procedural tones
