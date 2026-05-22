using System.Collections.Generic;

namespace BastionUA.Core
{
    public static class HostomelEventCatalog
    {
        public const string EventId = "hostomel";
        public const string TargetRegionId = "kyiv";

        public const string ChoiceHoldId = "hold_airport";
        public const string ChoiceRetreatId = "tactical_retreat";

        public static EventDefinition Create()
        {
            return new EventDefinition
            {
                EventId = EventId,
                Title = "Гостомель",
                Description = "Ворог намагається захопити аеропорт. Командування чекає на твоє рішення.",
                Choices = new List<EventChoiceDefinition>
                {
                    new EventChoiceDefinition
                    {
                        ChoiceId = ChoiceHoldId,
                        Label = "Утримати позиції",
                        AmmoDelta = -20,
                        MoraleDelta = 10,
                        TargetRegionId = TargetRegionId,
                        RegionStatusStep = 1
                    },
                    new EventChoiceDefinition
                    {
                        ChoiceId = ChoiceRetreatId,
                        Label = "Відступити та перегрупуватись",
                        AmmoDelta = 15,
                        MoraleDelta = -8,
                        TargetRegionId = TargetRegionId,
                        RegionStatusStep = -1
                    }
                }
            };
        }
    }
}
