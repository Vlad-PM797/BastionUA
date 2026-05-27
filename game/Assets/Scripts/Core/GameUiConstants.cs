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
        public const string ButtonQuit = "\u0412\u0438\u0445\u0456\u0434";
        public const string ButtonPrestige = "Prestige";
        public const string LabelEventLog = "Log";
        public const string LabelPrestige = "Prestige";
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
        public const string BattleConfirmTitle = "Confirm Battle";
        public const string BattleConfirmFightButton = "Fight";
        public const string BattleConfirmCancelButton = "Cancel";
        public const string BattleConfirmAmmoCost = "Ammo cost";
        public const string BattleConfirmEnemyHp = "Enemy HP";
        public const string BattleConfirmYourDamage = "Your damage / round";
        public const string BattleConfirmEnemyDamage = "Enemy damage / round";
        public const string BattleConfirmEstimatedRounds = "Est. rounds to win";
        public const string BattleConfirmInsufficientAmmo = "Need at least {0} ammo to fight";
        public const string BattleLabelCombatLog = "Combat log";
        public const string BattleLogRoundFormat = "R{0}: dealt {1}, took {2} ({3}/{4} HP)";
        public const string BattleLogTruncatedFormat = "... +{0} more rounds";
        public const float BattlePanelWidth = 680f;
        public const float BattlePanelHeight = 420f;
        public const float BattleConfirmPanelWidth = 680f;
        public const float BattleConfirmPanelHeight = 380f;

        public const string LabelUnits = "Units";
        public const string LabelUpgrades = "Upgrades";
        public const string UpgradeMaxLabel = "MAX";

        public const float EventPopupDelaySeconds = 2.5f;
        public const float EventOverlayAlpha = 0.72f;
        public const float EventPanelWidth = 760f;
        public const float EventPanelHeight = 450f;
        public const float EventBannerHeight = 88f;
        public const float EventTitleOffsetY = -118f;
        public const float EventBodyOffsetY = -200f;
        public const int EventTitleFontSize = 30;
        public const int EventBodyFontSize = 22;
        public const float PopupBorderExpand = 2f;
        public const float PopupAccentStripeHeight = 4f;
        public const float PopupFrameInset = 3f;
        public const float ButtonFrameInset = 3f;
        public const float PopupFadeInDurationSeconds = 0.2f;
        public const float PopupOutcomeStripeHeight = 5f;

        public const string OnboardingHint = "\u041e\u0431\u0435\u0440\u0438 \u0440\u0435\u0433\u0456\u043e\u043d \u043d\u0430 \u043a\u0430\u0440\u0442\u0456 \u2192 Battle. \u041f\u043e\u0434\u0456\u0457 \u0437\u2019\u044f\u0432\u043b\u044f\u044e\u0442\u044c\u0441\u044f \u043f\u0456\u0434 \u0447\u0430\u0441 \u0432\u0456\u0439\u043d\u0438.";
        public const string ObjectiveFallback = "\u0422\u0440\u0438\u043c\u0430\u0439 \u043e\u0431\u043e\u0440\u043e\u043d\u0443 \u0442\u0430 \u0440\u043e\u0437\u0432\u0438\u0432\u0430\u0439 \u0440\u0435\u0441\u0443\u0440\u0441\u0438.";
        public const string ObjectiveHostomelPending = "\u041f\u0440\u0438\u0439\u043c\u0438 \u0440\u0456\u0448\u0435\u043d\u043d\u044f \u0443 \u043f\u043e\u0434\u0456\u0457 \u00ab\u0413\u043e\u0441\u0442\u043e\u043c\u0435\u043b\u044c\u00bb.";
        public const string ObjectiveFirstBattle = "\u041f\u0440\u043e\u0432\u0435\u0434\u0438 1 \u0431\u0456\u0439, \u0449\u043e\u0431 \u0432\u0456\u0434\u043a\u0440\u0438\u0442\u0438 \u043d\u0430\u0441\u0442\u0443\u043f\u043d\u0443 \u043f\u043e\u0434\u0456\u044e.";
        public const string ObjectiveChornobaivkaPending = "\u041e\u0447\u0456\u043a\u0443\u0439 \u043f\u043e\u0434\u0456\u044e \u00ab\u0427\u043e\u0440\u043d\u043e\u0431\u0430\u0457\u0432\u043a\u0430\u00bb \u043f\u0456\u0441\u043b\u044f \u0431\u043e\u044e.";
        public const string ObjectiveSecondBattle = "\u041f\u0440\u043e\u0432\u0435\u0434\u0438 \u0449\u0435 1 \u0431\u0456\u0439 \u0434\u043b\u044f \u043f\u043e\u0434\u0456\u0457 \u00ab\u0406\u0440\u043f\u0456\u043d\u044c\u00bb.";
        public const string ObjectiveIrpinPending = "\u041f\u0440\u0438\u0439\u043c\u0438 \u0440\u0456\u0448\u0435\u043d\u043d\u044f \u0443 \u043f\u043e\u0434\u0456\u0457 \u00ab\u0406\u0440\u043f\u0456\u043d\u044c\u00bb.";
        public const string ObjectiveThirdBattle = "\u041f\u0440\u043e\u0432\u0435\u0434\u0438 \u0449\u0435 1 \u0431\u0456\u0439 \u0434\u043b\u044f \u043f\u043e\u0434\u0456\u0457 \u00ab\u0425\u0430\u0440\u043a\u0456\u0432\u00bb.";
        public const string ObjectiveKharkivEventPending = "\u041f\u0440\u0438\u0439\u043c\u0438 \u0440\u0456\u0448\u0435\u043d\u043d\u044f \u0443 \u043f\u043e\u0434\u0456\u0457 \u00ab\u0425\u0430\u0440\u043a\u0456\u0432\u00bb.";
        public const string ObjectiveFourthBattle = "\u041f\u0440\u043e\u0432\u0435\u0434\u0438 \u0449\u0435 1 \u0431\u0456\u0439 \u0434\u043b\u044f \u043f\u043e\u0434\u0456\u0457 \u00ab\u0427\u0435\u0440\u043d\u0456\u0433\u0456\u0432\u00bb.";
        public const string ObjectiveChernihivPending = "\u041f\u0440\u0438\u0439\u043c\u0438 \u0440\u0456\u0448\u0435\u043d\u043d\u044f \u0443 \u043f\u043e\u0434\u0456\u0457 \u00ab\u0427\u0435\u0440\u043d\u0456\u0433\u0456\u0432\u00bb.";
        public const string ObjectiveExpandTerritory = "\u0423\u0441\u0456 \u043f\u043e\u0434\u0456\u0457 \u043f\u0440\u043e\u0439\u0434\u0435\u043d\u043e. \u041f\u043e\u0441\u0438\u043b\u044c \u0440\u0435\u0433\u0456\u043e\u043d\u0438 \u0431\u043e\u044f\u043c\u0438.";
        public const string ObjectiveProgression = "\u041e\u0431\u0435\u0440\u0438 \u043f\u0456\u0434\u0440\u043e\u0437\u0434\u0456\u043b \u0456 \u043a\u0443\u043f\u0438 \u0430\u043f\u0433\u0440\u0435\u0439\u0434 \u0434\u043b\u044f \u0441\u0438\u043b\u044c\u043d\u0456\u0448\u0438\u0445 \u0431\u043e\u0457\u0432.";
        public const string ObjectiveKharkiv = "\u0417\u0432\u0456\u043b\u044c\u043d\u0438 Kharkiv \u0431\u043e\u044f\u043c \u043d\u0430 \u043a\u0430\u0440\u0442\u0456.";
        public const string ObjectivePrestigeReady =
            "\u041a\u0430\u043c\u043f\u0430\u043d\u0456\u044f \u043f\u0440\u043e\u0439\u0434\u0435\u043d\u0430. \u041d\u0430\u0442\u0438\u0441\u043d\u0438 Prestige \u0434\u043b\u044f \u0431\u043e\u043d\u0443\u0441\u0443 \u043d\u0430\u0441\u0442\u0443\u043f\u043d\u043e\u0433\u043e \u0446\u0438\u043a\u043b\u0443.";
        public const string ObjectiveLiberateRegions =
            "\u0417\u0432\u0456\u043b\u044c\u043d\u0438 \u0443\u0441\u0456 \u043e\u043a\u0443\u043f\u043e\u0432\u0430\u043d\u0456 \u0440\u0435\u0433\u0456\u043e\u043d\u0438 \u0431\u043e\u044f\u043c\u0438, \u0449\u043e\u0431 \u0432\u0456\u0434\u043a\u0440\u0438\u0442\u0438 Prestige.";
        public const string ObjectivePrestigeMax =
            "\u041c\u0430\u043a\u0441\u0438\u043c\u0430\u043b\u044c\u043d\u0438\u0439 Prestige \u0434\u043e\u0441\u044f\u0433\u043d\u0443\u0442\u043e. \u041f\u043e\u0441\u0438\u043b\u044c \u0440\u0435\u0433\u0456\u043e\u043d\u0438 \u0442\u0430 \u0433\u0440\u0430\u0439 \u0434\u0430\u043b\u0456.";

        public const float ReferenceWidth = 1920f;
        public const float ReferenceHeight = 1080f;
        public const float TopBarHeight = 72f;
        public const float ObjectiveBarHeight = 40f;
        public const float HudTopInset = TopBarHeight + ObjectiveBarHeight;
        public const float SidePanelWidth = 320f;
        public const float BottomBarHeight = 96f;
        public const int BaseFontSize = 22;
        public const int TitleFontSize = 26;
        public const int StatValueFontSize = 24;
        public const int ObjectiveFontSize = 20;
        public const int SectionTitleFontSize = 18;
        public const int CompactFontSize = 16;
        public const float StatTextWidth = 220f;
        public const float SelectedStatTextWidth = 360f;
        public const float ObjectiveTextWidth = 1680f;
        public const float AccentStripeHeight = 4f;
        public const float SidePanelAccentWidth = 4f;

        public const float ButtonPressedScale = 0.97f;
        public const float TapFlashDurationSeconds = 0.14f;
        public const float MarkerPulseSpeed = 3.5f;
        public const float MarkerPulseMinAlpha = 0.35f;
        public const float MarkerPulseMaxAlpha = 1f;
        public const float ButtonHighlightBlend = 0.12f;
        public const float ButtonPressedBlend = 0.2f;

        public const int MapVignetteTextureSize = 256;
        public const float MapVignetteInnerRadius = 0.38f;
        public const float MapFrameInnerInsetPixels = 4f;

        public const float SidebarLogPanelTopAnchor = 0.69f;
        public const float SidebarLogPanelBottomAnchor = 0.56f;
        public const float SidebarEventLogTitleAnchor = 0.71f;
        public const float SidebarUnitsTitleAnchor = 0.52f;
        public const float SidebarUnitsFirstAnchor = 0.48f;
        public const float SidebarUnitsRowSpacing = 0.06f;
        public const float SidebarUpgradeRowHeight = 34f;
        public const float SidebarFooterHeightPixels = 40f;
        public const float SidebarFooterBottomPaddingPixels = 8f;
        public const float SidebarUpgradeBottomPaddingPixels = 14f;
        public const float SidebarUpgradeStackSpacingPixels = 38f;
        public const float SidebarUpgradesTitleGapPixels = 10f;
        public const float SidebarResetButtonWidth = 236f;
        public const float SidebarResetButtonHeight = 34f;
        public const float SidebarPurchaseButtonWidth = 48f;

        public const string BrandTitle = "BASTION UA";
        public const string BrandSubtitle = "DEFENSE COMMAND";
        public const float BrandLockupLeftPadding = 18f;
        public const float BrandLockupWidth = 190f;
        public const float BrandFlagStripeHeight = 2f;
        public const float BrandFlagBlueStripeHeight = 3f;
        public const int BrandTitleFontSize = 24;
        public const int BrandSubtitleFontSize = 11;

        public const float AtmospherePulseSpeed = 0.35f;
        public const float AtmosphereGlowMinAlpha = 0.12f;
        public const float AtmosphereGlowMaxAlpha = 0.28f;
        public const int CanvasGradientTextureWidth = 4;
        public const int CanvasGradientTextureHeight = 256;
        public const int ProceduralCircleTextureSize = 64;
        public const int ProceduralCornerTextureSize = 64;
        public const int HudCornerLinePixels = 3;
        public const float HudCornerDisplaySize = 28f;

        public const float MapMarkerOccupiedPulseSpeed = 2.6f;
        public const float MapMarkerOccupiedPulseMinAlpha = 0.55f;
        public const float MapMarkerOccupiedPulseMaxAlpha = 1f;

        public const float HudStatAnchorAmmoIcon = 0.145f;
        public const float HudStatAnchorAmmoText = 0.169f;
        public const float HudStatAnchorPrestige = 0.31f;
        public const float HudStatAnchorMoraleIcon = 0.445f;
        public const float HudStatAnchorMoraleText = 0.469f;
        public const float HudStatAnchorBattleIcon = 0.585f;
        public const float HudStatAnchorSelected = 0.665f;

        public static float SidebarContentHeight =>
            ReferenceHeight - BottomBarHeight - HudTopInset;

        public static float GetSidebarUpgradeRowAnchor(int rowFromBottom)
        {
            var bottomOffset = SidebarFooterBottomPaddingPixels
                + SidebarFooterHeightPixels
                + SidebarUpgradeBottomPaddingPixels
                + rowFromBottom * SidebarUpgradeStackSpacingPixels
                + SidebarUpgradeRowHeight * 0.5f;
            return bottomOffset / SidebarContentHeight;
        }

        public static float GetSidebarUpgradesTitleAnchor(int upgradeCount)
        {
            var topRowFromBottom = Mathf.Max(upgradeCount - 1, 0);
            var topRowCenterPixels =
                SidebarFooterBottomPaddingPixels
                + SidebarFooterHeightPixels
                + SidebarUpgradeBottomPaddingPixels
                + topRowFromBottom * SidebarUpgradeStackSpacingPixels
                + SidebarUpgradeRowHeight * 0.5f;
            var titleAnchorPixels = topRowCenterPixels
                + SidebarUpgradeRowHeight * 0.5f
                + SidebarUpgradesTitleGapPixels;
            return titleAnchorPixels / SidebarContentHeight;
        }

        public const float HudStatIconSize = 22f;
        public const float HudStatIconTextOffset = 0.024f;
        public const float HudBattleIconSize = 26f;

        public static Color PanelBackground => GameVisualPalette.SidePanel;
        public static Color TextPrimary => GameVisualPalette.TextPrimary;
        public static Color ButtonNormal => GameVisualPalette.ButtonNeutral;
        public static Color ButtonSelected => GameVisualPalette.ButtonSelected;
        public static Color StatusSafe => GameVisualPalette.StatusSafe;
        public static Color StatusDanger => GameVisualPalette.StatusDanger;
        public static Color StatusOccupied => GameVisualPalette.StatusOccupied;
        public static Color EventOverlay => GameVisualPalette.EventOverlay;
        public static Color EventPanelBackground => GameVisualPalette.EventPanel;
        public static Color EventChoiceButton => GameVisualPalette.EventChoice;
    }
}
