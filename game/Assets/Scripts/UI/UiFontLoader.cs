using BastionUA.Core;
using UnityEngine;

namespace BastionUA.UI
{
    public static class UiFontLoader
    {
        private const string DisplayFontResourcePath = GameConstants.UiDisplayFontResourcePath;
        private const string BodyFontResourcePath = GameConstants.UiDisplayFontResourcePath;
        private const string FallbackFontResource = "LegacyRuntime.ttf";

        private static Font _displayFont;
        private static Font _bodyFont;
        private static Font _fallbackFont;

        public static Font GetDisplayFont()
        {
            if (_displayFont == null)
            {
                _displayFont = LoadFont(DisplayFontResourcePath) ?? GetFallbackFont();
            }

            return _displayFont;
        }

        public static Font GetBodyFont()
        {
            if (_bodyFont == null)
            {
                _bodyFont = LoadFont(BodyFontResourcePath) ?? GetFallbackFont();
            }

            return _bodyFont;
        }

        public static Font ResolveForStyle(UiTextStyle style)
        {
            switch (style)
            {
                case UiTextStyle.Title:
                case UiTextStyle.BrandTitle:
                case UiTextStyle.BrandSubtitle:
                case UiTextStyle.SectionTitle:
                case UiTextStyle.Accent:
                    return GetDisplayFont();
                default:
                    return GetBodyFont();
            }
        }

        public static Font ResolveForPopupTitle()
        {
            return GetDisplayFont();
        }

        public static Font ResolveForPopupBody()
        {
            return GetBodyFont();
        }

        private static Font GetFallbackFont()
        {
            if (_fallbackFont == null)
            {
                _fallbackFont = Resources.GetBuiltinResource<Font>(FallbackFontResource);
            }

            return _fallbackFont;
        }

        private static Font LoadFont(string resourcePath)
        {
            try
            {
                var font = Resources.Load<Font>(resourcePath);
                if (font == null)
                {
                    Debug.LogWarning($"[UiFontLoader] Font missing at Resources/{resourcePath}. Using fallback.");
                }

                return font;
            }
            catch (System.Exception exception)
            {
                Debug.LogError($"[UiFontLoader] Failed to load font '{resourcePath}': {exception}");
                return null;
            }
        }
    }
}
