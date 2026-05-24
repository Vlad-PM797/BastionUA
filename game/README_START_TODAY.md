# BastionUA — Unity Prototype (Week 8 ✅)

## Статус
- Week 1–8 checkpoints: [`docs/`](../docs/)
- GitHub: https://github.com/Vlad-PM797/BastionUA
- **Solo 8-week slice:** complete (demo freeze)

## Швидкий старт
Unity Hub → `E:\BastionUA\game` → Boot scene → **Play**

## Що працює
- Core loop + **карта UA (4 регіони, PNG + Крим)** + save/load
- **4 events:** Hostomel → Chornobaivka → Irpin → Kharkiv
- **Units / upgrades / battle result popup** (Week 8 styled)
- **UA visual theme** — palette, HUD reskin, popup polish
- **Battle balance** — регіон, morale, progression
- Dev hotkeys: T, **1-4 / QWER**, B, S

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
