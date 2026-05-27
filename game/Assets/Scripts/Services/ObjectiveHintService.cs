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

            if (!state.IsEventCompleted(KharkivEventCatalog.EventId))
            {
                if (state.TotalBattles < GameConstants.KharkivMinBattleCount)
                {
                    return GameUiConstants.ObjectiveThirdBattle;
                }

                return GameUiConstants.ObjectiveKharkivEventPending;
            }

            if (!state.IsEventCompleted(ChernihivEventCatalog.EventId))
            {
                if (state.TotalBattles < GameConstants.ChernihivMinBattleCount)
                {
                    return GameUiConstants.ObjectiveFourthBattle;
                }

                return GameUiConstants.ObjectiveChernihivPending;
            }

            if (HasOccupiedRegion(state))
            {
                return GameUiConstants.ObjectiveLiberateRegions;
            }

            if (CanPrestige(state))
            {
                return GameUiConstants.ObjectivePrestigeReady;
            }

            if (state.PrestigeLevel >= GameConstants.MaxPrestigeLevel)
            {
                return GameUiConstants.ObjectivePrestigeMax;
            }

            return GameUiConstants.ObjectiveProgression;
        }

        private static bool CanPrestige(GameState state)
        {
            return new PrestigeService().CanPrestige(state);
        }

        private static bool HasOccupiedRegion(GameState state)
        {
            if (state?.Regions == null)
            {
                return false;
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
    }
}
