using System.Collections.Generic;
using System.Linq;
using BastionUA.Core;
using UnityEngine;

namespace BastionUA.Services
{
    public sealed class MapService
    {
        public IReadOnlyList<RegionState> GetRegions(GameState state)
        {
            return state.Regions;
        }

        public RegionState GetRegion(GameState state, string regionId)
        {
            return state.Regions.FirstOrDefault(region => region.RegionId == regionId);
        }

        public void SelectRegion(GameState state, string regionId)
        {
            var region = GetRegion(state, regionId);
            if (region == null)
            {
                Debug.LogWarning($"[MapService] Unknown region requested: {regionId}");
                return;
            }

            state.LastSelectedRegionId = regionId;
            Debug.Log($"[MapService] Region selected: {region.DisplayName} ({region.Status})");
        }

        public void SetRegionStatus(GameState state, string regionId, RegionStatus status)
        {
            var region = GetRegion(state, regionId);
            if (region == null)
            {
                Debug.LogWarning($"[MapService] Cannot update status. Unknown region: {regionId}");
                return;
            }

            var oldStatus = region.Status;
            region.Status = status;
            Debug.Log($"[MapService] {region.DisplayName}: {oldStatus} -> {status}");
        }

        public void ResetRegionsToDefaults(GameState state)
        {
            if (state?.Regions == null)
            {
                return;
            }

            RegionCatalog.EnsureAllRegions(state.Regions);

            foreach (var definition in RegionCatalog.All)
            {
                var region = GetRegion(state, definition.RegionId);
                if (region == null)
                {
                    continue;
                }

                region.DisplayName = definition.DisplayName;
                region.Status = definition.DefaultStatus;
            }
        }
    }
}
