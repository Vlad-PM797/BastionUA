namespace BastionUA.Core
{
    public static class GameConstants
    {
        public const int InitialAmmo = 100;
        public const int InitialMorale = 50;

        public const int AutoTickAmmoGain = 1;
        public const float AutoTickSeconds = 1.0f;
        public const int TapAmmoGain = 5;

        public const string SaveFileName = "bastionua_save.json";
        public const string EventContentResourcePath = "Events/story_events";
        public const string UiDisplayFontResourcePath = "Fonts/Exo2-Variable";

        public const int BattleBaseEnemyHp = 120;
        public const int BattleBaseEnemyDamage = 8;
        public const int BattleBasePlayerHp = 140;
        public const int BattleBasePlayerDamage = 10;
        public const int BattleMaxAmmoBudget = 50;
        public const int BattleMinAmmoBudget = 10;
        public const int BattleVictoryMoraleGain = 5;
        public const int BattleDefeatMoraleLoss = 3;
        public const int BattleMaxRounds = 20;
        public const int BattleLogMaxLines = 5;
        public const int BattleAmmoDamageDivisor = 5;
        public const int BattleOccupiedEnemyHpBonus = 12;
        public const int BattleOccupiedEnemyDamageBonus = 4;
        public const int BattleMinEnemyDamage = 1;
        public const int BattleLowMoraleThreshold = 30;
        public const int BattleHighMoraleThreshold = 70;
        public const int BattleLowMoraleDamagePercent = 90;
        public const int BattleHighMoraleDamageBonus = 2;
        public const int BattleProgressionBattlesStep = 4;
        public const int BattleProgressionHpPerStep = 3;
        public const int BattleProgressionHpCap = 15;

        public const int MinMorale = 0;
        public const int MinAmmo = 0;
        public const int MaxRegionStatusStep = 1;
        public const int MinRegionStatusStep = -1;

        public const int ChornobaivkaMinBattleCount = 1;
        public const int IrpinMinBattleCount = 2;
        public const int KharkivMinBattleCount = 3;
        public const int ChernihivMinBattleCount = 4;

        public const int MaxPrestigeLevel = 5;
        public const int PrestigeDamageBonusPerLevel = 3;
        public const int PrestigeAmmoBonusPerLevel = 20;
        public const int PrestigeMoraleBonusPerLevel = 5;

        public const int EventLogMaxEntries = 5;
        public const int EventLogDisplayLines = 3;

        public const string ScreenshotCommandLineArg = "-bastionScreenshotPath";
        public const float ScreenshotCaptureDelaySeconds = 1.5f;
        public const float ScreenshotQuitDelaySeconds = 0.75f;

        public const int SfxSampleRate = 44100;
        public const float SfxMasterVolume = 0.35f;
        public const float SfxTapFrequencyHz = 520f;
        public const float SfxTapDurationSeconds = 0.08f;
        public const float SfxTapVolume = 0.25f;
        public const float SfxVictoryFrequencyHz = 660f;
        public const float SfxVictoryDurationSeconds = 0.18f;
        public const float SfxVictoryVolume = 0.3f;
        public const float SfxDefeatFrequencyHz = 180f;
        public const float SfxDefeatDurationSeconds = 0.22f;
        public const float SfxDefeatVolume = 0.28f;
        public const float SfxEventFrequencyHz = 440f;
        public const float SfxEventDurationSeconds = 0.12f;
        public const float SfxEventVolume = 0.26f;
        public const float SfxUpgradeFrequencyHz = 780f;
        public const float SfxUpgradeDurationSeconds = 0.1f;
        public const float SfxUpgradeVolume = 0.24f;
        public const float SfxPrestigeFrequencyHz = 920f;
        public const float SfxPrestigeDurationSeconds = 0.28f;
        public const float SfxPrestigeVolume = 0.32f;

        public const string SfxResourcesFolder = "Audio";
        public const string SfxTapResourceName = "sfx_tap";
        public const string SfxVictoryResourceName = "sfx_victory";
        public const string SfxDefeatResourceName = "sfx_defeat";
        public const string SfxEventResourceName = "sfx_event";
        public const string SfxUpgradeResourceName = "sfx_upgrade";
        public const string SfxPrestigeResourceName = "sfx_prestige";

        public const string PlaytestMetricsFileName = "bastionua_playtest_metrics.json";
        public const int PlaytestMetricsMaxSessions = 20;
    }
}
