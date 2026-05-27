using System.Collections.Generic;

namespace BastionUA.Core
{
    public static class GameEventRegistry
    {
        private static EventContentCache _cache;
        private static readonly EventCatalogLoader Loader = new EventCatalogLoader();

        public static void EnsureLoaded()
        {
            _ = GetCache();
        }

        public static IReadOnlyList<GameEventScheduleEntry> GetSchedule()
        {
            return GetCache().Schedule;
        }

        public static IReadOnlyList<string> GetPrestigeRequiredEventIds()
        {
            return GetCache().PrestigeRequiredEventIds;
        }

        public static EventDefinition CreateDefinition(string eventId)
        {
            return GetCache().GetDefinition(eventId);
        }

        private static EventContentCache GetCache()
        {
            if (_cache == null)
            {
                _cache = Loader.Load();
            }

            return _cache;
        }
    }
}
