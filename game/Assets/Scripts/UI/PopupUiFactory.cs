using BastionUA.Core;
using UnityEngine;
using UnityEngine.UI;

namespace BastionUA.UI
{
    public static class PopupUiFactory
    {
        public static Transform EnsureCanvas(string canvasName, int sortingOrder)
        {
            var existingCanvas = GameObject.Find(canvasName);
            if (existingCanvas != null)
            {
                return existingCanvas.transform;
            }

            var canvasObject = new GameObject(
                canvasName,
                typeof(Canvas),
                typeof(CanvasScaler),
                typeof(GraphicRaycaster));

            var canvas = canvasObject.GetComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.sortingOrder = sortingOrder;

            var scaler = canvasObject.GetComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(GameUiConstants.ReferenceWidth, GameUiConstants.ReferenceHeight);
            scaler.matchWidthOrHeight = 0.5f;

            return canvasObject.transform;
        }

        public static GameObject CreateOverlayRoot(Transform canvasTransform, string rootName)
        {
            var overlayRoot = new GameObject(rootName, typeof(RectTransform));
            overlayRoot.transform.SetParent(canvasTransform, false);
            StretchFullScreen(overlayRoot.GetComponent<RectTransform>());

            var overlayImage = overlayRoot.AddComponent<Image>();
            overlayImage.color = GameVisualPalette.EventOverlay;
            overlayImage.raycastTarget = true;

            return overlayRoot;
        }

        public static GameObject CreateStyledPanel(Transform parent, float width, float height)
        {
            var frameObject = new GameObject("PopupFrame", typeof(RectTransform), typeof(Image));
            frameObject.transform.SetParent(parent, false);

            var frameRect = frameObject.GetComponent<RectTransform>();
            frameRect.anchorMin = new Vector2(0.5f, 0.5f);
            frameRect.anchorMax = new Vector2(0.5f, 0.5f);
            frameRect.pivot = new Vector2(0.5f, 0.5f);
            frameRect.sizeDelta = new Vector2(width, height);
            frameObject.GetComponent<Image>().color = GameVisualPalette.PopupFrame;

            var panelObject = new GameObject("PopupPanel", typeof(RectTransform), typeof(Image));
            panelObject.transform.SetParent(frameObject.transform, false);
            StretchWithInset(panelObject.GetComponent<RectTransform>(), GameUiConstants.PopupFrameInset);
            panelObject.GetComponent<Image>().color = GameVisualPalette.EventPanel;

            CreatePanelAccentStripe(panelObject.transform);

            return panelObject;
        }

        public static Text CreateTitle(
            Transform parent,
            string name,
            string content,
            Vector2 anchoredPosition,
            Vector2 sizeDelta,
            Color color)
        {
            return CreateText(
                parent,
                name,
                content,
                anchoredPosition,
                sizeDelta,
                GameUiConstants.EventTitleFontSize,
                TextAnchor.UpperCenter,
                color,
                addShadow: true);
        }

        public static Text CreateBody(
            Transform parent,
            string name,
            string content,
            Vector2 anchoredPosition,
            Vector2 sizeDelta)
        {
            return CreateText(
                parent,
                name,
                content,
                anchoredPosition,
                sizeDelta,
                GameUiConstants.EventBodyFontSize,
                TextAnchor.UpperCenter,
                GameVisualPalette.TextPrimary,
                addShadow: false);
        }

        public static Button CreatePrimaryButton(Transform parent, string name, string label, Vector2 anchor)
        {
            return CreateFramedButton(
                parent,
                name,
                label,
                anchor,
                new Vector2(420f, 56f),
                GameVisualPalette.ButtonPrimary,
                GameVisualPalette.PopupFrame,
                GameVisualPalette.TextOnPrimaryButton);
        }

        public static Button CreateChoiceButton(Transform parent, string name, string label, Vector2 anchor)
        {
            return CreateFramedButton(
                parent,
                name,
                label,
                anchor,
                new Vector2(640f, 56f),
                GameVisualPalette.EventChoice,
                GameVisualPalette.ButtonNeutralBorder,
                GameVisualPalette.TextPrimary);
        }

        private static Button CreateFramedButton(
            Transform parent,
            string name,
            string label,
            Vector2 anchor,
            Vector2 size,
            Color fillColor,
            Color frameColor,
            Color labelColor)
        {
            var frameObject = new GameObject(name, typeof(RectTransform), typeof(Image), typeof(Button));
            frameObject.transform.SetParent(parent, false);

            var frameRect = frameObject.GetComponent<RectTransform>();
            frameRect.anchorMin = anchor;
            frameRect.anchorMax = anchor;
            frameRect.pivot = new Vector2(0.5f, 0.5f);
            frameRect.sizeDelta = size;
            frameObject.GetComponent<Image>().color = frameColor;

            var fillObject = new GameObject("Fill", typeof(RectTransform), typeof(Image));
            fillObject.transform.SetParent(frameObject.transform, false);
            StretchWithInset(fillObject.GetComponent<RectTransform>(), GameUiConstants.ButtonFrameInset);
            fillObject.GetComponent<Image>().color = fillColor;

            var button = frameObject.GetComponent<Button>();
            button.targetGraphic = fillObject.GetComponent<Image>();

            var labelObject = new GameObject("Label", typeof(RectTransform), typeof(Text));
            labelObject.transform.SetParent(frameObject.transform, false);
            StretchFullScreen(labelObject.GetComponent<RectTransform>());

            var labelText = labelObject.GetComponent<Text>();
            labelText.text = label;
            labelText.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            labelText.fontSize = GameUiConstants.BaseFontSize;
            labelText.color = labelColor;
            labelText.alignment = TextAnchor.MiddleCenter;

            return button;
        }

        private static void CreatePanelAccentStripe(Transform parent)
        {
            var stripeObject = new GameObject("PanelAccent", typeof(RectTransform), typeof(Image));
            stripeObject.transform.SetParent(parent, false);
            stripeObject.transform.SetAsFirstSibling();

            var rectTransform = stripeObject.GetComponent<RectTransform>();
            rectTransform.anchorMin = new Vector2(0f, 1f);
            rectTransform.anchorMax = new Vector2(1f, 1f);
            rectTransform.pivot = new Vector2(0.5f, 1f);
            rectTransform.sizeDelta = new Vector2(0f, GameUiConstants.PopupAccentStripeHeight);
            stripeObject.GetComponent<Image>().color = GameVisualPalette.AccentBlue;
        }

        private static Text CreateText(
            Transform parent,
            string name,
            string content,
            Vector2 anchoredPosition,
            Vector2 sizeDelta,
            int fontSize,
            TextAnchor alignment,
            Color color,
            bool addShadow)
        {
            var textObject = new GameObject(name, typeof(RectTransform), typeof(Text));
            textObject.transform.SetParent(parent, false);

            var rectTransform = textObject.GetComponent<RectTransform>();
            rectTransform.anchorMin = new Vector2(0.5f, 1f);
            rectTransform.anchorMax = new Vector2(0.5f, 1f);
            rectTransform.pivot = new Vector2(0.5f, 1f);
            rectTransform.anchoredPosition = anchoredPosition;
            rectTransform.sizeDelta = sizeDelta;

            var text = textObject.GetComponent<Text>();
            text.text = content;
            text.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            text.fontSize = fontSize;
            text.color = color;
            text.alignment = alignment;
            text.horizontalOverflow = HorizontalWrapMode.Wrap;
            text.verticalOverflow = VerticalWrapMode.Overflow;

            if (addShadow)
            {
                var outline = textObject.AddComponent<Outline>();
                outline.effectColor = new Color(0f, 0f, 0f, 0.65f);
                outline.effectDistance = new Vector2(1f, -1f);
            }

            return text;
        }

        private static void StretchFullScreen(RectTransform rectTransform)
        {
            rectTransform.anchorMin = Vector2.zero;
            rectTransform.anchorMax = Vector2.one;
            rectTransform.offsetMin = Vector2.zero;
            rectTransform.offsetMax = Vector2.zero;
        }

        private static void StretchWithInset(RectTransform rectTransform, float inset)
        {
            rectTransform.anchorMin = Vector2.zero;
            rectTransform.anchorMax = Vector2.one;
            rectTransform.offsetMin = new Vector2(inset, inset);
            rectTransform.offsetMax = new Vector2(-inset, -inset);
        }
    }
}
