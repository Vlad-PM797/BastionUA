using System;
using BastionUA.Core;
using UnityEngine;
using UnityEngine.UI;

namespace BastionUA.UI
{
    public static class UiSidebarFactory
    {
        public static RectTransform CreateInsetPanel(
            Transform parent,
            string name,
            Vector2 anchorMin,
            Vector2 anchorMax,
            Color backgroundColor,
            bool clipChildren = false)
        {
            var panelObject = new GameObject(name, typeof(RectTransform), typeof(Image));
            panelObject.transform.SetParent(parent, false);

            var rectTransform = panelObject.GetComponent<RectTransform>();
            rectTransform.anchorMin = anchorMin;
            rectTransform.anchorMax = anchorMax;
            rectTransform.offsetMin = Vector2.zero;
            rectTransform.offsetMax = Vector2.zero;

            var image = panelObject.GetComponent<Image>();
            image.color = backgroundColor;
            image.raycastTarget = false;

            if (clipChildren)
            {
                panelObject.AddComponent<RectMask2D>();
            }

            return rectTransform;
        }

        public static Text CreateEventLogText(Transform logPanel)
        {
            var textObject = new GameObject("EventLogText", typeof(RectTransform), typeof(Text));
            textObject.transform.SetParent(logPanel, false);

            var rectTransform = textObject.GetComponent<RectTransform>();
            rectTransform.anchorMin = Vector2.zero;
            rectTransform.anchorMax = Vector2.one;
            rectTransform.offsetMin = new Vector2(10f, 8f);
            rectTransform.offsetMax = new Vector2(-10f, -8f);

            var text = textObject.GetComponent<Text>();
            UiTextFactory.ApplyStyle(text, UiTextStyle.Muted, "--", TextAnchor.UpperLeft);
            text.fontSize = 14;
            text.horizontalOverflow = HorizontalWrapMode.Wrap;
            text.verticalOverflow = VerticalWrapMode.Truncate;
            return text;
        }

        public static Text CreateUpgradeRow(
            Transform parent,
            string upgradeId,
            float anchorY,
            Action onPurchase)
        {
            var rowObject = new GameObject($"UpgradeRow_{upgradeId}", typeof(RectTransform), typeof(Image));
            rowObject.transform.SetParent(parent, false);

            var rowRect = rowObject.GetComponent<RectTransform>();
            rowRect.anchorMin = new Vector2(0.5f, anchorY);
            rowRect.anchorMax = new Vector2(0.5f, anchorY);
            rowRect.pivot = new Vector2(0.5f, 0.5f);
            rowRect.sizeDelta = new Vector2(268f, GameUiConstants.SidebarUpgradeRowHeight);
            rowObject.GetComponent<Image>().color = GameVisualPalette.SidebarUpgradeRow;
            rowObject.GetComponent<Image>().raycastTarget = false;

            var labelObject = new GameObject("Label", typeof(RectTransform), typeof(Text));
            labelObject.transform.SetParent(rowObject.transform, false);
            var labelRect = labelObject.GetComponent<RectTransform>();
            labelRect.anchorMin = new Vector2(0f, 0f);
            labelRect.anchorMax = new Vector2(1f, 1f);
            labelRect.offsetMin = new Vector2(10f, 0f);
            labelRect.offsetMax = new Vector2(-58f, 0f);

            var labelText = labelObject.GetComponent<Text>();
            UiTextFactory.ApplyStyle(labelText, UiTextStyle.Muted, "--", TextAnchor.MiddleLeft);

            CreatePurchaseButton(rowObject.transform, $"UpgradeBtn_{upgradeId}", onPurchase);
            return labelText;
        }

        public static void CreateFooterResetButton(Transform parent, Action onReset)
        {
            var footerPanel = CreateInsetPanel(
                parent,
                "SidebarFooter",
                new Vector2(0.06f, GameUiConstants.SidebarFooterBottomAnchor),
                new Vector2(0.94f, GameUiConstants.SidebarFooterTopAnchor),
                GameVisualPalette.SidebarFooter);

            UiButtonFactory.CreateFramedButton(
                footerPanel,
                "ResetButton",
                GameUiConstants.ButtonResetSave,
                new Vector2(0.5f, 0.5f),
                new Vector2(GameUiConstants.SidebarResetButtonWidth, GameUiConstants.SidebarResetButtonHeight),
                onReset,
                isPrimary: false,
                enablePressScale: false);
        }

        private static void CreatePurchaseButton(Transform parent, string name, Action onClick)
        {
            var frameObject = new GameObject(name, typeof(RectTransform), typeof(Image), typeof(Button));
            frameObject.transform.SetParent(parent, false);

            var frameRect = frameObject.GetComponent<RectTransform>();
            frameRect.anchorMin = new Vector2(1f, 0.5f);
            frameRect.anchorMax = new Vector2(1f, 0.5f);
            frameRect.pivot = new Vector2(1f, 0.5f);
            frameRect.anchoredPosition = new Vector2(-6f, 0f);
            frameRect.sizeDelta = new Vector2(
                GameUiConstants.SidebarPurchaseButtonWidth,
                GameUiConstants.SidebarUpgradeRowHeight - 4f);
            frameObject.GetComponent<Image>().color = GameVisualPalette.AccentYellow;

            var fillObject = new GameObject("Fill", typeof(RectTransform), typeof(Image));
            fillObject.transform.SetParent(frameObject.transform, false);
            var fillRect = fillObject.GetComponent<RectTransform>();
            fillRect.anchorMin = Vector2.zero;
            fillRect.anchorMax = Vector2.one;
            var inset = GameUiConstants.ButtonFrameInset;
            fillRect.offsetMin = new Vector2(inset, inset);
            fillRect.offsetMax = new Vector2(-inset, -inset);
            var fillImage = fillObject.GetComponent<Image>();
            fillImage.color = GameVisualPalette.ButtonNeutral;

            var button = frameObject.GetComponent<Button>();
            button.targetGraphic = fillImage;
            UiButtonFactory.ApplyColorBlock(button, fillImage.color);
            button.onClick.AddListener(() => onClick());

            var labelObject = new GameObject("Label", typeof(RectTransform), typeof(Text));
            labelObject.transform.SetParent(frameObject.transform, false);
            Stretch(labelObject.GetComponent<RectTransform>());
            var labelText = labelObject.GetComponent<Text>();
            labelText.text = "+";
            labelText.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            labelText.fontSize = GameUiConstants.TitleFontSize;
            labelText.color = GameVisualPalette.TextAccent;
            labelText.alignment = TextAnchor.MiddleCenter;
            labelText.raycastTarget = false;
        }

        private static void Stretch(RectTransform rectTransform)
        {
            rectTransform.anchorMin = Vector2.zero;
            rectTransform.anchorMax = Vector2.one;
            rectTransform.offsetMin = Vector2.zero;
            rectTransform.offsetMax = Vector2.zero;
        }
    }
}
