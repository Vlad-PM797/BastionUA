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

            if (Regions == null || Regions.Count == 0)
            {
                Regions = CreateDefault().Regions;
            }
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
            return new GameState
            {
                Ammo = GameConstants.InitialAmmo,
                Morale = GameConstants.InitialMorale,
                LastSelectedRegionId = "kyiv",
                LastSavedUtc = DateTime.UtcNow,
                Regions = new List<RegionState>
                {
                    new RegionState("kyiv", "Kyiv", RegionStatus.Danger),
                    new RegionState("chernihiv", "Chernihiv", RegionStatus.Occupied),
                    new RegionState("sumy", "Sumy", RegionStatus.Danger)
                }
            };
        }
    }
}
