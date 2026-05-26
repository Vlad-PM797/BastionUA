# Bastion UA — Solo Visual v1 Design (A → D)

Статус: **APPROVED** (solo path)  
Дата: 2026-05-24  
Модель: free prototype · без monetization · Unity runtime UI

---

## Мета

Підняти **відчуття якості** playable demo без команди артистів: спочатку code-based polish (Phase A), потім 1–2 замінні assets (Phase D).

**Success criteria:**
- Playtester за 30 с каже «виглядає цілісно / по-українськи»
- Скріншот придатний для demo video і GitHub README
- Жодних нових екранів з wireframes (hub-only scope)

---

## Поточна база (Week 7)

- `GameVisualPalette` — military dark + #005BBB / #FFD700
- `MapArtLoader` + `ukraine_map.png` + rasterizer fallback
- `PopupUiFactory` — inset frames, gold border
- `RegionMapView` — pin markers, status rings
- Шрифт: `LegacyRuntime.ttf` (Unity builtin)

**Слабкі місця (з playtest/screenshots):**
- Flat UI, мало depth/hierarchy
- Маркери — квадрати, не «operational map»
- Немає micro-feedback (tap, battle, region select)
- Карта PNG — functional, не «hero art»
- Sidebar перевантажений текстом

---

## Phase A — Unity polish (code-only, ~2 тижні)

### Sprint A1 — Typography & hierarchy (2–3 дні)
| Task | Detail |
|------|--------|
| `UiTextFactory` | ✅ Єдине місце для Text + Outline/Shadow |
| Stat bar | ✅ Ammo / Morale / Prestige — label muted, value gold (rich text) |
| Section titles | ✅ Side panel — gold underline stripe |
| Objective bar | ✅ Marker dot + rich text + truncate |

**Files:** `HudController`, new `UiTextFactory.cs`, `GameUiConstants` font sizes

### Sprint A2 — Buttons & feedback (2–3 дні)
| Task | Detail |
|------|--------|
| `UiButtonFactory` | ✅ Shared ColorBlock hover/pressed/disabled |
| Primary Battle | ✅ Press scale 0.97 via `UiPressScaleBehavior` |
| Map markers | ✅ Selected gold ring pulse via `UiSelectionPulse` |
| Tap button | ✅ Brief gold flash via `UiFlashFeedback` |

**Files:** extract from `HudController` + `PopupUiFactory` where overlap

### Sprint A3 — Map panel depth (2–3 дні)
| Task | Detail |
|------|--------|
| Vignette | ✅ Procedural radial overlay (`MapArtLoader`) |
| Connection lines | ✅ Lower alpha (0.42) |
| Labels | ✅ Drop shadow on region names |
| Frame | ✅ Inner shadow inset (`MapFrameInner`) |

### Sprint A4 — Popup polish (2 дні)
| Task | Detail |
|------|--------|
| `CanvasGroup` fade | ✅ `UiPopupFadeIn` 0.2s on event + battle overlay |
| Victory/defeat | ✅ Green/red outcome stripe + title colors |
| Overlay click | ✅ Overlay blocks raycasts; dismiss only via buttons |

### Sprint A5 — Sidebar breathing room (1–2 дні)
| Task | Detail |
|------|--------|
| Upgrade rows | ✅ Fixed-height rows + gold-framed `+` buttons |
| Event log | ✅ Muted inset panel behind log text |
| Reset footer | ✅ Grouped footer panel, smaller secondary button |

**Phase A DoD:**
- [x] Play mode: visible feedback on tap, battle, region select
- [x] Popups fade in
- [x] Batchmode verification passes
- [x] Screenshot «before/after» у `docs/visual/`

---

## Phase D — Key assets (1–2 assets, ~1 тиждень)

### D1 — Hero map PNG (priority 1)
**Asset:** `Assets/Resources/Art/ukraine_map_v2.png`

| Approach | Pros | Cons |
|----------|------|------|
| **D1a — Improved rasterizer bake** | Solo, reproducible, no external art | Still stylized, not illustrator |
| **D1b — External illustrator PNG** | Best look | Time, cost, license |

**Recommendation:** **D1a first** — enhance `UkraineMapRasterizer` (coast glow, region tint hints, Crimea label zone), rebake via **BastionUA → Art → Bake Ukraine Map PNG**. Fallback path unchanged.

**Loader change:** try `Art/ukraine_map_v2`, fallback `Art/ukraine_map`, fallback rasterizer.

### D2 — UI icon trio (priority 2)
**Assets:** `Resources/Art/ui/icon_ammo.png`, `icon_morale.png`, `icon_battle.png`

- 64×64 PNG, flat military style, palette-aligned
- Source: Kenney Game Icons (CC0) recolored **or** procedural 64px bake in Editor tool
- Hook: stat bar left of Ammo/Morale text

**Phase D DoD:**
- [x] New map visible in Play + Release build
- [x] 3 icons in top bar
- [x] `MapArtLoader` + README note for asset replacement

---

## Out of scope (solo v1 visual)

- Figma full 12-screen wireframes
- 3D units, spine animations
- Custom font (Cyrillic webfont) — defer until Android text QA
- Splash / Auth screens
- DOTween dependency (use CanvasGroup + coroutines)

---

## Implementation order

```
A1 → A2 → A3 → A4 → A5 → D1 → D2
         ↑ playtest screenshot checkpoint after A3
```

---

## Risks

| Risk | Mitigation |
|------|------------|
| UI refactor breaks HUD layout | Small PRs per sprint; verification each sprint |
| Map v2 worse than v1 | Keep v1 path; A/B toggle constant |
| Scope creep to wireframes | Feature freeze: hub screen only |

---

## Next action

Start **Sprint A1** (`UiTextFactory` + stat bar hierarchy).
