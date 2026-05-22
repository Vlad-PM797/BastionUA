using BastionUA.Core;
using UnityEngine;
using UnityEngine.UI;

namespace BastionUA.UI
{
    public static class MapSilhouetteFactory
    {
        public static void BuildSilhouette(Transform mapRoot)
        {
            var width = Mathf.RoundToInt(MapUiConstants.MapLandmassWidth * MapUiConstants.MapSilhouetteTextureScale);
            var height = Mathf.RoundToInt(MapUiConstants.MapLandmassHeight * MapUiConstants.MapSilhouetteTextureScale);
            var sprite = CreatePolygonSprite(
                MapUiConstants.UkraineSilhouettePoints,
                width,
                height,
                MapUiConstants.MapSilhouetteFillColor,
                MapUiConstants.MapSilhouetteOutlineColor,
                MapUiConstants.MapSilhouetteCoastColor,
                MapUiConstants.MapSilhouetteBorderPixels * MapUiConstants.MapSilhouetteTextureScale,
                MapUiConstants.MapSilhouetteEdgeSoftnessPixels * MapUiConstants.MapSilhouetteTextureScale);

            var backdropObject = new GameObject("MapBackdrop", typeof(RectTransform), typeof(Image));
            backdropObject.transform.SetParent(mapRoot, false);
            backdropObject.transform.SetAsFirstSibling();
            StretchFullScreen(backdropObject.GetComponent<RectTransform>(), 0f);
            var backdropImage = backdropObject.GetComponent<Image>();
            backdropImage.color = MapUiConstants.MapLandColor;
            backdropImage.raycastTarget = false;

            var silhouetteObject = new GameObject("UaSilhouette", typeof(RectTransform), typeof(Image));
            silhouetteObject.transform.SetParent(mapRoot, false);
            silhouetteObject.transform.SetSiblingIndex(1);

            var silhouetteRect = silhouetteObject.GetComponent<RectTransform>();
            StretchFullScreen(silhouetteRect, 0f);

            var silhouetteImage = silhouetteObject.GetComponent<Image>();
            silhouetteImage.sprite = sprite;
            silhouetteImage.color = Color.white;
            silhouetteImage.preserveAspect = false;
            silhouetteImage.raycastTarget = false;

            Debug.Log(
                $"[MapSilhouetteFactory] Ukraine silhouette created ({MapUiConstants.UkraineSilhouettePoints.Length} border points).");
        }

        private static Sprite CreatePolygonSprite(
            Vector2[] normalizedPoints,
            int width,
            int height,
            Color fillColor,
            Color outlineColor,
            Color coastColor,
            int outlinePixels,
            float edgeSoftnessPixels)
        {
            var polygon = new Vector2[normalizedPoints.Length];
            for (var index = 0; index < normalizedPoints.Length; index++)
            {
                polygon[index] = new Vector2(
                    normalizedPoints[index].x * width,
                    normalizedPoints[index].y * height);
            }

            var pixels = new Color32[width * height];
            var transparent = new Color32(0, 0, 0, 0);
            for (var index = 0; index < pixels.Length; index++)
            {
                pixels[index] = transparent;
            }

            for (var y = 0; y < height; y++)
            {
                for (var x = 0; x < width; x++)
                {
                    var point = new Vector2(x + 0.5f, y + 0.5f);
                    if (!IsPointInsidePolygon(polygon, point))
                    {
                        continue;
                    }

                    var edgeDistance = GetDistanceToPolygonEdge(polygon, point);
                    pixels[(y * width) + x] = SampleSilhouetteColor(
                        fillColor,
                        outlineColor,
                        coastColor,
                        edgeDistance,
                        outlinePixels,
                        edgeSoftnessPixels);
                }
            }

            var texture = new Texture2D(width, height, TextureFormat.RGBA32, false);
            texture.filterMode = FilterMode.Bilinear;
            texture.wrapMode = TextureWrapMode.Clamp;
            texture.SetPixels32(pixels);
            texture.Apply();

            return Sprite.Create(
                texture,
                new Rect(0f, 0f, width, height),
                new Vector2(0.5f, 0.5f),
                MapUiConstants.MapSilhouetteTextureScale);
        }

        private static Color32 SampleSilhouetteColor(
            Color fillColor,
            Color outlineColor,
            Color coastColor,
            float edgeDistance,
            float outlinePixels,
            float edgeSoftnessPixels)
        {
            if (edgeDistance <= outlinePixels)
            {
                var outlineBlend = edgeDistance / Mathf.Max(outlinePixels, 0.001f);
                return (Color32)Color.Lerp(outlineColor, coastColor, outlineBlend * 0.35f);
            }

            if (edgeDistance <= outlinePixels + edgeSoftnessPixels)
            {
                var softnessT = (edgeDistance - outlinePixels) / Mathf.Max(edgeSoftnessPixels, 0.001f);
                return (Color32)Color.Lerp(coastColor, fillColor, softnessT);
            }

            return fillColor;
        }

        private static float GetDistanceToPolygonEdge(Vector2[] polygon, Vector2 point)
        {
            var minDistance = float.MaxValue;
            for (var index = 0; index < polygon.Length; index++)
            {
                var start = polygon[index];
                var end = polygon[(index + 1) % polygon.Length];
                minDistance = Mathf.Min(minDistance, DistancePointToSegment(point, start, end));
            }

            return minDistance;
        }

        private static float DistancePointToSegment(Vector2 point, Vector2 start, Vector2 end)
        {
            var segment = end - start;
            var segmentLengthSquared = segment.sqrMagnitude;
            if (segmentLengthSquared <= Mathf.Epsilon)
            {
                return Vector2.Distance(point, start);
            }

            var t = Mathf.Clamp01(Vector2.Dot(point - start, segment) / segmentLengthSquared);
            var projection = start + (segment * t);
            return Vector2.Distance(point, projection);
        }

        private static bool IsPointInsidePolygon(Vector2[] polygon, Vector2 point)
        {
            var inside = false;
            for (int i = 0, j = polygon.Length - 1; i < polygon.Length; j = i++)
            {
                var pi = polygon[i];
                var pj = polygon[j];
                var intersects = (pi.y > point.y) != (pj.y > point.y) &&
                                 point.x <
                                 ((pj.x - pi.x) * (point.y - pi.y) / (pj.y - pi.y + float.Epsilon)) + pi.x;
                if (intersects)
                {
                    inside = !inside;
                }
            }

            return inside;
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
