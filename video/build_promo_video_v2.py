from __future__ import annotations

import asyncio
import math
import subprocess
import wave
from pathlib import Path
from typing import List

import imageio.v2 as imageio
import imageio_ffmpeg
import numpy as np
from PIL import Image, ImageDraw, ImageFont

try:
    import edge_tts  # type: ignore
except Exception:
    edge_tts = None


WIDTH = 1920
HEIGHT = 1088  # divisible by 16 for codec compatibility
FPS = 30
VIDEO_NAME = "BastionUA_promo_v2.mp4"
VIDEO_SILENT_NAME = "BastionUA_promo_v2_silent.mp4"
VOICE_NAME = "BastionUA_voice_uk.mp3"
MUSIC_NAME = "BastionUA_bg_music.wav"

BG_COLOR = (9, 15, 27)
ACCENT = (252, 209, 74)
TEXT = (244, 247, 255)
MUTED = (165, 176, 203)
LINE = (55, 72, 110)

TOP_MARGIN = 90
LEFT_MARGIN = 110
RIGHT_MARGIN = 110


def font_try(names: List[str], size: int) -> ImageFont.ImageFont:
    for name in names:
        try:
            return ImageFont.truetype(name, size)
        except OSError:
            continue
    return ImageFont.load_default()


TITLE_FONT = font_try(["segoeuib.ttf", "arialbd.ttf", "DejaVuSans-Bold.ttf"], 62)
SUBTITLE_FONT = font_try(["segoeuib.ttf", "arialbd.ttf", "DejaVuSans-Bold.ttf"], 36)
BODY_FONT = font_try(["segoeui.ttf", "arial.ttf", "DejaVuSans.ttf"], 30)
FOOTER_FONT = font_try(["segoeui.ttf", "arial.ttf", "DejaVuSans.ttf"], 22)


SCENES = [
    {
        "title": "BASTION UA",
        "subtitle": "Грай. Підтримуй. Пам'ятай.",
        "bullets": [
            "Idle-strategy про оборону України з 24.02.2022",
            "Реальна карта, історичні події, гра за ЗСУ",
        ],
        "duration": 6,
        "voice": "Bastion UA. Грай, підтримуй, пам'ятай.",
    },
    {
        "title": "ПРОБЛЕМА",
        "subtitle": "Потрібен новий формат підтримки ЗСУ",
        "bullets": [
            "Падає увага і втомлюється донат-аудиторія",
            "Потрібен доступний масовий інструмент залучення",
        ],
        "duration": 6,
        "voice": "Підтримка ЗСУ поступово згасає. Потрібен новий масовий формат.",
    },
    {
        "title": "РІШЕННЯ",
        "subtitle": "Мобільна гра з реальним сенсом",
        "bullets": [
            "Сесії 5-10 хв + офлайн-прогрес",
            "Бригади, дрони, дипломатія, ключові операції",
        ],
        "duration": 6,
        "voice": "Рішення — мобільна idle-стратегія з реальним історичним контекстом.",
    },
    {
        "title": "MVP ТА ЕКОНОМІКА",
        "subtitle": "6 місяців до закритої бети",
        "bullets": [
            "Команда 7-9 FTE, бюджет близько 173 тисяч доларів",
            "Цільова юніт-економіка: LTV/CAC близько 7.3x",
        ],
        "duration": 7,
        "voice": "MVP за 6 місяців. Цільова юніт-економіка — LTV до CAC 7.3.",
    },
    {
        "title": "СОЦІАЛЬНА МІСІЯ",
        "subtitle": "Продукт, що підтримує армію",
        "bullets": [
            "20% чистого прибутку -> фонди ЗСУ",
            "Прозора звітність і legal-compliance",
        ],
        "duration": 7,
        "voice": "Двадцять відсотків чистого прибутку спрямовуються у фонди ЗСУ.",
    },
    {
        "title": "BASTION UA",
        "subtitle": "Перший soft-power gaming продукт України",
        "bullets": [
            "Документація: E:/BastionUA/docs",
            "Слава Україні! Героям слава!",
        ],
        "duration": 6,
        "voice": "Bastion UA. Перший soft power gaming продукт України.",
    },
]


def wrap_text(draw: ImageDraw.ImageDraw, text: str, font: ImageFont.ImageFont, max_width: int) -> List[str]:
    words = text.split()
    if not words:
        return [""]
    lines = [words[0]]
    for word in words[1:]:
        probe = f"{lines[-1]} {word}"
        box = draw.textbbox((0, 0), probe, font=font)
        if (box[2] - box[0]) <= max_width:
            lines[-1] = probe
        else:
            lines.append(word)
    return lines


def render_scene(scene: dict) -> Image.Image:
    img = Image.new("RGB", (WIDTH, HEIGHT), BG_COLOR)
    draw = ImageDraw.Draw(img)
    draw.line((LEFT_MARGIN, 70, WIDTH - RIGHT_MARGIN, 70), fill=LINE, width=2)
    draw.line((LEFT_MARGIN, HEIGHT - 90, WIDTH - RIGHT_MARGIN, HEIGHT - 90), fill=LINE, width=2)
    draw.rectangle((LEFT_MARGIN, TOP_MARGIN, LEFT_MARGIN + 18, TOP_MARGIN + 86), fill=ACCENT)

    draw.text((LEFT_MARGIN + 36, TOP_MARGIN), scene["title"], font=TITLE_FONT, fill=TEXT)
    draw.text((LEFT_MARGIN + 36, TOP_MARGIN + 78), scene["subtitle"], font=SUBTITLE_FONT, fill=ACCENT)

    y = TOP_MARGIN + 180
    max_w = WIDTH - LEFT_MARGIN - RIGHT_MARGIN - 40
    for bullet in scene["bullets"]:
        lines = wrap_text(draw, bullet, BODY_FONT, max_w)
        draw.text((LEFT_MARGIN + 34, y), "•", font=BODY_FONT, fill=ACCENT)
        ly = y
        for idx, line in enumerate(lines):
            draw.text((LEFT_MARGIN + (70 if idx == 0 else 88), ly), line, font=BODY_FONT, fill=TEXT)
            ly += 42
        y = ly + 16

    draw.text((LEFT_MARGIN, HEIGHT - 65), "Bastion UA • Teaser v2", font=FOOTER_FONT, fill=MUTED)
    return img


def render_silent_video(video_path: Path) -> float:
    total_seconds = sum(scene["duration"] for scene in SCENES)
    writer = imageio.get_writer(str(video_path), fps=FPS, codec="libx264", quality=8)
    try:
        for scene in SCENES:
            frame = np.asarray(render_scene(scene))
            for _ in range(scene["duration"] * FPS):
                writer.append_data(frame)
    finally:
        writer.close()
    return float(total_seconds)


def synth_music(wav_path: Path, duration_sec: float, sample_rate: int = 44100) -> None:
    t = np.linspace(0, duration_sec, int(sample_rate * duration_sec), endpoint=False)
    # Calm ambient triad progression
    melody = (
        0.35 * np.sin(2 * math.pi * 220 * t)
        + 0.22 * np.sin(2 * math.pi * 277.18 * t)
        + 0.18 * np.sin(2 * math.pi * 329.63 * t)
    )
    pad = 0.12 * np.sin(2 * math.pi * 110 * t)
    sweep = 0.05 * np.sin(2 * math.pi * (0.2 * t**2))
    signal = melody + pad + sweep
    # Soft attack/release envelope
    env = np.ones_like(signal)
    attack = int(0.8 * sample_rate)
    release = int(1.2 * sample_rate)
    env[:attack] = np.linspace(0.0, 1.0, attack)
    env[-release:] = np.linspace(1.0, 0.0, release)
    signal = signal * env
    signal = np.clip(signal, -1.0, 1.0)
    pcm = (signal * 32767).astype(np.int16)

    with wave.open(str(wav_path), "wb") as wf:
        wf.setnchannels(1)
        wf.setsampwidth(2)
        wf.setframerate(sample_rate)
        wf.writeframes(pcm.tobytes())


def build_voice_text() -> str:
    return " ".join(scene["voice"] for scene in SCENES)


async def synth_voice_mp3(mp3_path: Path, text: str) -> bool:
    if edge_tts is None:
        return False
    try:
        voice = edge_tts.Communicate(text=text, voice="uk-UA-PolinaNeural", rate="+0%")
        await voice.save(str(mp3_path))
        return True
    except Exception:
        return False


def mux_video_audio(silent_video: Path, music_wav: Path, output_mp4: Path, voice_mp3: Path | None) -> None:
    ffmpeg_bin = imageio_ffmpeg.get_ffmpeg_exe()
    if voice_mp3 is not None and voice_mp3.exists():
        cmd = [
            ffmpeg_bin,
            "-y",
            "-i",
            str(silent_video),
            "-i",
            str(music_wav),
            "-i",
            str(voice_mp3),
            "-filter_complex",
            "[1:a]volume=0.16[m];[2:a]volume=1.0[v];[m][v]amix=inputs=2:duration=first[a]",
            "-map",
            "0:v:0",
            "-map",
            "[a]",
            "-c:v",
            "copy",
            "-c:a",
            "aac",
            "-shortest",
            str(output_mp4),
        ]
    else:
        cmd = [
            ffmpeg_bin,
            "-y",
            "-i",
            str(silent_video),
            "-i",
            str(music_wav),
            "-map",
            "0:v:0",
            "-map",
            "1:a:0",
            "-c:v",
            "copy",
            "-c:a",
            "aac",
            "-shortest",
            str(output_mp4),
        ]
    subprocess.run(cmd, check=True, capture_output=True)


def main() -> None:
    work_dir = Path(__file__).resolve().parent
    silent_video = work_dir / VIDEO_SILENT_NAME
    final_video = work_dir / VIDEO_NAME
    voice_mp3 = work_dir / VOICE_NAME
    music_wav = work_dir / MUSIC_NAME

    total_seconds = render_silent_video(silent_video)
    synth_music(music_wav, duration_sec=total_seconds + 0.5)

    voice_ok = asyncio.run(synth_voice_mp3(voice_mp3, build_voice_text()))
    mux_video_audio(silent_video, music_wav, final_video, voice_mp3 if voice_ok else None)

    print(f"Video created: {final_video}")
    if voice_ok:
        print(f"Voice created: {voice_mp3}")
    else:
        print("Voice not available; exported with music only.")
    print(f"Music created: {music_wav}")


if __name__ == "__main__":
    main()
