using System.Collections.Generic;

namespace BastionUA.Core
{
    public static class GameEventRegistry
    {
        private static readonly GameEventScheduleEntry[] Schedule =
        {
            new GameEventScheduleEntry
            {
                EventId = HostomelEventCatalog.EventId,
                TriggerMode = EventTriggerMode.OnSessionStart,
                RequiredCompletedEventIds = new string[0],
                MinBattleCount = 0
            },
            new GameEventScheduleEntry
            {
                EventId = ChornobaivkaEventCatalog.EventId,
                TriggerMode = EventTriggerMode.OnProgress,
                RequiredCompletedEventIds = new[] { HostomelEventCatalog.EventId },
                MinBattleCount = GameConstants.ChornobaivkaMinBattleCount
            }
        };

        public static IReadOnlyList<GameEventScheduleEntry> GetSchedule()
        {
            return Schedule;
        }

        public static EventDefinition CreateDefinition(string eventId)
        {
            if (eventId == HostomelEventCatalog.EventId)
            {
                return HostomelEventCatalog.Create();
            }

            if (eventId == ChornobaivkaEventCatalog.EventId)
            {
                return ChornobaivkaEventCatalog.Create();
            }

            return null;
        }
    }
}
