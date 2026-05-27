using System.Collections.Generic;

namespace BastionUA.Core
{
    public static class ChernihivEventCatalog
    {
        public const string EventId = "chernihiv";
        public const string TargetRegionId = RegionCatalog.ChernihivId;

        public const string ChoiceBreakthroughId = "supply_breakthrough";
        public const string ChoiceHoldId = "hold_the_siege";

        public static EventDefinition Create()
        {
            return new EventDefinition
            {
                EventId = EventId,
                Title = "Чернігів",
                Description = "Північний фланг під облогою. Пробити коридор постачання чи утримати оборону міста?",
                Choices = new List<EventChoiceDefinition>
                {
                    new EventChoiceDefinition
                    {
                        ChoiceId = ChoiceBreakthroughId,
                        Label = "Пробити коридор постачання",
                        AmmoDelta = -22,
                        MoraleDelta = 14,
                        TargetRegionId = TargetRegionId,
                        RegionStatusStep = -1
                    },
                    new EventChoiceDefinition
                    {
                        ChoiceId = ChoiceHoldId,
                        Label = "Утримати оборону",
                        AmmoDelta = -12,
                        MoraleDelta = 9,
                        TargetRegionId = TargetRegionId,
                        RegionStatusStep = -1
                    }
                }
            };
        }
    }
}
