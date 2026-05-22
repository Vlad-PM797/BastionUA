using System;
using BastionUA.Bootstrap;
using BastionUA.Core;
using UnityEngine;
using UnityEngine.UI;

namespace BastionUA.UI
{
    public sealed class HudController : MonoBehaviour
    {
        private GameBootstrap _bootstrap;
        private Text _ammoText;
        private Text _moraleText;
        private Text _selectedRegionText;
        private Text _kyivStatusText;
        private Text _chernihivStatusText;
        private Text _sumyStatusText;
        private Button _kyivButton;
        private Button _chernihivButton;
        private Button _sumyButton;

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

            var topBar = CreatePanel(canvasObject.transform, "TopBar", new Vector2(0f, 1f), new Vector2(1f, 1f), new Vector2(0f, -GameUiConstants.TopBarHeight), Vector2.zero);
            _ammoText = CreateStatText(topBar.transform, "AmmoText", new Vector2(0.02f, 0.5f), $"{GameUiConstants.LabelAmmo}: --");
            _moraleText = CreateStatText(topBar.transform, "MoraleText", new Vector2(0.34f, 0.5f), $"{GameUiConstants.LabelMorale}: --");
            _selectedRegionText = CreateStatText(topBar.transform, "SelectedText", new Vector2(0.66f, 0.5f), $"{GameUiConstants.LabelSelectedRegion}: --");

            var sidePanel = CreatePanel(
                canvasObject.transform,
                "RegionPanel",
                new Vector2(0f, 0f),
                new Vector2(0f, 1f),
                new Vector2(0f, GameUiConstants.BottomBarHeight),
                new Vector2(GameUiConstants.SidePanelWidth, -GameUiConstants.TopBarHeight));
            CreateTitle(sidePanel.transform, "RegionsTitle", "Regions");
            _kyivButton = CreateRegionButton(sidePanel.transform, "KyivButton", "Kyiv", new Vector2(0.5f, 0.78f), OnKyivClicked);
            _kyivStatusText = CreateRegionStatusText(_kyivButton.transform, "KyivStatus");
            _chernihivButton = CreateRegionButton(sidePanel.transform, "ChernihivButton", "Chernihiv", new Vector2(0.5f, 0.52f), OnChernihivClicked);
            _chernihivStatusText = CreateRegionStatusText(_chernihivButton.transform, "ChernihivStatus");
            _sumyButton = CreateRegionButton(sidePanel.transform, "SumyButton", "Sumy", new Vector2(0.5f, 0.26f), OnSumyClicked);
            _sumyStatusText = CreateRegionStatusText(_sumyButton.transform, "SumyStatus");

            var bottomBar = CreatePanel(
                canvasObject.transform,
                "BottomBar",
                new Vector2(0f, 0f),
                new Vector2(1f, 0f),
                new Vector2(GameUiConstants.SidePanelWidth, 0f),
                new Vector2(0f, GameUiConstants.BottomBarHeight));
            CreateActionButton(bottomBar.transform, "TapButton", GameUiConstants.ButtonTap, new Vector2(0.2f, 0.5f), OnTapClicked);
            CreateActionButton(bottomBar.transform, "BattleButton", GameUiConstants.ButtonBattle, new Vector2(0.62f, 0.5f), OnBattleClicked);

            Debug.Log("[HudController] HUD built.");
        }

        private void RefreshAll()
        {
            var state = _bootstrap.GameState;
            _ammoText.text = $"{GameUiConstants.LabelAmmo}: {state.Ammo}";
            _moraleText.text = $"{GameUiConstants.LabelMorale}: {state.Morale}";

            var selectedRegion = _bootstrap.GetRegion(state.LastSelectedRegionId);
            var selectedLabel = selectedRegion != null ? selectedRegion.DisplayName : state.LastSelectedRegionId;
            _selectedRegionText.text = $"{GameUiConstants.LabelSelectedRegion}: {selectedLabel}";

            UpdateRegionEntry("kyiv", _kyivButton, _kyivStatusText, state);
            UpdateRegionEntry("chernihiv", _chernihivButton, _chernihivStatusText, state);
            UpdateRegionEntry("sumy", _sumyButton, _sumyStatusText, state);
        }

        private void UpdateRegionEntry(string regionId, Button button, Text statusText, GameState state)
        {
            var region = _bootstrap.GetRegion(regionId);
            if (region == null)
            {
                statusText.text = "--";
                return;
            }

            statusText.text = region.Status.ToString();
            statusText.color = GetStatusColor(region.Status);

            var colors = button.colors;
            var isSelected = state.LastSelectedRegionId == regionId;
            colors.normalColor = isSelected ? GameUiConstants.ButtonSelected : GameUiConstants.ButtonNormal;
            colors.highlightedColor = isSelected ? GameUiConstants.ButtonSelected : GameUiConstants.ButtonNormal * 1.08f;
            button.colors = colors;
        }

        private static Color GetStatusColor(RegionStatus status)
        {
            switch (status)
            {
                case RegionStatus.Safe:
                    return GameUiConstants.StatusSafe;
                case RegionStatus.Danger:
                    return GameUiConstants.StatusDanger;
                case RegionStatus.Occupied:
                    return GameUiConstants.StatusOccupied;
                default:
                    return GameUiConstants.TextPrimary;
            }
        }

        private void OnTapClicked()
        {
            _bootstrap?.ManualTap();
        }

        private void OnKyivClicked()
        {
            _bootstrap?.SelectRegion("kyiv");
        }

        private void OnChernihivClicked()
        {
            _bootstrap?.SelectRegion("chernihiv");
        }

        private void OnSumyClicked()
        {
            _bootstrap?.SelectRegion("sumy");
        }

        private void OnBattleClicked()
        {
            _bootstrap?.RunBattle();
        }

        private static void EnsureEventSystem()
        {
            if (FindAnyObjectByType<UnityEngine.EventSystems.EventSystem>() != null)
            {
                return;
            }

            var eventSystemObject = new GameObject("EventSystem", typeof(UnityEngine.EventSystems.EventSystem), typeof(UnityEngine.EventSystems.StandaloneInputModule));
            DontDestroyOnLoad(eventSystemObject);
        }

        private static GameObject CreatePanel(Transform parent, string name, Vector2 anchorMin, Vector2 anchorMax, Vector2 offsetMin, Vector2 offsetMax)
        {
            var panelObject = new GameObject(name, typeof(RectTransform), typeof(Image));
            panelObject.transform.SetParent(parent, false);

            var rectTransform = panelObject.GetComponent<RectTransform>();
            rectTransform.anchorMin = anchorMin;
            rectTransform.anchorMax = anchorMax;
            rectTransform.offsetMin = offsetMin;
            rectTransform.offsetMax = offsetMax;

            var image = panelObject.GetComponent<Image>();
            image.color = GameUiConstants.PanelBackground;

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

            return ConfigureText(textObject.GetComponent<Text>(), content, GameUiConstants.TitleFontSize, TextAnchor.MiddleLeft);
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

            ConfigureText(textObject.GetComponent<Text>(), content, GameUiConstants.TitleFontSize, TextAnchor.MiddleCenter);
        }

        private static Button CreateRegionButton(Transform parent, string name, string label, Vector2 anchor, Action onClick)
        {
            var buttonObject = new GameObject(name, typeof(RectTransform), typeof(Image), typeof(Button));
            buttonObject.transform.SetParent(parent, false);

            var rectTransform = buttonObject.GetComponent<RectTransform>();
            rectTransform.anchorMin = anchor;
            rectTransform.anchorMax = anchor;
            rectTransform.pivot = new Vector2(0.5f, 0.5f);
            rectTransform.sizeDelta = new Vector2(280f, 72f);

            var image = buttonObject.GetComponent<Image>();
            image.color = GameUiConstants.ButtonNormal;

            var button = buttonObject.GetComponent<Button>();
            var colors = button.colors;
            colors.normalColor = GameUiConstants.ButtonNormal;
            colors.highlightedColor = GameUiConstants.ButtonNormal * 1.08f;
            colors.pressedColor = GameUiConstants.ButtonNormal * 0.92f;
            button.colors = colors;
            button.onClick.AddListener(() => onClick());

            var labelObject = new GameObject("Label", typeof(RectTransform), typeof(Text));
            labelObject.transform.SetParent(buttonObject.transform, false);

            var labelRect = labelObject.GetComponent<RectTransform>();
            labelRect.anchorMin = Vector2.zero;
            labelRect.anchorMax = Vector2.one;
            labelRect.offsetMin = new Vector2(12f, 24f);
            labelRect.offsetMax = new Vector2(-12f, -8f);

            ConfigureText(labelObject.GetComponent<Text>(), label, GameUiConstants.BaseFontSize, TextAnchor.UpperCenter);

            return button;
        }

        private static Text CreateRegionStatusText(Transform parent, string name)
        {
            var textObject = new GameObject(name, typeof(RectTransform), typeof(Text));
            textObject.transform.SetParent(parent, false);

            var rectTransform = textObject.GetComponent<RectTransform>();
            rectTransform.anchorMin = new Vector2(0f, 0f);
            rectTransform.anchorMax = new Vector2(1f, 0f);
            rectTransform.pivot = new Vector2(0.5f, 0f);
            rectTransform.anchoredPosition = new Vector2(0f, 6f);
            rectTransform.sizeDelta = new Vector2(-16f, 22f);

            return ConfigureText(textObject.GetComponent<Text>(), "--", 18, TextAnchor.LowerCenter);
        }

        private static Button CreateActionButton(Transform parent, string name, string label, Vector2 anchor, Action onClick)
        {
            var buttonObject = new GameObject(name, typeof(RectTransform), typeof(Image), typeof(Button));
            buttonObject.transform.SetParent(parent, false);

            var rectTransform = buttonObject.GetComponent<RectTransform>();
            rectTransform.anchorMin = anchor;
            rectTransform.anchorMax = anchor;
            rectTransform.pivot = new Vector2(0.5f, 0.5f);
            rectTransform.sizeDelta = new Vector2(280f, 56f);

            var image = buttonObject.GetComponent<Image>();
            image.color = GameUiConstants.ButtonNormal;

            var button = buttonObject.GetComponent<Button>();
            button.onClick.AddListener(() => onClick());

            var labelObject = new GameObject("Label", typeof(RectTransform), typeof(Text));
            labelObject.transform.SetParent(buttonObject.transform, false);

            var labelRect = labelObject.GetComponent<RectTransform>();
            labelRect.anchorMin = Vector2.zero;
            labelRect.anchorMax = Vector2.one;
            labelRect.offsetMin = Vector2.zero;
            labelRect.offsetMax = Vector2.zero;

            ConfigureText(labelObject.GetComponent<Text>(), label, GameUiConstants.BaseFontSize, TextAnchor.MiddleCenter);

            return button;
        }

        private static Text ConfigureText(Text text, string content, int fontSize, TextAnchor alignment)
        {
            text.text = content;
            text.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            text.fontSize = fontSize;
            text.color = GameUiConstants.TextPrimary;
            text.alignment = alignment;
            text.horizontalOverflow = HorizontalWrapMode.Overflow;
            text.verticalOverflow = VerticalWrapMode.Overflow;
            return text;
        }
    }
}
