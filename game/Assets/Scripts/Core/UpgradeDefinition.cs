namespace BastionUA.Core
{
    public sealed class UpgradeDefinition
    {
        public string UpgradeId;
        public string DisplayName;
        public int MaxLevel;
        public int BaseCost;
        public int DamagePerLevel;
        public int MoralePerLevel;
        public int AmmoBudgetReductionPerLevel;

        public UpgradeDefinition(
            string upgradeId,
            string displayName,
            int maxLevel,
            int baseCost,
            int damagePerLevel,
            int moralePerLevel,
            int ammoBudgetReductionPerLevel)
        {
            UpgradeId = upgradeId;
            DisplayName = displayName;
            MaxLevel = maxLevel;
            BaseCost = baseCost;
            DamagePerLevel = damagePerLevel;
            MoralePerLevel = moralePerLevel;
            AmmoBudgetReductionPerLevel = ammoBudgetReductionPerLevel;
        }

        public int GetCostForNextLevel(int currentLevel)
        {
            return BaseCost + (currentLevel * BaseCost / 2);
        }
    }
}
