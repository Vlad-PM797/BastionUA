# Week 6 Checkpoint — Kharkiv Event & Map Polish

Статус: **DONE** (2026-05-21)

## Deliverables

### 1. Map silhouette v2
- `UkraineMapGeometry` — 48+ точок кордону з lat/lon
- **Крим включено** (AR Crimea)
- `MapSilhouetteFactory` — sprite 2× + anti-aliased outline
- Міста позиціоновані по GPS

### 2. Event #4 — Харків
| Поле | Значення |
|------|----------|
| Trigger | `OnProgress` після Irpin + **3 battles** |
| Регіон | Kharkiv |
| Вибір 1 | Контратака (−30 ammo, +12 morale, status↑) |
| Вибір 2 | Укріпити оборону (−15 ammo, +6 morale, status↑) |

### 3. Objective chain
Hostomel → Chornobaivka (1 battle) → Irpin (2 battles) → **Kharkiv (3 battles)** → liberate Kharkiv / progression

## Manual QA
1. Reset Save → пройти 4 події по черзі
2. Після Irpin objective: «ще 1 бій для Харків»
3. Після події Харків — статус регіону покращується

## Next
- Release Windows build
- Crimea region marker (optional)
- Mobile / Telegram spike
