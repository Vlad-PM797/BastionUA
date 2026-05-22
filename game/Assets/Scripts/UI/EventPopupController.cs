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
            _overlayRoot = new GameObject("EventPopupRoot", typeof(RectTransform));
            _overlayRoot.transform.SetParent(CreateEventCanvasTransform(), false);

            var overlayRect = _overlayRoot.GetComponent<RectTransform>();
            StretchFullScreen(overlayRect);

            var overlayImage = _overlayRoot.AddComponent<Image>();
            overlayImage.color = GameUiConstants.EventOverlay;

            var panelObject = new GameObject("EventPanel", typeof(RectTransform), typeof(Image));
            panelObject.transform.SetParent(_overlayRoot.transform, false);

            var panelRect = panelObject.GetComponent<RectTransform>();
            panelRect.anchorMin = new Vector2(0.5f, 0.5f);
            panelRect.anchorMax = new Vector2(0.5f, 0.5f);
            panelRect.pivot = new Vector2(0.5f, 0.5f);
            panelRect.sizeDelta = new Vector2(GameUiConstants.EventPanelWidth, GameUiConstants.EventPanelHeight);

            panelObject.GetComponent<Image>().color = GameUiConstants.EventPanelBackground;

            CreateText(
                panelObject.transform,
                "EventTitle",
                eventDefinition.Title,
                new Vector2(0.5f, 1f),
                new Vector2(0.5f, 1f),
                new Vector2(0f, -28f),
                new Vector2(680f, 48f),
                GameUiConstants.EventTitleFontSize,
                TextAnchor.UpperCenter);

            CreateText(
                panelObject.transform,
                "EventDescription",
                eventDefinition.Description,
                new Vector2(0.5f, 1f),
                new Vector2(0.5f, 1f),
                new Vector2(0f, -110f),
                new Vector2(680f, 140f),
                GameUiConstants.EventBodyFontSize,
                TextAnchor.UpperCenter);

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
            var buttonObject = new GameObject($"Choice_{choice.ChoiceId}", typeof(RectTransform), typeof(Image), typeof(Button));
            buttonObject.transform.SetParent(parent, false);

            var rectTransform = buttonObject.GetComponent<RectTransform>();
            rectTransform.anchorMin = anchor;
            rectTransform.anchorMax = anchor;
            rectTransform.pivot = new Vector2(0.5f, 0.5f);
            rectTransform.sizeDelta = new Vector2(640f, 56f);

            buttonObject.GetComponent<Image>().color = GameUiConstants.EventChoiceButton;

            var button = buttonObject.GetComponent<Button>();
            button.onClick.AddListener(() => OnChoiceSelected(eventDefinition, choiceIndex));

            var labelObject = new GameObject("Label", typeof(RectTransform), typeof(Text));
            labelObject.transform.SetParent(buttonObject.transform, false);

            var labelRect = labelObject.GetComponent<RectTransform>();
            StretchFullScreen(labelRect);

            var labelText = labelObject.GetComponent<Text>();
            labelText.text = choice.Label;
            labelText.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            labelText.fontSize = GameUiConstants.BaseFontSize;
            labelText.color = GameUiConstants.TextPrimary;
            labelText.alignment = TextAnchor.MiddleCenter;
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

        private static Transform CreateEventCanvasTransform()
        {
            var existingCanvas = GameObject.Find(GameUiConstants.EventCanvasName);
            if (existingCanvas != null)
            {
                return existingCanvas.transform;
            }

            var canvasObject = new GameObject(
                GameUiConstants.EventCanvasName,
                typeof(Canvas),
                typeof(CanvasScaler),
                typeof(GraphicRaycaster));

            var canvas = canvasObject.GetComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.sortingOrder = 200;

            var scaler = canvasObject.GetComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(GameUiConstants.ReferenceWidth, GameUiConstants.ReferenceHeight);
            scaler.matchWidthOrHeight = 0.5f;

            return canvasObject.transform;
        }

        private static void CreateText(
            Transform parent,
            string name,
            string content,
            Vector2 anchorMin,
            Vector2 anchorMax,
            Vector2 anchoredPosition,
            Vector2 sizeDelta,
            int fontSize,
            TextAnchor alignment)
        {
            var textObject = new GameObject(name, typeof(RectTransform), typeof(Text));
            textObject.transform.SetParent(parent, false);

            var rectTransform = textObject.GetComponent<RectTransform>();
            rectTransform.anchorMin = anchorMin;
            rectTransform.anchorMax = anchorMax;
            rectTransform.pivot = new Vector2(0.5f, 1f);
            rectTransform.anchoredPosition = anchoredPosition;
            rectTransform.sizeDelta = sizeDelta;

            var text = textObject.GetComponent<Text>();
            text.text = content;
            text.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            text.fontSize = fontSize;
            text.color = GameUiConstants.TextPrimary;
            text.alignment = alignment;
            text.horizontalOverflow = HorizontalWrapMode.Wrap;
            text.verticalOverflow = VerticalWrapMode.Overflow;
        }

        private static void StretchFullScreen(RectTransform rectTransform)
        {
            rectTransform.anchorMin = Vector2.zero;
            rectTransform.anchorMax = Vector2.one;
            rectTransform.offsetMin = Vector2.zero;
            rectTransform.offsetMax = Vector2.zero;
        }
    }
}
