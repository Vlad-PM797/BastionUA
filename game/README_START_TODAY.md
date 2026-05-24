# BastionUA — Unity Prototype (Week 9 ✅)

## Статус
- Week 1–9 checkpoints: [`docs/`](../docs/)
- GitHub: https://github.com/Vlad-PM797/BastionUA
- **Solo slice:** prestige loop + SFX stub + event log

## Швидкий старт
Unity Hub → `E:\BastionUA\game` → Boot scene → **Play**

## Що працює
- Core loop + **карта UA (4 регіони, PNG + Крим)** + save/load
- **4 events:** Hostomel → Chornobaivka → Irpin → Kharkiv
- **Prestige** — reset кампанії з permanent bonus (max L5)
- **Event log** — останні дії в sidebar
- **Procedural SFX** — tap, battle, event, upgrade, prestige
- Units / upgrades / battle popup + UA visual theme
- Dev hotkeys: T, **1-4 / QWER**, B, S, **P** (prestige)

## Windows build (share)
| Menu | Output |
|------|--------|
| **BastionUA → Build Windows Release** | `Builds/WindowsRelease/BastionUA.exe` |
| **BastionUA → Build Windows Dev** | `Builds/Windows/BastionUA.exe` |

Zip `BastionUA.exe` + `BastionUA_Data` for sharing.

## Verification (Editor closed)
```
Unity.exe -batchmode -nographics -quit -projectPath game -executeMethod BastionUA.EditorTools.UnityVerificationRunner.RunAll
```

## Post-prototype
Team MVP roadmap: [`docs/01_mvp_roadmap.md`](../docs/01_mvp_roadmap.md)
