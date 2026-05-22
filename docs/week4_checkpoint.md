# Week 4 Checkpoint — Progression & Battle Feedback

Статус: **DONE** (2026-05-21)

## Deliverables

### 1. Battle result popup
- Після **Battle** з’являється модальне вікно: Victory / Defeat
- Показує: регіон, ammo spent, HP, зміну статусу регіону
- **Continue** закриває popup і лише тоді чергує наступну подію

### 2. Units (4)
| ID | Label | Ефект |
|----|-------|-------|
| `territorial_defense` | TDF | +3 morale on victory (default) |
| `artillery` | ART | +8 damage |
| `drones` | DRN | +4 damage, −10 ammo budget |
| `medics` | MED | +6 morale on victory, −2 enemy damage |

Вибір у лівій панелі HUD; активний підрозділ підсвічується.

### 3. Upgrades (3)
| ID | Назва | Max | Ефект / рівень |
|----|-------|-----|----------------|
| `fire_training` | Fire Training | 3 | +2 damage |
| `ammo_logistics` | Ammo Logistics | 3 | −3 ammo budget |
| `morale_radio` | Morale Radio | 3 | +2 morale on victory |

Купівля за **Ammo** кнопкою **+** біля назви. Вартість зростає з рівнем.

### 4. Objective hint
Після проходження всіх подій: *«Обери підрозділ і купи апгрейд для сильніших боїв.»*

## Нові файли
- `Core/BattleModifiers.cs`, `UnitCatalog.cs`, `UpgradeCatalog.cs`
- `Services/ProgressionService.cs`
- `UI/BattleResultPopupController.cs`

## Manual QA
1. Play → обери регіон → **Battle** → перевір popup
2. Перемкни **TDF / ART / DRN / MED** → проведи бій, порівняй результат
3. Накопич ammo → купи апгрейд **+** → перевір списання ammo
4. **Reset Save** → unit/upgrades скидаються

## Known issues
- Ліва панель щільна на малих роздільностях
- Input Manager deprecation (Unity 6) — без змін

## Next (Week 5)
- Map v2 (силует UA / 4-й регіон)
- Баланс бою під progression
- GitHub remote + share `.exe`
