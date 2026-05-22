using System.Collections.Generic;

namespace BastionUA.Core
{
    public static class UnitCatalog
    {
        public const string TerritorialDefenseId = "territorial_defense";
        public const string ArtilleryId = "artillery";
        public const string DronesId = "drones";
        public const string MedicsId = "medics";

        private static readonly List<UnitDefinition> Units = new List<UnitDefinition>
        {
            new UnitDefinition(
                TerritorialDefenseId,
                "Territorial Defense",
                "TDF",
                damageBonus: 0,
                moraleBonusOnVictory: 3,
                ammoBudgetReduction: 0,
                enemyDamageReduction: 0),
            new UnitDefinition(
                ArtilleryId,
                "Artillery",
                "ART",
                damageBonus: 8,
                moraleBonusOnVictory: 0,
                ammoBudgetReduction: 0,
                enemyDamageReduction: 0),
            new UnitDefinition(
                DronesId,
                "Drone Recon",
                "DRN",
                damageBonus: 4,
                moraleBonusOnVictory: 0,
                ammoBudgetReduction: 10,
                enemyDamageReduction: 0),
            new UnitDefinition(
                MedicsId,
                "Medics",
                "MED",
                damageBonus: 0,
                moraleBonusOnVictory: 6,
                ammoBudgetReduction: 0,
                enemyDamageReduction: 2)
        };

        public static IReadOnlyList<UnitDefinition> All => Units;

        public static UnitDefinition GetById(string unitId)
        {
            foreach (var unit in Units)
            {
                if (unit.UnitId == unitId)
                {
                    return unit;
                }
            }

            return null;
        }

        public static UnitDefinition GetDefault()
        {
            return Units[0];
        }
    }
}
