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

            Debug.Log($"[MapArtLoader] Map art ready (source={(sprite != null ? sprite.name : "runtime")}).");
        }

        public static Sprite LoadMapSprite()
        {
            try
            {
                var resourceSprite = Resources.Load<Sprite>(GameVisualPalette.UkraineMapResourcePath);
                if (resourceSprite != null)
                {
                    return resourceSprite;
                }
            }
            catch (System.Exception exception)
            {
                Debug.LogWarning($"[MapArtLoader] Resource load failed: {exception.Message}");
            }

            var width = Mathf.RoundToInt(MapUiConstants.MapLandmassWidth * MapUiConstants.MapSilhouetteTextureScale);
            var height = Mathf.RoundToInt(MapUiConstants.MapLandmassHeight * MapUiConstants.MapSilhouetteTextureScale);
            return UkraineMapRasterizer.CreateMapSprite(width, height);
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

        private static void StretchFullScreen(RectTransform rectTransform, float expandPixels)
        {
            rectTransform.anchorMin = Vector2.zero;
            rectTransform.anchorMax = Vector2.one;
            rectTransform.offsetMin = new Vector2(expandPixels, expandPixels);
            rectTransform.offsetMax = new Vector2(-expandPixels, -expandPixels);
        }
    }
}
