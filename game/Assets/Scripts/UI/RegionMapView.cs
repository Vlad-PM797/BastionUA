using System;
using System.Collections.Generic;
using BastionUA.Bootstrap;
using BastionUA.Core;
using UnityEngine;
using UnityEngine.UI;

namespace BastionUA.UI
{
    public sealed class RegionMapView
    {
        private readonly Transform _mapRoot;
        private readonly Action<string> _onRegionSelected;
        private readonly Dictionary<string, MapMarkerUi> _markers = new Dictionary<string, MapMarkerUi>();

        public RegionMapView(Transform parent, Action<string> onRegionSelected)
        {
            _onRegionSelected = onRegionSelected;
            var landmassTransform = BuildMapPanel(parent);
            _mapRoot = landmassTransform;
            BuildSilhouette(_mapRoot);
            BuildConnections(_mapRoot);

            foreach (var region in RegionCatalog.All)
            {
                BuildMarker(_mapRoot, region.Layout);
            }
        }

        public void Refresh(GameBootstrap bootstrap, GameState state)
        {
            foreach (var pair in _markers)
            {
                var region = bootstrap.GetRegion(pair.Key);
                if (region == null)
                {
                    continue;
                }

                pair.Value.Update(region, state.LastSelectedRegionId == pair.Key);
            }
        }

        private Transform BuildMapPanel(Transform parent)
        {
            var mapPanel = CreatePanel(
                parent,
                "MapPanel",
                new Vector2(0f, 0f),
                new Vector2(1f, 1f),
                new Vector2(GameUiConstants.SidePanelWidth, GameUiConstants.BottomBarHeight),
                new Vector2(-24f, -GameUiConstants.HudTopInset));

            CreateTitle(mapPanel.transform, "MapTitle", MapUiConstants.MapPanelTitle);

            var landmass = new GameObject("MapLandmass", typeof(RectTransform), typeof(Image));
            landmass.transform.SetParent(mapPanel.transform, false);

            var landRect = landmass.GetComponent<RectTransform>();
            landRect.anchorMin = new Vector2(0.5f, 0.5f);
            landRect.anchorMax = new Vector2(0.5f, 0.5f);
            landRect.pivot = new Vector2(0.5f, 0.5f);
            landRect.sizeDelta = new Vector2(MapUiConstants.MapLandmassWidth, MapUiConstants.MapLandmassHeight);
            landmass.GetComponent<Image>().color = MapUiConstants.MapLandColor;

            return landmass.transform;
        }

        private static void BuildSilhouette(Transform mapRoot)
        {
            var silhouetteObject = new GameObject(
                "UaSilhouette",
                typeof(RectTransform),
                typeof(MapSilhouetteGraphic));
            silhouetteObject.transform.SetParent(mapRoot, false);
            silhouetteObject.transform.SetAsFirstSibling();

            var silhouetteRect = silhouetteObject.GetComponent<RectTransform>();
            StretchFullScreen(silhouetteRect, 0f);

            var silhouetteGraphic = silhouetteObject.GetComponent<MapSilhouetteGraphic>();
            silhouetteGraphic.color = MapUiConstants.MapSilhouetteColor;
            silhouetteGraphic.raycastTarget = false;
            silhouetteGraphic.SetNormalizedPoints(MapUiConstants.UkraineSilhouettePoints);

            var borderObject = new GameObject(
                "UaSilhouetteBorder",
                typeof(RectTransform),
                typeof(MapSilhouetteGraphic));
            borderObject.transform.SetParent(mapRoot, false);
            borderObject.transform.SetAsFirstSibling();

            var borderRect = borderObject.GetComponent<RectTransform>();
            StretchFullScreen(borderRect, -4f);

            var borderGraphic = borderObject.GetComponent<MapSilhouetteGraphic>();
            borderGraphic.color = MapUiConstants.MapSilhouetteBorderColor;
            borderGraphic.raycastTarget = false;
            borderGraphic.SetNormalizedPoints(MapUiConstants.UkraineSilhouettePoints);
        }

        private static void BuildConnections(Transform mapRoot)
        {
            var connections = MapUiConstants.MapConnections;
            for (var index = 0; index + 1 < connections.Length; index += 2)
            {
                CreateConnection(mapRoot, connections[index], connections[index + 1]);
            }
        }

        private void BuildMarker(Transform mapRoot, MapRegionLayout layout)
        {
            var markerObject = new GameObject($"Marker_{layout.RegionId}", typeof(RectTransform), typeof(Image), typeof(Button));
            markerObject.transform.SetParent(mapRoot, false);

            var rectTransform = markerObject.GetComponent<RectTransform>();
            rectTransform.anchorMin = new Vector2(layout.NormalizedX, layout.NormalizedY);
            rectTransform.anchorMax = new Vector2(layout.NormalizedX, layout.NormalizedY);
            rectTransform.pivot = new Vector2(0.5f, 0.5f);
            rectTransform.sizeDelta = new Vector2(MapUiConstants.MapMarkerSize, MapUiConstants.MapMarkerSize);

            var buttonImage = markerObject.GetComponent<Image>();
            buttonImage.color = GameUiConstants.ButtonNormal;

            var button = markerObject.GetComponent<Button>();
            var regionId = layout.RegionId;
            button.onClick.AddListener(() => _onRegionSelected(regionId));

            var ringObject = new GameObject("StatusRing", typeof(RectTransform), typeof(Image));
            ringObject.transform.SetParent(markerObject.transform, false);
            var ringRect = ringObject.GetComponent<RectTransform>();
            StretchFullScreen(ringRect, -MapUiConstants.MapMarkerRingThickness);

            var ringImage = ringObject.GetComponent<Image>();
            ringImage.color = GameUiConstants.StatusDanger;

            var selectionObject = new GameObject("SelectionRing", typeof(RectTransform), typeof(Image));
            selectionObject.transform.SetParent(markerObject.transform, false);
            var selectionRect = selectionObject.GetComponent<RectTransform>();
            StretchFullScreen(selectionRect, -MapUiConstants.MapMarkerRingThickness * 2.2f);
            var selectionImage = selectionObject.GetComponent<Image>();
            selectionImage.color = MapUiConstants.MapSelectionRing;
            selectionObject.SetActive(false);

            var labelObject = new GameObject("Label", typeof(RectTransform), typeof(Text));
            labelObject.transform.SetParent(markerObject.transform, false);
            var labelRect = labelObject.GetComponent<RectTransform>();
            labelRect.anchorMin = new Vector2(0.5f, 0f);
            labelRect.anchorMax = new Vector2(0.5f, 0f);
            labelRect.pivot = new Vector2(0.5f, 1f);
            labelRect.anchoredPosition = new Vector2(0f, -MapUiConstants.MapMarkerSize * 0.55f);
            labelRect.sizeDelta = new Vector2(160f, 28f);

            var labelText = labelObject.GetComponent<Text>();
            var definition = RegionCatalog.GetById(layout.RegionId);
            ConfigureText(
                labelText,
                definition != null ? definition.DisplayName : layout.RegionId,
                MapUiConstants.MapLabelFontSize,
                TextAnchor.UpperCenter);

            _markers[layout.RegionId] = new MapMarkerUi(buttonImage, ringImage, selectionObject, labelText);
        }

        private static void CreateConnection(Transform mapRoot, MapRegionLayout from, MapRegionLayout to)
        {
            var connectionObject = new GameObject("Connection", typeof(RectTransform), typeof(Image));
            connectionObject.transform.SetParent(mapRoot, false);
            connectionObject.transform.SetAsFirstSibling();

            var rectTransform = connectionObject.GetComponent<RectTransform>();
            var fromPos = new Vector2(from.NormalizedX, from.NormalizedY);
            var toPos = new Vector2(to.NormalizedX, to.NormalizedY);
            var delta = new Vector2(
                (toPos.x - fromPos.x) * MapUiConstants.MapLandmassWidth,
                (toPos.y - fromPos.y) * MapUiConstants.MapLandmassHeight);
            var length = delta.magnitude;
            var angle = Mathf.Atan2(delta.y, delta.x) * Mathf.Rad2Deg;

            rectTransform.anchorMin = fromPos;
            rectTransform.anchorMax = fromPos;
            rectTransform.pivot = new Vector2(0f, 0.5f);
            rectTransform.sizeDelta = new Vector2(length, MapUiConstants.MapConnectionThickness);
            rectTransform.localRotation = Quaternion.Euler(0f, 0f, angle);

            connectionObject.GetComponent<Image>().color = MapUiConstants.MapConnectionColor;
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

        private static void CreateTitle(Transform parent, string name, string content)
        {
            var textObject = new GameObject(name, typeof(RectTransform), typeof(Text));
            textObject.transform.SetParent(parent, false);

            var rectTransform = textObject.GetComponent<RectTransform>();
            rectTransform.anchorMin = new Vector2(0.5f, 1f);
            rectTransform.anchorMax = new Vector2(0.5f, 1f);
            rectTransform.pivot = new Vector2(0.5f, 1f);
            rectTransform.anchoredPosition = new Vector2(0f, -12f);
            rectTransform.sizeDelta = new Vector2(420f, 40f);

            ConfigureText(textObject.GetComponent<Text>(), content, GameUiConstants.TitleFontSize, TextAnchor.MiddleCenter);
        }

        private static void StretchFullScreen(RectTransform rectTransform, float expandPixels)
        {
            rectTransform.anchorMin = Vector2.zero;
            rectTransform.anchorMax = Vector2.one;
            rectTransform.offsetMin = new Vector2(expandPixels, expandPixels);
            rectTransform.offsetMax = new Vector2(-expandPixels, -expandPixels);
        }

        private static void ConfigureText(Text text, string content, int fontSize, TextAnchor alignment)
        {
            text.text = content;
            text.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            text.fontSize = fontSize;
            text.color = GameUiConstants.TextPrimary;
            text.alignment = alignment;
            text.horizontalOverflow = HorizontalWrapMode.Overflow;
            text.verticalOverflow = VerticalWrapMode.Overflow;
        }

        private sealed class MapMarkerUi
        {
            private readonly Image _buttonImage;
            private readonly Image _statusRing;
            private readonly GameObject _selectionRing;
            private readonly Text _label;

            public MapMarkerUi(Image buttonImage, Image statusRing, GameObject selectionRing, Text label)
            {
                _buttonImage = buttonImage;
                _statusRing = statusRing;
                _selectionRing = selectionRing;
                _label = label;
            }

            public void Update(RegionState region, bool isSelected)
            {
                _label.text = region.DisplayName;
                _statusRing.color = GetStatusColor(region.Status);
                _buttonImage.color = isSelected ? GameUiConstants.ButtonSelected : GameUiConstants.ButtonNormal;
                _selectionRing.SetActive(isSelected);
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
        }
    }
}
