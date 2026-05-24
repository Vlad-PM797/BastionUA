# MVP Gap Analysis — Solo Prototype vs Team MVP

Baseline: Unity prototype **Week 10** vs [GDD v0.1](./00_gdd_v0.1.md) + [MVP Roadmap W1–W26](./01_mvp_roadmap.md).

---

## Summary

| Area | Solo prototype | MVP target | Gap size |
|------|----------------|------------|----------|
| Core loop | ✅ Tap + auto + battle | Same + server tick validation | M |
| Map | 4 regions, PNG | 3 fronts + Bezier line + 10+ sectors | L |
| Units | Basic roster | 10 squads + 15 enemy types | L |
| Events | 4 hardcoded | JSON engine + 5+ scripted | M |
| Progression | 3 upgrades | Tech tree 20+ nodes | L |
| Prestige | L5, simple bonuses | Badges + 15 permanent upgrades | M |
| Save | Local JSON | Server + auth + anti-cheat | L |
| UI/UX | Runtime HUD | Branded UI, DOTween, a11y | L |
| Audio | Procedural beeps | Ambient + SFX + VO stub | M |
| Backend | None | FastAPI + PG + Redis | L |
| Monetization | None | IAP + ads + donate links | L |
| Analytics | None | Mixpanel/GA + crash reporting | M |
| Platforms | Win + Android dev | iOS + TG Mini App + store release | L |

**Legend:** S = days · M = weeks · L = months (team)

---

## What prototype proves (keep)

- Core loop is fun enough to demo
- Region status + battle changes map state
- Events create meaningful choices
- Prestige gives second-session hook
- UA visual identity works on map + HUD
- Save/load + verification pipeline exists

---

## MVP W1–W4 priorities (if team starts)

Aligned with roadmap Month 1:

1. **Unity project hygiene** — URP, Addressables, CI build artifacts
2. **Backend skeleton** — anonymous save, device_id, health check
3. **Map v1 team** — 3 regions as designed in GDD (Kyiv / Chernihiv / Sumy fronts)
4. **Battle v2** — 5 slots, auto-battle sim, sector ownership
5. **Data-driven units** — JSON configs, not hardcoded lists

Do **not** start W17–W20 (IAP, ads) until core retention validated.

---

## Solo → team handoff checklist

- [ ] Demo zip + video + [one-pager](./demo_one_pager_ua.md)
- [ ] [Playtest backlog](./playtest_backlog.md) with top-3 resolved
- [ ] GDD v0.1 reviewed; open questions listed in GDD §16
- [ ] Repo access + `UnityVerificationRunner` documented
- [ ] Architecture map: `Bootstrap → Services → UI` (existing pattern)

---

## Open product decisions (before W1)

| Question | Options | Recommendation |
|----------|---------|----------------|
| Unity version | 6000.4 vs 2022 LTS | Team vote; prototype migrates cleanly if services stay pure C# |
| Telegram Mini App | W1 vs post-MVP | Post core loop on mobile web wrapper |
| Real brigade names | MVP vs post-partnership | Stylized units only until MOU |
| Memorial / Hall of Honor | Full vs placeholder | Placeholder text only (legal) |
| Donate links | Menu item W19 | External links only, zero in-game reward |

---

## Estimated effort to MVP beta (team 7–9 FTE)

~26 weeks per roadmap · ~$130–173K · assumes feature freeze W18.

Solo cannot close this gap alone — prototype job is **de-risk core loop** for pitch and hiring.
