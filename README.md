# Bastion UA

*Безкоштовний idle-strategy про оборону України з 24.02.2022 — solo prototype, без monetization.*

---

## Поточний статус (solo · free)

| | |
|---|---|
| **Фаза** | Post-solo playtest → Solo v2 |
| **Модель** | **Free** · no ads · no IAP · no in-game donations |
| **Прототип** | Week 1–10 ✅ · [`game/`](./game/) Unity 6000.4.4f1 |
| **GitHub** | https://github.com/Vlad-PM797/BastionUA |

**Жанр:** Idle-Strategy + auto-battle lite  
**Платформи зараз:** Windows demo zip · Android dev APK  
**Аудиторія:** UA + діаспора · playtesters · indie/community

---

## Що вже працює в build

- Карта UA (4 регіони) + tap/auto resources + auto-battle  
- 4 story events: Hostomel → Chornobaivka → Irpin → Kharkiv  
- Units, upgrades, event log, prestige (L5)  
- Save/load · кнопка **«Вихід»** (save перед закриттям)  
- Demo zip для зовнішніх тестерів  

Dev guide: [`game/README_START_TODAY.md`](./game/README_START_TODAY.md)

---

## Завантажити / зібрати demo

| Menu (Unity) | Output |
|--------------|--------|
| **Build Windows Release** | `game/Builds/WindowsRelease/` |
| **Package Windows Release Demo** | `game/Builds/BastionUA-Windows-demo.zip` |
| **Build Android Dev** | `game/Builds/Android/BastionUA-dev.apk` |

Playtest: [`docs/playtest_guide_ua.md`](./docs/playtest_guide_ua.md) · One-pager: [`docs/demo_one_pager_ua.md`](./docs/demo_one_pager_ua.md)

Verification (Editor **закритий**):

```powershell
& "C:\Program Files\Unity\Hub\Editor\6000.4.4f1\Editor\Unity.exe" -batchmode -nographics -quit -projectPath "E:\BastionUA\game" -executeMethod BastionUA.EditorTools.UnityVerificationRunner.RunAll
```

---

## Solo roadmap (активний план)

1. **Playtest** — 3–5 людей → top-3 fixes → RC-2 ([`post_solo_plan.md`](./docs/post_solo_plan.md))  
2. **Demo package** — відео 60–90 с + one-pager  
3. **Solo v2** — JSON events, +content, SFX, mobile polish, closed beta  

**Не в scope solo зараз:** IAP, ads, Battle Pass, backend, Telegram Mini App, team MVP launch.

---

## Документація

### Прототип і playtest
| Документ | Зміст |
|----------|--------|
| [Post-Solo Plan](./docs/post_solo_plan.md) | Playtest → demo → solo v2 |
| [Playtest Guide](./docs/playtest_guide_ua.md) | Інструкція для тестерів |
| [Playtest Feedback](./docs/playtest_feedback_form.md) | 5 питань |
| [Demo One-Pager](./docs/demo_one_pager_ua.md) | Короткий опис для demo (free, no monetization) |
| [Solo Version Plan](./docs/08_solo_version_plan.md) | Реалістичний solo scope |
| Week 1–10 checkpoints | [`docs/week1_checkpoint.md`](./docs/week1_checkpoint.md) … [`week10`](./docs/week10_checkpoint.md) |

### GDD і дизайн
| # | Документ | Зміст |
|---|----------|--------|
| — | [GDD v0.1](./docs/00_gdd_v0.1.md) | Game design (канон для контенту) |
| 02 | [UI Wireframes](./docs/02_ui_wireframes.md) | Екрани, логіка UI |

---

## Future vision (team-scale · не поточний solo план)

Архівні матеріали для **можливого** масштабування з командою та фінансуванням. **Не описують те, що зараз будується solo.**

| # | Документ | Примітка |
|---|----------|----------|
| 01 | [MVP Roadmap](./docs/01_mvp_roadmap.md) | 6 міс · ~$173K · 7–9 FTE |
| 03 | [Pitch Deck](./docs/03_pitch_deck.md) | Інвесторський pitch |
| 04 | [Unit Economics](./docs/04_unit_economics.md) | ARPU/LTV/CAC |
| 05 | [Legal](./docs/05_legal.md) | Compliance, бригади, stores |

У тій vision згадуються F2P, IAP, ads, donation carve-out, soft-launch 5K DAU — **це опція на майбутнє, не commit solo-розробки.**

---

## Ключові ризики (solo)

1. **Scope creep** — тримати Solo v2 scope; не тягнути team MVP в один dev cycle  
2. **Етичний** — стилізований контекст, без likeness/імен; playtest feedback  
3. **Вигорання** — part-time rhythm, feature freeze після playtest wave  

---

## Контакти

- [Ім'я]: [email] · [Telegram]  
- Website: _TBD_

---

**Слава Україні!**
