# BastionUA — Visual Before / After

Порівняння HUD до і після **Solo Visual v1** (Phase A + Phase D + sidebar layout fix).

| Файл | Commit / стан | Опис |
|------|----------------|------|
| `before_v1_hud.png` | `b49c7d2` (pre–Phase A) | Flat UI, без icons/vignette/sidebar polish |
| `after_v1_hud.png` | `master` (A1–A5, D, sidebar fix) | Typography, feedback, map v2, icons, inset panels |

## Як оновити скріншоти

```powershell
# Before (commit b49c7d2 baseline build у worktree)
powershell -ExecutionPolicy Bypass -File tools/capture_game_window.ps1 `
  -ExePath ".worktrees/pre-visual/game/Builds/WindowsRelease/BastionUA.exe" `
  -OutputPath "docs/visual/before_v1_hud.png"

# After (поточний master release build)
powershell -ExecutionPolicy Bypass -File tools/capture_game_window.ps1 `
  -ExePath "game/Builds/WindowsRelease/BastionUA.exe" `
  -OutputPath "docs/visual/after_v1_hud.png"
```

Build передає `-bastionScreenshotPath`; гра сама зберігає PNG після bootstrap HUD і закривається.

**Editor (ручно):** Play → **BastionUA → Visual → Capture HUD Screenshot (Play Mode)**

Референс: [`solo_visual_v1_design.md`](../solo_visual_v1_design.md)
