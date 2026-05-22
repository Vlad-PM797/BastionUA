using System.Collections.Generic;

namespace BastionUA.Core
{
    public sealed class RegionDefinition
    {
        public string RegionId;
        public string DisplayName;
        public RegionStatus DefaultStatus;
        public int EnemyHpBonus;
        public int EnemyDamageBonus;
        public MapRegionLayout Layout;

        public RegionDefinition(
            string regionId,
            string displayName,
            RegionStatus defaultStatus,
            int enemyHpBonus,
            int enemyDamageBonus,
            MapRegionLayout layout)
        {
            RegionId = regionId;
            DisplayName = displayName;
            DefaultStatus = defaultStatus;
            EnemyHpBonus = enemyHpBonus;
            EnemyDamageBonus = enemyDamageBonus;
            Layout = layout;
        }
    }

    public static class RegionCatalog
    {
        public const string KyivId = "kyiv";
        public const string ChernihivId = "chernihiv";
        public const string SumyId = "sumy";
        public const string KharkivId = "kharkiv";

        private static readonly List<RegionDefinition> Regions = new List<RegionDefinition>
        {
            new RegionDefinition(
                KyivId,
                "Kyiv",
                RegionStatus.Danger,
                enemyHpBonus: 0,
                enemyDamageBonus: 0,
                MapUiConstants.KyivLayout),
            new RegionDefinition(
                ChernihivId,
                "Chernihiv",
                RegionStatus.Occupied,
                enemyHpBonus: 10,
                enemyDamageBonus: 2,
                MapUiConstants.ChernihivLayout),
            new RegionDefinition(
                SumyId,
                "Sumy",
                RegionStatus.Danger,
                enemyHpBonus: 6,
                enemyDamageBonus: 1,
                MapUiConstants.SumyLayout),
            new RegionDefinition(
                KharkivId,
                "Kharkiv",
                RegionStatus.Occupied,
                enemyHpBonus: 18,
                enemyDamageBonus: 3,
                MapUiConstants.KharkivLayout)
        };

        public static IReadOnlyList<RegionDefinition> All => Regions;

        public static RegionDefinition GetById(string regionId)
        {
            foreach (var region in Regions)
            {
                if (region.RegionId == regionId)
                {
                    return region;
                }
            }

            return null;
        }

        public static void EnsureAllRegions(List<RegionState> regions)
        {
            if (regions == null)
            {
                return;
            }

            foreach (var definition in Regions)
            {
                var existing = regions.Find(region => region.RegionId == definition.RegionId);
                if (existing != null)
                {
                    continue;
                }

                regions.Add(new RegionState(
                    definition.RegionId,
                    definition.DisplayName,
                    definition.DefaultStatus));
            }
        }
    }
}
