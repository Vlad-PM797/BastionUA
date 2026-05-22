namespace BastionUA.Core
{
    public sealed class UnitDefinition
    {
        public string UnitId;
        public string DisplayName;
        public string ShortLabel;
        public int DamageBonus;
        public int MoraleBonusOnVictory;
        public int AmmoBudgetReduction;
        public int EnemyDamageReduction;

        public UnitDefinition(
            string unitId,
            string displayName,
            string shortLabel,
            int damageBonus,
            int moraleBonusOnVictory,
            int ammoBudgetReduction,
            int enemyDamageReduction)
        {
            UnitId = unitId;
            DisplayName = displayName;
            ShortLabel = shortLabel;
            DamageBonus = damageBonus;
            MoraleBonusOnVictory = moraleBonusOnVictory;
            AmmoBudgetReduction = ammoBudgetReduction;
            EnemyDamageReduction = enemyDamageReduction;
        }
    }
}
