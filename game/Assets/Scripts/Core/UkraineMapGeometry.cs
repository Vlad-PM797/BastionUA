using UnityEngine;

namespace BastionUA.Core
{
    public static class UkraineMapGeometry
    {
        public const float MinLongitude = 22.5f;
        public const float MaxLongitude = 40.0f;
        public const float MinLatitude = 44.28f;
        public const float MaxLatitude = 52.4f;

        // Ukraine with Crimea (AR Crimea). Clockwise from SW; Crimea bulge on the southern edge.
        private static readonly (float latitude, float longitude)[] BorderCoordinates =
        {
            (45.35f, 29.55f),
            (45.42f, 28.55f),
            (45.58f, 28.25f),
            (46.05f, 28.35f),
            (46.55f, 28.85f),
            (46.95f, 29.25f),
            (47.45f, 29.05f),
            (47.95f, 28.55f),
            (48.35f, 27.15f),
            (48.55f, 26.05f),
            (48.75f, 25.05f),
            (49.05f, 23.95f),
            (49.35f, 23.05f),
            (49.75f, 22.65f),
            (50.05f, 22.85f),
            (50.35f, 23.55f),
            (50.65f, 24.55f),
            (50.95f, 25.65f),
            (51.25f, 26.75f),
            (51.55f, 27.85f),
            (51.80f, 28.95f),
            (52.00f, 30.05f),
            (52.18f, 31.15f),
            (52.32f, 32.35f),
            (52.38f, 33.55f),
            (52.30f, 34.75f),
            (52.05f, 35.55f),
            (51.55f, 36.15f),
            (50.95f, 36.65f),
            (50.35f, 37.05f),
            (49.75f, 37.85f),
            (49.15f, 38.55f),
            (48.65f, 39.05f),
            (48.15f, 39.35f),
            (47.65f, 39.15f),
            (47.25f, 38.55f),
            (47.05f, 37.85f),
            (46.95f, 37.15f),
            (46.85f, 36.45f),
            (46.75f, 35.95f),
            (46.65f, 35.45f),
            (46.55f, 34.95f),
            (46.48f, 35.55f),
            (46.38f, 36.05f),
            (45.88f, 36.55f),
            (45.62f, 36.52f),
            (45.42f, 36.15f),
            (45.15f, 35.55f),
            (44.82f, 34.85f),
            (44.55f, 34.15f),
            (44.38f, 33.55f),
            (44.62f, 33.18f),
            (44.98f, 32.98f),
            (45.35f, 33.02f),
            (45.68f, 33.15f),
            (45.98f, 33.38f),
            (46.28f, 33.55f),
            (46.18f, 33.05f),
            (46.05f, 32.45f),
            (45.85f, 31.95f),
            (45.68f, 31.45f),
            (45.52f, 30.95f)
        };

        public static readonly Vector2[] SilhouettePoints = BuildSilhouettePoints();

        public static readonly MapRegionLayout KyivLayout = CreateCityLayout("kyiv", 50.45f, 30.52f);
        public static readonly MapRegionLayout ChernihivLayout = CreateCityLayout("chernihiv", 51.50f, 31.29f);
        public static readonly MapRegionLayout SumyLayout = CreateCityLayout("sumy", 50.92f, 34.80f);
        public static readonly MapRegionLayout KharkivLayout = CreateCityLayout("kharkiv", 49.99f, 36.23f);

        public static Vector2 ToNormalized(float latitude, float longitude)
        {
            var normalizedX = (longitude - MinLongitude) / (MaxLongitude - MinLongitude);
            var normalizedY = (latitude - MinLatitude) / (MaxLatitude - MinLatitude);
            return new Vector2(Mathf.Clamp01(normalizedX), Mathf.Clamp01(normalizedY));
        }

        private static Vector2[] BuildSilhouettePoints()
        {
            var points = new Vector2[BorderCoordinates.Length];
            for (var index = 0; index < BorderCoordinates.Length; index++)
            {
                var coordinate = BorderCoordinates[index];
                points[index] = ToNormalized(coordinate.latitude, coordinate.longitude);
            }

            return points;
        }

        private static MapRegionLayout CreateCityLayout(string regionId, float latitude, float longitude)
        {
            var normalized = ToNormalized(latitude, longitude);
            return new MapRegionLayout(regionId, normalized.x, normalized.y);
        }
    }
}
