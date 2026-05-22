# Bastion UA — Unit Economics

## 1. Ключові метрики казуального/idle F2P (benchmarks)

Джерела: Sensor Tower, AppMagic, data.ai, GameAnalytics 2024 reports.

| Метрика | Mobile Casual | Idle/Clicker | Bastion UA ціль |
|---|---|---|---|
| D1 retention | 35–45% | 38–50% | 40% |
| D7 retention | 10–17% | 12–20% | 15% |
| D30 retention | 3–6% | 4–8% | 5% |
| Session length | 4–8 хв | 6–12 хв | 8 хв |
| Sessions / day | 2.5 | 3.5 | 3.5 |
| IAP conversion | 1.5–3% | 2–4% | 2.5% |
| Ad ARPDAU | $0.03–0.08 | $0.04–0.10 | $0.05 |
| IAP ARPDAU | $0.10–0.30 | $0.12–0.40 | $0.15 |
| Subscription attach | 0.5–1.5% | 0.8–2% | 1% |
| Blended ARPDAU | $0.13–0.38 | $0.16–0.50 | **$0.20–0.30** |
| ARPPU (payer) | $15–40 | $18–50 | $25 |
| Whale share (top 10% payers) | 70–80% | 75–85% | 80% |

**Примітка**: Bastion UA — тематично-заряджений продукт. Whale-поведінка може бути підвищена через емоційну прив'язку ("я допомагаю армії"). Historical benchmarks для "пам'яткових" ігор (This War of Mine, 11 bit) → ARPPU до $40+.

---

## 2. Модель ARPU by geo

| Region | % MAU (очікування) | ARPU | Вага |
|---|---|---|---|
| Україна | 45% | $0.35 | $0.158 |
| Польща | 12% | $0.90 | $0.108 |
| Німеччина | 8% | $2.20 | $0.176 |
| США/Канада | 15% | $2.80 | $0.420 |
| UK | 6% | $2.10 | $0.126 |
| Балтика (EE, LV, LT) | 4% | $1.40 | $0.056 |
| Інші EU | 7% | $1.50 | $0.105 |
| Rest of World | 3% | $0.60 | $0.018 |
| **Blended ARPU (щомісяця на MAU)** | | | **≈ $1.17** |

**Annual ARPU (простий)** ≈ $14 / year per MAU — занадто оптимістично для casual.
**Robust ARPU**: для monthly cohorts churn = 30% → **ARPMAU ≈ $0.80–1.20/міс** у стабільній фазі.

---

## 3. Юніт-економіка (payer funnel)

### Cohort 10 000 installs

```
10,000 installs
   │
   ├─ D1 активні:       4,000  (40%)
   │    │
   │    ├─ D7 активні:    1,500  (15%)
   │    │     │
   │    │     ├─ D30:        500  (5%)
   │    │     │    │
   │    │     │    └─ Платоспром.: 250  (50% від D30)
   │    │     │           │
   │    │     │           ├─ First payer (D30): 100
   │    │     │           ├─ Repeat: 80
   │    │     │           └─ Whale: 10 (LTV $150+)
```

### LTV розрахунок (simple)

`LTV = ARPDAU × avg lifetime`

- Blended ARPDAU: **$0.25** (консервативна ціль)
- Avg lifetime (days): 45 (mix casual UA + EU)
- **LTV = $0.25 × 45 = $11.25**

Для IAP whales (10% payers, 80% revenue):
- ARPPU_whale ≈ $150
- Fraction of MAU: 0.25%
- Contribution: $150 × 0.0025 = $0.375/MAU

**Robust LTV (cohort blend)** ≈ **$10–14 per install**

---

## 4. CAC цілі

### Target CAC (installed user)

| Канал | CPI | Quality | Scale |
|---|---|---|---|
| TG Mini App (virality) | $0.30–0.80 | High | 100K/mo |
| YouTube UA-influencers | $1.20–2.50 | Very high | 50K/mo |
| Meta Ads (Instagram UA) | $0.80–1.80 | Medium | 300K/mo |
| TikTok UA | $0.60–1.40 | Medium | 200K/mo |
| Google UAC (global EN) | $1.80–4.00 | Low-Med | 500K/mo |
| Apple Search Ads (EU) | $2.00–3.50 | High | 100K/mo |
| Twitter/X (global) | $2.50–5.00 | High intent | 50K/mo |
| Organic (PR, word-of-mouth) | $0 | Highest | ~20–40% |

**Blended CAC target (рік 1)**: **$1.50**
**Blended CAC target (рік 2, оптимізація)**: **$1.00**

### LTV:CAC ratio

`LTV:CAC = 11 / 1.5 ≈ 7.3×`
**Здорово для casual F2P** (нормa: ≥ 3×).

---

## 5. P&L проекція (3 роки)

### Припущення:
- Soft-launch: Q2 2026 (M6 після старту dev)
- Growth curve: консервативна
- Donation carve-out: 20% від **чистого прибутку (не revenue)** → фонди ЗСУ

### Year 1 (2026, 6 місяців soft-launch → end of year)

| Місяць | MAU | Revenue | User Acq spend | Gross | OpEx | Net |
|---|---|---|---|---|---|---|
| M7 | 15K | $12K | $20K | -$8K | $22K | -$30K |
| M8 | 35K | $30K | $40K | -$10K | $22K | -$32K |
| M9 | 60K | $60K | $50K | $10K | $22K | -$12K |
| M10 | 90K | $100K | $60K | $40K | $24K | $16K |
| M11 | 130K | $150K | $70K | $80K | $24K | $56K |
| M12 | 180K | $215K | $80K | $135K | $26K | $109K |
| **Y1 sum** | | **$567K** | **$320K** | **$247K** | **$140K** | **$107K** |

### Year 2 (2027)

| Квартал | MAU (avg) | Revenue | UA spend | Gross | OpEx | Net |
|---|---|---|---|---|---|---|
| Q1 | 220K | $780K | $240K | $540K | $120K | $420K |
| Q2 | 280K | $1.05M | $300K | $750K | $140K | $610K |
| Q3 | 320K | $1.25M | $320K | $930K | $160K | $770K |
| Q4 | 380K | $1.55M | $360K | $1.19M | $180K | $1.01M |
| **Y2 sum** | | **$4.63M** | **$1.22M** | **$3.41M** | **$600K** | **$2.81M** |

### Year 3 (2028, зрілість + web + steam)

| Квартал | MAU (avg) | Revenue | UA | Gross | OpEx | Net |
|---|---|---|---|---|---|---|
| Q1–Q4 (sum) | 500K avg | $7.5M | $1.8M | $5.7M | $1.2M | $4.5M |

---

## 6. Donation carve-out (партнерські виплати ЗСУ)

**Політика**: 20% від **net profit** → партнерським фондам. (Не від revenue, щоб не зламати юніт-економіку).

| Рік | Net profit | → Фонди ЗСУ |
|---|---|---|
| Y1 | $107K | $21K |
| Y2 | $2.81M | $562K |
| Y3 | $4.5M | $900K |
| **3Y total** | | **~$1.48M** |

**Розподіл по фондах**:
- 30% Фонд Сергія Притули (загальні потреби)
- 25% KOLO (звʼязок, РЕБ, дрони-розвідники)
- 20% Повернись Живим
- 15% UNITED24 (тематичні кампанії)
- 10% прямі збори конкретних бригад-партнерів

**Додатковий revenue stream для фондів**:
- Прямі донати через in-app link (не проходять через нас) — **не враховано вище**
- За benchmark від Notcoin / подібних TG mini apps → можна очікувати до $100K/міс прямих донатів на зрілій стадії
- Загальний прогноз (Y3): $1–3M прямих донатів + $900K carve-out = **~$2–4M загалом** на підтримку ЗСУ

---

## 7. Cost structure (щорічно, у стабільній фазі)

| Категорія | %revenue | Note |
|---|---|---|
| Platform fee (Apple/Google) | 30% | Hard cost |
| Payment processing | 2% | Stripe/Adyen |
| Ad network rev-share (rewarded) | включено в revenue net |
| User acquisition | 25–30% | scale-dependent |
| Staff (team 12–15 people) | 18% | UA salaries |
| Server/infra | 3% | Hetzner + CDN |
| 3rd party services | 2% | Sentry, analytics, loc |
| Marketing/PR/community | 5% | |
| Legal/compliance | 1% | |
| Licensing (brigades, music) | 2% | LOIs + royalties |
| **Operating margin** | | **~17%** |
| Donation carve-out | 20% of margin | ~3.4% of revenue |
| **Final net margin** | | **~13.5%** |

Для порівняння: Playgendary margin = 25-30%, SayGames = 20-25%. Наш рівень нижчий через donation carve-out — це прописаний soft-cost у моделі, не penalty.

---

## 8. Sensitivity / сценарний аналіз

### Base (як вище)
- Y3 ARR: $7.5M
- Y3 Net: $4.5M
- Donations Y3: $0.9M

### Conservative (-30% to base)
- Причини: нижчий ретеншн, нижча конверсія, слабкий виход у DE/US
- Y3 ARR: $5M
- Y3 Net: $2.5M
- Donations Y3: $0.5M

### Optimistic (+50% to base)
- Причини: viral TG Mini App, Steam-реліз успіх, підтримка МКСК
- Y3 ARR: $11M
- Y3 Net: $7M
- Donations Y3: $1.4M

### Pessimistic (-60%)
- Причини: app store-reject, PR-скандал, війна закінчується швидко
- Y3 ARR: $3M
- Net: $0.5M (на межі беззбитковості)
- Decision: pivot у турні-меморіал або Steam-тільки

---

## 9. Key risks для економіки

1. **Geo-mix risk** — якщо основну масу MAU дадуть лише UA користувачі (низький ARPU), LTV падає до $5–7, CAC стає критичним
2. **Donation signaling** — якщо буде сприйнято як "грошезбирання під брендом ЗСУ без реальної користі" → crash в ASO рейтингах
3. **Apple/Google rev-share** — 30% залишається, не впливає на маржу але забирає
4. **Localization quality** — погана локалізація → низький retention в EU = зниження ARPU
5. **Scope creep** → роздуття OpEx → знижує маржу → менше carve-out

---

## 10. Показники для інвесторів / грантів

### Key milestone metrics

| Стадія | Ключовий показник | Ціль | Deadline |
|---|---|---|---|
| MVP | DAU | 5K | M6 |
| Soft-launch | D7 retention | 15% | M7 |
| Product-market fit | ARPDAU | $0.20+ | M9 |
| Scale-up | CAC payback | < 90 днів | M12 |
| Break-even | Monthly operating profit | > 0 | M10–11 |
| Scale-growth | MoM growth | > 15% for 6 mo | M12–18 |
| Series A readiness | $3M+ ARR run-rate | | M18 |

### Грантові KPI (для UCF, Creative Europe)

- Охоплення UA/EU аудиторії: 500K унікальних користувачів за рік
- Освітній impact: X івентів прочитано N разів
- Меморіальний impact: X героїв представлено, звʼязок з N родинами
- Донації ЗСУ: сума + звіти
- Культурний impact: локалізація X мов, міжнародна публікація

---

## 11. Summary table (для 1-page TL;DR)

| Метрика | Y1 | Y2 | Y3 |
|---|---|---|---|
| MAU (avg) | 85K | 300K | 500K |
| DAU (avg) | 25K | 90K | 150K |
| ARPMAU / mo | $0.60 | $1.20 | $1.50 |
| Revenue | $567K | $4.63M | $7.5M |
| UA spend | $320K | $1.22M | $1.80M |
| OpEx | $140K | $600K | $1.20M |
| Net profit | $107K | $2.81M | $4.5M |
| Donations → ЗСУ | $21K | $562K | $900K |
| LTV:CAC | 4.5× | 7.3× | 8.5× |
| CAC payback | 150 днів | 90 днів | 70 днів |
