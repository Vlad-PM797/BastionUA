using System.Collections.Generic;

namespace BastionUA.Core
{
    public static class IrpinEventCatalog
    {
        public const string EventId = "irpin";
        public const string TargetRegionId = "kyiv";

        public const string ChoiceEvacuateId = "evacuate_civilians";
        public const string ChoiceHoldId = "hold_the_line";

        public static EventDefinition Create()
        {
            return new EventDefinition
            {
                EventId = EventId,
                Title = "Ірпінь",
                Description = "Окупанти тиснуть на передмістя Києва. Евакуювати цивільних чи тримати оборону?",
                Choices = new List<EventChoiceDefinition>
                {
                    new EventChoiceDefinition
                    {
                        ChoiceId = ChoiceEvacuateId,
                        Label = "Евакуювати цивільних",
                        AmmoDelta = -15,
                        MoraleDelta = 12,
                        TargetRegionId = TargetRegionId,
                        RegionStatusStep = 1
                    },
                    new EventChoiceDefinition
                    {
                        ChoiceId = ChoiceHoldId,
                        Label = "Тримати лінію оборони",
                        AmmoDelta = -25,
                        MoraleDelta = 8,
                        TargetRegionId = TargetRegionId,
                        RegionStatusStep = 1
                    }
                }
            };
        }
    }
}
