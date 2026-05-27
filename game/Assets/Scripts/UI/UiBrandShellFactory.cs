using BastionUA.Core;
using UnityEngine;
using UnityEngine.UI;

namespace BastionUA.UI
{
    public static class UiBrandShellFactory
    {
        public static void BuildCanvasAtmosphere(Transform canvasTransform)
        {
            var backgroundObject = new GameObject("CanvasBackground", typeof(RectTransform), typeof(Image));
            backgroundObject.transform.SetParent(canvasTransform, false);
            backgroundObject.transform.SetAsFirstSibling();

            StretchFullScreen(backgroundObject.GetComponent<RectTransform>());
            var backgroundImage = backgroundObject.GetComponent<Image>();
            backgroundImage.sprite = UiProceduralSpriteFactory.GetCanvasGradientSprite();
            backgroundImage.type = Image.Type.Simple;
            backgroundImage.color = Color.white;
            backgroundImage.raycastTarget = false;

            var glowObject = new GameObject("CanvasAtmosphereGlow", typeof(RectTransform), typeof(Image));
            glowObject.transform.SetParent(canvasTransform, false);
            glowObject.transform.SetSiblingIndex(1);
            StretchFullScreen(glowObject.GetComponent<RectTransform>());

            var glowImage = glowObject.GetComponent<Image>();
            glowImage.color = GameVisualPalette.AtmosphereBlueGlow;
            glowImage.raycastTarget = false;

            var atmosphere = canvasTransform.gameObject.AddComponent<UiCanvasAtmosphere>();
            atmosphere.BindGlowOverlay(glowImage);
        }

        public static void ApplyTopBarBrand(Transform topBarTransform)
        {
            CreateDualFlagStripe(topBarTransform, "TopBarStripeBlue", GameVisualPalette.AccentBlue, 0f);
            CreateDualFlagStripe(
                topBarTransform,
                "TopBarStripeYellow",
                GameVisualPalette.AccentYellow,
                GameUiConstants.BrandFlagBlueStripeHeight);

            var brandRoot = new GameObject("BrandLockup", typeof(RectTransform));
            brandRoot.transform.SetParent(topBarTransform, false);

            var brandRect = brandRoot.GetComponent<RectTransform>();
            brandRect.anchorMin = new Vector2(0f, 0.5f);
            brandRect.anchorMax = new Vector2(0f, 0.5f);
            brandRect.pivot = new Vector2(0f, 0.5f);
            brandRect.anchoredPosition = new Vector2(GameUiConstants.BrandLockupLeftPadding, 0f);
            brandRect.sizeDelta = new Vector2(GameUiConstants.BrandLockupWidth, GameUiConstants.TopBarHeight - 8f);

            UiTextFactory.Create(
                brandRoot.transform,
                "BrandTitle",
                new Vector2(0f, 0.62f),
                new Vector2(GameUiConstants.BrandLockupWidth, 30f),
                UiTextStyle.BrandTitle,
                GameUiConstants.BrandTitle,
                TextAnchor.MiddleLeft);

            UiTextFactory.Create(
                brandRoot.transform,
                "BrandSubtitle",
                new Vector2(0f, 0.28f),
                new Vector2(GameUiConstants.BrandLockupWidth, 18f),
                UiTextStyle.BrandSubtitle,
                GameUiConstants.BrandSubtitle,
                TextAnchor.MiddleLeft);
        }

        public static void ApplyHudCorners(Transform panelTransform)
        {
            var cornerSprite = UiProceduralSpriteFactory.GetHudCornerSprite();
            var size = GameUiConstants.HudCornerDisplaySize;
            CreateCorner(panelTransform, "HudCornerTopLeft", cornerSprite, size, new Vector2(0f, 1f), new Vector2(0f, 0f), 0f);
            CreateCorner(panelTransform, "HudCornerTopRight", cornerSprite, size, new Vector2(1f, 1f), new Vector2(1f, 0f), 90f);
            CreateCorner(panelTransform, "HudCornerBottomLeft", cornerSprite, size, new Vector2(0f, 0f), new Vector2(0f, 1f), 270f);
            CreateCorner(panelTransform, "HudCornerBottomRight", cornerSprite, size, new Vector2(1f, 0f), new Vector2(1f, 1f), 180f);
        }

        private static void CreateDualFlagStripe(Transform parent, string name, Color color, float bottomOffset)
        {
            var stripeObject = new GameObject(name, typeof(RectTransform), typeof(Image));
            stripeObject.transform.SetParent(parent, false);
            stripeObject.transform.SetAsFirstSibling();

            var rectTransform = stripeObject.GetComponent<RectTransform>();
            rectTransform.anchorMin = new Vector2(0f, 0f);
            rectTransform.anchorMax = new Vector2(1f, 0f);
            rectTransform.pivot = new Vector2(0.5f, 0f);
            rectTransform.anchoredPosition = new Vector2(0f, bottomOffset);
            rectTransform.sizeDelta = new Vector2(0f, GameUiConstants.BrandFlagStripeHeight);
            stripeObject.GetComponent<Image>().color = color;
            stripeObject.GetComponent<Image>().raycastTarget = false;
        }

        private static void CreateCorner(
            Transform parent,
            string name,
            Sprite sprite,
            float size,
            Vector2 anchor,
            Vector2 pivot,
            float rotationZ)
        {
            var cornerObject = new GameObject(name, typeof(RectTransform), typeof(Image));
            cornerObject.transform.SetParent(parent, false);
            cornerObject.transform.SetAsLastSibling();

            var rectTransform = cornerObject.GetComponent<RectTransform>();
            rectTransform.anchorMin = anchor;
            rectTransform.anchorMax = anchor;
            rectTransform.pivot = pivot;
            rectTransform.anchoredPosition = Vector2.zero;
            rectTransform.sizeDelta = new Vector2(size, size);
            rectTransform.localRotation = Quaternion.Euler(0f, 0f, rotationZ);

            var image = cornerObject.GetComponent<Image>();
            image.sprite = sprite;
            image.color = GameVisualPalette.AccentYellow;
            image.raycastTarget = false;
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
