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
        private RegionMapView _regionMapView;
        private Text _ammoText;
        private Text _moraleText;
        private Text _selectedRegionText;
        private Text _objectiveText;

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

            var topBar = CreatePanel(
                canvasObject.transform,
                "TopBar",
                new Vector2(0f, 1f),
                new Vector2(1f, 1f),
                new Vector2(0f, -GameUiConstants.TopBarHeight),
                Vector2.zero);
            _ammoText = CreateStatText(topBar.transform, "AmmoText", new Vector2(0.02f, 0.5f), $"{GameUiConstants.LabelAmmo}: --");
            _moraleText = CreateStatText(topBar.transform, "MoraleText", new Vector2(0.34f, 0.5f), $"{GameUiConstants.LabelMorale}: --");
            _selectedRegionText = CreateStatText(topBar.transform, "SelectedText", new Vector2(0.66f, 0.5f), $"{GameUiConstants.LabelSelectedRegion}: --");

            BuildObjectiveBar(canvasObject.transform);
            BuildLegendPanel(canvasObject.transform);
            _regionMapView = new RegionMapView(canvasObject.transform, OnRegionSelected);

            var bottomBar = CreatePanel(
                canvasObject.transform,
                "BottomBar",
                new Vector2(0f, 0f),
                new Vector2(1f, 0f),
                new Vector2(GameUiConstants.SidePanelWidth, 0f),
                new Vector2(0f, GameUiConstants.BottomBarHeight));
            CreateActionButton(bottomBar.transform, "TapButton", GameUiConstants.ButtonTap, new Vector2(0.2f, 0.5f), OnTapClicked);
            CreateActionButton(bottomBar.transform, "BattleButton", GameUiConstants.ButtonBattle, new Vector2(0.62f, 0.5f), OnBattleClicked);

            Debug.Log("[HudController] HUD with map built.");
        }

        private void BuildObjectiveBar(Transform canvasTransform)
        {
            var objectiveBar = CreatePanel(
                canvasTransform,
                "ObjectiveBar",
                new Vector2(0f, 1f),
                new Vector2(1f, 1f),
                new Vector2(0f, -GameUiConstants.HudTopInset),
                new Vector2(0f, -GameUiConstants.TopBarHeight));

            _objectiveText = CreateStatText(objectiveBar.transform, "ObjectiveText", new Vector2(0.02f, 0.5f), $"{GameUiConstants.LabelObjective}: --");
            _objectiveText.fontSize = GameUiConstants.BaseFontSize;
        }

        private void BuildLegendPanel(Transform canvasTransform)
        {
            var legendPanel = CreatePanel(
                canvasTransform,
                "LegendPanel",
                new Vector2(0f, 0f),
                new Vector2(0f, 1f),
                new Vector2(0f, GameUiConstants.BottomBarHeight),
                new Vector2(GameUiConstants.SidePanelWidth, -GameUiConstants.HudTopInset));

            CreateTitle(legendPanel.transform, "LegendTitle", MapUiConstants.LegendTitle);
            CreateLegendEntry(legendPanel.transform, "LegendSafe", MapUiConstants.LegendSafe, GameUiConstants.StatusSafe, 0.78f);
            CreateLegendEntry(legendPanel.transform, "LegendDanger", MapUiConstants.LegendDanger, GameUiConstants.StatusDanger, 0.62f);
            CreateLegendEntry(legendPanel.transform, "LegendOccupied", MapUiConstants.LegendOccupied, GameUiConstants.StatusOccupied, 0.46f);
            CreateActionButton(legendPanel.transform, "ResetButton", GameUiConstants.ButtonResetSave, new Vector2(0.5f, 0.12f), OnResetClicked);
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

            _regionMapView.Refresh(_bootstrap, state);
        }

        private void OnResetClicked()
        {
            _bootstrap?.ResetProgress();
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
            Vector2 offsetMax)
        {
            var panelObject = new GameObject(name, typeof(RectTransform), typeof(Image));
            panelObject.transform.SetParent(parent, false);

            var rectTransform = panelObject.GetComponent<RectTransform>();
            rectTransform.anchorMin = anchorMin;
            rectTransform.anchorMax = anchorMax;
            rectTransform.offsetMin = offsetMin;
            rectTransform.offsetMax = offsetMax;
            panelObject.GetComponent<Image>().color = GameUiConstants.PanelBackground;

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

            ConfigureText(labelObject.GetComponent<Text>(), label, 18, TextAnchor.MiddleLeft);
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

            buttonObject.GetComponent<Image>().color = GameUiConstants.ButtonNormal;
            var button = buttonObject.GetComponent<Button>();
            button.onClick.AddListener(() => onClick());

            var labelObject = new GameObject("Label", typeof(RectTransform), typeof(Text));
            labelObject.transform.SetParent(buttonObject.transform, false);
            StretchFullScreen(labelObject.GetComponent<RectTransform>());
            ConfigureText(labelObject.GetComponent<Text>(), label, GameUiConstants.BaseFontSize, TextAnchor.MiddleCenter);

            return button;
        }

        private static void StretchFullScreen(RectTransform rectTransform)
        {
            rectTransform.anchorMin = Vector2.zero;
            rectTransform.anchorMax = Vector2.one;
            rectTransform.offsetMin = Vector2.zero;
            rectTransform.offsetMax = Vector2.zero;
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
