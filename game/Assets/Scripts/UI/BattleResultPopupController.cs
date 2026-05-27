using System;
using System.Collections.Generic;
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
            var canvasTransform = PopupUiFactory.EnsureCanvas(GameUiConstants.BattleCanvasName, 250);
            _overlayRoot = PopupUiFactory.CreateOverlayRoot(canvasTransform, "BattleResultPopupRoot");

            var panelObject = PopupUiFactory.CreateStyledPanel(
                _overlayRoot.transform,
                GameUiConstants.BattlePanelWidth,
                GameUiConstants.BattlePanelHeight);

            PopupUiFactory.CreateBattleOutcomeStripe(panelObject.transform, result.IsVictory);
            PopupUiFactory.CreatePopupIcon(
                panelObject.transform,
                "BattlePopupIcon",
                UiIconKind.Battle,
                new Vector2(0.5f, 1f),
                new Vector2(0f, -28f),
                GameUiConstants.HudBattleIconSize);

            var title = result.IsVictory ? GameUiConstants.BattleVictoryTitle : GameUiConstants.BattleDefeatTitle;
            var titleColor = result.IsVictory
                ? GameVisualPalette.TextTitleVictory
                : GameVisualPalette.TextTitleDefeat;

            PopupUiFactory.CreateTitle(
                panelObject.transform,
                "BattleTitle",
                title,
                new Vector2(0f, -36f),
                new Vector2(640f, 48f),
                titleColor);

            PopupUiFactory.CreateBody(
                panelObject.transform,
                "BattleBody",
                BuildBodyText(result),
                new Vector2(0f, -128f),
                new Vector2(640f, 240f));

            var continueButton = PopupUiFactory.CreatePrimaryButton(
                panelObject.transform,
                "ContinueButton",
                GameUiConstants.BattleContinueButton,
                new Vector2(0.5f, 0.12f));
            continueButton.onClick.AddListener(ClosePopup);
        }

        private static string BuildBodyText(BattleResult result)
        {
            var lines = new List<string>
            {
                $"{GameUiConstants.BattleLabelRegion}: {result.RegionDisplayName}",
                $"{GameUiConstants.BattleLabelAmmoSpent}: {result.AmmoSpent}",
                $"{GameUiConstants.BattleLabelHp}: {result.PlayerHpRemaining} / {result.EnemyHpRemaining}",
                $"{GameUiConstants.BattleLabelRegionStatus}: {result.RegionStatusBefore} → {result.RegionStatusAfter}"
            };

            if (result.CombatLog != null && result.CombatLog.Count > 0)
            {
                lines.Add(string.Empty);
                lines.Add(GameUiConstants.BattleLabelCombatLog + ":");
                lines.AddRange(result.CombatLog);
            }

            return string.Join("\n", lines);
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
    }
}
