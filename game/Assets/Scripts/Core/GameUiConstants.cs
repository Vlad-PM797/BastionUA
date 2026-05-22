using UnityEngine;

namespace BastionUA.Core
{
    public static class GameUiConstants
    {
        public const string LabelAmmo = "Ammo";
        public const string LabelMorale = "Morale";
        public const string LabelSelectedRegion = "Selected";
        public const string LabelObjective = "Objective";
        public const string ButtonTap = "+ Ammo (Tap)";
        public const string ButtonBattle = "Battle";
        public const string ButtonResetSave = "Reset Save";
        public const string HudRootName = "HUD";
        public const string CanvasName = "HudCanvas";
        public const string EventCanvasName = "EventCanvas";
        public const string BattleCanvasName = "BattleCanvas";

        public const string BattleVictoryTitle = "Victory";
        public const string BattleDefeatTitle = "Defeat";
        public const string BattleLabelRegion = "Region";
        public const string BattleLabelAmmoSpent = "Ammo spent";
        public const string BattleLabelHp = "HP (you / enemy)";
        public const string BattleLabelRegionStatus = "Region status";
        public const string BattleContinueButton = "Continue";
        public const float BattlePanelWidth = 680f;
        public const float BattlePanelHeight = 360f;

        public const string LabelUnits = "Units";
        public const string LabelUpgrades = "Upgrades";
        public const string UpgradeMaxLabel = "MAX";

        public const float EventPopupDelaySeconds = 2.5f;
        public const float EventOverlayAlpha = 0.72f;
        public const float EventPanelWidth = 760f;
        public const float EventPanelHeight = 420f;
        public const int EventTitleFontSize = 30;
        public const int EventBodyFontSize = 22;

        public const string OnboardingHint = "\u041e\u0431\u0435\u0440\u0438 \u0440\u0435\u0433\u0456\u043e\u043d \u043d\u0430 \u043a\u0430\u0440\u0442\u0456 \u2192 Battle. \u041f\u043e\u0434\u0456\u0457 \u0437\u2019\u044f\u0432\u043b\u044f\u044e\u0442\u044c\u0441\u044f \u043f\u0456\u0434 \u0447\u0430\u0441 \u0432\u0456\u0439\u043d\u0438.";
        public const string ObjectiveFallback = "\u0422\u0440\u0438\u043c\u0430\u0439 \u043e\u0431\u043e\u0440\u043e\u043d\u0443 \u0442\u0430 \u0440\u043e\u0437\u0432\u0438\u0432\u0430\u0439 \u0440\u0435\u0441\u0443\u0440\u0441\u0438.";
        public const string ObjectiveHostomelPending = "\u041f\u0440\u0438\u0439\u043c\u0438 \u0440\u0456\u0448\u0435\u043d\u043d\u044f \u0443 \u043f\u043e\u0434\u0456\u0457 \u00ab\u0413\u043e\u0441\u0442\u043e\u043c\u0435\u043b\u044c\u00bb.";
        public const string ObjectiveFirstBattle = "\u041f\u0440\u043e\u0432\u0435\u0434\u0438 1 \u0431\u0456\u0439, \u0449\u043e\u0431 \u0432\u0456\u0434\u043a\u0440\u0438\u0442\u0438 \u043d\u0430\u0441\u0442\u0443\u043f\u043d\u0443 \u043f\u043e\u0434\u0456\u044e.";
        public const string ObjectiveChornobaivkaPending = "\u041e\u0447\u0456\u043a\u0443\u0439 \u043f\u043e\u0434\u0456\u044e \u00ab\u0427\u043e\u0440\u043d\u043e\u0431\u0430\u0457\u0432\u043a\u0430\u00bb \u043f\u0456\u0441\u043b\u044f \u0431\u043e\u044e.";
        public const string ObjectiveSecondBattle = "\u041f\u0440\u043e\u0432\u0435\u0434\u0438 \u0449\u0435 1 \u0431\u0456\u0439 \u0434\u043b\u044f \u043f\u043e\u0434\u0456\u0457 \u00ab\u0406\u0440\u043f\u0456\u043d\u044c\u00bb.";
        public const string ObjectiveIrpinPending = "\u041f\u0440\u0438\u0439\u043c\u0438 \u0440\u0456\u0448\u0435\u043d\u043d\u044f \u0443 \u043f\u043e\u0434\u0456\u0457 \u00ab\u0406\u0440\u043f\u0456\u043d\u044c\u00bb.";
        public const string ObjectiveExpandTerritory = "\u0423\u0441\u0456 \u043f\u043e\u0434\u0456\u0457 \u043f\u0440\u043e\u0439\u0434\u0435\u043d\u043e. \u041f\u043e\u0441\u0438\u043b\u044c \u0440\u0435\u0433\u0456\u043e\u043d\u0438 \u0431\u043e\u044f\u043c\u0438.";
        public const string ObjectiveProgression = "\u041e\u0431\u0435\u0440\u0438 \u043f\u0456\u0434\u0440\u043e\u0437\u0434\u0456\u043b \u0456 \u043a\u0443\u043f\u0438 \u0430\u043f\u0433\u0440\u0435\u0439\u0434 \u0434\u043b\u044f \u0441\u0438\u043b\u044c\u043d\u0456\u0448\u0438\u0445 \u0431\u043e\u0457\u0432.";
        public const string ObjectiveKharkiv = "\u041f\u0440\u043e\u0441\u0443\u043d\u044c\u0441\u044f \u043d\u0430 \u0441\u0445\u0456\u0434 \u2014 \u0437\u0432\u0456\u043b\u044c\u043d\u0438 Kharkiv.";

        public const float ReferenceWidth = 1920f;
        public const float ReferenceHeight = 1080f;
        public const float TopBarHeight = 72f;
        public const float ObjectiveBarHeight = 40f;
        public const float HudTopInset = TopBarHeight + ObjectiveBarHeight;
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
