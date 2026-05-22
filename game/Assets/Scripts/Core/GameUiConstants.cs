using UnityEngine;

namespace BastionUA.Core
{
    public static class GameUiConstants
    {
        public const string LabelAmmo = "Ammo";
        public const string LabelMorale = "Morale";
        public const string LabelSelectedRegion = "Selected";
        public const string ButtonTap = "+ Ammo (Tap)";
        public const string ButtonBattle = "Battle";
        public const string HudRootName = "HUD";
        public const string CanvasName = "HudCanvas";
        public const string EventCanvasName = "EventCanvas";

        public const float EventPopupDelaySeconds = 2.5f;
        public const float EventOverlayAlpha = 0.72f;
        public const float EventPanelWidth = 760f;
        public const float EventPanelHeight = 420f;
        public const int EventTitleFontSize = 30;
        public const int EventBodyFontSize = 22;

        public const float ReferenceWidth = 1920f;
        public const float ReferenceHeight = 1080f;
        public const float TopBarHeight = 72f;
        public const float SidePanelWidth = 320f;
        public const float BottomBarHeight = 96f;
        public const int BaseFontSize = 22;
        public const int TitleFontSize = 26;

        public static readonly Color PanelBackground = new Color(0.08f, 0.09f, 0.11f, 0.88f);
        public static readonly Color TextPrimary = new Color(0.95f, 0.96f, 0.98f, 1f);
        public static readonly Color ButtonNormal = new Color(0.18f, 0.24f, 0.34f, 1f);
        public static readonly Color ButtonSelected = new Color(0.24f, 0.42f, 0.62f, 1f);
        public static readonly Color StatusSafe = new Color(0.35f, 0.78f, 0.45f, 1f);
        public static readonly Color StatusDanger = new Color(0.95f, 0.78f, 0.22f, 1f);
        public static readonly Color StatusOccupied = new Color(0.92f, 0.32f, 0.28f, 1f);
        public static readonly Color EventOverlay = new Color(0.02f, 0.03f, 0.06f, 0.72f);
        public static readonly Color EventPanelBackground = new Color(0.11f, 0.13f, 0.18f, 0.96f);
        public static readonly Color EventChoiceButton = new Color(0.22f, 0.31f, 0.45f, 1f);
    }
}
