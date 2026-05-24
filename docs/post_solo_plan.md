# Bastion UA — Post-Solo Plan

Статус: **ACTIVE**  
Попередній етап: [Week 10 Checkpoint](./week10_checkpoint.md) ✅  
Ціль: перетворити solo prototype на **pitch-ready demo** + підготовку до team MVP.

---

## Фази

| Фаза | Тривалість | Результат |
|------|------------|-----------|
| **A — Playtest & polish** | 1–2 тижні | 3–5 зовнішніх тестерів, top-3 fixes, RC build |
| **B — Demo package** | 3–5 днів | zip/APK, відео 60–90 с, one-pager |
| **C — MVP gate** | 1–2 тижні | GDD lock, gap analysis, W1 backlog, funding/outreach |

Team-scale roadmap: [01_mvp_roadmap.md](./01_mvp_roadmap.md).

---

## Фаза A — Playtest & polish

### A1. Підготовка (solo, 1 день)
- [ ] **Build Windows Release** → **Package Windows Release Demo**
- [ ] (Optional) **Build Android Dev** → APK для мобільних тестерів
- [x] Local playtest metrics (`PlaytestMetricsService` → `bastionua_playtest_metrics.json`)
- [x] Real SFX drop-in folder (`Assets/Resources/Audio/` — fallback to procedural)
- [ ] Розіслати [playtest guide](./playtest_guide_ua.md) + [feedback form](./playtest_feedback_form.md)
- [ ] Створити backlog `docs/playtest_backlog.md` (issues від тестерів)

### A2. Збір feedback (3–5 тестерів, 3–5 днів)
Мінімум 3 людини, які **не писали код**. Критерії:
- пройшли onboarding без підказок розробника;
- зіграли ≥15 хв;
- заповнили форму (5 питань).

### A3. Top-3 fixes (solo, 2–4 дні)
Пріоритет за severity × frequency:
1. **Blocker** — не можна завершити loop / crash / save corrupt
2. **Confusion** — не зрозуміло objective, prestige, battle
3. **Feel** — контраст, швидкість, feedback, звук

Після фіксів: RC-2 build + короткий regression (verification batchmode).

### A4. Definition of Done — Фаза A
- [ ] ≥3 external playtests завершені
- [ ] Top-3 issues fixed або задокументовані як known
- [ ] `docs/playtest_backlog.md` оновлений
- [ ] RC-2 build заархівований

---

## Фаза B — Demo package

### B1. Артефакти
| Артефакт | Шлях / формат |
|----------|----------------|
| Windows demo zip | `game/Builds/BastionUA-Windows-demo.zip` |
| Android APK (optional) | `game/Builds/Android/BastionUA-dev.apk` |
| Demo video | 60–90 с, gameplay + prestige moment |
| One-pager | [demo_one_pager_ua.md](./demo_one_pager_ua.md) |

### B2. Сценарій відео (рекомендований)
1. Splash / map (5 с)
2. Tap + auto ammo (10 с)
3. Battle → region liberated (15 с)
4. Event choice (Hostomel або Irpin) (15 с)
5. Upgrade + event log (10 с)
6. Prestige unlock hint → reset (15 с)
7. End card: «Bastion UA — prototype» + GitHub / contact

### B3. Definition of Done — Фаза B
- [ ] Zip/APK перевірений на чистій машині
- [ ] Відео змонтоване і викладене (Drive / YouTube unlisted)
- [ ] One-pager готовий для pitch / Discord / Telegram

---

## Фаза C — MVP gate (team kickoff)

Не починати код MVP, поки не закрито A + B.

### C1. Документи
- [ ] [GDD v0.1](./00_gdd_v0.1.md) — review + sign-off (scope lock)
- [ ] [MVP gap analysis](./mvp_gap_analysis.md) — що є vs що треба
- [ ] Backlog W1 з [01_mvp_roadmap.md](./01_mvp_roadmap.md) → Linear/Trello

### C2. Org & funding (parallel)
- [ ] Landing + pre-registration (astro/next або Tilda)
- [ ] Discord / Telegram community channel
- [ ] 3 LOI outreach (фонди ЗСУ) — без in-game rewards
- [ ] Legal skim: [05_legal.md](./05_legal.md) — TM, EULA outline

### C3. Tech decisions перед W1
| Рішення | Solo зараз | MVP target |
|---------|------------|------------|
| Unity | 6000.4.4f1 | 2022 LTS або 6 LTS (team vote) |
| Save | Local JSON | Server + device_id |
| Map | 4 regions PNG | 3 fronts + Bezier front line |
| Events | 4 hardcoded | JSON scenario engine |
| SFX | Procedural | Licensed packs (Kenney / Sonniss) |
| Backend | None | FastAPI skeleton W1 |

### C4. Definition of Done — Фаза C
- [ ] GDD v0.2 або v0.1 signed
- [ ] MVP backlog W1–W4 imported у task tracker
- [ ] Seed/grant pipeline стартований OR co-founder Unity hire in pipeline
- [ ] Prototype demo link у pitch deck [03_pitch_deck.md](./03_pitch_deck.md)

---

## Щотижневий ритм (solo post-solo)

| День | Фокус |
|------|--------|
| Пн | Playtest outreach / fix #1 |
| Вт | Fix #2 + regression |
| Ср | Fix #3 + demo video raw capture |
| Чт | Video edit + one-pager |
| Пт | Docs, MVP gap, backlog grooming |
| Сб–Нд | Buffer / optional Android QA |

---

## Наступний крок зараз

**Фаза A1** — зібрати demo zip і надіслати 3 тестерам [playtest guide](./playtest_guide_ua.md).

Після feedback — повернутися сюди і оновити `playtest_backlog.md`; top-3 fixes робимо в Unity разом.
