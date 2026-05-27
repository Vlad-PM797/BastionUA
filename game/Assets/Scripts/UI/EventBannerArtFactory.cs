using System.Collections.Generic;
using BastionUA.Core;
using UnityEngine;

namespace BastionUA.UI
{
    public readonly struct EventBannerPalette
    {
        public readonly Color Left;
        public readonly Color Right;
        public readonly Color Accent;

        public EventBannerPalette(Color left, Color right, Color accent)
        {
            Left = left;
            Right = right;
            Accent = accent;
        }
    }

    public static class EventBannerArtFactory
    {
        private const int BannerWidth = 720;
        private const int BannerHeight = 96;

        private static readonly Dictionary<string, Sprite> BannerCache = new Dictionary<string, Sprite>();

        public static Sprite GetBannerSprite(string eventId)
        {
            var key = string.IsNullOrWhiteSpace(eventId) ? "default" : eventId;
            if (BannerCache.TryGetValue(key, out var cachedSprite))
            {
                return cachedSprite;
            }

            var palette = ResolvePalette(key);
            var sprite = BuildBannerSprite(palette);
            BannerCache[key] = sprite;
            return sprite;
        }

        private static EventBannerPalette ResolvePalette(string eventId)
        {
            switch (eventId)
            {
                case "hostomel":
                    return new EventBannerPalette(Hex("#0B1E36"), Hex("#1A4570"), GameVisualPalette.AccentYellow);
                case "chornobaivka":
                    return new EventBannerPalette(Hex("#2E2618"), Hex("#5A4A22"), Hex("#D4A24A"));
                case "irpin":
                    return new EventBannerPalette(Hex("#1A2E28"), Hex("#355748"), Hex("#8FD0A8"));
                case "kharkiv":
                    return new EventBannerPalette(Hex("#301018"), Hex("#6A2028"), Hex("#FF8A70"));
                case "chernihiv":
                    return new EventBannerPalette(Hex("#122438"), Hex("#2A5578"), Hex("#9AD0FF"));
                default:
                    return new EventBannerPalette(Hex("#101923"), Hex("#1A3550"), GameVisualPalette.AccentBlue);
            }
        }

        private static Sprite BuildBannerSprite(EventBannerPalette palette)
        {
            var texture = new Texture2D(BannerWidth, BannerHeight, TextureFormat.RGBA32, false);
            texture.wrapMode = TextureWrapMode.Clamp;
            texture.filterMode = FilterMode.Bilinear;

            for (var y = 0; y < BannerHeight; y++)
            {
                var verticalT = y / (float)(BannerHeight - 1);
                for (var x = 0; x < BannerWidth; x++)
                {
                    var horizontalT = x / (float)(BannerWidth - 1);
                    var baseColor = Color.Lerp(palette.Left, palette.Right, horizontalT);
                    baseColor = Color.Lerp(baseColor, Color.black, verticalT * 0.22f);

                    var diagonal = (horizontalT + (1f - verticalT)) * 0.5f;
                    if (diagonal > 0.72f)
                    {
                        var accentBlend = Mathf.InverseLerp(0.72f, 0.95f, diagonal);
                        baseColor = Color.Lerp(baseColor, palette.Accent, accentBlend * 0.35f);
                    }

                    if (y < 4)
                    {
                        baseColor = Color.Lerp(baseColor, palette.Accent, 0.55f);
                    }

                    texture.SetPixel(x, y, baseColor);
                }
            }

            texture.Apply();
            return Sprite.Create(
                texture,
                new Rect(0f, 0f, BannerWidth, BannerHeight),
                new Vector2(0.5f, 0.5f),
                100f);
        }

        private static Color Hex(string hex)
        {
            if (ColorUtility.TryParseHtmlString(hex, out var color))
            {
                return color;
            }

            return Color.white;
        }
    }
}
