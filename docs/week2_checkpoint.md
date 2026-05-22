# Bastion UA — Week 2 Checkpoint

Статус: **DONE**  
Дата закриття: 2026-05-21  
Unity: **6000.4.4f1**  
Проєкт: `E:\BastionUA\game`

---

## 1. Definition of Done — результат

| Критерій | Статус |
|----------|--------|
| Git repository з baseline commit | ✅ |
| Event trigger system (prerequisites) | ✅ |
| Event #2: Чорнобаївка | ✅ |
| HUD map visualization | ✅ |
| Manual QA (map + events) | ✅ |
| Known issues зафіксовані | ✅ |

**Week 2 вважається завершеним.**

---

## 2. Changelog (Week 2)

### Git & process
- `git init` + `.gitignore` (Unity)
- Initial commit: `3c5da04` — Week 1 core + Week 2 event pipeline

### Event system v2
- `GameEventRegistry` — центральний schedule подій
- `EventTriggerService` — перевірка prerequisites
- `EventTriggerMode`: `OnSessionStart` / `OnProgress`
- `GameState.TotalBattles` — лічильник боїв для тригерів

### Events
| Event | Trigger | Умова |
|-------|---------|-------|
| **Гостомель** | OnSessionStart | перший запуск (~2.5 сек) |
| **Чорнобаївка** | OnProgress | Hostomel done + ≥1 battle |

**Чорнобаївка — вибори:**
- *Артилерійський удар* → Ammo −25, Morale +12, Sumy ↑
- *Зберегти боєприпаси* → Ammo +10, Morale −6, Sumy ↓

### HUD / Map
- `RegionMapView` — інтерактивна node-карта
- `MapUiConstants` — позиції Kyiv / Chernihiv / Sumy
- Легенда статусів (Safe / Danger / Occupied)
- Клік по вузлу = вибір регіону
- Кольорове кільце = status, синє = selected

### Manual QA (підтверджено)
- Карта відображається, вузли клікабельні
- Hostomel + Chornobaivka flow працює
- Auto tick, save, battle — без регресій

---

## 3. Smoke test checklist (повторити перед Week 3)

- [ ] Play → HUD: top bar + legend + map + bottom buttons
- [ ] Клік **Kyiv / Chernihiv / Sumy** на карті → `Selected` змінюється
- [ ] Статуси на карті (кольорові кільця) оновлюються після **Battle**
- [ ] Hostomel popup (новий save) → choice → ефект
- [ ] Після Hostomel + 1× **Battle** → popup **Чорнобаївка**
- [ ] Stop → Play → прогрес відновлюється

---

## 4. Known issues (Week 3 backlog)

| # | Issue | Пріоритет |
|---|-------|-----------|
| 1 | Uncommitted changes після map (потрібен commit) | P1 |
| 2 | Input Manager deprecation warning | P2 |
| 3 | Карта — abstract nodes, не silhouette UA | P2 |
| 4 | Chornobaivka прив'язана до Sumy (placeholder) | P3 |
| 5 | Немає remote git (GitHub) | P2 |
| 6 | `DateTime` у JsonUtility save | P3 |
| 7 | Event popup блокує gameplay — by design for now | P3 |

---

## 5. Save file

```
%USERPROFILE%\AppData\LocalLow\BastionUA\BastionUA\bastionua_save.json
```

Поля save (Week 2):
- `Ammo`, `Morale`, `Regions`, `LastSelectedRegionId`
- `CompletedEventIds` — пройдені події
- `TotalBattles` — для тригерів

---

## 6. Week 3 entry point (рекомендований порядок)

1. **Git commit** — map + docs (незакомічені зміни)
2. **Remote repo** — GitHub (optional)
3. **Event #3** — Ірпінь або Чорнобаївка polish
4. **Map v2** — спрощений silhouette UA або 4-й регіон
5. **Input System** — прибрати deprecation warning
6. **Build** — Windows dev build / playable demo

---

## 7. Verification command

```powershell
& "C:\Program Files\Unity\Hub\Editor\6000.4.4f1\Editor\Unity.exe" `
  -batchmode -nographics -quit `
  -projectPath "E:\BastionUA\game" `
  -executeMethod BastionUA.EditorTools.UnityVerificationRunner.RunAll `
  -logFile "E:\BastionUA\logs\unity_verify.log"
```

Очікування: `[UnityVerification] ALL CHECKS PASSED.`

---

**Week 2 closed. Ready for Week 3.**
