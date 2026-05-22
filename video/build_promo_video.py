from __future__ import annotations

from pathlib import Path
from typing import List

from PIL import Image, ImageDraw, ImageFont
import imageio.v2 as imageio
import numpy as np


WIDTH = 1920
HEIGHT = 1080
FPS = 30
OUTPUT_NAME = "BastionUA_promo_v1.mp4"

BG_COLOR = (10, 16, 28)
ACCENT_COLOR = (255, 210, 76)
TEXT_COLOR = (245, 247, 255)
MUTED_COLOR = (165, 177, 205)
LINE_COLOR = (54, 70, 107)

TITLE_SIZE = 64
SUBTITLE_SIZE = 40
BODY_SIZE = 34
FOOTER_SIZE = 24

TOP_MARGIN = 110
LEFT_MARGIN = 120
RIGHT_MARGIN = 120


def find_font(preferred: List[str], fallback_size: int) -> ImageFont.ImageFont:
    for font_name in preferred:
        try:
            return ImageFont.truetype(font_name, fallback_size)
        except OSError:
            continue
    return ImageFont.load_default()


TITLE_FONT = find_font(
    ["segoeuib.ttf", "arialbd.ttf", "DejaVuSans-Bold.ttf"],
    TITLE_SIZE,
)
SUBTITLE_FONT = find_font(
    ["segoeuib.ttf", "arialbd.ttf", "DejaVuSans-Bold.ttf"],
    SUBTITLE_SIZE,
)
BODY_FONT = find_font(
    ["segoeui.ttf", "arial.ttf", "DejaVuSans.ttf"],
    BODY_SIZE,
)
FOOTER_FONT = find_font(
    ["segoeui.ttf", "arial.ttf", "DejaVuSans.ttf"],
    FOOTER_SIZE,
)


SCENES = [
    {
        "title": "BASTION UA",
        "subtitle": "Грай. Підтримуй. Пам'ятай.",
        "bullets": [
            "Idle-strategy про оборону України з 24.02.2022",
            "Реальна карта, історичні івенти, гра за ЗСУ",
            "20% чистого прибутку -> фонди ЗСУ",
        ],
        "duration_sec": 8,
    },
    {
        "title": "ПРОБЛЕМА",
        "subtitle": "Підтримка ЗСУ потребує нових форматів",
        "bullets": [
            "Втома від донатів і падіння уваги аудиторії",
            "Молодь 14-25 живе в середовищі короткої уваги",
            "Немає масового інтерактивного формату підтримки",
        ],
        "duration_sec": 8,
    },
    {
        "title": "РІШЕННЯ",
        "subtitle": "Мобільна idle-стратегія з офлайн-прогресом",
        "bullets": [
            "5-10 хвилин сесії + авто-прогрес офлайн",
            "Бригади, tech tree, дрони, дипломатія",
            "Історичні події: Гостомель, Чорнобаївка, Херсон",
        ],
        "duration_sec": 9,
    },
    {
        "title": "MVP ЗА 6 МІСЯЦІВ",
        "subtitle": "План, команда, бюджет",
        "bullets": [
            "Android + Telegram Mini App, закрита бета",
            "Команда 7-9 FTE, бюджет ~173K USD",
            "Цілі бети: 5K DAU, D1 >= 40%, D7 >= 15%",
        ],
        "duration_sec": 9,
    },
    {
        "title": "БІЗНЕС-МОДЕЛЬ",
        "subtitle": "Стійка економіка + соціальна місія",
        "bullets": [
            "IAP + Battle Pass + Infinity Pass + Rewarded Ads",
            "Base LTV:CAC ~7.3x, break-even близько 60K MAU",
            "Прозорі звіти і in-game лічильник підтримки ЗСУ",
        ],
        "duration_sec": 9,
    },
    {
        "title": "ПРАВОВА РАМКА",
        "subtitle": "MOU з бригадами + compliance App Store/Google Play",
        "bullets": [
            "Донати не через IAP: тільки зовнішні посилання",
            "Ліцензування символіки через партнерські угоди",
            "Фокус на етичній подачі без романтизації насильства",
        ],
        "duration_sec": 9,
    },
    {
        "title": "ROADMAP 18 МІСЯЦІВ",
        "subtitle": "Від MVP до глобального релізу",
        "bullets": [
            "Q2 2026: closed beta + soft launch UA/PL/EE",
            "Q3 2026: global launch (EN/DE/PL)",
            "Q2 2027: Steam + Web, сезонні історичні оновлення",
        ],
        "duration_sec": 8,
    },
    {
        "title": "BASTION UA",
        "subtitle": "Перший soft-power gaming продукт України",
        "bullets": [
            "Місія: зберегти історію і підтримувати ЗСУ",
            "Документація: E:/BastionUA/docs",
            "Слава Україні! Героям слава!",
        ],
        "duration_sec": 8,
    },
]


def wrap_text(draw: ImageDraw.ImageDraw, text: str, font: ImageFont.ImageFont, max_width: int) -> List[str]:
    words = text.split()
    if not words:
        return [""]
    lines: List[str] = []
    current = words[0]
    for word in words[1:]:
        trial = f"{current} {word}"
        box = draw.textbbox((0, 0), trial, font=font)
        if box[2] - box[0] <= max_width:
            current = trial
        else:
            lines.append(current)
            current = word
    lines.append(current)
    return lines


def render_scene(scene: dict) -> Image.Image:
    img = Image.new("RGB", (WIDTH, HEIGHT), BG_COLOR)
    draw = ImageDraw.Draw(img)

    # Header lines
    draw.line((LEFT_MARGIN, 75, WIDTH - RIGHT_MARGIN, 75), fill=LINE_COLOR, width=2)
    draw.line((LEFT_MARGIN, HEIGHT - 95, WIDTH - RIGHT_MARGIN, HEIGHT - 95), fill=LINE_COLOR, width=2)

    # Accent block
    draw.rectangle((LEFT_MARGIN, TOP_MARGIN, LEFT_MARGIN + 20, TOP_MARGIN + 90), fill=ACCENT_COLOR)

    # Title
    draw.text((LEFT_MARGIN + 40, TOP_MARGIN), scene["title"], font=TITLE_FONT, fill=TEXT_COLOR)
    draw.text((LEFT_MARGIN + 40, TOP_MARGIN + 82), scene["subtitle"], font=SUBTITLE_FONT, fill=ACCENT_COLOR)

    # Bullets
    y = TOP_MARGIN + 190
    max_text_width = WIDTH - LEFT_MARGIN - RIGHT_MARGIN - 40
    for bullet in scene["bullets"]:
        lines = wrap_text(draw, bullet, BODY_FONT, max_text_width)
        draw.text((LEFT_MARGIN + 40, y), "•", font=BODY_FONT, fill=ACCENT_COLOR)
        line_y = y
        for i, line in enumerate(lines):
            x = LEFT_MARGIN + 80 if i == 0 else LEFT_MARGIN + 98
            draw.text((x, line_y), line, font=BODY_FONT, fill=TEXT_COLOR)
            line_y += 48
        y = line_y + 20

    # Footer
    draw.text(
        (LEFT_MARGIN, HEIGHT - 70),
        "Bastion UA • Presentation teaser • v1",
        font=FOOTER_FONT,
        fill=MUTED_COLOR,
    )
    return img


def build_video(output_path: Path) -> None:
    output_path.parent.mkdir(parents=True, exist_ok=True)
    writer = imageio.get_writer(str(output_path), fps=FPS, codec="libx264", quality=8)
    try:
        for scene in SCENES:
            frame = render_scene(scene)
            frame_np = np.asarray(frame)
            frame_count = int(scene["duration_sec"] * FPS)
            for _ in range(frame_count):
                writer.append_data(frame_np)
    finally:
        writer.close()


def main() -> None:
    video_dir = Path(__file__).resolve().parent
    output = video_dir / OUTPUT_NAME
    build_video(output)
    print(f"Video created: {output}")


if __name__ == "__main__":
    main()
