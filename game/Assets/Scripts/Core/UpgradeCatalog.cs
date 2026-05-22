using System.Collections.Generic;

namespace BastionUA.Core
{
    public static class UpgradeCatalog
    {
        public const string FireTrainingId = "fire_training";
        public const string AmmoLogisticsId = "ammo_logistics";
        public const string MoraleRadioId = "morale_radio";

        private static readonly List<UpgradeDefinition> Upgrades = new List<UpgradeDefinition>
        {
            new UpgradeDefinition(
                FireTrainingId,
                "Fire Training",
                maxLevel: 3,
                baseCost: 50,
                damagePerLevel: 2,
                moralePerLevel: 0,
                ammoBudgetReductionPerLevel: 0),
            new UpgradeDefinition(
                AmmoLogisticsId,
                "Ammo Logistics",
                maxLevel: 3,
                baseCost: 40,
                damagePerLevel: 0,
                moralePerLevel: 0,
                ammoBudgetReductionPerLevel: 3),
            new UpgradeDefinition(
                MoraleRadioId,
                "Morale Radio",
                maxLevel: 3,
                baseCost: 35,
                damagePerLevel: 0,
                moralePerLevel: 2,
                ammoBudgetReductionPerLevel: 0)
        };

        public static IReadOnlyList<UpgradeDefinition> All => Upgrades;

        public static UpgradeDefinition GetById(string upgradeId)
        {
            foreach (var upgrade in Upgrades)
            {
                if (upgrade.UpgradeId == upgradeId)
                {
                    return upgrade;
                }
            }

            return null;
        }
    }
}
