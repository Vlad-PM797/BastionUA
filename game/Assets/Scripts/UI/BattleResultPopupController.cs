using System;
using BastionUA.Bootstrap;
using BastionUA.Core;
using BastionUA.Services;
using UnityEngine;
using UnityEngine.UI;

namespace BastionUA.UI
{
    public sealed class BattleResultPopupController : MonoBehaviour
    {
        private GameBootstrap _bootstrap;
        private GameObject _overlayRoot;
        private bool _isPopupVisible;
        private Action _onClosed;

        private void Start()
        {
            try
            {
                _bootstrap = FindAnyObjectByType<GameBootstrap>();
                if (_bootstrap == null)
                {
                    Debug.LogError("[BattleResultPopupController] GameBootstrap not found.");
                }
            }
            catch (Exception exception)
            {
                Debug.LogError($"[BattleResultPopupController] Start failed: {exception}");
            }
        }

        public void ShowBattleResult(BattleResult result, Action onClosed)
        {
            if (result == null)
            {
                Debug.LogWarning("[BattleResultPopupController] Cannot show null battle result.");
                onClosed?.Invoke();
                return;
            }

            if (_isPopupVisible)
            {
                Debug.LogWarning("[BattleResultPopupController] Battle popup already visible.");
                return;
            }

            try
            {
                _onClosed = onClosed;
                BuildPopup(result);
                _isPopupVisible = true;

                if (_bootstrap == null)
                {
                    _bootstrap = FindAnyObjectByType<GameBootstrap>();
                }

                _bootstrap?.SetGameplayPaused(true);
                Debug.Log($"[BattleResultPopupController] Showing battle result: victory={result.IsVictory}");
            }
            catch (Exception exception)
            {
                Debug.LogError($"[BattleResultPopupController] Failed to show battle result: {exception}");
                _bootstrap?.SetGameplayPaused(false);
                onClosed?.Invoke();
            }
        }

        private void BuildPopup(BattleResult result)
        {
            _overlayRoot = new GameObject("BattleResultPopupRoot", typeof(RectTransform));
            _overlayRoot.transform.SetParent(CreatePopupCanvasTransform(), false);

            var overlayRect = _overlayRoot.GetComponent<RectTransform>();
            StretchFullScreen(overlayRect);

            var overlayImage = _overlayRoot.AddComponent<Image>();
            overlayImage.color = GameUiConstants.EventOverlay;

            var panelObject = new GameObject("BattleResultPanel", typeof(RectTransform), typeof(Image));
            panelObject.transform.SetParent(_overlayRoot.transform, false);

            var panelRect = panelObject.GetComponent<RectTransform>();
            panelRect.anchorMin = new Vector2(0.5f, 0.5f);
            panelRect.anchorMax = new Vector2(0.5f, 0.5f);
            panelRect.pivot = new Vector2(0.5f, 0.5f);
            panelRect.sizeDelta = new Vector2(GameUiConstants.BattlePanelWidth, GameUiConstants.BattlePanelHeight);
            panelObject.GetComponent<Image>().color = GameUiConstants.EventPanelBackground;

            var title = result.IsVictory ? GameUiConstants.BattleVictoryTitle : GameUiConstants.BattleDefeatTitle;
            CreateText(
                panelObject.transform,
                "BattleTitle",
                title,
                new Vector2(0.5f, 1f),
                new Vector2(0.5f, 1f),
                new Vector2(0f, -28f),
                new Vector2(640f, 48f),
                GameUiConstants.EventTitleFontSize,
                TextAnchor.UpperCenter);

            var body = BuildBodyText(result);
            CreateText(
                panelObject.transform,
                "BattleBody",
                body,
                new Vector2(0.5f, 1f),
                new Vector2(0.5f, 1f),
                new Vector2(0f, -120f),
                new Vector2(640f, 180f),
                GameUiConstants.EventBodyFontSize,
                TextAnchor.UpperCenter);

            CreateContinueButton(panelObject.transform);
        }

        private static string BuildBodyText(BattleResult result)
        {
            var statusLine = $"{GameUiConstants.BattleLabelRegionStatus}: {result.RegionStatusBefore} → {result.RegionStatusAfter}";
            return string.Join(
                "\n",
                $"{GameUiConstants.BattleLabelRegion}: {result.RegionDisplayName}",
                $"{GameUiConstants.BattleLabelAmmoSpent}: {result.AmmoSpent}",
                $"{GameUiConstants.BattleLabelHp}: {result.PlayerHpRemaining} / {result.EnemyHpRemaining}",
                statusLine);
        }

        private void CreateContinueButton(Transform parent)
        {
            var buttonObject = new GameObject("ContinueButton", typeof(RectTransform), typeof(Image), typeof(Button));
            buttonObject.transform.SetParent(parent, false);

            var rectTransform = buttonObject.GetComponent<RectTransform>();
            rectTransform.anchorMin = new Vector2(0.5f, 0.12f);
            rectTransform.anchorMax = new Vector2(0.5f, 0.12f);
            rectTransform.pivot = new Vector2(0.5f, 0.5f);
            rectTransform.sizeDelta = new Vector2(420f, 56f);

            buttonObject.GetComponent<Image>().color = GameUiConstants.EventChoiceButton;
            buttonObject.GetComponent<Button>().onClick.AddListener(ClosePopup);

            var labelObject = new GameObject("Label", typeof(RectTransform), typeof(Text));
            labelObject.transform.SetParent(buttonObject.transform, false);
            StretchFullScreen(labelObject.GetComponent<RectTransform>());

            var labelText = labelObject.GetComponent<Text>();
            labelText.text = GameUiConstants.BattleContinueButton;
            labelText.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            labelText.fontSize = GameUiConstants.BaseFontSize;
            labelText.color = GameUiConstants.TextPrimary;
            labelText.alignment = TextAnchor.MiddleCenter;
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

            var callback = _onClosed;
            _onClosed = null;
            callback?.Invoke();
        }

        private static Transform CreatePopupCanvasTransform()
        {
            var existingCanvas = GameObject.Find(GameUiConstants.BattleCanvasName);
            if (existingCanvas != null)
            {
                return existingCanvas.transform;
            }

            var canvasObject = new GameObject(
                GameUiConstants.BattleCanvasName,
                typeof(Canvas),
                typeof(CanvasScaler),
                typeof(GraphicRaycaster));

            var canvas = canvasObject.GetComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.sortingOrder = 250;

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
