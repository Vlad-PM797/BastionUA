# Week 7 Checkpoint — Art-lite: Palette & UI Reskin

Статус: **DONE** (2026-05-21)

## Deliverables

### 1. Visual palette (`GameVisualPalette`)
- Темний military UI (#0B1118)
- Акценти прапора: **#005BBB** + **#FFD700**
- Статуси Safe / Danger / Occupied
- Кольори карти, popup-ів, кнопок

### 2. PNG карта
- `Assets/Resources/Art/ukraine_map.png` — baked sprite (Крим включено)
- Runtime: `Resources.Load` → fallback `UkraineMapRasterizer`
- Editor menu: **BastionUA → Art → Bake Ukraine Map PNG**

### 3. UI reskin
- Canvas background, top bar + blue stripe
- Side panel + yellow accent
- Battle button — primary (blue + gold border)
- Map frame, pin-style markers (ring + core)
- Gradient map fill + terrain noise

## Manual QA
1. Stop → Play на Boot
2. Перевір: темний фон, жовто-сині акценти, карта з градієнтом
3. **BastionUA → Art → Bake Ukraine Map PNG** — оновити asset після змін геометрії

## Next (Week 8)
- Release Windows build
- Popup/event UI polish
- Optional: illustrator PNG map replace
