using System;
using System.Collections.Generic;

namespace BastionUA.Core
{
    [Serializable]
    public sealed class EventContentBundle
    {
        public List<GameEventScheduleEntry> schedule = new List<GameEventScheduleEntry>();
        public List<EventDefinition> definitions = new List<EventDefinition>();
    }
}
