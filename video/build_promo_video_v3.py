from __future__ import annotations

from pathlib import Path
from typing import List

import imageio.v2 as imageio
import numpy as np
from PIL import Image, ImageDraw, ImageFont


WIDTH = 1920
HEIGHT = 1080
FPS = 30
OUTPUT_NAME = "BastionUA_game_trailer_v3_silent.mp4"

BG = (7, 12, 22)
CARD = (16, 24, 40)
TEXT = (242, 245, 252)
MUTED = (155, 170, 197)
ACCENT = (255, 213, 79)
SAFE = (52, 168, 83)
DANGER = (244, 180, 0)
OCCUPIED = (219, 68, 55)
LINE = (44, 58, 89)


def load_font(names: List[str], size: int) -> ImageFont.ImageFont:
    for name in names:
        try:
            return ImageFont.truetype(name, size)
        except OSError:
            continue
    return ImageFont.load_default()


TITLE = load_font(["segoeuib.ttf", "arialbd.ttf", "DejaVuSans-Bold.ttf"], 56)
SUB = load_font(["segoeuib.ttf", "arialbd.ttf", "DejaVuSans-Bold.ttf"], 30)
BODY = load_font(["segoeui.ttf", "arial.ttf", "DejaVuSans.ttf"], 26)
SMALL = load_font(["segoeui.ttf", "arial.ttf", "DejaVuSans.ttf"], 20)


SCENES = [
    {
        "kind": "open",
        "title": "24.02.2022",
        "subtitle": "Кампанія: Оборона Київщини",
        "caption": "Карта прокидається. Напруга вже тут.",
        "duration": 12,
    },
    {
        "kind": "map",
        "title": "Карта кампанії",
        "subtitle": "Kyiv · Chernihiv · Sumy",
        "caption": "Три регіони. Три різні стани. Один фронт.",
        "duration": 14,
    },
    {
        "kind": "resource",
        "title": "Перший ресурсний цикл",
        "subtitle": "Набої ростуть вручну і автоматично",
        "caption": "Короткі сесії. Швидкий ритм. Постійний прогрес.",
        "duration": 14,
    },
    {
        "kind": "units",
        "title": "Підрозділи",
        "subtitle": "Оборона · Штурм · Розвідка",
        "caption": "Ролі змінюють стиль проходження.",
        "duration": 18,
    },
    {
        "kind": "battle",
        "title": "Перший бій",
        "subtitle": "Автобій з видимим результатом",
        "caption": "Витрати ресурс. Втримай сектор. Зміни карту.",
        "duration": 20,
    },
    {
        "kind": "event",
        "title": "Подія",
        "subtitle": "Вибір і наслідок",
        "caption": "Події дають характер, а не просто текст.",
        "duration": 17,
    },
    {
        "kind": "upgrade",
        "title": "Апгрейд",
        "subtitle": "Швидший темп і сильніший цикл",
        "caption": "Прогрес відчувається одразу.",
        "duration": 14,
    },
    {
        "kind": "map2",
        "title": "Кампанія змінюється",
        "subtitle": "Карта реагує на твої рішення",
        "caption": "Не екран заради екрану. Реальний стан кампанії.",
        "duration": 15,
    },
    {
        "kind": "prestige",
        "title": "Ставка",
        "subtitle": "Новий цикл. Сильніший старт.",
        "caption": "Ти повертаєшся у кампанію з досвідом.",
        "duration": 10,
    },
]


def lerp(a: float, b: float, t: float) -> float:
    return a + (b - a) * t


def draw_header(draw: ImageDraw.ImageDraw, title: str, subtitle: str) -> None:
    draw.line((90, 72, WIDTH - 90, 72), fill=LINE, width=2)
    draw.line((90, HEIGHT - 72, WIDTH - 90, HEIGHT - 72), fill=LINE, width=2)
    draw.rectangle((90, 104, 108, 190), fill=ACCENT)
    draw.text((128, 98), title, font=TITLE, fill=TEXT)
    draw.text((128, 166), subtitle, font=SUB, fill=ACCENT)


def draw_footer(draw: ImageDraw.ImageDraw, caption: str) -> None:
    draw.text((90, HEIGHT - 54), caption, font=SMALL, fill=MUTED)


def region_card(draw: ImageDraw.ImageDraw, x: int, y: int, label: str, color: tuple[int, int, int]) -> None:
    draw.rounded_rectangle((x, y, x + 220, y + 110), radius=18, fill=CARD, outline=LINE, width=2)
    draw.ellipse((x + 18, y + 24, x + 44, y + 50), fill=color)
    draw.text((x + 60, y + 22), label, font=BODY, fill=TEXT)
    status = "SAFE" if color == SAFE else "DANGER" if color == DANGER else "OCCUPIED"
    draw.text((x + 60, y + 58), status, font=SMALL, fill=MUTED)


def draw_map_scene(draw: ImageDraw.ImageDraw, t: float, second_variant: bool = False) -> None:
    map_box = (260, 250, 1460, 870)
    draw.rounded_rectangle(map_box, radius=28, fill=(12, 20, 34), outline=LINE, width=3)
    # stylized map panel
    draw.polygon([(470, 430), (640, 330), (780, 360), (980, 300), (1160, 420), (1120, 670), (850, 760), (570, 700)], outline=(80, 96, 130), fill=(14, 27, 44))
    pulse = int(8 * (1 + np.sin(t * 6.28)))
    draw.ellipse((700 - pulse, 450 - pulse, 700 + 22 + pulse, 472 + pulse), fill=SAFE if second_variant else DANGER)
    draw.ellipse((910 - pulse, 395 - pulse, 910 + 22 + pulse, 417 + pulse), fill=SAFE if second_variant else DANGER)
    draw.ellipse((1040 - pulse, 520 - pulse, 1040 + 22 + pulse, 542 + pulse), fill=OCCUPIED if not second_variant else DANGER)
    draw.text((670, 485), "Kyiv", font=SMALL, fill=TEXT)
    draw.text((885, 430), "Chernihiv", font=SMALL, fill=TEXT)
    draw.text((1015, 555), "Sumy", font=SMALL, fill=TEXT)
    region_card(draw, 1500, 300, "Kyiv", SAFE if second_variant else DANGER)
    region_card(draw, 1500, 430, "Chernihiv", SAFE if second_variant else DANGER)
    region_card(draw, 1500, 560, "Sumy", DANGER if second_variant else OCCUPIED)


def draw_resource_scene(draw: ImageDraw.ImageDraw, t: float) -> None:
    draw.rounded_rectangle((250, 260, 1670, 820), radius=28, fill=(12, 20, 34), outline=LINE, width=3)
    ammo = int(120 + lerp(0, 420, t))
    draw.text((340, 340), "Ammo", font=SUB, fill=MUTED)
    draw.text((340, 390), f"{ammo}", font=TITLE, fill=TEXT)
    draw.text((340, 470), "+ tap    + auto tick", font=BODY, fill=ACCENT)
    draw.rounded_rectangle((1030, 330, 1450, 450), radius=18, fill=CARD, outline=LINE, width=2)
    draw.text((1080, 370), "Tap +12", font=BODY, fill=TEXT)
    draw.rounded_rectangle((1030, 500, 1450, 620), radius=18, fill=CARD, outline=LINE, width=2)
    draw.text((1060, 540), "Auto +4 / sec", font=BODY, fill=TEXT)
    draw.text((340, 610), "Objective: gather resources for first battle", font=BODY, fill=TEXT)


def draw_units_scene(draw: ImageDraw.ImageDraw, t: float) -> None:
    draw.rounded_rectangle((230, 240, 1690, 840), radius=28, fill=(12, 20, 34), outline=LINE, width=3)
    cards = [
        ("Vanguard", "DEF +15%", 300),
        ("Falcons", "ATK +12%", 760),
        ("Recon", "Crit +10%", 1220),
    ]
    for i, (name, perk, x) in enumerate(cards):
        y = 320 + (0 if i != 1 else int(10 * np.sin(t * 6.28)))
        draw.rounded_rectangle((x, y, x + 330, y + 230), radius=22, fill=CARD, outline=ACCENT if i == 1 else LINE, width=3)
        draw.text((x + 30, y + 28), name, font=SUB, fill=TEXT)
        draw.text((x + 30, y + 88), perk, font=BODY, fill=ACCENT)
        draw.text((x + 30, y + 150), "Role active", font=SMALL, fill=MUTED)
    draw.rounded_rectangle((770, 610, 1150, 720), radius=18, fill=(22, 36, 58), outline=LINE, width=2)
    draw.text((810, 646), "Drag to battle slot", font=BODY, fill=TEXT)


def draw_battle_scene(draw: ImageDraw.ImageDraw, t: float) -> None:
    draw.rounded_rectangle((180, 230, 1740, 860), radius=28, fill=(12, 20, 34), outline=LINE, width=3)
    player_hp = int(86 - lerp(0, 18, t))
    enemy_hp = int(100 - lerp(0, 78, t))
    draw.text((270, 300), "Battle: Northern Sector", font=SUB, fill=TEXT)
    draw.text((270, 360), "Enemy", font=BODY, fill=MUTED)
    draw.rounded_rectangle((430, 360, 1130, 400), radius=12, fill=(50, 20, 20))
    draw.rounded_rectangle((430, 360, 430 + int(7 * enemy_hp), 400), radius=12, fill=OCCUPIED)
    draw.text((1160, 360), f"{enemy_hp}%", font=BODY, fill=TEXT)
    draw.text((270, 430), "Allies", font=BODY, fill=MUTED)
    draw.rounded_rectangle((430, 430, 1130, 470), radius=12, fill=(20, 44, 30))
    draw.rounded_rectangle((430, 430, 430 + int(7 * player_hp), 470), radius=12, fill=SAFE)
    draw.text((1160, 430), f"{player_hp}%", font=BODY, fill=TEXT)
    draw.rounded_rectangle((310, 550, 630, 670), radius=20, fill=CARD, outline=LINE, width=2)
    draw.text((360, 595), "Vanguard", font=BODY, fill=TEXT)
    draw.rounded_rectangle((710, 550, 1030, 670), radius=20, fill=CARD, outline=LINE, width=2)
    draw.text((778, 595), "Falcons", font=BODY, fill=TEXT)
    draw.rounded_rectangle((1110, 550, 1430, 670), radius=20, fill=CARD, outline=LINE, width=2)
    draw.text((1185, 595), "Recon", font=BODY, fill=TEXT)
    result = "Sector secured" if t > 0.75 else "Auto-battle in progress"
    draw.text((270, 740), result, font=SUB, fill=ACCENT if t > 0.75 else TEXT)


def draw_event_scene(draw: ImageDraw.ImageDraw, t: float) -> None:
    draw.rounded_rectangle((340, 220, 1580, 860), radius=28, fill=(12, 20, 34), outline=LINE, width=3)
    draw.text((430, 300), "28.02.2022", font=SMALL, fill=ACCENT)
    draw.text((430, 340), "Event: Airfield under pressure", font=SUB, fill=TEXT)
    draw.text((430, 410), "Enemy forces are testing the line. How do you react?", font=BODY, fill=TEXT)
    opts = [
        ("Send reserves", 500),
        ("Hold ammunition", 610),
        ("Destroy runway", 720),
    ]
    selected = min(2, int(t * 3))
    for idx, (label, y) in enumerate(opts):
        draw.rounded_rectangle((430, y, 1240, y + 72), radius=18, fill=CARD, outline=ACCENT if idx == selected else LINE, width=3)
        draw.text((470, y + 20), label, font=BODY, fill=TEXT)
    draw.text((430, 810), "Outcome: morale +8, Kyiv line stabilizes", font=BODY, fill=ACCENT)


def draw_upgrade_scene(draw: ImageDraw.ImageDraw, t: float) -> None:
    draw.rounded_rectangle((250, 230, 1670, 850), radius=28, fill=(12, 20, 34), outline=LINE, width=3)
    draw.text((330, 300), "Tech Tree", font=SUB, fill=TEXT)
    nodes = [(460, 470), (760, 390), (1060, 510), (1320, 390)]
    for i, (x, y) in enumerate(nodes):
        fill = SAFE if i <= int(t * 3.2) else CARD
        draw.ellipse((x, y, x + 84, y + 84), fill=fill, outline=LINE, width=3)
        if i < len(nodes) - 1:
            nx, ny = nodes[i + 1]
            draw.line((x + 84, y + 42, nx, ny + 42), fill=LINE, width=5)
    draw.text((330, 660), "Upgrade unlocked: Ammo efficiency", font=BODY, fill=ACCENT)
    draw.text((330, 710), "Resource tempo increased", font=BODY, fill=TEXT)


def draw_prestige_scene(draw: ImageDraw.ImageDraw, t: float) -> None:
    draw.rounded_rectangle((330, 250, 1590, 830), radius=30, fill=(12, 20, 34), outline=LINE, width=3)
    draw.text((450, 330), "Ставка", font=TITLE, fill=TEXT)
    draw.text((450, 420), "Campaign complete. Start a stronger cycle.", font=SUB, fill=ACCENT)
    draw.text((450, 510), "Permanent bonus: production speed +10%", font=BODY, fill=TEXT)
    glow = int(180 + 60 * t)
    draw.rounded_rectangle((450, 620, 930, 720), radius=22, fill=(glow, 180, 50), outline=None)
    draw.text((540, 650), "Begin new cycle", font=BODY, fill=BG)


def render_scene(scene: dict, frame_idx: int, frame_count: int) -> Image.Image:
    img = Image.new("RGB", (WIDTH, HEIGHT), BG)
    draw = ImageDraw.Draw(img)
    t = frame_idx / max(frame_count - 1, 1)
    draw_header(draw, scene["title"], scene["subtitle"])
    kind = scene["kind"]
    if kind == "open":
        draw.rounded_rectangle((270, 280, 1650, 800), radius=30, fill=(12, 20, 34), outline=LINE, width=3)
        alpha = int(80 + 170 * t)
        draw.polygon([(520, 500), (760, 340), (1120, 360), (1320, 540), (1180, 730), (720, 700)], outline=(alpha, alpha, alpha), fill=(14, 27, 44))
        for x, y, label in [(790, 470, "Kyiv"), (1040, 430, "Chernihiv"), (1110, 590, "Sumy")]:
            draw.ellipse((x, y, x + 24, y + 24), fill=ACCENT)
            draw.text((x + 36, y - 2), label, font=SMALL, fill=TEXT)
    elif kind == "map":
        draw_map_scene(draw, t, second_variant=False)
    elif kind == "resource":
        draw_resource_scene(draw, t)
    elif kind == "units":
        draw_units_scene(draw, t)
    elif kind == "battle":
        draw_battle_scene(draw, t)
    elif kind == "event":
        draw_event_scene(draw, t)
    elif kind == "upgrade":
        draw_upgrade_scene(draw, t)
    elif kind == "map2":
        draw_map_scene(draw, t, second_variant=True)
    elif kind == "prestige":
        draw_prestige_scene(draw, t)
    draw_footer(draw, scene["caption"])
    return img


def main() -> None:
    out_path = Path(__file__).resolve().parent / OUTPUT_NAME
    writer = imageio.get_writer(str(out_path), fps=FPS, codec="libx264", quality=8)
    try:
        for scene in SCENES:
            frame_count = scene["duration"] * FPS
            for i in range(frame_count):
                frame = np.asarray(render_scene(scene, i, frame_count))
                writer.append_data(frame)
    finally:
        writer.close()
    print(f"Video created: {out_path}")


if __name__ == "__main__":
    main()
