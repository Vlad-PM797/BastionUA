using System;
using System.Collections.Generic;

namespace BastionUA.Core
{
    [Serializable]
    public sealed class EventChoiceDefinition
    {
        public string ChoiceId;
        public string Label;
        public int AmmoDelta;
        public int MoraleDelta;
        public string TargetRegionId;
        public int RegionStatusStep;
    }

    [Serializable]
    public sealed class EventDefinition
    {
        public string EventId;
        public string Title;
        public string Description;
        public List<EventChoiceDefinition> Choices = new List<EventChoiceDefinition>();
    }
}
