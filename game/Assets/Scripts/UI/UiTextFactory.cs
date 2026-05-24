using BastionUA.Core;
using UnityEngine;
using UnityEngine.UI;

namespace BastionUA.UI
{
    public enum UiTextStyle
    {
        StatBar,
        StatBarAccent,
        Title,
        SectionTitle,
        Body,
        Muted,
        Objective,
        Accent,
    }

    public static class UiTextFactory
    {
        private const string BuiltinFontResource = "LegacyRuntime.ttf";
        private const int ObjectiveMaxCharacters = 96;
        private const float TextShadowAlpha = 0.55f;

        public static string FormatStatLine(string label, string value, bool accentValue = true)
        {
            var labelColor = ColorToHex(GameVisualPalette.TextMuted);
            if (!accentValue)
            {
                return $"<color={labelColor}>{label}</color>: {value}";
            }

            var valueColor = ColorToHex(GameVisualPalette.TextAccent);
            return $"<color={labelColor}>{label}</color>: <color={valueColor}>{value}</color>";
        }

        public static string FormatObjectiveLine(string hint)
        {
            var labelColor = ColorToHex(GameVisualPalette.AccentBlueLight);
            var bodyColor = ColorToHex(GameVisualPalette.TextObjective);
            var trimmedHint = Truncate(hint, ObjectiveMaxCharacters);
            return $"<color={labelColor}>{GameUiConstants.LabelObjective}</color>: <color={bodyColor}>{trimmedHint}</color>";
        }

        public static Text Create(
            Transform parent,
            string name,
            Vector2 anchor,
            Vector2 sizeDelta,
            UiTextStyle style,
            string content,
            TextAnchor alignment = TextAnchor.MiddleLeft)
        {
            var textObject = new GameObject(name, typeof(RectTransform), typeof(Text));
            textObject.transform.SetParent(parent, false);

            var rectTransform = textObject.GetComponent<RectTransform>();
            rectTransform.anchorMin = anchor;
            rectTransform.anchorMax = anchor;
            rectTransform.pivot = new Vector2(0f, 0.5f);
            rectTransform.anchoredPosition = Vector2.zero;
            rectTransform.sizeDelta = sizeDelta;

            var text = textObject.GetComponent<Text>();
            ApplyStyle(text, style, content, alignment);
            return text;
        }

        public static Text CreateMultiline(
            Transform parent,
            string name,
            Vector2 anchorMin,
            Vector2 anchorMax,
            UiTextStyle style,
            string content)
        {
            var textObject = new GameObject(name, typeof(RectTransform), typeof(Text));
            textObject.transform.SetParent(parent, false);

            var rectTransform = textObject.GetComponent<RectTransform>();
            rectTransform.anchorMin = anchorMin;
            rectTransform.anchorMax = anchorMax;
            rectTransform.offsetMin = Vector2.zero;
            rectTransform.offsetMax = Vector2.zero;

            var text = textObject.GetComponent<Text>();
            ApplyStyle(text, style, content, TextAnchor.UpperLeft);
            text.horizontalOverflow = HorizontalWrapMode.Wrap;
            text.verticalOverflow = VerticalWrapMode.Truncate;
            return text;
        }

        public static void ApplyStyle(Text text, UiTextStyle style, string content, TextAnchor alignment)
        {
            text.text = content;
            text.font = Resources.GetBuiltinResource<Font>(BuiltinFontResource);
            text.alignment = alignment;
            text.horizontalOverflow = HorizontalWrapMode.Overflow;
            text.verticalOverflow = VerticalWrapMode.Overflow;
            text.supportRichText = style == UiTextStyle.StatBar ||
                                   style == UiTextStyle.StatBarAccent ||
                                   style == UiTextStyle.Objective;

            switch (style)
            {
                case UiTextStyle.StatBar:
                    text.fontSize = GameUiConstants.StatValueFontSize;
                    text.color = GameVisualPalette.TextPrimary;
                    break;
                case UiTextStyle.StatBarAccent:
                    text.fontSize = GameUiConstants.StatValueFontSize;
                    text.color = GameVisualPalette.TextPrimary;
                    break;
                case UiTextStyle.Title:
                    text.fontSize = GameUiConstants.TitleFontSize;
                    text.color = GameVisualPalette.TextAccent;
                    AddShadow(text);
                    break;
                case UiTextStyle.SectionTitle:
                    text.fontSize = GameUiConstants.SectionTitleFontSize;
                    text.color = GameVisualPalette.AccentBlueLight;
                    break;
                case UiTextStyle.Body:
                    text.fontSize = GameUiConstants.BaseFontSize;
                    text.color = GameVisualPalette.TextPrimary;
                    break;
                case UiTextStyle.Muted:
                    text.fontSize = GameUiConstants.CompactFontSize;
                    text.color = GameVisualPalette.TextMuted;
                    break;
                case UiTextStyle.Objective:
                    text.fontSize = GameUiConstants.ObjectiveFontSize;
                    text.color = GameVisualPalette.TextObjective;
                    break;
                case UiTextStyle.Accent:
                    text.fontSize = GameUiConstants.BaseFontSize;
                    text.color = GameVisualPalette.TextAccent;
                    break;
            }
        }

        public static void CreateSectionUnderline(Transform parent, float anchorY)
        {
            var lineObject = new GameObject("SectionUnderline", typeof(RectTransform), typeof(Image));
            lineObject.transform.SetParent(parent, false);

            var rectTransform = lineObject.GetComponent<RectTransform>();
            rectTransform.anchorMin = new Vector2(0.5f, anchorY);
            rectTransform.anchorMax = new Vector2(0.5f, anchorY);
            rectTransform.pivot = new Vector2(0.5f, 1f);
            rectTransform.anchoredPosition = new Vector2(0f, -22f);
            rectTransform.sizeDelta = new Vector2(220f, 2f);
            lineObject.GetComponent<Image>().color = GameVisualPalette.AccentYellow;
            lineObject.GetComponent<Image>().raycastTarget = false;
        }

        public static void CreateObjectiveMarker(Transform parent)
        {
            var markerObject = new GameObject("ObjectiveMarker", typeof(RectTransform), typeof(Image));
            markerObject.transform.SetParent(parent, false);

            var rectTransform = markerObject.GetComponent<RectTransform>();
            rectTransform.anchorMin = new Vector2(0f, 0.5f);
            rectTransform.anchorMax = new Vector2(0f, 0.5f);
            rectTransform.pivot = new Vector2(0.5f, 0.5f);
            rectTransform.anchoredPosition = new Vector2(18f, 0f);
            rectTransform.sizeDelta = new Vector2(10f, 10f);
            markerObject.GetComponent<Image>().color = GameVisualPalette.AccentYellow;
            markerObject.GetComponent<Image>().raycastTarget = false;
        }

        public static void AddDropShadow(Text text)
        {
            AddShadow(text);
        }

        private static void AddShadow(Text text)
        {
            var shadow = text.gameObject.AddComponent<Shadow>();
            shadow.effectColor = new Color(0f, 0f, 0f, TextShadowAlpha);
            shadow.effectDistance = new Vector2(1f, -1f);
        }

        private static string ColorToHex(Color color)
        {
            return "#" + ColorUtility.ToHtmlStringRGB(color);
        }

        private static string Truncate(string value, int maxLength)
        {
            if (string.IsNullOrEmpty(value) || value.Length <= maxLength)
            {
                return value;
            }

            return value.Substring(0, maxLength - 1) + "…";
        }
    }
}
