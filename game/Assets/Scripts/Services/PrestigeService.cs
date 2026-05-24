using System.Collections.Generic;
using BastionUA.Core;
using UnityEngine;

namespace BastionUA.Services
{
    public sealed class PrestigeService
    {
        private static readonly string[] RequiredEventIds =
        {
            HostomelEventCatalog.EventId,
            ChornobaivkaEventCatalog.EventId,
            IrpinEventCatalog.EventId,
            KharkivEventCatalog.EventId
        };

        public bool CanPrestige(GameState state)
        {
            if (state == null || state.PrestigeLevel >= GameConstants.MaxPrestigeLevel)
            {
                return false;
            }

            if (!AreAllStoryEventsCompleted(state))
            {
                return false;
            }

            return !HasOccupiedRegion(state);
        }

        public bool TryPrestige(GameState state, MapService mapService)
        {
            if (state == null || mapService == null)
            {
                Debug.LogWarning("[PrestigeService] Cannot prestige. Missing state or map service.");
                return false;
            }

            if (!CanPrestige(state))
            {
                Debug.Log("[PrestigeService] Prestige not available yet.");
                return false;
            }

            state.PrestigeLevel++;
            ResetCampaignProgress(state, mapService);

            Debug.Log(
                $"[PrestigeService] Prestige activated. Level={state.PrestigeLevel}, " +
                $"damage bonus={GetDamageBonus(state)}, ammo={state.Ammo}, morale={state.Morale}");
            return true;
        }

        public int GetDamageBonus(GameState state)
        {
            if (state == null || state.PrestigeLevel <= 0)
            {
                return 0;
            }

            return state.PrestigeLevel * GameConstants.PrestigeDamageBonusPerLevel;
        }

        private static bool AreAllStoryEventsCompleted(GameState state)
        {
            foreach (var eventId in RequiredEventIds)
            {
                if (!state.IsEventCompleted(eventId))
                {
                    return false;
                }
            }

            return true;
        }

        private static bool HasOccupiedRegion(GameState state)
        {
            if (state.Regions == null)
            {
                return true;
            }

            foreach (var region in state.Regions)
            {
                if (region.Status == RegionStatus.Occupied)
                {
                    return true;
                }
            }

            return false;
        }

        private static void ResetCampaignProgress(GameState state, MapService mapService)
        {
            state.CompletedEventIds = new List<string>();
            state.TotalBattles = 0;
            state.SelectedUnitId = UnitCatalog.TerritorialDefenseId;
            state.UpgradeLevels = new List<UpgradeProgressState>();
            state.HasSeenOnboarding = true;

            mapService.ResetRegionsToDefaults(state);

            var ammoBonus = state.PrestigeLevel * GameConstants.PrestigeAmmoBonusPerLevel;
            var moraleBonus = state.PrestigeLevel * GameConstants.PrestigeMoraleBonusPerLevel;
            state.Ammo = GameConstants.InitialAmmo + ammoBonus;
            state.Morale = Mathf.Min(100, GameConstants.InitialMorale + moraleBonus);
            state.LastSelectedRegionId = RegionCatalog.KyivId;
        }
    }
}
