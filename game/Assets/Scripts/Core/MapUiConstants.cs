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
        public const float MapMarkerLabelBadgeWidth = 168f;
        public const float MapMarkerLabelBadgeHeight = 24f;
        public const float MapConnectionThickness = 3f;
        public const float MapLandmassWidth = 820f;
        public const float MapLandmassHeight = 620f;
        public const int MapLabelFontSize = 16;

        public static readonly Color MapLandColor = GameVisualPalette.MapBackdrop;
        public static readonly Color MapSilhouetteFillColor = GameVisualPalette.MapFillNorth;
        public static readonly Color MapSilhouetteOutlineColor = GameVisualPalette.MapOutline;
        public static readonly Color MapSilhouetteCoastColor = GameVisualPalette.MapCoast;
        public static readonly Color MapConnectionColor = GameVisualPalette.MapConnection;
        public static readonly Color MapSelectionRing = GameVisualPalette.MapSelectionRing;

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
        public const float MapV2CoastGlowMultiplier = 1.85f;
        public const float MapV2HighlightStrength = 0.18f;
        public const float MapV2NoiseStrength = 0.11f;
        public const float MapV2RegionTintRadius = 0.14f;
        public const float MapV2RegionTintStrength = 0.22f;
        public const float MapV2CrimeaZoneRadius = 0.16f;
        public const float MapV2CrimeaZoneStrength = 0.28f;
        public static readonly Vector2 MapV2CrimeaZoneCenter = new Vector2(0.62f, 0.11f);
        public static readonly Vector2 MapV2HighlightCenter = new Vector2(0.48f, 0.46f);
        public const float MapV2HighlightRadius = 0.42f;

        public const float MapV3CoastGlowMultiplier = 2.15f;
        public const float MapV3CityGlowRadius = 0.045f;
        public const float MapV3CityGlowStrength = 0.42f;
        public const int MapV3CityGlowRadiusPixels = 14;
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
