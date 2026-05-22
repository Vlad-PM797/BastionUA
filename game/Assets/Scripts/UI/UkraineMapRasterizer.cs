using BastionUA.Core;
using UnityEngine;

namespace BastionUA.UI
{
    public static class UkraineMapRasterizer
    {
        public static Texture2D CreateMapTexture(int width, int height)
        {
            var polygon = BuildPixelPolygon(MapUiConstants.UkraineSilhouettePoints, width, height);
            var pixels = new Color32[width * height];
            var transparent = new Color32(0, 0, 0, 0);

            for (var index = 0; index < pixels.Length; index++)
            {
                pixels[index] = transparent;
            }

            var outlinePixels = MapUiConstants.MapSilhouetteBorderPixels * MapUiConstants.MapSilhouetteTextureScale;
            var edgeSoftness = MapUiConstants.MapSilhouetteEdgeSoftnessPixels * MapUiConstants.MapSilhouetteTextureScale;

            for (var y = 0; y < height; y++)
            {
                for (var x = 0; x < width; x++)
                {
                    var point = new Vector2(x + 0.5f, y + 0.5f);
                    if (!IsPointInsidePolygon(polygon, point))
                    {
                        continue;
                    }

                    var normalizedY = y / (float)height;
                    var fillColor = Color.Lerp(
                        GameVisualPalette.MapFillSouth,
                        GameVisualPalette.MapFillNorth,
                        normalizedY);
                    fillColor = ApplyTerrainNoise(fillColor, x, y, width, height);

                    var edgeDistance = GetDistanceToPolygonEdge(polygon, point);
                    pixels[(y * width) + x] = SampleSilhouetteColor(
                        fillColor,
                        GameVisualPalette.MapOutline,
                        GameVisualPalette.MapCoast,
                        edgeDistance,
                        outlinePixels,
                        edgeSoftness);
                }
            }

            var texture = new Texture2D(width, height, TextureFormat.RGBA32, false);
            texture.filterMode = FilterMode.Bilinear;
            texture.wrapMode = TextureWrapMode.Clamp;
            texture.SetPixels32(pixels);
            texture.Apply();
            return texture;
        }

        public static Sprite CreateMapSprite(int width, int height)
        {
            var texture = CreateMapTexture(width, height);
            return Sprite.Create(
                texture,
                new Rect(0f, 0f, width, height),
                new Vector2(0.5f, 0.5f),
                MapUiConstants.MapSilhouetteTextureScale);
        }

        private static Color ApplyTerrainNoise(Color baseColor, int x, int y, int width, int height)
        {
            var noise = Mathf.PerlinNoise(x * 0.018f, y * 0.018f) * 0.08f - 0.04f;
            var latitudeShade = (y / (float)height - 0.5f) * 0.06f;
            return new Color(
                Mathf.Clamp01(baseColor.r + noise + latitudeShade),
                Mathf.Clamp01(baseColor.g + noise),
                Mathf.Clamp01(baseColor.b + noise - latitudeShade),
                baseColor.a);
        }

        private static Vector2[] BuildPixelPolygon(Vector2[] normalizedPoints, int width, int height)
        {
            var polygon = new Vector2[normalizedPoints.Length];
            for (var index = 0; index < normalizedPoints.Length; index++)
            {
                polygon[index] = new Vector2(
                    normalizedPoints[index].x * width,
                    normalizedPoints[index].y * height);
            }

            return polygon;
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
    }
}
