# Bastion UA — Week 3 Checkpoint

Статус: **DONE**  
Дата закриття: 2026-05-21  
Unity: **6000.4.4f1**

---

## 1. Definition of Done

| Критерій | Статус |
|----------|--------|
| Event #3 Ірпінь | ✅ |
| Objective hint bar | ✅ |
| Onboarding hint | ✅ |
| Reset Save | ✅ |
| Windows dev build menu | ✅ |
| Manual QA | ✅ (user confirmed) |

**Week 3 closed.**

---

## 2. Changelog

### Event #3 — Ірпінь
- Trigger: Chornobaivka done + TotalBattles ≥ 2
- **Евакуювати цивільних** / **Тримати лінію оборони**
- Ефект на Kyiv + ammo/morale

### Usability
- `ObjectiveHintService` — dynamic objective under top bar
- `HasSeenOnboarding` — first-session hint
- **Reset Save** button in legend panel
- `SaveService.DeleteSave()` + `GameBootstrap.ResetProgress()`

### Build
- **BastionUA → Build Windows Dev** → `game/Builds/Windows/BastionUA.exe`

---

## 3. Event pipeline (full)

| # | Event | Trigger | Prerequisites |
|---|-------|---------|---------------|
| 1 | Гостомель | OnSessionStart | — |
| 2 | Чорнобаївка | OnProgress | Hostomel + ≥1 battle |
| 3 | Ірпінь | OnProgress | Chornobaivka + ≥2 battles |

---

## 4. Week 4 entry

1. Units / battle modifiers
2. 3 upgrades branch
3. Battle result popup
4. Map silhouette v2 (optional)

---

**Ready for Week 4.**
