using BastionUA.Core;
using UnityEngine;

namespace BastionUA.Services
{
    public sealed class EventService
    {
        public bool CanTrigger(GameState state, EventDefinition eventDefinition)
        {
            if (state == null || eventDefinition == null || string.IsNullOrEmpty(eventDefinition.EventId))
            {
                return false;
            }

            return !state.IsEventCompleted(eventDefinition.EventId);
        }

        public bool TryApplyChoice(
            GameState state,
            MapService mapService,
            EventDefinition eventDefinition,
            int choiceIndex,
            out EventChoiceDefinition appliedChoice)
        {
            appliedChoice = null;

            if (state == null || mapService == null || eventDefinition == null)
            {
                Debug.LogWarning("[EventService] Cannot apply choice. Missing state or event.");
                return false;
            }

            if (eventDefinition.Choices == null ||
                choiceIndex < 0 ||
                choiceIndex >= eventDefinition.Choices.Count)
            {
                Debug.LogWarning($"[EventService] Invalid choice index: {choiceIndex}");
                return false;
            }

            if (state.IsEventCompleted(eventDefinition.EventId))
            {
                Debug.LogWarning($"[EventService] Event already completed: {eventDefinition.EventId}");
                return false;
            }

            appliedChoice = eventDefinition.Choices[choiceIndex];
            ApplyResourceChanges(state, appliedChoice);
            ApplyRegionChanges(state, mapService, appliedChoice);
            state.MarkEventCompleted(eventDefinition.EventId);

            Debug.Log(
                $"[EventService] Applied {eventDefinition.EventId}/{appliedChoice.ChoiceId}. " +
                $"Ammo={state.Ammo}, Morale={state.Morale}");

            return true;
        }

        private static void ApplyResourceChanges(GameState state, EventChoiceDefinition choice)
        {
            state.Ammo = Mathf.Max(GameConstants.MinAmmo, state.Ammo + choice.AmmoDelta);
            state.Morale = Mathf.Max(GameConstants.MinMorale, state.Morale + choice.MoraleDelta);
        }

        private static void ApplyRegionChanges(GameState state, MapService mapService, EventChoiceDefinition choice)
        {
            if (choice.RegionStatusStep == 0 || string.IsNullOrEmpty(choice.TargetRegionId))
            {
                return;
            }

            var region = mapService.GetRegion(state, choice.TargetRegionId);
            if (region == null)
            {
                Debug.LogWarning($"[EventService] Target region not found: {choice.TargetRegionId}");
                return;
            }

            var step = Mathf.Clamp(
                choice.RegionStatusStep,
                GameConstants.MinRegionStatusStep,
                GameConstants.MaxRegionStatusStep);

            var newStatusValue = Mathf.Clamp((int)region.Status + step, (int)RegionStatus.Safe, (int)RegionStatus.Occupied);
            var newStatus = (RegionStatus)newStatusValue;
            mapService.SetRegionStatus(state, choice.TargetRegionId, newStatus);
        }
    }
}
