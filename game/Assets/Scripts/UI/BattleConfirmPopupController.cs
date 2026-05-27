using System;
using System.Collections.Generic;
using BastionUA.Bootstrap;
using BastionUA.Core;
using BastionUA.Services;
using UnityEngine;
using UnityEngine.UI;

namespace BastionUA.UI
{
    public sealed class BattleConfirmPopupController : MonoBehaviour
    {
        private GameBootstrap _bootstrap;
        private GameObject _overlayRoot;
        private bool _isPopupVisible;
        private Action _onConfirmed;
        private Action _onCancelled;

        private void Start()
        {
            try
            {
                _bootstrap = FindAnyObjectByType<GameBootstrap>();
                if (_bootstrap == null)
                {
                    Debug.LogError("[BattleConfirmPopupController] GameBootstrap not found.");
                }
            }
            catch (Exception exception)
            {
                Debug.LogError($"[BattleConfirmPopupController] Start failed: {exception}");
            }
        }

        public void ShowBattleConfirm(BattlePreview preview, Action onConfirmed, Action onCancelled)
        {
            if (preview == null)
            {
                Debug.LogWarning("[BattleConfirmPopupController] Cannot show null battle preview.");
                onCancelled?.Invoke();
                return;
            }

            if (_isPopupVisible)
            {
                Debug.LogWarning("[BattleConfirmPopupController] Confirm popup already visible.");
                return;
            }

            try
            {
                _onConfirmed = onConfirmed;
                _onCancelled = onCancelled;
                BuildPopup(preview);
                _isPopupVisible = true;

                if (_bootstrap == null)
                {
                    _bootstrap = FindAnyObjectByType<GameBootstrap>();
                }

                _bootstrap?.SetGameplayPaused(true);
                Debug.Log($"[BattleConfirmPopupController] Showing battle confirm for {preview.RegionDisplayName}.");
            }
            catch (Exception exception)
            {
                Debug.LogError($"[BattleConfirmPopupController] Failed to show confirm popup: {exception}");
                _bootstrap?.SetGameplayPaused(false);
                onCancelled?.Invoke();
            }
        }

        private void BuildPopup(BattlePreview preview)
        {
            var canvasTransform = PopupUiFactory.EnsureCanvas(GameUiConstants.BattleCanvasName, 250);
            _overlayRoot = PopupUiFactory.CreateOverlayRoot(canvasTransform, "BattleConfirmPopupRoot");

            var panelObject = PopupUiFactory.CreateStyledPanel(
                _overlayRoot.transform,
                GameUiConstants.BattleConfirmPanelWidth,
                GameUiConstants.BattleConfirmPanelHeight);

            PopupUiFactory.CreatePopupIcon(
                panelObject.transform,
                "BattleConfirmIcon",
                UiIconKind.Battle,
                new Vector2(0.5f, 1f),
                new Vector2(0f, -28f),
                GameUiConstants.HudBattleIconSize);

            PopupUiFactory.CreateTitle(
                panelObject.transform,
                "BattleConfirmTitle",
                GameUiConstants.BattleConfirmTitle,
                new Vector2(0f, -36f),
                new Vector2(640f, 48f),
                GameVisualPalette.TextPrimary);

            PopupUiFactory.CreateBody(
                panelObject.transform,
                "BattleConfirmBody",
                BuildBodyText(preview),
                new Vector2(0f, -120f),
                new Vector2(640f, 200f));

            var fightButton = PopupUiFactory.CreatePrimaryButton(
                panelObject.transform,
                "FightButton",
                GameUiConstants.BattleConfirmFightButton,
                new Vector2(0.62f, 0.12f));
            fightButton.interactable = preview.CanAfford;
            fightButton.onClick.AddListener(OnConfirmClicked);

            var cancelButton = PopupUiFactory.CreateChoiceButton(
                panelObject.transform,
                "CancelButton",
                GameUiConstants.BattleConfirmCancelButton,
                new Vector2(0.38f, 0.12f));
            cancelButton.onClick.AddListener(OnCancelClicked);
        }

        private static string BuildBodyText(BattlePreview preview)
        {
            var lines = new List<string>
            {
                $"{GameUiConstants.BattleLabelRegion}: {preview.RegionDisplayName} ({preview.RegionStatus})",
                $"{GameUiConstants.BattleConfirmAmmoCost}: {preview.AmmoCost} / {preview.CurrentAmmo}",
                $"{GameUiConstants.BattleConfirmEnemyHp}: {preview.EnemyHp}",
                $"{GameUiConstants.BattleConfirmYourDamage}: {preview.PlayerDamagePerRound}",
                $"{GameUiConstants.BattleConfirmEnemyDamage}: {preview.EnemyDamagePerRound}",
                $"{GameUiConstants.BattleConfirmEstimatedRounds}: {preview.EstimatedRoundsToWin}"
            };

            if (!preview.CanAfford)
            {
                lines.Add(string.Format(
                    GameUiConstants.BattleConfirmInsufficientAmmo,
                    GameConstants.BattleMinAmmoBudget));
            }

            return string.Join("\n", lines);
        }

        private void OnConfirmClicked()
        {
            ClosePopup(confirmed: true);
        }

        private void OnCancelClicked()
        {
            ClosePopup(confirmed: false);
        }

        private void ClosePopup(bool confirmed)
        {
            if (_overlayRoot != null)
            {
                Destroy(_overlayRoot);
                _overlayRoot = null;
            }

            _isPopupVisible = false;
            _bootstrap?.SetGameplayPaused(false);

            if (confirmed)
            {
                var callback = _onConfirmed;
                _onConfirmed = null;
                _onCancelled = null;
                callback?.Invoke();
                return;
            }

            _onConfirmed = null;
            var cancelCallback = _onCancelled;
            _onCancelled = null;
            cancelCallback?.Invoke();
        }
    }
}
