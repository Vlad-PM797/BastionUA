using System;

namespace BastionUA.Core
{
    public enum EventTriggerMode
    {
        OnSessionStart = 0,
        OnProgress = 1
    }

    [Serializable]
    public sealed class GameEventScheduleEntry
    {
        public string EventId;
        public EventTriggerMode TriggerMode;
        public string[] RequiredCompletedEventIds;
        public int MinBattleCount;
        public bool RequiredForPrestige = true;
    }
}
