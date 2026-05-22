using UnityEngine;

namespace BastionUA.Core
{
    public static class MapUiConstants
    {
        public const string MapPanelTitle = "Карта України";
        public const string LegendTitle = "Статуси";
        public const string LegendSafe = "Safe — стабільно";
        public const string LegendDanger = "Danger — загроза";
        public const string LegendOccupied = "Occupied — окуповано";

        public const float MapMarkerSize = 58f;
        public const float MapMarkerRingThickness = 6f;
        public const float MapConnectionThickness = 3f;
        public const float MapLandmassWidth = 760f;
        public const float MapLandmassHeight = 560f;
        public const int MapLabelFontSize = 17;

        public static readonly Color MapLandColor = new Color(0.12f, 0.15f, 0.20f, 0.92f);
        public static readonly Color MapSilhouetteColor = new Color(0.20f, 0.28f, 0.38f, 0.88f);
        public static readonly Color MapSilhouetteBorderColor = new Color(0.42f, 0.58f, 0.72f, 0.55f);
        public static readonly Color MapConnectionColor = new Color(0.35f, 0.42f, 0.52f, 0.75f);
        public static readonly Color MapSelectionRing = new Color(0.45f, 0.72f, 0.98f, 1f);

        public static readonly MapRegionLayout KyivLayout = new MapRegionLayout("kyiv", 0.40f, 0.40f);
        public static readonly MapRegionLayout ChernihivLayout = new MapRegionLayout("chernihiv", 0.42f, 0.68f);
        public static readonly MapRegionLayout SumyLayout = new MapRegionLayout("sumy", 0.56f, 0.50f);
        public static readonly MapRegionLayout KharkivLayout = new MapRegionLayout("kharkiv", 0.72f, 0.52f);

        public static readonly MapRegionLayout[] MapConnections =
        {
            ChernihivLayout,
            KyivLayout,
            KyivLayout,
            SumyLayout,
            SumyLayout,
            KharkivLayout,
            KyivLayout,
            KharkivLayout
        };

        public static readonly Vector2[] UkraineSilhouettePoints =
        {
            new Vector2(0.12f, 0.38f),
            new Vector2(0.18f, 0.62f),
            new Vector2(0.30f, 0.82f),
            new Vector2(0.48f, 0.90f),
            new Vector2(0.68f, 0.78f),
            new Vector2(0.88f, 0.58f),
            new Vector2(0.92f, 0.36f),
            new Vector2(0.80f, 0.16f),
            new Vector2(0.56f, 0.08f),
            new Vector2(0.34f, 0.10f),
            new Vector2(0.18f, 0.22f)
        };
    }

    public readonly struct MapRegionLayout
    {
        public readonly string RegionId;
        public readonly float NormalizedX;
        public readonly float NormalizedY;

        public MapRegionLayout(string regionId, float normalizedX, float normalizedY)
        {
            RegionId = regionId;
            NormalizedX = normalizedX;
            NormalizedY = normalizedY;
        }
    }
}
