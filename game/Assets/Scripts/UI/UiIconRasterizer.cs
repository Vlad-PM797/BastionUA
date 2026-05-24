using BastionUA.Core;
using UnityEngine;

namespace BastionUA.UI
{
    public static class UiIconRasterizer
    {
        private const int IconSize = 64;
        private const float IconPixelsPerUnit = 64f;

        public static Sprite CreateIcon(UiIconKind kind)
        {
            var pixels = new Color32[IconSize * IconSize];
            var transparent = new Color32(0, 0, 0, 0);
            for (var index = 0; index < pixels.Length; index++)
            {
                pixels[index] = transparent;
            }

            switch (kind)
            {
                case UiIconKind.Ammo:
                    DrawAmmoIcon(pixels);
                    break;
                case UiIconKind.Morale:
                    DrawMoraleIcon(pixels);
                    break;
                case UiIconKind.Battle:
                    DrawBattleIcon(pixels);
                    break;
            }

            var texture = new Texture2D(IconSize, IconSize, TextureFormat.RGBA32, false);
            texture.filterMode = FilterMode.Bilinear;
            texture.wrapMode = TextureWrapMode.Clamp;
            texture.SetPixels32(pixels);
            texture.Apply();

            return Sprite.Create(
                texture,
                new Rect(0f, 0f, IconSize, IconSize),
                new Vector2(0.5f, 0.5f),
                IconPixelsPerUnit);
        }

        private static void DrawAmmoIcon(Color32[] pixels)
        {
            FillCircle(pixels, 32f, 32f, 24f, ToColor32(GameVisualPalette.AccentYellow));
            FillRect(pixels, 24, 18, 16, 28, ToColor32(GameVisualPalette.ButtonPrimary));
            FillRect(pixels, 28, 22, 8, 20, ToColor32(GameVisualPalette.TextPrimary));
        }

        private static void DrawMoraleIcon(Color32[] pixels)
        {
            FillCircle(pixels, 32f, 34f, 22f, ToColor32(GameVisualPalette.StatusSafe));
            FillCircle(pixels, 32f, 34f, 14f, ToColor32(GameVisualPalette.MapFillNorth));
            FillRect(pixels, 30, 14, 4, 12, ToColor32(GameVisualPalette.TextPrimary));
        }

        private static void DrawBattleIcon(Color32[] pixels)
        {
            FillCircle(pixels, 32f, 32f, 24f, ToColor32(GameVisualPalette.ButtonPrimaryBorder));
            FillCircle(pixels, 32f, 32f, 18f, ToColor32(GameVisualPalette.ButtonPrimary));
            DrawLine(pixels, 18, 18, 46, 46, 4, ToColor32(GameVisualPalette.TextPrimary));
            DrawLine(pixels, 46, 18, 18, 46, 4, ToColor32(GameVisualPalette.TextPrimary));
        }

        private static void FillCircle(Color32[] pixels, float centerX, float centerY, float radius, Color32 color)
        {
            var radiusSquared = radius * radius;
            for (var y = 0; y < IconSize; y++)
            {
                for (var x = 0; x < IconSize; x++)
                {
                    var dx = x + 0.5f - centerX;
                    var dy = y + 0.5f - centerY;
                    if ((dx * dx) + (dy * dy) <= radiusSquared)
                    {
                        pixels[(y * IconSize) + x] = color;
                    }
                }
            }
        }

        private static void FillRect(Color32[] pixels, int xMin, int yMin, int width, int height, Color32 color)
        {
            for (var y = yMin; y < yMin + height; y++)
            {
                for (var x = xMin; x < xMin + width; x++)
                {
                    if (x < 0 || y < 0 || x >= IconSize || y >= IconSize)
                    {
                        continue;
                    }

                    pixels[(y * IconSize) + x] = color;
                }
            }
        }

        private static void DrawLine(
            Color32[] pixels,
            int x0,
            int y0,
            int x1,
            int y1,
            int thickness,
            Color32 color)
        {
            var distance = Mathf.Max(Mathf.Abs(x1 - x0), Mathf.Abs(y1 - y0));
            for (var step = 0; step <= distance; step++)
            {
                var t = distance <= 0 ? 0f : step / (float)distance;
                var x = Mathf.RoundToInt(Mathf.Lerp(x0, x1, t));
                var y = Mathf.RoundToInt(Mathf.Lerp(y0, y1, t));
                FillRect(pixels, x - thickness / 2, y - thickness / 2, thickness, thickness, color);
            }
        }

        private static Color32 ToColor32(Color color)
        {
            return color;
        }
    }
}
