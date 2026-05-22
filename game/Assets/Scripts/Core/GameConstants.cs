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

        public const int BattleBaseEnemyHp = 120;
        public const int BattleBaseEnemyDamage = 8;
        public const int BattleBasePlayerHp = 140;
        public const int BattleBasePlayerDamage = 10;
        public const int BattleMaxAmmoBudget = 50;
        public const int BattleMinAmmoBudget = 10;
        public const int BattleVictoryMoraleGain = 5;
        public const int BattleDefeatMoraleLoss = 3;
        public const int BattleMaxRounds = 20;
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
    }
}
