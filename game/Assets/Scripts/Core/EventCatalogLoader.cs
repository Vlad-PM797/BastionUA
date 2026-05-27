using System;
using System.Collections.Generic;
using UnityEngine;

namespace BastionUA.Core
{
    public sealed class EventContentCache
    {
        public IReadOnlyList<GameEventScheduleEntry> Schedule { get; }
        public IReadOnlyList<string> PrestigeRequiredEventIds { get; }
        private readonly Dictionary<string, EventDefinition> _definitionsById;

        public EventContentCache(
            IReadOnlyList<GameEventScheduleEntry> schedule,
            IReadOnlyList<string> prestigeRequiredEventIds,
            Dictionary<string, EventDefinition> definitionsById)
        {
            Schedule = schedule;
            PrestigeRequiredEventIds = prestigeRequiredEventIds;
            _definitionsById = definitionsById;
        }

        public EventDefinition GetDefinition(string eventId)
        {
            if (string.IsNullOrWhiteSpace(eventId))
            {
                return null;
            }

            return _definitionsById.TryGetValue(eventId, out var definition) ? definition : null;
        }
    }

    public sealed class EventCatalogLoader
    {
        public EventContentCache Load()
        {
            try
            {
                var textAsset = Resources.Load<TextAsset>(GameConstants.EventContentResourcePath);
                if (textAsset == null)
                {
                    Debug.LogWarning(
                        $"[EventCatalogLoader] Missing resource '{GameConstants.EventContentResourcePath}'. Using catalog fallback.");
                    return BuildCacheFromFallbackBundle();
                }

                var bundle = JsonUtility.FromJson<EventContentBundle>(textAsset.text);
                if (!TryValidateBundle(bundle, out var validationMessage))
                {
                    Debug.LogWarning(
                        $"[EventCatalogLoader] Invalid event JSON: {validationMessage}. Using catalog fallback.");
                    return BuildCacheFromFallbackBundle();
                }

                var cache = BuildCache(bundle);
                Debug.Log(
                    $"[EventCatalogLoader] Loaded {cache.Schedule.Count} schedule entries and " +
                    $"{cache.PrestigeRequiredEventIds.Count} prestige-required events from JSON.");
                return cache;
            }
            catch (Exception exception)
            {
                Debug.LogError($"[EventCatalogLoader] Load failed: {exception}. Using catalog fallback.");
                return BuildCacheFromFallbackBundle();
            }
        }

        public static EventContentBundle BuildFallbackBundle()
        {
            return new EventContentBundle
            {
                schedule = new List<GameEventScheduleEntry>
                {
                    new GameEventScheduleEntry
                    {
                        EventId = HostomelEventCatalog.EventId,
                        TriggerMode = EventTriggerMode.OnSessionStart,
                        RequiredCompletedEventIds = Array.Empty<string>(),
                        MinBattleCount = 0,
                        RequiredForPrestige = true
                    },
                    new GameEventScheduleEntry
                    {
                        EventId = ChornobaivkaEventCatalog.EventId,
                        TriggerMode = EventTriggerMode.OnProgress,
                        RequiredCompletedEventIds = new[] { HostomelEventCatalog.EventId },
                        MinBattleCount = GameConstants.ChornobaivkaMinBattleCount,
                        RequiredForPrestige = true
                    },
                    new GameEventScheduleEntry
                    {
                        EventId = IrpinEventCatalog.EventId,
                        TriggerMode = EventTriggerMode.OnProgress,
                        RequiredCompletedEventIds = new[] { ChornobaivkaEventCatalog.EventId },
                        MinBattleCount = GameConstants.IrpinMinBattleCount,
                        RequiredForPrestige = true
                    },
                    new GameEventScheduleEntry
                    {
                        EventId = KharkivEventCatalog.EventId,
                        TriggerMode = EventTriggerMode.OnProgress,
                        RequiredCompletedEventIds = new[] { IrpinEventCatalog.EventId },
                        MinBattleCount = GameConstants.KharkivMinBattleCount,
                        RequiredForPrestige = true
                    },
                    new GameEventScheduleEntry
                    {
                        EventId = ChernihivEventCatalog.EventId,
                        TriggerMode = EventTriggerMode.OnProgress,
                        RequiredCompletedEventIds = new[] { KharkivEventCatalog.EventId },
                        MinBattleCount = GameConstants.ChernihivMinBattleCount,
                        RequiredForPrestige = true
                    }
                },
                definitions = new List<EventDefinition>
                {
                    HostomelEventCatalog.Create(),
                    ChornobaivkaEventCatalog.Create(),
                    IrpinEventCatalog.Create(),
                    KharkivEventCatalog.Create(),
                    ChernihivEventCatalog.Create()
                }
            };
        }

        private static EventContentCache BuildCacheFromFallbackBundle()
        {
            return BuildCache(BuildFallbackBundle());
        }

        private static EventContentCache BuildCache(EventContentBundle bundle)
        {
            var definitionsById = new Dictionary<string, EventDefinition>(StringComparer.Ordinal);
            foreach (var definition in bundle.definitions)
            {
                if (definition == null || string.IsNullOrWhiteSpace(definition.EventId))
                {
                    continue;
                }

                definitionsById[definition.EventId] = definition;
            }

            var prestigeRequiredEventIds = new List<string>();
            foreach (var entry in bundle.schedule)
            {
                if (entry == null || string.IsNullOrWhiteSpace(entry.EventId))
                {
                    continue;
                }

                if (entry.RequiredForPrestige)
                {
                    prestigeRequiredEventIds.Add(entry.EventId);
                }
            }

            return new EventContentCache(bundle.schedule, prestigeRequiredEventIds, definitionsById);
        }

        private static bool TryValidateBundle(EventContentBundle bundle, out string message)
        {
            message = string.Empty;
            if (bundle == null)
            {
                message = "bundle is null";
                return false;
            }

            if (bundle.schedule == null || bundle.schedule.Count == 0)
            {
                message = "schedule is empty";
                return false;
            }

            if (bundle.definitions == null || bundle.definitions.Count == 0)
            {
                message = "definitions are empty";
                return false;
            }

            var definitionIds = new HashSet<string>(StringComparer.Ordinal);
            foreach (var definition in bundle.definitions)
            {
                if (definition == null || string.IsNullOrWhiteSpace(definition.EventId))
                {
                    message = "definition missing EventId";
                    return false;
                }

                if (!definitionIds.Add(definition.EventId))
                {
                    message = $"duplicate definition id '{definition.EventId}'";
                    return false;
                }

                if (definition.Choices == null || definition.Choices.Count < 2)
                {
                    message = $"event '{definition.EventId}' needs at least two choices";
                    return false;
                }
            }

            var scheduleIds = new HashSet<string>(StringComparer.Ordinal);
            foreach (var entry in bundle.schedule)
            {
                if (entry == null || string.IsNullOrWhiteSpace(entry.EventId))
                {
                    message = "schedule entry missing EventId";
                    return false;
                }

                if (!scheduleIds.Add(entry.EventId))
                {
                    message = $"duplicate schedule id '{entry.EventId}'";
                    return false;
                }

                if (!definitionIds.Contains(entry.EventId))
                {
                    message = $"schedule entry '{entry.EventId}' has no definition";
                    return false;
                }

                if (entry.RequiredCompletedEventIds == null)
                {
                    entry.RequiredCompletedEventIds = Array.Empty<string>();
                }
            }

            return true;
        }
    }
}
