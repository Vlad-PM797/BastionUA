# BastionUA — Unity Prototype (Week 10 ✅)

## Статус
- Week 1–10 checkpoints: [`docs/`](../docs/)
- GitHub: https://github.com/Vlad-PM797/BastionUA

## Швидкий старт
Unity Hub → `E:\BastionUA\game` → Boot scene → **Play**

## Що працює
- Core loop + карта UA (4 регіони) + save/load + **prestige** (L5)
- 4 story events, units, upgrades, battle/event popups, event log, SFX stub
- Dev hotkeys: T, 1-4/QWER, B, S, P

## Builds
| Menu | Output |
|------|--------|
| **BastionUA → Build Windows Release** | `Builds/WindowsRelease/` |
| **BastionUA → Package Windows Release Demo** | `Builds/BastionUA-Windows-demo.zip` |
| **BastionUA → Build Android Dev** | `Builds/Android/BastionUA-dev.apk` |
| **BastionUA → Build Windows Dev** | `Builds/Windows/` |

## Next (post-solo)
- Master plan: [`docs/post_solo_plan.md`](../docs/post_solo_plan.md)
- Playtest: [`playtest_guide_ua.md`](../docs/playtest_guide_ua.md)
- MVP gap: [`mvp_gap_analysis.md`](../docs/mvp_gap_analysis.md)

## Art assets (Phase D)
| Menu | Output |
|------|--------|
| **BastionUA → Art → Bake All Art Assets** | map v1/v2 + UI icons |
| **BastionUA → Art → Bake Ukraine Map V2 PNG** | `Resources/Art/ukraine_map_v2.png` |
| **BastionUA → Art → Bake UI Icons PNG** | `Resources/Art/ui/icon_*.png` |

Runtime fallback: `MapArtLoader` tries **v2 → v1 → enhanced rasterizer**; icons use **PNG → procedural**.

Replace baked PNGs with external art (same paths/names) and reimport — no code changes needed.

## Verification (Editor closed)
```
Unity.exe -batchmode -nographics -quit -projectPath game -executeMethod BastionUA.EditorTools.UnityVerificationRunner.RunAll
```
