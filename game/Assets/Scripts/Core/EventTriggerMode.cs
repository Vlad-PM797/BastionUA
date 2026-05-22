namespace BastionUA.Core
{
    public enum EventTriggerMode
    {
        OnSessionStart = 0,
        OnProgress = 1
    }

    public sealed class GameEventScheduleEntry
    {
        public string EventId;
        public EventTriggerMode TriggerMode;
        public string[] RequiredCompletedEventIds;
        public int MinBattleCount;
    }
}
