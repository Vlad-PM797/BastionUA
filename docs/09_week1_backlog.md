# Bastion UA — Week 1 Backlog

Статус: draft  
Дата: 2026-04-24  
Призначення: конкретний backlog для першого тижня solo-розробки за планом `08_solo_version_plan.md`.

---

## 1. Goal of Week 1

До кінця першого тижня має існувати **мінімальний working prototype state**, у якому:
- Unity-проєкт стабільно відкривається;
- є базовий `GameState`;
- ресурс генерується вручну і автоматично;
- значення ресурсу видно в HUD;
- є точка входу для подальших систем другого тижня.

Week 1 не про красу, не про контент і не про поліровку.  
Week 1 про те, щоб створити живий технічний каркас.

---

## 2. Definition of Done

Тиждень вважається завершеним, якщо:
- сцена запускається без compile errors;
- `GameBootstrap` ініціалізує гру;
- ресурс збільшується по таймеру;
- ресурс можна додати вручну через кнопку або dev input;
- HUD показує актуальне значення ресурсу;
- стан не розкиданий по випадкових MonoBehaviour без контролю;
- є короткий список known issues для Week 2.

---

## 3. Priority Order

Порядок виконання не міняти без причини:

1. Project skeleton
2. Game state
3. Resource loop
4. HUD binding
5. Dev controls
6. Cleanup

---

## 4. Backlog Items

## W1-01 Project Skeleton

**Priority:** P0  
**Estimate:** 2-4 години

### Ціль

Підготувати чисту стартову структуру Unity-проєкту без зайвого хаосу.

### Задачі

- Перевірити або створити базову структуру папок:
  - `Assets/Scripts/Bootstrap`
  - `Assets/Scripts/Core`
  - `Assets/Scripts/Services`
  - `Assets/Scripts/UI`
  - `Assets/Scripts/Data`
  - `Assets/Scenes`
- Створити стартову сцену `Boot`
- Додати `GameBootstrap` на сцену
- Переконатися, що проєкт стартує без відсутніх посилань

### Acceptance Criteria

- є одна чиста стартова сцена;
- є один об'єкт входу `GameBootstrap`;
- структура папок не викликає питань "куди класти нові скрипти".

---

## W1-02 Core Game State

**Priority:** P0  
**Estimate:** 3-5 годин

### Ціль

Завести єдину точку правди для базового стану гри.

### Задачі

- Створити `GameState`
- Додати мінімальні поля:
  - `ammo`
  - `morale` або placeholder під morale
  - `selectedRegionId` або placeholder
  - `lastTickTime` або інший технічний маркер
- Визначити стартові дефолтні значення
- Забезпечити читання і зміну стану через контрольовані сервіси

### Acceptance Criteria

- стан гри існує як окрема модель;
- ресурс не живе тільки в UI;
- `GameBootstrap` може створити або ініціалізувати цей стан.

---

## W1-03 Resource Service

**Priority:** P0  
**Estimate:** 4-6 годин

### Ціль

Реалізувати першу живу механіку: ручну і автоматичну генерацію ресурсу.

### Задачі

- Створити `ResourceService`
- Додати метод ручного приросту ресурсу
- Додати auto tick з інтервалом
- Додати конфігуровані значення:
  - manual gain
  - auto gain
  - tick interval
- Підключити сервіс до `GameState`

### Acceptance Criteria

- ресурс зростає автоматично;
- ресурс зростає вручну;
- значення можна змінити в одному місці без пошуку по коду.

---

## W1-04 HUD Prototype

**Priority:** P0  
**Estimate:** 2-4 години

### Ціль

Показати гравцю, що система працює.

### Задачі

- Створити простий Canvas
- Додати текст/лейбл для ammo
- Додати placeholder для morale
- Зв'язати HUD зі станом або сервісом
- Забезпечити оновлення UI без ручного перезапуску сцени

### Acceptance Criteria

- HUD відображає актуальний ammo;
- при зміні ресурсу значення на екрані змінюється одразу;
- UI не містить зайвих декоративних елементів, які гальмують старт.

---

## W1-05 Dev Input / Debug Controls

**Priority:** P1  
**Estimate:** 1-2 години

### Ціль

Прискорити розробку другого тижня і не залежати від UI для кожної перевірки.

### Задачі

- Додати одну dev-кнопку або hotkey для ручного додавання ammo
- Додати опціональний reset action
- Вивести короткі debug messages лише якщо це реально допомагає

### Acceptance Criteria

- можна швидко перевірити ресурс без зайвих кліків по інтерфейсу;
- dev controls не плутаються з майбутнім player-facing UI.

---

## W1-06 Constants / Config

**Priority:** P1  
**Estimate:** 1-2 години

### Ціль

Не ховати баланс у випадкових числах по всьому коду.

### Задачі

- Створити `GameConstants` або `GameConfig`
- Винести туди:
  - starting ammo
  - manual gain
  - auto gain
  - tick interval
- Підготувати місце для future constants

### Acceptance Criteria

- ключові базові числа лежать централізовано;
- змінити темп ресурсу можна швидко і без ризику щось зламати.

---

## W1-07 Code Cleanup Pass

**Priority:** P1  
**Estimate:** 1-3 години

### Ціль

Не залишити після першого тижня хаос, який загальмує другий.

### Задачі

- Перевірити назви класів і файлів
- Прибрати тимчасовий шум, який більше не потрібен
- Додати 2-3 короткі коментарі тільки там, де логіка неочевидна
- Перевірити, чи немає дублювання між bootstrap/state/service/UI

### Acceptance Criteria

- код читається без внутрішньої паніки;
- немає відчуття, що прототип уже "забруднений" на старті.

---

## W1-08 End-of-Week Checkpoint

**Priority:** P0  
**Estimate:** 1-2 години

### Ціль

Завершити тиждень у контрольованому стані.

### Задачі

- Пройти короткий smoke test:
  - сцена стартує
  - ammo видно
  - auto tick працює
  - manual gain працює
- Записати короткий changelog
- Зафіксувати known issues
- Записати 30-60 секунд відео або зробити серію скринів

### Acceptance Criteria

- є підтвердження, що перший тиждень реально завершився working state;
- зрозуміло, з чого починати Week 2.

---

## 5. Suggested Day Split

### Day 1

- W1-01 Project Skeleton
- W1-02 Core Game State

### Day 2

- W1-03 Resource Service
- W1-06 Constants / Config

### Day 3

- W1-04 HUD Prototype
- W1-05 Dev Input / Debug Controls

### Day 4

- Fix/iterate on state + resource loop
- Закрити все, що заважає stability

### Day 5

- W1-07 Code Cleanup Pass
- W1-08 End-of-Week Checkpoint

### Buffer

Якщо будь-яка задача переїжджає:
- не додавати нові фічі;
- використати залишок тижня тільки на завершення P0 задач.

---

## 6. Known Traps

- Не починати карту в Week 1.
- Не додавати бойову систему раніше стабільного ресурсу.
- Не витрачати день на красивий HUD.
- Не роздувати архітектуру "на майбутнє".
- Не лізти в backend, поки local loop не живий.

---

## 7. Deliverables

До кінця Week 1 бажано мати:
- working Unity scene;
- базовий набір скриптів;
- короткий changelog;
- список проблем для Week 2.

---

## 8. Recommended Week 2 Entry Point

Якщо Week 1 завершений успішно, Week 2 треба починати так:
1. додати регіони;
2. зробити region selection;
3. візуалізувати статуси;
4. додати battle trigger;
5. прив'язати outcome до map state.
