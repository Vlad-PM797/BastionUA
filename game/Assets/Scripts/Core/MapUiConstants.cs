using UnityEngine;

namespace BastionUA.Core
{
    public static class MapUiConstants
    {
        public const string MapPanelTitle = "Карта";
        public const string LegendTitle = "Статуси";
        public const string LegendSafe = "Safe — стабільно";
        public const string LegendDanger = "Danger — загроза";
        public const string LegendOccupied = "Occupied — окуповано";

        public const float MapMarkerSize = 64f;
        public const float MapMarkerRingThickness = 6f;
        public const float MapConnectionThickness = 4f;
        public const float MapLandmassWidth = 720f;
        public const float MapLandmassHeight = 520f;
        public const int MapLabelFontSize = 18;

        public static readonly Color MapLandColor = new Color(0.14f, 0.17f, 0.22f, 0.95f);
        public static readonly Color MapConnectionColor = new Color(0.35f, 0.42f, 0.52f, 0.85f);
        public static readonly Color MapSelectionRing = new Color(0.45f, 0.72f, 0.98f, 1f);

        public static readonly MapRegionLayout KyivLayout = new MapRegionLayout("kyiv", 0.42f, 0.36f);
        public static readonly MapRegionLayout ChernihivLayout = new MapRegionLayout("chernihiv", 0.40f, 0.72f);
        public static readonly MapRegionLayout SumyLayout = new MapRegionLayout("sumy", 0.64f, 0.44f);
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
