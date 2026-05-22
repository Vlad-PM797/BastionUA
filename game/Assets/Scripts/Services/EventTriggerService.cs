using BastionUA.Core;
using UnityEngine;

namespace BastionUA.Services
{
    public sealed class EventTriggerService
    {
        public EventDefinition GetNextEvent(GameState state, EventTriggerMode triggerMode)
        {
            if (state == null)
            {
                return null;
            }

            foreach (var entry in GameEventRegistry.GetSchedule())
            {
                if (entry.TriggerMode != triggerMode)
                {
                    continue;
                }

                if (!IsEntryReady(state, entry))
                {
                    continue;
                }

                var definition = GameEventRegistry.CreateDefinition(entry.EventId);
                if (definition == null)
                {
                    Debug.LogWarning($"[EventTriggerService] Missing definition for event: {entry.EventId}");
                    continue;
                }

                return definition;
            }

            return null;
        }

        private static bool IsEntryReady(GameState state, GameEventScheduleEntry entry)
        {
            if (state.IsEventCompleted(entry.EventId))
            {
                return false;
            }

            if (state.TotalBattles < entry.MinBattleCount)
            {
                return false;
            }

            if (entry.RequiredCompletedEventIds == null)
            {
                return true;
            }

            foreach (var requiredEventId in entry.RequiredCompletedEventIds)
            {
                if (string.IsNullOrEmpty(requiredEventId))
                {
                    continue;
                }

                if (!state.IsEventCompleted(requiredEventId))
                {
                    return false;
                }
            }

            return true;
        }
    }
}
