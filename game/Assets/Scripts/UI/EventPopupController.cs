using System;
using System.Collections;
using BastionUA.Bootstrap;
using BastionUA.Core;
using UnityEngine;
using UnityEngine.UI;

namespace BastionUA.UI
{
    public sealed class EventPopupController : MonoBehaviour
    {
        private GameBootstrap _bootstrap;
        private GameObject _overlayRoot;
        private bool _isPopupVisible;

        private void Start()
        {
            try
            {
                _bootstrap = FindAnyObjectByType<GameBootstrap>();
                if (_bootstrap == null)
                {
                    Debug.LogError("[EventPopupController] GameBootstrap not found.");
                }
            }
            catch (Exception exception)
            {
                Debug.LogError($"[EventPopupController] Start failed: {exception}");
            }
        }

        public void QueueEvent(EventDefinition eventDefinition, float delaySeconds)
        {
            if (eventDefinition == null)
            {
                Debug.LogWarning("[EventPopupController] Cannot queue null event.");
                return;
            }

            StartCoroutine(ShowEventAfterDelay(eventDefinition, delaySeconds));
        }

        private IEnumerator ShowEventAfterDelay(EventDefinition eventDefinition, float delaySeconds)
        {
            yield return new WaitForSeconds(delaySeconds);

            if (_bootstrap == null)
            {
                _bootstrap = FindAnyObjectByType<GameBootstrap>();
            }

            if (_bootstrap == null || !_bootstrap.CanTriggerEvent(eventDefinition))
            {
                yield break;
            }

            ShowEvent(eventDefinition);
        }

        private void ShowEvent(EventDefinition eventDefinition)
        {
            if (_isPopupVisible)
            {
                Debug.LogWarning("[EventPopupController] Event popup already visible.");
                return;
            }

            try
            {
                BuildPopup(eventDefinition);
                _isPopupVisible = true;
                _bootstrap.SetGameplayPaused(true);
                Debug.Log($"[EventPopupController] Showing event: {eventDefinition.EventId}");
            }
            catch (Exception exception)
            {
                Debug.LogError($"[EventPopupController] Failed to show event: {exception}");
                _bootstrap.SetGameplayPaused(false);
            }
        }

        private void BuildPopup(EventDefinition eventDefinition)
        {
            var canvasTransform = PopupUiFactory.EnsureCanvas(GameUiConstants.EventCanvasName, 200);
            _overlayRoot = PopupUiFactory.CreateOverlayRoot(canvasTransform, "EventPopupRoot");

            var panelObject = PopupUiFactory.CreateStyledPanel(
                _overlayRoot.transform,
                GameUiConstants.EventPanelWidth,
                GameUiConstants.EventPanelHeight);

            PopupUiFactory.CreateEventBanner(panelObject.transform, eventDefinition.EventId);

            PopupUiFactory.CreateTitle(
                panelObject.transform,
                "EventTitle",
                eventDefinition.Title,
                new Vector2(0f, GameUiConstants.EventTitleOffsetY),
                new Vector2(680f, 48f),
                GameVisualPalette.TextAccent);

            PopupUiFactory.CreateBody(
                panelObject.transform,
                "EventDescription",
                eventDefinition.Description,
                new Vector2(0f, GameUiConstants.EventBodyOffsetY),
                new Vector2(680f, 130f));

            if (eventDefinition.Choices == null || eventDefinition.Choices.Count == 0)
            {
                return;
            }

            CreateChoiceButton(panelObject.transform, eventDefinition, 0, new Vector2(0.5f, 0.22f));

            if (eventDefinition.Choices.Count > 1)
            {
                CreateChoiceButton(panelObject.transform, eventDefinition, 1, new Vector2(0.5f, 0.1f));
            }
        }

        private void CreateChoiceButton(
            Transform parent,
            EventDefinition eventDefinition,
            int choiceIndex,
            Vector2 anchor)
        {
            var choice = eventDefinition.Choices[choiceIndex];
            var button = PopupUiFactory.CreateChoiceButton(
                parent,
                $"Choice_{choice.ChoiceId}",
                choice.Label,
                anchor);
            button.onClick.AddListener(() => OnChoiceSelected(eventDefinition, choiceIndex));
        }

        private void OnChoiceSelected(EventDefinition eventDefinition, int choiceIndex)
        {
            try
            {
                if (_bootstrap != null)
                {
                    _bootstrap.ApplyEventChoice(eventDefinition, choiceIndex);
                }
            }
            catch (Exception exception)
            {
                Debug.LogError($"[EventPopupController] Choice apply failed: {exception}");
            }
            finally
            {
                ClosePopup();
            }
        }

        private void ClosePopup()
        {
            if (_overlayRoot != null)
            {
                Destroy(_overlayRoot);
                _overlayRoot = null;
            }

            _isPopupVisible = false;

            if (_bootstrap != null)
            {
                _bootstrap.SetGameplayPaused(false);
            }
        }
    }
}
