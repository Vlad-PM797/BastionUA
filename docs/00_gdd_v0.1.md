# Bastion UA — GDD v0.1

Статус: draft  
Версія: 0.1  
Дата: 2026-04-24  
Призначення: канонічний документ для MVP/pre-production, зведений з `README.md`, `docs/01-07`, `docs/implementation_plan.md` та `game/README_START_TODAY.md`.

---

## 1. Product Summary

**Bastion UA** — мобільна idle-strategy гра з історичною кампанією, натхненною подіями оборони України 2022 року, яка поєднує:
- короткі сесії 5-10 хвилин,
- офлайн-прогрес,
- карту України з фронтовими секторами,
- історично натхненні події,
- стилізовані або ліцензовані підрозділи,
- прозору соціальну місію: 20% чистого прибутку спрямовується партнерським фондам підтримки ЗСУ.

Гравець проходить історично натхненну кампанію через оборону регіонів, розвиток підрозділів, відкриття технологій, прийняття рішень у подіях і поступове перезавантаження прогресу через prestige-механіку.

---

## 2. Vision

### 2.1 Product Vision

Створити доступну масовій аудиторії гру, яка:
- дає емоційно сильний, але не травматичний досвід взаємодії з історичною кампанією оборони України;
- працює як entertainment-first продукт, а не як благодійний віджет;
- зберігає історичну пам'ять через інтерактивний формат;
- перетворює довгострокову підтримку України на звичну частину дозвілля.

### 2.2 Design Pillars

1. **Грай одразу**  
   Перші дії, перший бій і перше відчуття прогресу мають відбутися протягом 90 секунд.

2. **Повага до теми**  
   Без gore, без приниження реальних жертв, без карикатурного ставлення до війни.

3. **Зрозумілий прогрес**  
   Усі системи мають підсилювати основний цикл: ресурси → підготовка → бій → нагорода → розвиток.

4. **Мобільна дисципліна**  
   Короткі сесії, чистий UI, мінімум фрикції, сильний офлайн-цикл.

5. **Прозора місія**  
   Соціальна складова є частиною бренду, але не втручається у platform billing/compliance і не впливає на power progression.

---

## 3. Target Audience

### 3.1 Primary Audience

- Українці 18-45, які активно користуються смартфоном.
- Люди, яким близька тема оборони України, але які не обов'язково грають у hardcore strategy.
- Гравці idle/casual/F2P жанрів, яким важливий відчутний прогрес за короткий час.

### 3.2 Secondary Audience

- Українська діаспора в ЄС, UK, США, Канаді.
- Західні прихильники України, які хочуть підтримувати тему через культурний продукт.
- Telegram Mini App аудиторія, чутлива до вірусного поширення.

### 3.3 Audience Constraints

- Частина аудиторії може відкидати проєкт як "монетизацію війни".
- Частина аудиторії не знає контексту подій 2022 року.
- Тому продукт має бути емоційно точним, але достатньо зрозумілим без спеціальної підготовки і без враження live-war exploitation.

---

## 4. Platforms and Release Strategy

### 4.1 MVP Platforms

- Android
- Telegram Mini App

### 4.2 Post-MVP Platforms

- iOS
- Web
- Steam

### 4.3 MVP Release Goal

MVP має завершитися закритою бетою/soft-launch версією з однією повною історичною кампанією:
- театр: оборона Київщини;
- регіони MVP: Київ, Чернігів, Суми;
- одна завершувана прогресійна дуга;
- базова аналітика;
- стабільний save/load;
- перевірений core loop.

---

## 5. Core Fantasy

Гравець переживає відчуття:
- "Я організовую оборону";
- "Мої рішення змінюють карту";
- "Я розвиваю сили та технології";
- "Я проходжу крізь історично натхненні події";
- "Моя участь у грі має символічний і практичний вплив".

---

## 6. Core Loop

### 6.1 Primary Loop

1. Гравець заходить у гру та збирає ресурси.
2. Інвестує ресурси в бригади, апгрейди або підготовку до бою.
3. Обирає регіон/сектор на карті.
4. Запускає auto-battle або реагує на історичну подію.
5. Отримує результат: зміна статусу регіону, нові ресурси, досвід, morale, unlocks.
6. Повертається на карту, де бачить наслідки своїх дій.
7. Поступово відкриває нові системи та наближається до prestige.

### 6.2 Meta Loop

1. Завершення ключових секторів кампанії.
2. Отримання permanent currency / prestige rewards.
3. Перезапуск прогресу з постійними бонусами.
4. Швидший та глибший наступний прохід.

### 6.3 Session Loop

- Збір офлайн-ресурсів.
- 1-3 короткі дії на карті.
- 1 бій або 1 подія.
- 1 апгрейд або unlock.
- Вихід із чітким next objective.

---

## 7. MVP Scope

### 7.1 In Scope for MVP

- Одна кампанія: оборона Київщини.
- 3 регіони: Kyiv, Chernihiv, Sumy.
- Базова карта зі статусами регіонів.
- Один playable battle flow.
- Базовий ресурсний цикл: tap + auto generation.
- 1-2 додаткові ресурси поверх базового.
- 5-10 стилізованих підрозділів у спрощеному вигляді.
- Початкове tech tree.
- 1 prestige loop.
- 5-10 історично натхненних подій.
- Save/load.
- Базовий HUD та екранна навігація.
- Basic analytics/crash reporting.
- Донат-екран із зовнішніми лінками.

### 7.2 Explicitly Out of Scope for MVP

- Повний масштаб усієї карти України.
- Клани, PvP, live guild systems.
- Повноцінний chat або UGC.
- Великий live-ops календар.
- Велика кількість сезонів, battle pass depth, cosmetics at scale.
- Повна дипломатична система в широкому вигляді.
- Реальні назви, шеврони, емблеми й інша бригадна айдентика без письмових прав.
- Реальні публічні особи як персонажі, гіди, голоси або key art без окремих дозволів.
- Меморіальна зала з реальними іменами, фото, портретами або біографіями.
- Будь-які нагороди за зовнішній донат.
- Клани, чат, user-generated content і social systems без повного moderation stack.
- Розгалужений backend beyond MVP needs.

### 7.3 Scope Rule

Будь-яка фіча, яка не підсилює core loop "ресурси → підготовка → бій → нагорода → розвиток", не входить у MVP без окремого обґрунтування.

---

## 8. Game Pillars by System

### 8.1 Map Layer

Карта України є головним hub-екраном історичної кампанії.

Функції:
- показує поточний стан регіонів;
- дає доступ до активних секторів;
- відображає прогрес кампанії;
- є головною візуальною винагородою за гру.

Статуси регіонів:
- `Safe`
- `Danger`
- `Occupied`

У MVP карта має бути читабельною, легкою для навігації та достатньо стилізованою для мобільного формату.

### 8.2 Battle Layer

Бій у MVP є **спрощеним auto-battle** сценарієм.

Функції:
- витрачає ресурси;
- дає швидкий outcome;
- змінює статус сектора/регіону;
- створює відчуття військової операції без надмірної складності.

Базові параметри:
- HP
- damage
- speed/cooldown
- line/slot positioning
- battle modifiers by terrain/event/upgrades

### 8.3 Resource Layer

Базові ресурси MVP:
- `Ammo` або еквівалентна основна валюта;
- `Ideas` / tech currency;
- `Morale` як модифікатор прогресу/бойової стійкості.

Принцип:
- одна головна ресурсна петля має бути зрозуміла з першої сесії;
- додаткові ресурси відкриваються поступово.

### 8.4 Upgrade Layer

Апгрейди мають:
- підсилювати tap/auto generation;
- покращувати результати бою;
- відкривати нові тактичні можливості;
- бути достатньо простими, щоб гравець відчував контроль над прогресом.

### 8.5 Event Layer

Історично натхненні події є не просто flavor-text, а механічно значущими рішеннями.

Кожна подія має:
- дату або історичний контекст;
- короткий опис;
- 2-3 варіанти рішення;
- конкретні наслідки для ресурсів, morale, unlocks або карти.

### 8.6 Prestige Layer

Prestige у світі гри оформлений як "Ставка" або інший тематичний meta-reset без необхідності використання реальних посадових осіб як персонажів.

Його роль:
- давати довгострокову мету;
- виправдовувати повторні проходження;
- зберігати відчуття зростання навіть після завершення локальної кампанії.

---

## 9. Player Progression

### 9.1 Early Game

Цілі:
- навчити збирати ресурси;
- показати карту;
- дати перший бій;
- дати першу перемогу;
- відкрити перший апгрейд.

Час до first fun:
- 30-90 секунд.

### 9.2 Mid Game

Цілі:
- відкрити 3 регіони;
- дати перші історично натхненні події;
- дати вибір між кількома бригадами/апгрейдами;
- показати, що карта реагує на прогрес.

### 9.3 Late MVP Game

Цілі:
- завершити кампанію Київщини;
- відкрити prestige;
- закласти підставу для повторного проходження;
- підготувати гравця до майбутніх кампаній.

---

## 10. Narrative and Tone

### 10.1 Narrative Frame

Гра натхненна реальними подіями, але не повинна бути документальним симулятором 1:1 і не повинна позиціонуватися як експлуатація поточного конфлікту.

Підхід:
- історична основа;
- повага до фактів;
- стилізація там, де повна документальність шкодить геймдизайну або legal safety;
- відмова від елементів, які вимагають rights clearance, якщо вони не критичні для MVP.

### 10.2 Tone Guidelines

Тон гри:
- серйозний, але не похмурий;
- героїчний, але не пафосний до втрати довіри;
- емоційний, але не експлуатаційний;
- патріотичний, але зрозумілий міжнародній аудиторії.

### 10.3 Content Restrictions

Не використовувати:
- gore;
- пряме приниження жертв;
- "вбити конкретну реальну особу" як ігрову мету;
- реальні голоси, портрети або likeness без дозволу;
- сумнівні сатиричні механіки, які можуть зламати App Store compliance.

---

## 11. Art and UX Direction

### 11.1 Visual Direction

Візуальний стиль MVP:
- clean stylized military UI;
- читабельна карта;
- strong national color coding;
- достатньо символічності без перевантаження дрібними деталями.

Базова палітра:
- синій `#005BBB`
- жовтий `#FFD500`
- зелений для safe-state
- червоний для active danger/battle
- темний нейтральний фон

### 11.2 UI Principles

- portrait-first mobile layout;
- карта як головний центр уваги;
- великі зрозумілі CTA;
- видимий прогрес без зайвих overlay;
- один екран = одна домінантна задача.

### 11.3 Accessibility

MVP має враховувати:
- достатній контраст;
- базове масштабування тексту;
- зрозумілі кольорові й іконкові стани;
- мінімізацію перевантаження анімацією.

---

## 12. Economy and Monetization

### 12.1 Business Model

Модель:
- F2P
- IAP
- rewarded ads
- battle pass or subscription later in MVP+/post-MVP

### 12.2 Monetization Rule for MVP

MVP не повинен залежати від повного live monetization stack, щоб довести core fun.

Порядок пріоритету:
1. Спочатку core retention.
2. Потім проста монетизація.
3. Потім розширені monetization layers.

### 12.3 Donation Rule

Реальні донати на фонди:
- не проходять через in-app purchase;
- йдуть через зовнішні посилання/deep-links;
- мають бути прозоро пояснені;
- не повинні обіцяти геймплейну перевагу, валюту, power boost, unlock або progression reward;
- у MVP не верифікуються всередині гри для видачі нагород.

### 12.4 Revenue Philosophy

Соціальна місія важлива для бренду, але гра має бути життєздатною як продукт навіть без емоційного "one-time support spike" і без policy-sensitive donation gamification.

---

## 13. Technical Design (MVP Level)

### 13.1 Engine and Stack

- Unity 2022 LTS
- URP
- Addressables
- Mobile-first architecture

Backend for MVP:
- optional/lightweight where needed;
- save, telemetry, remote config can start minimal;
- avoid overbuilding backend before prototype validation.

### 13.2 Suggested Game Architecture

На рівні прототипу/раннього MVP структура вже окреслена як service-oriented:
- `GameBootstrap`
- `GameState`
- `RegionState`
- `ResourceService`
- `MapService`
- `BattleService`
- `SaveService`
- `GameConstants`

Це підходить для першого playable vertical slice.

### 13.3 Save System

Save/load є blocker-level системою.

MVP save має зберігати:
- ресурси;
- стан регіонів;
- unlocked upgrades;
- історію ключових подій;
- progression markers;
- prestige state.

### 13.4 Analytics

Обов'язкові події MVP:
- app start
- tutorial start/finish
- region select
- battle start/end
- event choice
- upgrade purchase
- session length
- save/load success/failure

---

## 14. Content Plan for MVP

### 14.1 Regions

MVP регіони:
- Kyiv
- Chernihiv
- Sumy

### 14.2 Brigades

Для MVP бажано працювати у двох шарах:
- gameplay layer: унікальні бойові ролі;
- narrative layer: історичні референси без використання захищеної символіки або персональних прав без дозволу.

Базове правило MVP:
- за замовчуванням використовувати стилізовані/узагальнені підрозділи;
- реальні бренди бригад додавати лише після письмових домовленостей;
- не блокувати vertical slice через branding negotiations.

### 14.3 Events

MVP події мають бути короткими, зрозумілими і production-friendly.

Критерії відбору:
- висока історична впізнаваність;
- придатність до 2-3 choice structure;
- відсутність необхідності складного cutscene pipeline;
- відсутність залежності від прав на конкретну особу, цитату чи зображення.

### 14.4 Tech Tree

Початковий tech tree має містити лише вузли, які реально відчуваються у формулах гри.

MVP guideline:
- менше вузлів;
- сильніші ефекти;
- краща читабельність.

---

## 15. MVP Success Metrics

Цільові продуктові KPI:
- D1 retention: 40%
- D7 retention: 15%
- Session length: 8 хв
- Sessions/day: 3+
- Crash-free sessions: 99%+

Прототипні KPI до soft-launch:
- гравець розуміє core loop без зовнішнього пояснення;
- перша перемога досягається швидко;
- карта читається інтуїтивно;
- прогрес відчувається після кожної сесії;
- save/load стабільний.

### 15.1 Validation Questions

MVP має відповісти на 5 головних питань:
1. Чи цікавий core loop без сильного production polish?
2. Чи працює карта як main fantasy driver?
3. Чи не руйнує історична тема retention?
4. Чи зрозуміла різниця між IAP і donation support?
5. Чи достатньо сильний prestige, щоб стимулювати повернення?

---

## 16. Risks and Mitigations

### 16.1 Product Risks

- Scope creep  
  Мітигація: жорсткий MVP cut, feature freeze.

- Overcomplexity in systems  
  Мітигація: battle/event/tech tree спрощуються до читабельного vertical slice.

- Weak first-session clarity  
  Мітигація: onboarding + visible next objective.

### 16.2 Ethical and PR Risks

- Сприйняття як "гра на крові"  
  Мітигація: consultant review, respectful tone, historical framing, no gore, transparent mission.

- PR attacks / review bombing  
  Мітигація: moderation plan, communications prep, platform reporting readiness.

### 16.3 Legal and Platform Risks

- Ліцензії на символіку/бригади  
  Мітигація: MOU-first approach, fallback to stylized representation.

- App Store / Google policy conflicts  
  Мітигація: no direct donations via IAP, no donation rewards, no targeted real-person violence, age/content compliance.

- Privacy/compliance overhead  
  Мітигація: minimal PII, early legal checklist ownership.

---

## 17. Production Priorities

### 17.1 Vertical Slice First

Першочергова мета команди:
- довести playable loop, а не ширину контенту.

Порядок виробництва:
1. resource loop
2. map state
3. battle result
4. save/load
5. onboarding/basic HUD
6. 1 historical-style event
7. 1 upgrade branch
8. analytics

### 17.2 Content Expansion Later

Після підтвердження vertical slice додаються:
- додаткові події;
- більше бригад;
- depth in tech tree;
- monetization layers;
- richer meta systems.

---

## 18. Immediate Next Steps

1. Затвердити цей документ як `single source of truth` для MVP.
2. Вирізати з MVP backlog усе, що не підсилює vertical slice.
3. Оновити roadmap і sprint docs відповідно до цього GDD.
4. Формалізувати legal fallback: "реальні бригади" vs "стилізовані аналоги".
5. Додати compliance gate перед soft-launch: branding, real persons, donation UX, review notes.
6. Зібрати clickable prototype/UI flow.
7. Визначити owner'ів для product, tech, legal, validation.

---

## 19. Open Decisions

Потребують окремого затвердження:
- чи входять реальні бренди бригад у MVP, чи лише після партнерських домовленостей;
- чи входить diplomacy screen у MVP, чи переноситься у post-MVP;
- чи входить Memorial Hall у MVP, чи лише як заглушка/placeholder;
- чи потрібен backend у першій playable версії, чи локальний save достатній;
- який exact monetization stack перевіряється в soft-launch;
- чи donation screen взагалі входить у перший store submission, чи активується пізніше;
- які елементи потребують письмового rights clearance до релізу.

---

## 20. Canonical Source Mapping

Цей GDD зведений на базі:
- продуктного summary та ризиків з `README.md`
- виробничого scope і KPI з `docs/01_mvp_roadmap.md`
- UX структури з `docs/02_ui_wireframes.md`
- business framing з `docs/03_pitch_deck.md`
- економічних припущень з `docs/04_unit_economics.md`
- compliance рамки з `docs/05_legal.md`
- prototype execution docs з `docs/06_14_day_execution_plan.md`, `docs/07_7_day_sprint.md`
- early implementation scaffold з `docs/implementation_plan.md` і `game/README_START_TODAY.md`

У разі конфлікту між документами для MVP-пріоритетів основним документом вважається саме цей GDD до виходу наступної затвердженої версії.
