# Task: Bastion UA — Week 2 (in progress)

Статус: **IN PROGRESS**  
Старт: 2026-05-21

## Week 2 Goals
- [x] Git repository initialized
- [x] Event trigger system (schedule + prerequisites)
- [x] Event #2: Чорнобаївка
- [ ] HUD polish / map visualization
- [ ] Week 2 checkpoint

## Event pipeline
| Event | Trigger | Prerequisites |
|-------|---------|---------------|
| Гостомель | OnSessionStart | — |
| Чорнобаївка | OnProgress | Hostomel done + ≥1 battle |

## Architecture additions
- `GameEventRegistry` — schedule + definitions
- `EventTriggerService` — prerequisite checks
- `GameState.TotalBattles` — battle counter for triggers
- `ChornobaivkaEventCatalog`

## Verification
- `UnityVerificationRunner` — Hostomel + Chornobaivka flows
