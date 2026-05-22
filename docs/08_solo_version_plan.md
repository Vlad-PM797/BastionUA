# Bastion UA — Solo Version Plan

Статус: draft  
Дата: 2026-04-24  
Призначення: реалістичний план для одного розробника з фокусом на vertical slice / prototype-first delivery.

---

## 1. Коротка відповідь

Так, **Bastion UA можна реалізувати одному розробнику**, але не в поточному широкому MVP-обсязі.

Реалістична solo-ціль:
- не full MVP;
- не multi-platform production launch;
- не legal-heavy branded version;
- а **сильний vertical slice**, який доводить:
  - core loop,
  - fantasy,
  - читаємість карти,
  - базову прогресію,
  - потенціал для подальшої збірки команди або фінансування.

---

## 2. Solo Goal

### 2.1 Що ми реально будуємо

За 8 тижнів одна людина має націлюватися на:
- один Unity prototype;
- Windows dev build або Android dev build;
- 3 регіони;
- 1 базовий ресурсний цикл;
- 1 battle flow;
- 3-5 стилізованих підрозділів;
- 3 історично натхненні івенти;
- 1 просту upgrade branch;
- 1 prestige reset;
- save/load;
- базовий HUD + screen flow;
- короткий демо-ролик або playable demo.

### 2.2 Що вважається успіхом

Solo success = це коли:
- прототип запускається без помилок;
- core loop проходиться end-to-end;
- людина з боку розуміє, що тут цікавого, за перші 2-3 хвилини;
- після 15-20 хвилин гри хочеться ще один цикл;
- є що показувати інвестору, партнеру або майбутньому співзасновнику.

### 2.3 Recommended Timeline

Рекомендований горизонт для solo version:
- **8 тижнів** як базовий план;
- **6 тижнів** лише якщо вже є готовий технічний фундамент;
- **10-12 тижнів** якщо розробка йде паралельно з роботою, навчанням або іншими зобов'язаннями.

Чому саме 8 тижнів:
- вистачає часу на core loop;
- є простір для 1-2 хвиль виправлень;
- можна провести мінімальний playtest;
- знижується ризик вигоряння від нереалістичного 30-денного марафону.

---

## 3. Solo Scope

### 3.1 Must Have

- Unity 2022 LTS проект
- 1 сцена hub + 1 battle scene або один combined screen flow
- 3 регіони: Kyiv / Chernihiv / Sumy
- region state machine: `Safe / Danger / Occupied`
- tap + auto resource generation
- battle simulation
- 3-5 стилізованих підрозділів
- 3 апгрейди
- 3 події
- prestige/reset
- local save/load
- базовий onboarding
- базовий UI

### 3.2 Should Have

- event log
- dev/debug panel
- basic balancing config
- simple local analytics log
- один короткий “historical archive” screen

### 3.3 Nice to Have

- Android build
- sound pass
- juice animations
- remote config
- simple mock shop screen without real billing

### 3.4 Hard No for Solo v1

- iOS + Android + Telegram одночасно
- реальні бригади, шеврони, rights negotiations
- реальні публічні особи як гіди/портрети/голоси
- Memorial Hall з персоналіями
- backend-first architecture
- клани, чат, UGC
- Battle Pass / subscription / real IAP integration у першій хвилі
- live ops
- масштабна локалізація
- soft-launch у кількох країнах

---

## 4. Product Cut

### 4.1 Core Fantasy Version for Solo

Не "велика історична live game про всю війну", а:

**"Мобільний/desktop vertical slice історично натхненної idle-strategy кампанії про оборону Київщини."**

### 4.2 Solo Campaign Shape

Один компактний цикл:
1. Гравець входить у кампанію.
2. Бачить 3 регіони.
3. Заробляє ресурс.
4. Підсилює підрозділи.
5. Виграє/програє бої.
6. Проходить 2-3 події.
7. Звільняє 3 регіони.
8. Активує prestige.
9. Починає цикл з бонусом.

Якщо цей цикл працює, проєкт живий.

---

## 5. Solo Architecture

### 5.1 Recommended Technical Shape

Одна людина має робити просту і стабільну структуру:

- `Bootstrap`
- `GameState`
- `RegionState`
- `UnitDefinition`
- `UpgradeDefinition`
- `EventDefinition`
- `ResourceService`
- `BattleService`
- `MapService`
- `ProgressionService`
- `SaveService`
- `UIController`

### 5.2 Principles

- мінімум магії;
- мінімум складної DI;
- мінімум premature backend work;
- ScriptableObject або JSON лише там, де це реально спрощує iteration;
- кожен день закінчується working build або working play mode state.

---

## 6. Solo Production Strategy

### 6.1 Build Order

Робити тільки в такому порядку:

1. Game state
2. Resource loop
3. Region selection
4. Battle result
5. Save/load
6. Basic HUD
7. Upgrades
8. Events
9. Prestige
10. Polish

### 6.2 Anti-Burnout Rule

Якщо задача не дає playable value за 1 день, її треба:
- або спростити,
- або перенести,
- або викинути.

### 6.3 Daily Rule

Кожен день має закінчуватись одним із результатів:
- нова механіка працює;
- старий flow став стабільнішим;
- UI став зрозумілішим;
- build став придатним для показу.

---

## 7. 8-Week Plan

## Week 1 — Foundation

Ціль тижня:
- підняти технічну основу;
- зібрати мінімальний working loop;
- не займатись поліруванням раніше часу.

Задачі:
- Створити/очистити Unity project structure
- Підняти bootstrap + game state
- Завести constants/config
- Реалізувати tap resource
- Реалізувати auto tick
- Показати ресурси в HUD

Результат тижня:
- ресурс росте;
- проєкт стартує стабільно;
- є базовий екран з цифрами та оновленням стану.

## Week 2 — Map and Battle

Ціль тижня:
- дати гравцю об’єкт вибору;
- прив’язати ресурс до простого outcome.

Задачі:
- Додати 3 регіони
- Додати region selection
- Візуалізувати статуси регіонів
- Додати battle trigger
- Додати просту формулу бою
- Оновлювати статус регіону після бою
- Додати battle result popup

Результат тижня:
- уже можна вибрати регіон, провести бій і побачити наслідок.

## Week 3 — Save and Usability

Ціль тижня:
- зробити loop придатним для щоденного використання;
- прибрати найболючіші UX-проблеми на ранньому етапі.

Задачі:
- Додати save/load
- Перевірити cold restart
- Зробити reset save
- Привести базовий UI в читабельний стан
- Додати selected region panel
- Додати next objective hint
- Додати onboarding text

Результат тижня:
- вертикальний зріз уже не розсипається після перезапуску;
- зрозуміло, куди натискати і що робити далі.

## Week 4 — Units and Upgrades

Ціль тижня:
- додати відчутну прогресію;
- зробити гру не лише "натисни бій", а системою з вибором.

Задачі:
- Додати 3-5 стилізованих підрозділів
- Прив'язати їх до battle modifiers
- Додати unlock flow підрозділів
- Додати ранги або простий upgrade level
- Додати 3 апгрейди
- Зв'язати їх з ресурсами і боєм

Результат тижня:
- гравець вже відчуває відмінність між підрозділами і розвитком.

## Week 5 — Events and Context

Ціль тижня:
- додати наративний шар без перевантаження;
- перевірити, чи події реально підсилюють fantasy.

Задачі:
- Додати event system skeleton
- Створити event data format
- Додати Event 1
- Додати Event 2
- Додати Event 3
- Прив'язати наслідки подій до карти/ресурсів
- Додати simple archive / lore screen без персоналій

Результат тижня:
- у гри з’являється характер, а не лише механіка.

## Week 6 — Prestige and Balance

Ціль тижня:
- замкнути повний цикл кампанії;
- зробити перезапуск осмисленим.

Задачі:
- Додати prestige/reset
- Додати permanent bonus
- Перевірити повний campaign loop
- Відбалансувати темп early/mid game
- Зменшити усе, що робить цикл нудним або заплутаним

Результат тижня:
- є повний повторюваний gameplay loop, який можна показувати людям.

## Week 7 — Feedback Pass

Ціль тижня:
- перестати дивитися на гру тільки очима автора;
- прибрати найбільш токсичні UX-блокери.

Задачі:
- Записати fresh build
- Дати 1-3 людям пройти прототип
- Зібрати фідбек
- Виправити top UX blockers
- Додати local event log або simple analytics log
- Прибрати dev-noise з UI

Результат тижня:
- прототип стає зрозумілішим для сторонньої людини.

## Week 8 — Demo Freeze

Ціль тижня:
- підготувати версію, яку не соромно показати;
- не вигадувати нові механіки.

Задачі:
- Повний regression pass
- Перевірити save compatibility
- Підкрутити темп ресурсу/бою/апгрейдів
- Додати простий звук або stub FX, якщо це реально підсилює демо
- Зібрати demo build
- Перевірити clean start flow
- Записати demo video 60-90 sec
- Оновити документацію по прототипу
- Freeze version
- Сформувати список “що далі робити лише з командою”

Результат тижня:
- готовий demo package: build, відео, короткий опис і план наступного етапу.

---

## 8. Definition of Done for Solo Prototype

Прототип вважається готовим, якщо:
- стартує без критичних помилок;
- має зрозумілий onboarding;
- дає пройти мінімум один повний session loop;
- має відчутну різницю між регіонами;
- має хоча б 1 meaningful decision через event;
- зберігає прогрес;
- має working prestige/reset;
- придатний для відеодемо або live-demo.

---

## 9. What I Can Help With

З моєю допомогою один розробник може суттєво прискорити роботу, бо я можу:
- проєктувати архітектуру;
- писати і рефакторити код;
- збирати технічний backlog;
- допомагати з балансом формул;
- готувати data schemas;
- спрощувати scope;
- готувати UI structure;
- вичищати документацію;
- допомагати з debug і review;
- формувати demo-ready версію.

### 9.1 Що я не заміняю

Я не заміняю:
- зовнішнє playtesting на живих людях;
- юридичний висновок;
- store review authority;
- повноцінний арт і звук production на великому обсязі.

---

## 10. Recommended Solo Milestones

### Milestone A — End of Week 2

Working loop:
- ресурс
- карта
- бій
- базовий UI

### Milestone B — End of Week 4

Playable slice:
- save/load
- підрозділи
- апгрейди
- ціль сесії

### Milestone C — End of Week 6

Pitchable prototype:
- події
- prestige
- цілісний flow

### Milestone D — End of Week 8

Demo package:
- build
- відео
- короткий one-pager
- список наступних кроків

---

## 11. Recommended Next Step Right Now

Почати не з monetization, не з backend і не з legal-heavy контенту.

Почати з:
1. одного ігрового екрану або двох простих екранів;
2. трьох регіонів;
3. одного battle formula;
4. одного save file;
5. одного короткого playable loop.

Якщо це працює, усе інше ще має сенс. Якщо ні, добре, що це стало видно дешево і рано.
