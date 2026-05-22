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

        public const float MapMarkerSize = 54f;
        public const float MapMarkerRingThickness = 5f;
        public const float MapConnectionThickness = 3f;
        public const float MapLandmassWidth = 820f;
        public const float MapLandmassHeight = 620f;
        public const int MapLabelFontSize = 16;

        public static readonly Color MapLandColor = new Color(0.06f, 0.08f, 0.11f, 0.45f);
        public static readonly Color MapSilhouetteFillColor = new Color(0.24f, 0.46f, 0.62f, 0.96f);
        public static readonly Color MapSilhouetteOutlineColor = new Color(0.86f, 0.94f, 1.00f, 1.00f);
        public static readonly Color MapSilhouetteCoastColor = new Color(0.42f, 0.68f, 0.86f, 0.90f);
        public static readonly Color MapConnectionColor = new Color(0.45f, 0.56f, 0.68f, 0.80f);
        public static readonly Color MapSelectionRing = new Color(0.45f, 0.72f, 0.98f, 1f);

        public static readonly MapRegionLayout KyivLayout = UkraineMapGeometry.KyivLayout;
        public static readonly MapRegionLayout ChernihivLayout = UkraineMapGeometry.ChernihivLayout;
        public static readonly MapRegionLayout SumyLayout = UkraineMapGeometry.SumyLayout;
        public static readonly MapRegionLayout KharkivLayout = UkraineMapGeometry.KharkivLayout;

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

        public static readonly Vector2[] UkraineSilhouettePoints = UkraineMapGeometry.SilhouettePoints;

        public const int MapSilhouetteBorderPixels = 4;
        public const float MapSilhouetteEdgeSoftnessPixels = 2.5f;
        public const int MapSilhouetteTextureScale = 2;
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
