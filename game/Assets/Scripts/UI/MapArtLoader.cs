using BastionUA.Core;
using UnityEngine;
using UnityEngine.UI;

namespace BastionUA.UI
{
    public static class MapArtLoader
    {
        public static void BuildMapArt(Transform mapRoot)
        {
            BuildBackdrop(mapRoot);

            var sprite = LoadMapSprite();
            var mapObject = new GameObject("UaMapSprite", typeof(RectTransform), typeof(Image));
            mapObject.transform.SetParent(mapRoot, false);
            mapObject.transform.SetSiblingIndex(1);

            var mapRect = mapObject.GetComponent<RectTransform>();
            StretchFullScreen(mapRect, 0f);

            var mapImage = mapObject.GetComponent<Image>();
            mapImage.sprite = sprite;
            mapImage.color = Color.white;
            mapImage.preserveAspect = false;
            mapImage.raycastTarget = false;

            BuildVignetteOverlay(mapRoot);

            Debug.Log($"[MapArtLoader] Map art ready (source={(sprite != null ? sprite.name : "runtime")}).");
        }

        public static Sprite LoadMapSprite()
        {
            var v2Sprite = TryLoadResourceSprite(GameVisualPalette.UkraineMapV2ResourcePath);
            if (v2Sprite != null)
            {
                return v2Sprite;
            }

            var v1Sprite = TryLoadResourceSprite(GameVisualPalette.UkraineMapResourcePath);
            if (v1Sprite != null)
            {
                return v1Sprite;
            }

            var width = Mathf.RoundToInt(MapUiConstants.MapLandmassWidth * MapUiConstants.MapSilhouetteTextureScale);
            var height = Mathf.RoundToInt(MapUiConstants.MapLandmassHeight * MapUiConstants.MapSilhouetteTextureScale);
            return UkraineMapRasterizer.CreateMapSprite(width, height, MapTextureQuality.Enhanced);
        }

        private static Sprite TryLoadResourceSprite(string resourcePath)
        {
            try
            {
                return Resources.Load<Sprite>(resourcePath);
            }
            catch (System.Exception exception)
            {
                Debug.LogWarning($"[MapArtLoader] Resource load failed for {resourcePath}: {exception.Message}");
                return null;
            }
        }

        private static void BuildBackdrop(Transform mapRoot)
        {
            var backdropObject = new GameObject("MapBackdrop", typeof(RectTransform), typeof(Image));
            backdropObject.transform.SetParent(mapRoot, false);
            backdropObject.transform.SetAsFirstSibling();
            StretchFullScreen(backdropObject.GetComponent<RectTransform>(), 0f);
            backdropObject.GetComponent<Image>().color = GameVisualPalette.MapBackdrop;
            backdropObject.GetComponent<Image>().raycastTarget = false;
        }

        private static void BuildVignetteOverlay(Transform mapRoot)
        {
            try
            {
                var vignetteObject = new GameObject("MapVignette", typeof(RectTransform), typeof(Image));
                vignetteObject.transform.SetParent(mapRoot, false);
                vignetteObject.transform.SetAsLastSibling();

                StretchFullScreen(vignetteObject.GetComponent<RectTransform>(), 0f);
                var vignetteImage = vignetteObject.GetComponent<Image>();
                vignetteImage.sprite = CreateVignetteSprite();
                vignetteImage.type = Image.Type.Simple;
                vignetteImage.color = Color.white;
                vignetteImage.raycastTarget = false;
            }
            catch (System.Exception exception)
            {
                Debug.LogWarning($"[MapArtLoader] Vignette build failed: {exception.Message}");
            }
        }

        private static Sprite CreateVignetteSprite()
        {
            var size = GameUiConstants.MapVignetteTextureSize;
            var texture = new Texture2D(size, size, TextureFormat.RGBA32, false);
            texture.wrapMode = TextureWrapMode.Clamp;
            texture.filterMode = FilterMode.Bilinear;

            var center = (size - 1) * 0.5f;
            var maxDistance = center;
            var innerRadius = GameUiConstants.MapVignetteInnerRadius;
            var vignetteColor = GameVisualPalette.MapVignette;

            for (var y = 0; y < size; y++)
            {
                for (var x = 0; x < size; x++)
                {
                    var dx = (x - center) / maxDistance;
                    var dy = (y - center) / maxDistance;
                    var distance = Mathf.Sqrt(dx * dx + dy * dy);
                    var alpha = Mathf.InverseLerp(innerRadius, 1f, distance) * vignetteColor.a;
                    texture.SetPixel(x, y, new Color(vignetteColor.r, vignetteColor.g, vignetteColor.b, alpha));
                }
            }

            texture.Apply();
            return Sprite.Create(
                texture,
                new Rect(0f, 0f, size, size),
                new Vector2(0.5f, 0.5f),
                size);
        }

        private static void StretchFullScreen(RectTransform rectTransform, float expandPixels)
        {
            rectTransform.anchorMin = Vector2.zero;
            rectTransform.anchorMax = Vector2.one;
            rectTransform.offsetMin = new Vector2(expandPixels, expandPixels);
            rectTransform.offsetMax = new Vector2(-expandPixels, -expandPixels);
        }
    }
}
