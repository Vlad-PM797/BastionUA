using BastionUA.Core;
using UnityEngine;

namespace BastionUA.UI
{
    public static class UkraineMapRasterizer
    {
        private static readonly MapRegionLayout[] RegionTintLayouts =
        {
            MapUiConstants.KyivLayout,
            MapUiConstants.ChernihivLayout,
            MapUiConstants.SumyLayout,
            MapUiConstants.KharkivLayout,
        };

        public static Texture2D CreateMapTexture(
            int width,
            int height,
            MapTextureQuality quality = MapTextureQuality.Standard)
        {
            var polygon = BuildPixelPolygon(MapUiConstants.UkraineSilhouettePoints, width, height);
            var pixels = new Color32[width * height];
            var transparent = new Color32(0, 0, 0, 0);
            var isEnhanced = quality >= MapTextureQuality.Enhanced;
            var isHero = quality == MapTextureQuality.Hero;

            for (var index = 0; index < pixels.Length; index++)
            {
                pixels[index] = transparent;
            }

            var outlinePixels = MapUiConstants.MapSilhouetteBorderPixels * MapUiConstants.MapSilhouetteTextureScale;
            var edgeSoftness = MapUiConstants.MapSilhouetteEdgeSoftnessPixels * MapUiConstants.MapSilhouetteTextureScale;
            if (isEnhanced)
            {
                edgeSoftness *= MapUiConstants.MapV2CoastGlowMultiplier;
            }

            if (isHero)
            {
                edgeSoftness *= MapUiConstants.MapV3CoastGlowMultiplier;
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

                    var normalizedY = y / (float)height;
                    var normalizedX = x / (float)width;
                    var fillColor = Color.Lerp(
                        GameVisualPalette.MapFillSouth,
                        GameVisualPalette.MapFillNorth,
                        normalizedY);
                    fillColor = ApplyTerrainNoise(fillColor, x, y, width, height, isEnhanced);

                    if (isEnhanced)
                    {
                        fillColor = ApplyRegionTintHint(fillColor, normalizedX, normalizedY);
                        fillColor = ApplyCrimeaZoneTint(fillColor, normalizedX, normalizedY);
                        fillColor = ApplyRadialHighlight(fillColor, normalizedX, normalizedY);
                    }

                    if (isHero)
                    {
                        fillColor = ApplyCityGlowHint(fillColor, normalizedX, normalizedY);
                    }

                    var edgeDistance = GetDistanceToPolygonEdge(polygon, point);
                    pixels[(y * width) + x] = SampleSilhouetteColor(
                        fillColor,
                        GameVisualPalette.MapOutline,
                        GameVisualPalette.MapCoast,
                        edgeDistance,
                        outlinePixels,
                        edgeSoftness,
                        isEnhanced);
                }
            }

            var texture = new Texture2D(width, height, TextureFormat.RGBA32, false);
            texture.filterMode = FilterMode.Bilinear;
            texture.wrapMode = TextureWrapMode.Clamp;
            texture.SetPixels32(pixels);
            texture.Apply();
            return texture;
        }

        public static Sprite CreateMapSprite(
            int width,
            int height,
            MapTextureQuality quality = MapTextureQuality.Standard)
        {
            var texture = CreateMapTexture(width, height, quality);
            return Sprite.Create(
                texture,
                new Rect(0f, 0f, width, height),
                new Vector2(0.5f, 0.5f),
                MapUiConstants.MapSilhouetteTextureScale);
        }

        private static Color ApplyTerrainNoise(
            Color baseColor,
            int x,
            int y,
            int width,
            int height,
            bool isEnhanced)
        {
            var noiseScale = isEnhanced ? MapUiConstants.MapV2NoiseStrength : 0.08f;
            var noise = Mathf.PerlinNoise(x * 0.018f, y * 0.018f) * noiseScale - (noiseScale * 0.5f);
            var latitudeShade = (y / (float)height - 0.5f) * 0.06f;
            return new Color(
                Mathf.Clamp01(baseColor.r + noise + latitudeShade),
                Mathf.Clamp01(baseColor.g + noise),
                Mathf.Clamp01(baseColor.b + noise - latitudeShade),
                baseColor.a);
        }

        private static Color ApplyRegionTintHint(Color baseColor, float normalizedX, float normalizedY)
        {
            var pixel = new Vector2(normalizedX, normalizedY);
            var tintStrength = 0f;
            var tintShift = Vector3.zero;

            for (var index = 0; index < RegionTintLayouts.Length; index++)
            {
                var layout = RegionTintLayouts[index];
                var regionPoint = new Vector2(layout.NormalizedX, layout.NormalizedY);
                var distance = Vector2.Distance(pixel, regionPoint);
                var influence = Mathf.InverseLerp(MapUiConstants.MapV2RegionTintRadius, 0f, distance);
                if (influence <= 0f)
                {
                    continue;
                }

                tintStrength = Mathf.Max(tintStrength, influence);
                tintShift += GetRegionTintVector(layout.RegionId) * influence;
            }

            if (tintStrength <= 0f)
            {
                return baseColor;
            }

            tintShift /= Mathf.Max(tintStrength, 0.001f);
            var blend = tintStrength * MapUiConstants.MapV2RegionTintStrength;
            return new Color(
                Mathf.Clamp01(baseColor.r + (tintShift.x * blend)),
                Mathf.Clamp01(baseColor.g + (tintShift.y * blend)),
                Mathf.Clamp01(baseColor.b + (tintShift.z * blend)),
                baseColor.a);
        }

        private static Vector3 GetRegionTintVector(string regionId)
        {
            switch (regionId)
            {
                case "kyiv":
                    return new Vector3(0.04f, 0.03f, -0.02f);
                case "chernihiv":
                    return new Vector3(-0.02f, 0.04f, 0.02f);
                case "sumy":
                    return new Vector3(0.02f, 0.02f, 0.04f);
                case "kharkiv":
                    return new Vector3(0.05f, -0.01f, -0.03f);
                default:
                    return Vector3.zero;
            }
        }

        private static Color ApplyCrimeaZoneTint(Color baseColor, float normalizedX, float normalizedY)
        {
            var crimeaCenter = MapUiConstants.MapV2CrimeaZoneCenter;
            var distance = Vector2.Distance(new Vector2(normalizedX, normalizedY), crimeaCenter);
            var influence = Mathf.InverseLerp(MapUiConstants.MapV2CrimeaZoneRadius, 0f, distance);
            if (influence <= 0f)
            {
                return baseColor;
            }

            var crimeaTint = GameVisualPalette.MapCrimeaZoneTint;
            var blend = influence * MapUiConstants.MapV2CrimeaZoneStrength;
            return Color.Lerp(baseColor, crimeaTint, blend);
        }

        private static Color ApplyRadialHighlight(Color baseColor, float normalizedX, float normalizedY)
        {
            var center = MapUiConstants.MapV2HighlightCenter;
            var distance = Vector2.Distance(new Vector2(normalizedX, normalizedY), center);
            var influence = Mathf.InverseLerp(MapUiConstants.MapV2HighlightRadius, 0f, distance);
            if (influence <= 0f)
            {
                return baseColor;
            }

            var highlight = influence * MapUiConstants.MapV2HighlightStrength;
            return new Color(
                Mathf.Clamp01(baseColor.r + highlight),
                Mathf.Clamp01(baseColor.g + highlight),
                Mathf.Clamp01(baseColor.b + highlight),
                baseColor.a);
        }

        private static Color ApplyCityGlowHint(Color baseColor, float normalizedX, float normalizedY)
        {
            var pixel = new Vector2(normalizedX, normalizedY);
            var glowStrength = 0f;

            for (var index = 0; index < RegionTintLayouts.Length; index++)
            {
                var layout = RegionTintLayouts[index];
                var cityPoint = new Vector2(layout.NormalizedX, layout.NormalizedY);
                var distance = Vector2.Distance(pixel, cityPoint);
                var influence = Mathf.InverseLerp(MapUiConstants.MapV3CityGlowRadius, 0f, distance);
                glowStrength = Mathf.Max(glowStrength, influence);
            }

            if (glowStrength <= 0f)
            {
                return baseColor;
            }

            var glowColor = GameVisualPalette.MapCityGlow;
            var blend = glowStrength * MapUiConstants.MapV3CityGlowStrength;
            return Color.Lerp(baseColor, glowColor, blend);
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
            float edgeSoftnessPixels,
            bool isEnhanced)
        {
            if (edgeDistance <= outlinePixels)
            {
                var outlineBlend = edgeDistance / Mathf.Max(outlinePixels, 0.001f);
                var coastMix = isEnhanced ? 0.55f : 0.35f;
                return (Color32)Color.Lerp(outlineColor, coastColor, outlineBlend * coastMix);
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
