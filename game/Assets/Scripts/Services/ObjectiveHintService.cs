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

            return GameUiConstants.ObjectiveExpandTerritory;
        }
    }
}
