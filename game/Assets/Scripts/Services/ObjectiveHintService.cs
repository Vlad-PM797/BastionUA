using BastionUA.Core;

namespace BastionUA.Services
{
    public static class ObjectiveHintService
    {
        public static string GetHint(GameState state)
        {
            if (state == null)
            {
                return GameUiConstants.ObjectiveFallback;
            }

            if (!state.HasSeenOnboarding)
            {
                return GameUiConstants.OnboardingHint;
            }

            if (!state.IsEventCompleted(HostomelEventCatalog.EventId))
            {
                return GameUiConstants.ObjectiveHostomelPending;
            }

            if (!state.IsEventCompleted(ChornobaivkaEventCatalog.EventId))
            {
                if (state.TotalBattles < GameConstants.ChornobaivkaMinBattleCount)
                {
                    return GameUiConstants.ObjectiveFirstBattle;
                }

                return GameUiConstants.ObjectiveChornobaivkaPending;
            }

            if (!state.IsEventCompleted(IrpinEventCatalog.EventId))
            {
                if (state.TotalBattles < GameConstants.IrpinMinBattleCount)
                {
                    return GameUiConstants.ObjectiveSecondBattle;
                }

                return GameUiConstants.ObjectiveIrpinPending;
            }

            if (IsRegionOccupied(state, RegionCatalog.KharkivId))
            {
                return GameUiConstants.ObjectiveKharkiv;
            }

            return GameUiConstants.ObjectiveProgression;
        }

        private static bool IsRegionOccupied(GameState state, string regionId)
        {
            if (state?.Regions == null)
            {
                return false;
            }

            foreach (var region in state.Regions)
            {
                if (region.RegionId == regionId)
                {
                    return region.Status == RegionStatus.Occupied;
                }
            }

            return false;
        }
    }
}
