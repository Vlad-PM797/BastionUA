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
            var panelObject = new GameObject("PopupPanel", typeof(RectTransform), typeof(Image));
            panelObject.transform.SetParent(parent, false);

            var panelRect = panelObject.GetComponent<RectTransform>();
            panelRect.anchorMin = new Vector2(0.5f, 0.5f);
            panelRect.anchorMax = new Vector2(0.5f, 0.5f);
            panelRect.pivot = new Vector2(0.5f, 0.5f);
            panelRect.sizeDelta = new Vector2(width, height);

            CreatePanelBorder(panelObject.transform);
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
                color);
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
                GameVisualPalette.TextObjective);
        }

        public static Button CreatePrimaryButton(Transform parent, string name, string label, Vector2 anchor)
        {
            return CreateButton(
                parent,
                name,
                label,
                anchor,
                new Vector2(420f, 56f),
                GameVisualPalette.ButtonPrimary,
                GameVisualPalette.ButtonPrimaryBorder,
                GameVisualPalette.TextAccent);
        }

        public static Button CreateChoiceButton(Transform parent, string name, string label, Vector2 anchor)
        {
            return CreateButton(
                parent,
                name,
                label,
                anchor,
                new Vector2(640f, 56f),
                GameVisualPalette.EventChoice,
                GameVisualPalette.ButtonNeutralBorder,
                GameVisualPalette.TextPrimary);
        }

        private static Button CreateButton(
            Transform parent,
            string name,
            string label,
            Vector2 anchor,
            Vector2 size,
            Color fillColor,
            Color borderColor,
            Color labelColor)
        {
            var buttonObject = new GameObject(name, typeof(RectTransform), typeof(Image), typeof(Button));
            buttonObject.transform.SetParent(parent, false);

            var rectTransform = buttonObject.GetComponent<RectTransform>();
            rectTransform.anchorMin = anchor;
            rectTransform.anchorMax = anchor;
            rectTransform.pivot = new Vector2(0.5f, 0.5f);
            rectTransform.sizeDelta = size;

            CreateButtonBorder(buttonObject.transform, borderColor);
            buttonObject.GetComponent<Image>().color = fillColor;

            var labelObject = new GameObject("Label", typeof(RectTransform), typeof(Text));
            labelObject.transform.SetParent(buttonObject.transform, false);
            StretchFullScreen(labelObject.GetComponent<RectTransform>());

            var labelText = labelObject.GetComponent<Text>();
            labelText.text = label;
            labelText.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            labelText.fontSize = GameUiConstants.BaseFontSize;
            labelText.color = labelColor;
            labelText.alignment = TextAnchor.MiddleCenter;

            return buttonObject.GetComponent<Button>();
        }

        private static void CreatePanelBorder(Transform parent)
        {
            var borderObject = new GameObject("PanelBorder", typeof(RectTransform), typeof(Image));
            borderObject.transform.SetParent(parent, false);
            borderObject.transform.SetAsFirstSibling();

            var rectTransform = borderObject.GetComponent<RectTransform>();
            rectTransform.anchorMin = Vector2.zero;
            rectTransform.anchorMax = Vector2.one;
            rectTransform.offsetMin = new Vector2(-GameUiConstants.PopupBorderExpand, -GameUiConstants.PopupBorderExpand);
            rectTransform.offsetMax = new Vector2(GameUiConstants.PopupBorderExpand, GameUiConstants.PopupBorderExpand);
            borderObject.GetComponent<Image>().color = GameVisualPalette.ButtonPrimaryBorder;
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

        private static void CreateButtonBorder(Transform parent, Color borderColor)
        {
            var borderObject = new GameObject("Border", typeof(RectTransform), typeof(Image));
            borderObject.transform.SetParent(parent, false);
            borderObject.transform.SetAsFirstSibling();

            var rectTransform = borderObject.GetComponent<RectTransform>();
            rectTransform.anchorMin = Vector2.zero;
            rectTransform.anchorMax = Vector2.one;
            rectTransform.offsetMin = new Vector2(-2f, -2f);
            rectTransform.offsetMax = new Vector2(2f, 2f);
            borderObject.GetComponent<Image>().color = borderColor;
        }

        private static Text CreateText(
            Transform parent,
            string name,
            string content,
            Vector2 anchoredPosition,
            Vector2 sizeDelta,
            int fontSize,
            TextAnchor alignment,
            Color color)
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
            return text;
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
