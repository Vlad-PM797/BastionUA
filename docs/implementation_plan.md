# Bastion UA — Week 2 (CLOSED)

Статус: **DONE** (2026-05-21)  
Checkpoint: [`docs/week2_checkpoint.md`](week2_checkpoint.md)  
Previous: [`docs/week1_checkpoint.md`](week1_checkpoint.md)

## Week 2 Deliverables
- [x] Git repository + initial commit
- [x] Event trigger system (`GameEventRegistry`, `EventTriggerService`)
- [x] Event #2: Чорнобаївка (after Hostomel + 1 battle)
- [x] `GameState.TotalBattles`
- [x] Interactive map HUD (`RegionMapView`, `MapUiConstants`)
- [x] Legend panel + node-based region selection
- [x] Manual QA confirmed (map, events)
- [x] Week 2 checkpoint doc

## Event pipeline
| Event | Trigger | Prerequisites |
|-------|---------|---------------|
| Гостомель | OnSessionStart | — |
| Чорнобаївка | OnProgress | Hostomel + ≥1 battle |

## Week 3 Entry
See [`docs/week2_checkpoint.md`](week2_checkpoint.md) §6.
