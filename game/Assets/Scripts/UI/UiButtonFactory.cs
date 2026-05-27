using System;
using BastionUA.Core;
using UnityEngine;
using UnityEngine.UI;

namespace BastionUA.UI
{
    public sealed class UiFramedButtonResult
    {
        public GameObject Root { get; set; }
        public Button Button { get; set; }
        public Image FillImage { get; set; }
    }

    public static class UiButtonFactory
    {
        public static UiFramedButtonResult CreateFramedButton(
            Transform parent,
            string name,
            string label,
            Vector2 anchor,
            Vector2 size,
            Action onClick,
            bool isPrimary,
            bool enablePressScale = false)
        {
            var frameColor = isPrimary
                ? GameVisualPalette.ButtonPrimaryBorder
                : GameVisualPalette.ButtonNeutralBorder;
            var fillColor = isPrimary
                ? GameVisualPalette.ButtonPrimary
                : GameVisualPalette.ButtonNeutral;
            var labelColor = isPrimary
                ? GameVisualPalette.TextOnPrimaryButton
                : GameVisualPalette.TextPrimary;

            var frameObject = new GameObject(name, typeof(RectTransform), typeof(Image), typeof(Button));
            frameObject.transform.SetParent(parent, false);

            var frameRect = frameObject.GetComponent<RectTransform>();
            frameRect.anchorMin = anchor;
            frameRect.anchorMax = anchor;
            frameRect.pivot = new Vector2(0.5f, 0.5f);
            frameRect.sizeDelta = size;
            frameObject.GetComponent<Image>().color = frameColor;
            frameObject.GetComponent<Image>().raycastTarget = true;

            var fillObject = new GameObject("Fill", typeof(RectTransform), typeof(Image));
            fillObject.transform.SetParent(frameObject.transform, false);
            var fillRect = fillObject.GetComponent<RectTransform>();
            fillRect.anchorMin = Vector2.zero;
            fillRect.anchorMax = Vector2.one;
            var inset = GameUiConstants.ButtonFrameInset;
            fillRect.offsetMin = new Vector2(inset, inset);
            fillRect.offsetMax = new Vector2(-inset, -inset);
            var fillImage = fillObject.GetComponent<Image>();
            fillImage.color = fillColor;

            var button = frameObject.GetComponent<Button>();
            button.targetGraphic = fillImage;
            ApplyColorBlock(button, fillColor);
            button.onClick.AddListener(() => onClick());

            var labelObject = new GameObject("Label", typeof(RectTransform), typeof(Text));
            labelObject.transform.SetParent(frameObject.transform, false);
            StretchFullScreen(labelObject.GetComponent<RectTransform>());
            var labelText = labelObject.GetComponent<Text>();
            labelText.text = label;
            labelText.font = UiFontLoader.GetBodyFont();
            labelText.fontSize = GameUiConstants.BaseFontSize;
            labelText.color = labelColor;
            labelText.alignment = TextAnchor.MiddleCenter;
            labelText.raycastTarget = false;

            if (enablePressScale)
            {
                var pressScale = frameObject.AddComponent<UiPressScaleBehavior>();
                pressScale.Configure(GameUiConstants.ButtonPressedScale);
            }

            return new UiFramedButtonResult
            {
                Root = frameObject,
                Button = button,
                FillImage = fillImage,
            };
        }

        public static Button CreateCompactButton(
            Transform parent,
            string name,
            string label,
            Vector2 anchor,
            Vector2 size,
            Action onClick,
            Color? fillColorOverride = null)
        {
            var fillColor = fillColorOverride ?? GameVisualPalette.ButtonNeutral;
            var buttonObject = new GameObject(name, typeof(RectTransform), typeof(Image), typeof(Button));
            buttonObject.transform.SetParent(parent, false);

            var rectTransform = buttonObject.GetComponent<RectTransform>();
            rectTransform.anchorMin = anchor;
            rectTransform.anchorMax = anchor;
            rectTransform.pivot = new Vector2(0.5f, 0.5f);
            rectTransform.sizeDelta = size;

            var image = buttonObject.GetComponent<Image>();
            image.color = fillColor;

            var button = buttonObject.GetComponent<Button>();
            button.targetGraphic = image;
            ApplyColorBlock(button, fillColor);
            button.onClick.AddListener(() => onClick());

            var labelObject = new GameObject("Label", typeof(RectTransform), typeof(Text));
            labelObject.transform.SetParent(buttonObject.transform, false);
            StretchFullScreen(labelObject.GetComponent<RectTransform>());
            var labelText = labelObject.GetComponent<Text>();
            labelText.text = label;
            labelText.font = UiFontLoader.GetBodyFont();
            labelText.fontSize = GameUiConstants.CompactFontSize;
            labelText.color = GameVisualPalette.TextPrimary;
            labelText.alignment = TextAnchor.MiddleCenter;
            labelText.raycastTarget = false;

            return button;
        }

        public static void ApplyColorBlock(Button button, Color normalColor)
        {
            var colors = button.colors;
            colors.normalColor = normalColor;
            colors.highlightedColor = BlendColor(normalColor, Color.white, GameUiConstants.ButtonHighlightBlend);
            colors.pressedColor = BlendColor(normalColor, Color.black, GameUiConstants.ButtonPressedBlend);
            colors.selectedColor = colors.highlightedColor;
            colors.disabledColor = BlendColor(normalColor, Color.gray, 0.35f);
            colors.colorMultiplier = 1f;
            colors.fadeDuration = 0.08f;
            button.colors = colors;
        }

        private static Color BlendColor(Color baseColor, Color blendColor, float amount)
        {
            return Color.Lerp(baseColor, blendColor, amount);
        }

        private static void StretchFullScreen(RectTransform rectTransform)
        {
            rectTransform.anchorMin = Vector2.zero;
            rectTransform.anchorMax = Vector2.one;
            rectTransform.offsetMin = Vector2.zero;
            rectTransform.offsetMax = Vector2.zero;
        }
    }
}
