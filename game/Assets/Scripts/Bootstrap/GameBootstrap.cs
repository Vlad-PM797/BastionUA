using BastionUA.Core;
using BastionUA.Services;
using BastionUA.UI;
using UnityEngine;

namespace BastionUA.Bootstrap
{
    public sealed class GameBootstrap : MonoBehaviour
    {
        private SaveService _saveService;
        private ResourceService _resourceService;
        private MapService _mapService;
        private BattleService _battleService;
        private EventService _eventService;
        private EventTriggerService _eventTriggerService;
        private ProgressionService _progressionService;
        private EventPopupController _eventPopupController;
        private BattleResultPopupController _battleResultPopupController;
        private GameState _gameState;

        private float _autosaveAccumulator;
        private bool _gameplayPaused;

        private const float AutosaveSeconds = 15f;

        public GameState GameState => _gameState;
        public bool IsGameplayPaused => _gameplayPaused;

        public string GetObjectiveHint()
        {
            return ObjectiveHintService.GetHint(_gameState);
        }

        private void Awake()
        {
            _saveService = new SaveService();
            _resourceService = new ResourceService();
            _mapService = new MapService();
            _battleService = new BattleService();
            _eventService = new EventService();
            _eventTriggerService = new EventTriggerService();
            _progressionService = new ProgressionService();

            _gameState = _saveService.LoadOrCreate();
            _gameState.Normalize();
            EnsureHud();

            Debug.Log("[GameBootstrap] Initialized.");
            LogCurrentState();
        }

        private void Start()
        {
            TryQueueNextEvent(EventTriggerMode.OnSessionStart);
        }

        private void Update()
        {
            if (_gameplayPaused)
            {
                return;
            }

            _resourceService.Tick(_gameState, Time.deltaTime);

            _autosaveAccumulator += Time.deltaTime;
            if (_autosaveAccumulator >= AutosaveSeconds)
            {
                _autosaveAccumulator = 0f;
                _saveService.Save(_gameState);
            }

            HandleDevKeyboardInput();
        }

        public void SetGameplayPaused(bool paused)
        {
            _gameplayPaused = paused;
        }

        public bool CanTriggerEvent(EventDefinition eventDefinition)
        {
            return _eventService.CanTrigger(_gameState, eventDefinition);
        }

        public void ApplyEventChoice(EventDefinition eventDefinition, int choiceIndex)
        {
            if (_eventService.TryApplyChoice(_gameState, _mapService, eventDefinition, choiceIndex, out var choice))
            {
                _saveService.Save(_gameState);
                Debug.Log(
                    $"[GameBootstrap] Event choice applied: {eventDefinition.EventId}/{choice.ChoiceId}. " +
                    $"Ammo={_gameState.Ammo}, Morale={_gameState.Morale}, Selected={_gameState.LastSelectedRegionId}");
                LogCurrentState();
                TryQueueNextEvent(EventTriggerMode.OnProgress);
            }
        }

        public RegionState GetRegion(string regionId)
        {
            return _mapService.GetRegion(_gameState, regionId);
        }

        public void ManualTap()
        {
            if (_gameplayPaused)
            {
                return;
            }

            _resourceService.ManualTap(_gameState);
            MarkOnboardingSeen();
            LogCurrentState();
        }

        public void SelectRegion(string regionId)
        {
            if (_gameplayPaused)
            {
                return;
            }

            _mapService.SelectRegion(_gameState, regionId);
            MarkOnboardingSeen();

            var region = _mapService.GetRegion(_gameState, regionId);
            if (region == null)
            {
                return;
            }

            Debug.Log(
                $"[GameBootstrap] >>> ACTIVE REGION: {region.DisplayName} ({regionId}) | Status: {region.Status} | " +
                $"Ammo={_gameState.Ammo}, Morale={_gameState.Morale}");
        }

        public void RunBattle()
        {
            if (_gameplayPaused)
            {
                return;
            }

            var region = _mapService.GetRegion(_gameState, _gameState.LastSelectedRegionId);
            if (region == null)
            {
                Debug.LogWarning("[GameBootstrap] Cannot start battle. No selected region.");
                return;
            }

            var modifiers = _progressionService.GetBattleModifiers(_gameState);
            var result = _battleService.Simulate(_gameState, region, modifiers);
            _gameState.TotalBattles++;
            MarkOnboardingSeen();
            _saveService.Save(_gameState);
            LogCurrentState();
            ShowBattleResult(result);
        }

        public void SelectUnit(string unitId)
        {
            if (_gameplayPaused)
            {
                return;
            }

            if (_progressionService.TrySelectUnit(_gameState, unitId))
            {
                _saveService.Save(_gameState);
                LogCurrentState();
            }
        }

        public void PurchaseUpgrade(string upgradeId)
        {
            if (_gameplayPaused)
            {
                return;
            }

            if (_progressionService.TryPurchaseUpgrade(_gameState, upgradeId))
            {
                _saveService.Save(_gameState);
                LogCurrentState();
            }
        }

        public int GetUpgradeLevel(string upgradeId)
        {
            return _gameState.GetUpgradeLevel(upgradeId);
        }

        private void ShowBattleResult(BattleResult result)
        {
            if (_battleResultPopupController == null)
            {
                _battleResultPopupController = FindAnyObjectByType<BattleResultPopupController>();
            }

            if (_battleResultPopupController == null)
            {
                Debug.LogWarning("[GameBootstrap] BattleResultPopupController not found.");
                TryQueueNextEvent(EventTriggerMode.OnProgress);
                return;
            }

            _battleResultPopupController.ShowBattleResult(result, () =>
            {
                TryQueueNextEvent(EventTriggerMode.OnProgress);
            });
        }

        public void SaveNow()
        {
            _saveService.Save(_gameState);
            Debug.Log("[GameBootstrap] Manual save triggered.");
        }

        public void ResetProgress()
        {
            try
            {
                _saveService.DeleteSave();
                _gameState = GameState.CreateDefault();
                _gameState.Normalize();
                _saveService.Save(_gameState);
                LogCurrentState();
                TryQueueNextEvent(EventTriggerMode.OnSessionStart);
                Debug.Log("[GameBootstrap] Progress reset.");
            }
            catch (System.Exception exception)
            {
                Debug.LogError($"[GameBootstrap] Reset failed: {exception}");
            }
        }

        private void MarkOnboardingSeen()
        {
            if (_gameState.HasSeenOnboarding)
            {
                return;
            }

            _gameState.HasSeenOnboarding = true;
            _saveService.Save(_gameState);
        }

        private void OnApplicationQuit()
        {
            _saveService.Save(_gameState);
        }

        private void TryQueueNextEvent(EventTriggerMode triggerMode)
        {
            if (_eventPopupController == null)
            {
                _eventPopupController = FindAnyObjectByType<EventPopupController>();
            }

            if (_eventPopupController == null)
            {
                Debug.LogWarning("[GameBootstrap] EventPopupController not found.");
                return;
            }

            var nextEvent = _eventTriggerService.GetNextEvent(_gameState, triggerMode);
            if (nextEvent == null)
            {
                return;
            }

            if (!_eventService.CanTrigger(_gameState, nextEvent))
            {
                return;
            }

            _eventPopupController.QueueEvent(nextEvent, GameUiConstants.EventPopupDelaySeconds);
            Debug.Log($"[GameBootstrap] Event queued: {nextEvent.EventId} ({triggerMode}).");
        }

        private void EnsureHud()
        {
            if (FindAnyObjectByType<HudController>() != null &&
                FindAnyObjectByType<EventPopupController>() != null &&
                FindAnyObjectByType<BattleResultPopupController>() != null)
            {
                return;
            }

            var hudRoot = GameObject.Find(GameUiConstants.HudRootName);
            if (hudRoot == null)
            {
                hudRoot = new GameObject(GameUiConstants.HudRootName);
            }

            if (hudRoot.GetComponent<HudController>() == null)
            {
                hudRoot.AddComponent<HudController>();
            }

            _eventPopupController = hudRoot.GetComponent<EventPopupController>();
            if (_eventPopupController == null)
            {
                _eventPopupController = hudRoot.AddComponent<EventPopupController>();
            }

            _battleResultPopupController = hudRoot.GetComponent<BattleResultPopupController>();
            if (_battleResultPopupController == null)
            {
                _battleResultPopupController = hudRoot.AddComponent<BattleResultPopupController>();
            }

            Debug.Log("[GameBootstrap] HUD, event popup, and battle popup auto-created.");
        }

        private void HandleDevKeyboardInput()
        {
            if (Input.GetKeyDown(KeyCode.T))
            {
                ManualTap();
            }

            if (IsRegionSelectPressed(KeyCode.Alpha1, KeyCode.Keypad1, KeyCode.Q))
            {
                SelectRegion("kyiv");
            }

            if (IsRegionSelectPressed(KeyCode.Alpha2, KeyCode.Keypad2, KeyCode.W))
            {
                SelectRegion("chernihiv");
            }

            if (IsRegionSelectPressed(KeyCode.Alpha3, KeyCode.Keypad3, KeyCode.E))
            {
                SelectRegion("sumy");
            }

            if (Input.GetKeyDown(KeyCode.B))
            {
                RunBattle();
            }

            if (Input.GetKeyDown(KeyCode.S))
            {
                SaveNow();
            }
        }

        private static bool IsRegionSelectPressed(KeyCode primary, KeyCode keypad, KeyCode alternate)
        {
            return Input.GetKeyDown(primary) || Input.GetKeyDown(keypad) || Input.GetKeyDown(alternate);
        }

        private void LogCurrentState()
        {
            Debug.Log(
                $"[GameState] Ammo={_gameState.Ammo}, Morale={_gameState.Morale}, " +
                $"Selected={_gameState.LastSelectedRegionId}, Battles={_gameState.TotalBattles}, " +
                $"Unit={_gameState.SelectedUnitId}");

            foreach (var region in _gameState.Regions)
            {
                Debug.Log($"[Region] {region.DisplayName} -> {region.Status}");
            }
        }
    }
}
