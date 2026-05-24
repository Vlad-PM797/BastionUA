using System;
using System.Collections.Generic;

namespace BastionUA.Core
{
    [Serializable]
    public enum RegionStatus
    {
        Safe = 0,
        Danger = 1,
        Occupied = 2
    }

    [Serializable]
    public sealed class RegionState
    {
        public string RegionId;
        public string DisplayName;
        public RegionStatus Status;

        public RegionState(string regionId, string displayName, RegionStatus status)
        {
            RegionId = regionId;
            DisplayName = displayName;
            Status = status;
        }
    }

    [Serializable]
    public sealed class UpgradeProgressState
    {
        public string UpgradeId;
        public int Level;

        public UpgradeProgressState(string upgradeId, int level)
        {
            UpgradeId = upgradeId;
            Level = level;
        }
    }

    [Serializable]
    public sealed class GameState
    {
        public int Ammo;
        public int Morale;
        public List<RegionState> Regions = new List<RegionState>();
        public string LastSelectedRegionId;
        public DateTime LastSavedUtc;
        public List<string> CompletedEventIds = new List<string>();
        public int TotalBattles;
        public bool HasSeenOnboarding;
        public string SelectedUnitId;
        public List<UpgradeProgressState> UpgradeLevels = new List<UpgradeProgressState>();
        public int PrestigeLevel;

        public void Normalize()
        {
            if (CompletedEventIds == null)
            {
                CompletedEventIds = new List<string>();
            }

            if (TotalBattles < 0)
            {
                TotalBattles = 0;
            }

            if (PrestigeLevel < 0)
            {
                PrestigeLevel = 0;
            }

            if (PrestigeLevel > GameConstants.MaxPrestigeLevel)
            {
                PrestigeLevel = GameConstants.MaxPrestigeLevel;
            }

            if (Regions == null || Regions.Count == 0)
            {
                Regions = CreateDefault().Regions;
            }

            RegionCatalog.EnsureAllRegions(Regions);

            if (string.IsNullOrEmpty(SelectedUnitId) || UnitCatalog.GetById(SelectedUnitId) == null)
            {
                SelectedUnitId = UnitCatalog.TerritorialDefenseId;
            }

            if (UpgradeLevels == null)
            {
                UpgradeLevels = new List<UpgradeProgressState>();
            }

            foreach (var upgrade in UpgradeCatalog.All)
            {
                if (GetUpgradeLevel(upgrade.UpgradeId) < 0)
                {
                    SetUpgradeLevel(upgrade.UpgradeId, 0);
                }
            }
        }

        public int GetUpgradeLevel(string upgradeId)
        {
            if (UpgradeLevels == null)
            {
                return 0;
            }

            foreach (var entry in UpgradeLevels)
            {
                if (entry.UpgradeId == upgradeId)
                {
                    return entry.Level;
                }
            }

            return 0;
        }

        public void SetUpgradeLevel(string upgradeId, int level)
        {
            if (UpgradeLevels == null)
            {
                UpgradeLevels = new List<UpgradeProgressState>();
            }

            for (var index = 0; index < UpgradeLevels.Count; index++)
            {
                if (UpgradeLevels[index].UpgradeId == upgradeId)
                {
                    UpgradeLevels[index].Level = level;
                    return;
                }
            }

            UpgradeLevels.Add(new UpgradeProgressState(upgradeId, level));
        }

        public bool IsEventCompleted(string eventId)
        {
            return CompletedEventIds != null && CompletedEventIds.Contains(eventId);
        }

        public void MarkEventCompleted(string eventId)
        {
            if (CompletedEventIds == null)
            {
                CompletedEventIds = new List<string>();
            }

            if (!CompletedEventIds.Contains(eventId))
            {
                CompletedEventIds.Add(eventId);
            }
        }

        public static GameState CreateDefault()
        {
            var defaultState = new GameState
            {
                Ammo = GameConstants.InitialAmmo,
                Morale = GameConstants.InitialMorale,
                LastSelectedRegionId = RegionCatalog.KyivId,
                LastSavedUtc = DateTime.UtcNow,
                SelectedUnitId = UnitCatalog.TerritorialDefenseId,
                Regions = new List<RegionState>()
            };

            RegionCatalog.EnsureAllRegions(defaultState.Regions);
            return defaultState;
        }
    }
}
