using System.Collections.Generic;

namespace BastionUA.Core
{
    public static class ChornobaivkaEventCatalog
    {
        public const string EventId = "chornobaivka";
        public const string TargetRegionId = "sumy";

        public const string ChoiceStrikeId = "artillery_strike";
        public const string ChoiceConserveId = "conserve_ammo";

        public static EventDefinition Create()
        {
            return new EventDefinition
            {
                EventId = EventId,
                Title = "Чорнобаївка",
                Description = "Колона бронетехніки рухається південним напрямком. Ударити зараз чи зберегти ресурси?",
                Choices = new List<EventChoiceDefinition>
                {
                    new EventChoiceDefinition
                    {
                        ChoiceId = ChoiceStrikeId,
                        Label = "Артилерійський удар",
                        AmmoDelta = -25,
                        MoraleDelta = 12,
                        TargetRegionId = TargetRegionId,
                        RegionStatusStep = 1
                    },
                    new EventChoiceDefinition
                    {
                        ChoiceId = ChoiceConserveId,
                        Label = "Зберегти боєприпаси",
                        AmmoDelta = 10,
                        MoraleDelta = -6,
                        TargetRegionId = TargetRegionId,
                        RegionStatusStep = -1
                    }
                }
            };
        }
    }
}
