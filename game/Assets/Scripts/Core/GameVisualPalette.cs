using UnityEngine;

namespace BastionUA.Core
{
    public static class GameVisualPalette
    {
        public const string UkraineMapResourcePath = "Art/ukraine_map";

        public static readonly Color CanvasBackground = Hex("#0B1118");
        public static readonly Color TopBar = Hex("#121C28", 0.96f);
        public static readonly Color ObjectiveBar = Hex("#0F1722", 0.92f);
        public static readonly Color SidePanel = Hex("#101923", 0.94f);
        public static readonly Color MapFrame = Hex("#2A3D52", 0.85f);
        public static readonly Color MapBackdrop = Hex("#070B10", 0.72f);

        public static readonly Color TextPrimary = Hex("#F2F6FA");
        public static readonly Color TextMuted = Hex("#9FB0C3");
        public static readonly Color TextObjective = Hex("#DDE8F4");
        public static readonly Color TextAccent = Hex("#FFD700");

        public static readonly Color AccentBlue = Hex("#005BBB");
        public static readonly Color AccentYellow = Hex("#FFD700");
        public static readonly Color AccentBlueLight = Hex("#3D7EB8");

        public static readonly Color ButtonNeutral = Hex("#1A2838");
        public static readonly Color ButtonNeutralBorder = Hex("#2E445C");
        public static readonly Color ButtonPrimary = Hex("#1E4D7A");
        public static readonly Color ButtonPrimaryBorder = Hex("#FFD700");
        public static readonly Color ButtonSelected = Hex("#2A6798");
        public static readonly Color ButtonSelectedBorder = Hex("#FFD700");

        public static readonly Color StatusSafe = Hex("#3FBF6A");
        public static readonly Color StatusDanger = Hex("#E8B923");
        public static readonly Color StatusOccupied = Hex("#D6453D");

        public static readonly Color MapFillNorth = Hex("#1E5678");
        public static readonly Color MapFillSouth = Hex("#123D58");
        public static readonly Color MapOutline = Hex("#F5FAFF");
        public static readonly Color MapCoast = Hex("#6EB5E8");
        public static readonly Color MapConnection = Hex("#4A6885", 0.75f);
        public static readonly Color MapMarkerCore = Hex("#F8FBFF");
        public static readonly Color MapSelectionRing = Hex("#FFD700");

        public static readonly Color EventOverlay = Hex("#05080E", 0.78f);
        public static readonly Color EventPanel = Hex("#121C28", 0.98f);
        public static readonly Color EventChoice = Hex("#1A3550");

        private static Color Hex(string hex, float alphaOverride = 1f)
        {
            if (ColorUtility.TryParseHtmlString(hex, out var color))
            {
                color.a = alphaOverride;
                return color;
            }

            return Color.white;
        }
    }
}
