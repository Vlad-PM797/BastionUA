# Bastion UA — 14 Day Execution Plan (Solo/Small Team)

## Goal for 14 days
Ship a stable **playable prototype** with:
- resource loop (tap + auto),
- 3 map regions with statuses,
- 1 battle flow,
- save/load,
- basic UI,
- internal test build + feedback loop.

---

## Rules (do not break)
- Max scope: only MVP core.
- Every day ends with a playable build.
- If feature is not critical for core loop, postpone it.
- Track bugs in one backlog file, fix top-3 daily.

---

## Daily cadence (recommended)
- **Deep work 1 (2h):** core implementation
- **Deep work 2 (2h):** integration/polish
- **QA (45m):** manual test script
- **Ops (15m):** backup + changelog

---

## Day-by-day plan

### Day 1 — Foundation
- [ ] Unity 2022 LTS project setup (URP)
- [ ] Import `Assets/Scripts` from `E:\BastionUA\game\Assets\Scripts`
- [ ] Add `GameBootstrap` in Boot scene
- [ ] Validate hotkeys: `T`, `1/2/3`, `B`, `S`
- [ ] Save first playable build

### Day 2 — Minimal UI shell
- [ ] Canvas + HUD (Ammo, Morale, Selected Region)
- [ ] Buttons for region select (Kyiv/Chernihiv/Sumy)
- [ ] Button for battle trigger
- [ ] Replace keyboard-only flow with UI actions
- [ ] Build + smoke test

### Day 3 — Map panel and status visuals
- [ ] Region cards with color by status (`Safe/Danger/Occupied`)
- [ ] Selected region highlight
- [ ] Region detail panel (name, status, last battle result)
- [ ] Manual status update test
- [ ] Build + checkpoint video (30-60 sec)

### Day 4 — Battle UX pass
- [ ] Battle start panel with confirmation
- [ ] Battle result popup (victory/defeat, ammo spent, morale delta)
- [ ] Update region status from battle result
- [ ] Add battle logs in UI (last 3 events)
- [ ] Build + test script run

### Day 5 — Resource balancing v1
- [ ] Tune tap/auto gain constants
- [ ] Add ammo spend preview before battle
- [ ] Add morale bounds and warnings
- [ ] Run 30-min balancing session
- [ ] Save balancing notes

### Day 6 — Save/load robustness
- [ ] Validate save after each major action
- [ ] Add “Load successful / fallback default” system messages
- [ ] Corrupted save handling test (manual invalid JSON)
- [ ] Add “Reset save” dev button
- [ ] Build + cold restart verification

### Day 7 — Event system v0
- [ ] Add simple event model (`id`, `title`, `text`, `choice A/B`)
- [ ] Trigger first event: “Hostomel”
- [ ] Apply choice effects to ammo/morale/region
- [ ] Display event history
- [ ] Weekly checkpoint build

### Day 8 — Progression skeleton
- [ ] Add 3 upgrade nodes (ammo efficiency, battle damage, morale resistance)
- [ ] Add upgrade cost and unlock conditions
- [ ] Connect upgrades to battle/resource formulas
- [ ] Persist upgrades in save
- [ ] Build + quick regression test

### Day 9 — Session loop polish
- [ ] First-time player flow (quick onboarding panel)
- [ ] Add “Next objective” hint
- [ ] Improve feedback timing (toasts, labels, transitions)
- [ ] Remove confusing debug outputs from UI
- [ ] Build + 20-min self-play

### Day 10 — Metrics instrumentation
- [ ] Add local event logging: app_start, battle_start, battle_end, save
- [ ] Track session length and number of battles
- [ ] Save metrics JSON locally
- [ ] Add debug metrics panel
- [ ] Build + verify metrics file updates

### Day 11 — QA bug bash
- [ ] Run full manual checklist (restart, save, battle, event, upgrade)
- [ ] Fix top 10 bugs by severity
- [ ] Re-test fixed issues
- [ ] Record known non-blockers
- [ ] Build candidate RC-1

### Day 12 — External mini-playtest prep
- [ ] Create short tester instructions (1 page)
- [ ] Prepare Android/Windows build for 3-5 testers
- [ ] Define feedback form (5 questions max)
- [ ] Collect first feedback wave
- [ ] Prioritize issues from feedback

### Day 13 — Feedback implementation
- [ ] Fix biggest UX blockers from testers
- [ ] Improve clarity of objective/progression
- [ ] Rebalance battle if needed
- [ ] Validate save compatibility after changes
- [ ] Build candidate RC-2

### Day 14 — Prototype freeze
- [ ] Final regression pass
- [ ] Export demo video (60-90 sec)
- [ ] Update docs: current scope + known gaps + next sprint
- [ ] Tag prototype version
- [ ] Backup project and assets

---

## Definition of done (Day 14)
- [ ] App starts without errors
- [ ] Core loop playable end-to-end
- [ ] Save/load stable across restarts
- [ ] 1 event + 3 upgrades integrated
- [ ] At least one external playtest completed
- [ ] Demo video captured

---

## Risk control
- If a task spills >1 day: cut non-critical UI polish first.
- If battle tuning blocks progress: freeze formulas and move on.
- If performance issue appears: reduce update frequency and UI redraws.

---

## Next sprint after Day 14
- Add 2 more regions and 5-10 events
- Add prestige prototype
- Add analytics backend (optional)
- Prepare closed beta checklist
