using System;
using System.Collections.Generic;
using BastionUA.Bootstrap;
using BastionUA.Core;
using UnityEngine;
using UnityEngine.UI;

namespace BastionUA.UI
{
    public sealed class HudController : MonoBehaviour
    {
        private GameBootstrap _bootstrap;
        private RegionMapView _regionMapView;
        private Text _ammoText;
        private Text _moraleText;
        private Text _selectedRegionText;
        private Text _objectiveText;
        private Text _prestigeText;
        private Text _eventLogText;
        private GameObject _prestigeButtonObject;
        private readonly Dictionary<string, Image> _unitButtonBackgrounds = new Dictionary<string, Image>();
        private readonly Dictionary<string, Text> _upgradeButtonLabels = new Dictionary<string, Text>();

        private void Awake()
        {
            try
            {
                BuildHud();
            }
            catch (Exception exception)
            {
                Debug.LogError($"[HudController] Failed to build HUD: {exception}");
            }
        }

        private void Start()
        {
            try
            {
                _bootstrap = FindAnyObjectByType<GameBootstrap>();
                if (_bootstrap == null)
                {
                    Debug.LogError("[HudController] GameBootstrap not found in scene.");
                    return;
                }

                RefreshAll();
            }
            catch (Exception exception)
            {
                Debug.LogError($"[HudController] Start failed: {exception}");
            }
        }

        private void Update()
        {
            if (_bootstrap == null)
            {
                return;
            }

            RefreshAll();
        }

        private void BuildHud()
        {
            var canvasObject = new GameObject(GameUiConstants.CanvasName, typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster));
            canvasObject.transform.SetParent(transform, false);

            var canvas = canvasObject.GetComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.sortingOrder = 100;

            var scaler = canvasObject.GetComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(GameUiConstants.ReferenceWidth, GameUiConstants.ReferenceHeight);
            scaler.matchWidthOrHeight = 0.5f;

            EnsureEventSystem();

            CreateCanvasBackground(canvasObject.transform);

            var topBar = CreatePanel(
                canvasObject.transform,
                "TopBar",
                new Vector2(0f, 1f),
                new Vector2(1f, 1f),
                new Vector2(0f, -GameUiConstants.TopBarHeight),
                Vector2.zero,
                GameVisualPalette.TopBar);
            CreateFlagStripe(topBar.transform, "TopBarStripe");
            _ammoText = CreateStatText(topBar.transform, "AmmoText", new Vector2(0.02f, 0.5f), $"{GameUiConstants.LabelAmmo}: --");
            _prestigeText = CreateStatText(topBar.transform, "PrestigeText", new Vector2(0.24f, 0.5f), $"{GameUiConstants.LabelPrestige}: 0");
            _prestigeText.fontSize = GameUiConstants.BaseFontSize;
            _moraleText = CreateStatText(topBar.transform, "MoraleText", new Vector2(0.42f, 0.5f), $"{GameUiConstants.LabelMorale}: --");
            _selectedRegionText = CreateStatText(topBar.transform, "SelectedText", new Vector2(0.60f, 0.5f), $"{GameUiConstants.LabelSelectedRegion}: --");
            CreateActionButton(
                topBar.transform,
                "QuitButton",
                GameUiConstants.ButtonQuit,
                new Vector2(0.96f, 0.5f),
                OnQuitClicked,
                false,
                new Vector2(120f, 48f));

            BuildObjectiveBar(canvasObject.transform);
            BuildLegendPanel(canvasObject.transform);
            _regionMapView = new RegionMapView(canvasObject.transform, OnRegionSelected);

            var bottomBar = CreatePanel(
                canvasObject.transform,
                "BottomBar",
                new Vector2(0f, 0f),
                new Vector2(1f, 0f),
                new Vector2(GameUiConstants.SidePanelWidth, 0f),
                new Vector2(0f, GameUiConstants.BottomBarHeight),
                GameVisualPalette.TopBar);
            CreateActionButton(bottomBar.transform, "TapButton", GameUiConstants.ButtonTap, new Vector2(0.2f, 0.5f), OnTapClicked, false);
            CreateActionButton(bottomBar.transform, "BattleButton", GameUiConstants.ButtonBattle, new Vector2(0.52f, 0.5f), OnBattleClicked, true);
            _prestigeButtonObject = CreateActionButton(
                bottomBar.transform,
                "PrestigeButton",
                GameUiConstants.ButtonPrestige,
                new Vector2(0.82f, 0.5f),
                OnPrestigeClicked,
                true,
                new Vector2(200f, 56f)).gameObject;
            _prestigeButtonObject.SetActive(false);

            Debug.Log("[HudController] HUD with Week 9 progression theme built.");
        }

        private static void CreateCanvasBackground(Transform canvasTransform)
        {
            var backgroundObject = new GameObject("CanvasBackground", typeof(RectTransform), typeof(Image));
            backgroundObject.transform.SetParent(canvasTransform, false);
            backgroundObject.transform.SetAsFirstSibling();

            var rectTransform = backgroundObject.GetComponent<RectTransform>();
            rectTransform.anchorMin = Vector2.zero;
            rectTransform.anchorMax = Vector2.one;
            rectTransform.offsetMin = Vector2.zero;
            rectTransform.offsetMax = Vector2.zero;
            backgroundObject.GetComponent<Image>().color = GameVisualPalette.CanvasBackground;
            backgroundObject.GetComponent<Image>().raycastTarget = false;
        }

        private static void CreateFlagStripe(Transform parent, string name)
        {
            var stripeObject = new GameObject(name, typeof(RectTransform), typeof(Image));
            stripeObject.transform.SetParent(parent, false);
            stripeObject.transform.SetAsFirstSibling();

            var rectTransform = stripeObject.GetComponent<RectTransform>();
            rectTransform.anchorMin = new Vector2(0f, 0f);
            rectTransform.anchorMax = new Vector2(1f, 0f);
            rectTransform.pivot = new Vector2(0.5f, 0f);
            rectTransform.sizeDelta = new Vector2(0f, GameUiConstants.AccentStripeHeight);
            stripeObject.GetComponent<Image>().color = GameVisualPalette.AccentBlue;
        }

        private void BuildObjectiveBar(Transform canvasTransform)
        {
            var objectiveBar = CreatePanel(
                canvasTransform,
                "ObjectiveBar",
                new Vector2(0f, 1f),
                new Vector2(1f, 1f),
                new Vector2(0f, -GameUiConstants.HudTopInset),
                new Vector2(0f, -GameUiConstants.TopBarHeight),
                GameVisualPalette.ObjectiveBar);

            _objectiveText = CreateStatText(objectiveBar.transform, "ObjectiveText", new Vector2(0.02f, 0.5f), $"{GameUiConstants.LabelObjective}: --");
            _objectiveText.fontSize = GameUiConstants.BaseFontSize;
            _objectiveText.color = GameVisualPalette.TextObjective;
        }

        private void BuildLegendPanel(Transform canvasTransform)
        {
            var legendPanel = CreatePanel(
                canvasTransform,
                "LegendPanel",
                new Vector2(0f, 0f),
                new Vector2(0f, 1f),
                new Vector2(0f, GameUiConstants.BottomBarHeight),
                new Vector2(GameUiConstants.SidePanelWidth, -GameUiConstants.HudTopInset),
                GameVisualPalette.SidePanel);

            CreateSideAccent(legendPanel.transform);
            CreateTitle(legendPanel.transform, "LegendTitle", MapUiConstants.LegendTitle);
            CreateLegendEntry(legendPanel.transform, "LegendSafe", MapUiConstants.LegendSafe, GameUiConstants.StatusSafe, 0.90f);
            CreateLegendEntry(legendPanel.transform, "LegendDanger", MapUiConstants.LegendDanger, GameUiConstants.StatusDanger, 0.84f);
            CreateLegendEntry(legendPanel.transform, "LegendOccupied", MapUiConstants.LegendOccupied, GameUiConstants.StatusOccupied, 0.78f);
            CreateSectionTitle(legendPanel.transform, "EventLogTitle", GameUiConstants.LabelEventLog, 0.72f);
            _eventLogText = CreateMultilineText(legendPanel.transform, "EventLogText", 0.66f);
            BuildProgressionPanel(legendPanel.transform);
            CreateActionButton(legendPanel.transform, "ResetButton", GameUiConstants.ButtonResetSave, new Vector2(0.5f, 0.04f), OnResetClicked, false);
        }

        private static void CreateSideAccent(Transform parent)
        {
            var accentObject = new GameObject("SideAccent", typeof(RectTransform), typeof(Image));
            accentObject.transform.SetParent(parent, false);
            accentObject.transform.SetAsFirstSibling();

            var rectTransform = accentObject.GetComponent<RectTransform>();
            rectTransform.anchorMin = new Vector2(0f, 0f);
            rectTransform.anchorMax = new Vector2(0f, 1f);
            rectTransform.pivot = new Vector2(0f, 0.5f);
            rectTransform.sizeDelta = new Vector2(GameUiConstants.SidePanelAccentWidth, 0f);
            accentObject.GetComponent<Image>().color = GameVisualPalette.AccentYellow;
        }

        private void BuildProgressionPanel(Transform legendPanel)
        {
            CreateSectionTitle(legendPanel, "UnitsTitle", GameUiConstants.LabelUnits, 0.56f);

            var unitAnchors = new[] { 0.50f, 0.44f, 0.38f, 0.32f };
            var unitIndex = 0;
            foreach (var unit in UnitCatalog.All)
            {
                var anchorY = unitAnchors[unitIndex];
                var button = CreateCompactButton(
                    legendPanel,
                    $"Unit_{unit.UnitId}",
                    unit.ShortLabel,
                    new Vector2(0.5f, anchorY),
                    () => OnUnitClicked(unit.UnitId));
                _unitButtonBackgrounds[unit.UnitId] = button.GetComponent<Image>();
                unitIndex++;
            }

            CreateSectionTitle(legendPanel, "UpgradesTitle", GameUiConstants.LabelUpgrades, 0.26f);

            var upgradeAnchors = new[] { 0.20f, 0.14f, 0.08f };
            var upgradeIndex = 0;
            foreach (var upgrade in UpgradeCatalog.All)
            {
                var anchorY = upgradeAnchors[upgradeIndex];
                var labelText = CreateCompactButtonLabel(legendPanel, $"Upgrade_{upgrade.UpgradeId}", anchorY);
                _upgradeButtonLabels[upgrade.UpgradeId] = labelText;

                CreateCompactButton(
                    legendPanel,
                    $"UpgradeBtn_{upgrade.UpgradeId}",
                    "+",
                    new Vector2(0.88f, anchorY),
                    () => OnUpgradeClicked(upgrade.UpgradeId),
                    new Vector2(44f, 32f));

                upgradeIndex++;
            }
        }

        private void RefreshAll()
        {
            var state = _bootstrap.GameState;
            _ammoText.text = $"{GameUiConstants.LabelAmmo}: {state.Ammo}";
            _moraleText.text = $"{GameUiConstants.LabelMorale}: {state.Morale}";

            var selectedRegion = _bootstrap.GetRegion(state.LastSelectedRegionId);
            var selectedLabel = selectedRegion != null ? selectedRegion.DisplayName : state.LastSelectedRegionId;
            _selectedRegionText.text = $"{GameUiConstants.LabelSelectedRegion}: {selectedLabel}";
            _objectiveText.text = $"{GameUiConstants.LabelObjective}: {_bootstrap.GetObjectiveHint()}";
            _prestigeText.text = $"{GameUiConstants.LabelPrestige}: {state.PrestigeLevel}";

            if (_prestigeButtonObject != null)
            {
                _prestigeButtonObject.SetActive(_bootstrap.CanPrestige());
            }

            RefreshEventLog();

            _regionMapView.Refresh(_bootstrap, state);
            RefreshProgressionPanel(state);
        }

        private void RefreshEventLog()
        {
            if (_eventLogText == null || _bootstrap == null)
            {
                return;
            }

            var entries = _bootstrap.GetEventLogEntries();
            if (entries.Count == 0)
            {
                _eventLogText.text = "--";
                return;
            }

            var lines = new System.Text.StringBuilder();
            var limit = Mathf.Min(entries.Count, GameConstants.EventLogDisplayLines);
            for (var index = 0; index < limit; index++)
            {
                if (index > 0)
                {
                    lines.Append('\n');
                }

                lines.Append(entries[index]);
            }

            _eventLogText.text = lines.ToString();
        }

        private void RefreshProgressionPanel(GameState state)
        {
            foreach (var unitEntry in _unitButtonBackgrounds)
            {
                var isSelected = state.SelectedUnitId == unitEntry.Key;
                unitEntry.Value.color = isSelected
                    ? GameVisualPalette.ButtonSelected
                    : GameVisualPalette.ButtonNeutral;
            }

            foreach (var upgrade in UpgradeCatalog.All)
            {
                if (!_upgradeButtonLabels.TryGetValue(upgrade.UpgradeId, out var label))
                {
                    continue;
                }

                var level = state.GetUpgradeLevel(upgrade.UpgradeId);
                if (level >= upgrade.MaxLevel)
                {
                    label.text = $"{upgrade.DisplayName} {GameUiConstants.UpgradeMaxLabel}";
                    continue;
                }

                var cost = upgrade.GetCostForNextLevel(level);
                label.text = $"{upgrade.DisplayName} L{level} ({cost})";
            }
        }

        private void OnPrestigeClicked()
        {
            _bootstrap?.RunPrestige();
        }

        private void OnResetClicked()
        {
            _bootstrap?.ResetProgress();
        }

        private void OnQuitClicked()
        {
            _bootstrap?.QuitApplication();
        }

        private void OnRegionSelected(string regionId)
        {
            _bootstrap?.SelectRegion(regionId);
        }

        private void OnTapClicked()
        {
            _bootstrap?.ManualTap();
        }

        private void OnBattleClicked()
        {
            _bootstrap?.RunBattle();
        }

        private void OnUnitClicked(string unitId)
        {
            _bootstrap?.SelectUnit(unitId);
        }

        private void OnUpgradeClicked(string upgradeId)
        {
            _bootstrap?.PurchaseUpgrade(upgradeId);
        }

        private static void EnsureEventSystem()
        {
            if (FindAnyObjectByType<UnityEngine.EventSystems.EventSystem>() != null)
            {
                return;
            }

            var eventSystemObject = new GameObject(
                "EventSystem",
                typeof(UnityEngine.EventSystems.EventSystem),
                typeof(UnityEngine.EventSystems.StandaloneInputModule));
            DontDestroyOnLoad(eventSystemObject);
        }

        private static GameObject CreatePanel(
            Transform parent,
            string name,
            Vector2 anchorMin,
            Vector2 anchorMax,
            Vector2 offsetMin,
            Vector2 offsetMax,
            Color backgroundColor)
        {
            var panelObject = new GameObject(name, typeof(RectTransform), typeof(Image));
            panelObject.transform.SetParent(parent, false);

            var rectTransform = panelObject.GetComponent<RectTransform>();
            rectTransform.anchorMin = anchorMin;
            rectTransform.anchorMax = anchorMax;
            rectTransform.offsetMin = offsetMin;
            rectTransform.offsetMax = offsetMax;
            panelObject.GetComponent<Image>().color = backgroundColor;

            return panelObject;
        }

        private static Text CreateStatText(Transform parent, string name, Vector2 anchor, string content)
        {
            var textObject = new GameObject(name, typeof(RectTransform), typeof(Text));
            textObject.transform.SetParent(parent, false);

            var rectTransform = textObject.GetComponent<RectTransform>();
            rectTransform.anchorMin = anchor;
            rectTransform.anchorMax = anchor;
            rectTransform.pivot = new Vector2(0f, 0.5f);
            rectTransform.anchoredPosition = Vector2.zero;
            rectTransform.sizeDelta = new Vector2(520f, 48f);

            return ConfigureText(
                textObject.GetComponent<Text>(),
                content,
                GameUiConstants.TitleFontSize,
                TextAnchor.MiddleLeft,
                GameVisualPalette.TextPrimary);
        }

        private static void CreateTitle(Transform parent, string name, string content)
        {
            var textObject = new GameObject(name, typeof(RectTransform), typeof(Text));
            textObject.transform.SetParent(parent, false);

            var rectTransform = textObject.GetComponent<RectTransform>();
            rectTransform.anchorMin = new Vector2(0.5f, 1f);
            rectTransform.anchorMax = new Vector2(0.5f, 1f);
            rectTransform.pivot = new Vector2(0.5f, 1f);
            rectTransform.anchoredPosition = new Vector2(0f, -12f);
            rectTransform.sizeDelta = new Vector2(280f, 40f);

            ConfigureText(
                textObject.GetComponent<Text>(),
                content,
                GameUiConstants.TitleFontSize,
                TextAnchor.MiddleCenter,
                GameVisualPalette.TextAccent);
        }

        private static void CreateLegendEntry(Transform parent, string name, string label, Color color, float anchorY)
        {
            var rowObject = new GameObject(name, typeof(RectTransform));
            rowObject.transform.SetParent(parent, false);

            var rowRect = rowObject.GetComponent<RectTransform>();
            rowRect.anchorMin = new Vector2(0.5f, anchorY);
            rowRect.anchorMax = new Vector2(0.5f, anchorY);
            rowRect.pivot = new Vector2(0.5f, 0.5f);
            rowRect.sizeDelta = new Vector2(260f, 28f);

            var swatchObject = new GameObject("Swatch", typeof(RectTransform), typeof(Image));
            swatchObject.transform.SetParent(rowObject.transform, false);
            var swatchRect = swatchObject.GetComponent<RectTransform>();
            swatchRect.anchorMin = new Vector2(0f, 0.5f);
            swatchRect.anchorMax = new Vector2(0f, 0.5f);
            swatchRect.pivot = new Vector2(0f, 0.5f);
            swatchRect.sizeDelta = new Vector2(18f, 18f);
            swatchObject.GetComponent<Image>().color = color;

            var labelObject = new GameObject("Label", typeof(RectTransform), typeof(Text));
            labelObject.transform.SetParent(rowObject.transform, false);
            var labelRect = labelObject.GetComponent<RectTransform>();
            labelRect.anchorMin = new Vector2(0f, 0.5f);
            labelRect.anchorMax = new Vector2(1f, 0.5f);
            labelRect.offsetMin = new Vector2(28f, -12f);
            labelRect.offsetMax = new Vector2(0f, 12f);

            ConfigureText(labelObject.GetComponent<Text>(), label, 18, TextAnchor.MiddleLeft, GameVisualPalette.TextMuted);
        }

        private static void CreateSectionTitle(Transform parent, string name, string content, float anchorY)
        {
            var textObject = new GameObject(name, typeof(RectTransform), typeof(Text));
            textObject.transform.SetParent(parent, false);

            var rectTransform = textObject.GetComponent<RectTransform>();
            rectTransform.anchorMin = new Vector2(0.5f, anchorY);
            rectTransform.anchorMax = new Vector2(0.5f, anchorY);
            rectTransform.pivot = new Vector2(0.5f, 1f);
            rectTransform.anchoredPosition = Vector2.zero;
            rectTransform.sizeDelta = new Vector2(280f, 24f);

            ConfigureText(textObject.GetComponent<Text>(), content, 18, TextAnchor.MiddleCenter, GameVisualPalette.AccentBlueLight);
        }

        private static Text CreateCompactButtonLabel(Transform parent, string name, float anchorY)
        {
            var textObject = new GameObject(name, typeof(RectTransform), typeof(Text));
            textObject.transform.SetParent(parent, false);

            var rectTransform = textObject.GetComponent<RectTransform>();
            rectTransform.anchorMin = new Vector2(0.08f, anchorY);
            rectTransform.anchorMax = new Vector2(0.72f, anchorY);
            rectTransform.pivot = new Vector2(0f, 0.5f);
            rectTransform.anchoredPosition = Vector2.zero;
            rectTransform.sizeDelta = new Vector2(200f, 32f);

            return ConfigureText(textObject.GetComponent<Text>(), "--", 16, TextAnchor.MiddleLeft, GameVisualPalette.TextMuted);
        }

        private static Text CreateMultilineText(Transform parent, string name, float anchorY)
        {
            var textObject = new GameObject(name, typeof(RectTransform), typeof(Text));
            textObject.transform.SetParent(parent, false);

            var rectTransform = textObject.GetComponent<RectTransform>();
            rectTransform.anchorMin = new Vector2(0.08f, anchorY);
            rectTransform.anchorMax = new Vector2(0.92f, anchorY);
            rectTransform.pivot = new Vector2(0.5f, 1f);
            rectTransform.anchoredPosition = Vector2.zero;
            rectTransform.sizeDelta = new Vector2(260f, 72f);

            var text = textObject.GetComponent<Text>();
            text.text = "--";
            text.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            text.fontSize = 14;
            text.color = GameVisualPalette.TextObjective;
            text.alignment = TextAnchor.UpperLeft;
            text.horizontalOverflow = HorizontalWrapMode.Wrap;
            text.verticalOverflow = VerticalWrapMode.Truncate;
            return text;
        }

        private static Button CreateCompactButton(
            Transform parent,
            string name,
            string label,
            Vector2 anchor,
            Action onClick,
            Vector2? size = null)
        {
            var buttonSize = size ?? new Vector2(260f, 32f);
            var buttonObject = new GameObject(name, typeof(RectTransform), typeof(Image), typeof(Button));
            buttonObject.transform.SetParent(parent, false);

            var rectTransform = buttonObject.GetComponent<RectTransform>();
            rectTransform.anchorMin = anchor;
            rectTransform.anchorMax = anchor;
            rectTransform.pivot = new Vector2(0.5f, 0.5f);
            rectTransform.sizeDelta = buttonSize;

            buttonObject.GetComponent<Image>().color = GameVisualPalette.ButtonNeutral;
            var button = buttonObject.GetComponent<Button>();
            button.onClick.AddListener(() => onClick());

            var labelObject = new GameObject("Label", typeof(RectTransform), typeof(Text));
            labelObject.transform.SetParent(buttonObject.transform, false);
            StretchFullScreen(labelObject.GetComponent<RectTransform>());
            ConfigureText(labelObject.GetComponent<Text>(), label, 16, TextAnchor.MiddleCenter, GameVisualPalette.TextPrimary);

            return button;
        }

        private static Button CreateActionButton(
            Transform parent,
            string name,
            string label,
            Vector2 anchor,
            Action onClick,
            bool isPrimary,
            Vector2? buttonSize = null)
        {
            var resolvedSize = buttonSize ?? new Vector2(280f, 56f);
            var frameColor = isPrimary
                ? GameVisualPalette.ButtonPrimaryBorder
                : GameVisualPalette.ButtonNeutralBorder;
            var fillColor = isPrimary
                ? GameVisualPalette.ButtonPrimary
                : GameVisualPalette.ButtonNeutral;
            var labelColor = isPrimary
                ? GameVisualPalette.TextOnPrimaryButton
                : GameVisualPalette.TextPrimary;

            var frameObject = new GameObject(name, typeof(RectTransform), typeof(Image), typeof(Button));
            frameObject.transform.SetParent(parent, false);

            var frameRect = frameObject.GetComponent<RectTransform>();
            frameRect.anchorMin = anchor;
            frameRect.anchorMax = anchor;
            frameRect.pivot = new Vector2(0.5f, 0.5f);
            frameRect.sizeDelta = resolvedSize;
            frameObject.GetComponent<Image>().color = frameColor;

            var fillObject = new GameObject("Fill", typeof(RectTransform), typeof(Image));
            fillObject.transform.SetParent(frameObject.transform, false);
            var fillRect = fillObject.GetComponent<RectTransform>();
            fillRect.anchorMin = Vector2.zero;
            fillRect.anchorMax = Vector2.one;
            var inset = GameUiConstants.ButtonFrameInset;
            fillRect.offsetMin = new Vector2(inset, inset);
            fillRect.offsetMax = new Vector2(-inset, -inset);
            fillObject.GetComponent<Image>().color = fillColor;

            var button = frameObject.GetComponent<Button>();
            button.targetGraphic = fillObject.GetComponent<Image>();
            button.onClick.AddListener(() => onClick());

            var labelObject = new GameObject("Label", typeof(RectTransform), typeof(Text));
            labelObject.transform.SetParent(frameObject.transform, false);
            StretchFullScreen(labelObject.GetComponent<RectTransform>());
            ConfigureText(
                labelObject.GetComponent<Text>(),
                label,
                GameUiConstants.BaseFontSize,
                TextAnchor.MiddleCenter,
                labelColor);

            return button;
        }

        private static void StretchFullScreen(RectTransform rectTransform)
        {
            rectTransform.anchorMin = Vector2.zero;
            rectTransform.anchorMax = Vector2.one;
            rectTransform.offsetMin = Vector2.zero;
            rectTransform.offsetMax = Vector2.zero;
        }

        private static Text ConfigureText(Text text, string content, int fontSize, TextAnchor alignment, Color color)
        {
            text.text = content;
            text.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            text.fontSize = fontSize;
            text.color = color;
            text.alignment = alignment;
            text.horizontalOverflow = HorizontalWrapMode.Overflow;
            text.verticalOverflow = VerticalWrapMode.Overflow;
            return text;
        }
    }
}
