using System.Collections.Generic;

namespace BastionUA.Core
{
    public static class KharkivEventCatalog
    {
        public const string EventId = "kharkiv";
        public const string TargetRegionId = RegionCatalog.KharkivId;

        public const string ChoiceCounterattackId = "counterattack";
        public const string ChoiceFortifyId = "fortify_defense";

        public static EventDefinition Create()
        {
            return new EventDefinition
            {
                EventId = EventId,
                Title = "\u0425\u0430\u0440\u043a\u0456\u0432",
                Description =
                    "\u041e\u043a\u0443\u043f\u0430\u043d\u0442\u0438 \u0442\u0438\u0441\u043d\u0443\u0442\u044c \u043d\u0430 \u0441\u0445\u0456\u0434. " +
                    "\u041a\u043e\u043d\u0442\u0440\u0430\u0442\u0430\u043a\u0443\u0432\u0430\u0442\u0438 \u0447\u0438 \u0437\u043c\u0456\u0446\u043d\u0438\u0442\u0438 \u043e\u0431\u043e\u0440\u043e\u043d\u0443 \u043c\u0456\u0441\u0442\u0430?",
                Choices = new List<EventChoiceDefinition>
                {
                    new EventChoiceDefinition
                    {
                        ChoiceId = ChoiceCounterattackId,
                        Label = "\u041a\u043e\u043d\u0442\u0440\u0430\u0442\u0430\u043a\u0430 \u043d\u0430 \u0444\u043b\u0430\u043d\u0433\u0456",
                        AmmoDelta = -30,
                        MoraleDelta = 12,
                        TargetRegionId = TargetRegionId,
                        RegionStatusStep = 1
                    },
                    new EventChoiceDefinition
                    {
                        ChoiceId = ChoiceFortifyId,
                        Label = "\u0423\u043a\u0440\u0456\u043f\u0438\u0442\u0438 \u043e\u0431\u043e\u0440\u043e\u043d\u0443",
                        AmmoDelta = -15,
                        MoraleDelta = 6,
                        TargetRegionId = TargetRegionId,
                        RegionStatusStep = 1
                    }
                }
            };
        }
    }
}
