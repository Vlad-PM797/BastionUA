# Week 8 Checkpoint — Demo Freeze

Статус: **DONE** (2026-05-22)

## Deliverables

### 1. Popup UI polish
- `PopupUiFactory` — shared styled panels (gold border, blue accent stripe)
- **Event popup** — accent title, styled choice buttons
- **Battle result popup** — gold victory / red defeat title, primary Continue button

### 2. Release Windows build
- **BastionUA → Build Windows Release** → `Builds/WindowsRelease/BastionUA.exe`
- Dev build unchanged: **BastionUA → Build Windows Dev** → `Builds/Windows/`

### 3. Verification
- Extended `UnityVerificationRunner` — palette wiring + map PNG asset check
- Batch: `-executeMethod BastionUA.EditorTools.UnityVerificationRunner.RunAll`

## Manual QA
1. Play **Boot** scene → trigger event → check popup styling
2. Run **Battle** → check result popup (victory/defeat colors)
3. **BastionUA → Build Windows Release** → run exe, clean start + save/load

## Demo package checklist
- [ ] Release build zipped (`BastionUA.exe` + `BastionUA_Data`)
- [ ] 60–90 sec demo video (optional, on you)
- [ ] Share link / one-pager for pitch

## Next (post-solo prototype)
- Team-scale MVP per [`docs/01_mvp_roadmap.md`](01_mvp_roadmap.md)
- Mobile / Telegram spike
- Illustrator map replace (optional)
- Sound / FX pass
