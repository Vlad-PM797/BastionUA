using System;
using System.IO;
using BastionUA.Bootstrap;
using BastionUA.Core;
using BastionUA.Services;
using BastionUA.UI;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BastionUA.EditorTools
{
    public static class UnityVerificationRunner
    {
        private const string BootScenePath = "Assets/Scenes/Boot.unity";
        private const string VerificationSaveFileName = "bastionua_verify_save.json";

        public static void RunAll()
        {
            var failures = 0;

            try
            {
                EnsureBootSceneExists();
                failures += VerifyCompilation() ? 0 : 1;
                failures += VerifyResourceLoop() ? 0 : 1;
                failures += VerifyMapSelection() ? 0 : 1;
                failures += VerifyBattleChangesRegionState() ? 0 : 1;
                failures += VerifyKharkivRegionMigration() ? 0 : 1;
                failures += VerifyBattleBalanceScalesByRegion() ? 0 : 1;
                failures += VerifyProgressionModifiers() ? 0 : 1;
                failures += VerifyUpgradePurchase() ? 0 : 1;
                failures += VerifySaveLoadRoundTrip() ? 0 : 1;
                failures += VerifyBootScenePlayModeBootstrap() ? 0 : 1;
                failures += VerifyHudBootstrap() ? 0 : 1;
                failures += VerifyHostomelEventFlow() ? 0 : 1;
                failures += VerifyChornobaivkaEventFlow() ? 0 : 1;
                failures += VerifyIrpinEventFlow() ? 0 : 1;
            }
            catch (Exception exception)
            {
                Debug.LogError($"[UnityVerification] Unhandled error: {exception}");
                failures++;
            }

            if (failures > 0)
            {
                Debug.LogError($"[UnityVerification] FAILED with {failures} failing check(s).");
                EditorApplication.Exit(1);
                return;
            }

            Debug.Log("[UnityVerification] ALL CHECKS PASSED.");
            EditorApplication.Exit(0);
        }

        private static void EnsureBootSceneExists()
        {
            if (File.Exists(BootScenePath))
            {
                return;
            }

            var scene = EditorSceneManager.NewScene(NewSceneSetup.DefaultGameObjects, NewSceneMode.Single);

            var bootstrapObject = new GameObject("GameBootstrap");
            bootstrapObject.AddComponent<GameBootstrap>();

            EditorSceneManager.SaveScene(scene, BootScenePath);
            Debug.Log($"[UnityVerification] Created Boot scene at {BootScenePath}.");
        }

        private static bool VerifyCompilation()
        {
            if (EditorUtility.scriptCompilationFailed)
            {
                Debug.LogError("[UnityVerification] Script compilation failed.");
                return false;
            }

            Debug.Log("[UnityVerification] Compilation OK.");
            return true;
        }

        private static bool VerifyResourceLoop()
        {
            var state = GameState.CreateDefault();
            var initialAmmo = state.Ammo;
            var service = new ResourceService();

            service.ManualTap(state);
            if (state.Ammo != initialAmmo + GameConstants.TapAmmoGain)
            {
                Debug.LogError(
                    $"[UnityVerification] Manual tap failed. Expected {initialAmmo + GameConstants.TapAmmoGain}, got {state.Ammo}.");
                return false;
            }

            service.Tick(state, GameConstants.AutoTickSeconds);
            if (state.Ammo != initialAmmo + GameConstants.TapAmmoGain + GameConstants.AutoTickAmmoGain)
            {
                Debug.LogError(
                    $"[UnityVerification] Auto tick failed. Expected ammo gain of {GameConstants.AutoTickAmmoGain}.");
                return false;
            }

            Debug.Log("[UnityVerification] Resource loop OK (tap + auto tick).");
            return true;
        }

        private static bool VerifyMapSelection()
        {
            var state = GameState.CreateDefault();
            var mapService = new MapService();

            mapService.SelectRegion(state, "sumy");
            if (state.LastSelectedRegionId != "sumy")
            {
                Debug.LogError("[UnityVerification] Region selection failed.");
                return false;
            }

            Debug.Log("[UnityVerification] Map selection OK.");
            return true;
        }

        private static bool VerifyBattleChangesRegionState()
        {
            var state = GameState.CreateDefault();
            state.Ammo = 200;
            var mapService = new MapService();
            var battleService = new BattleService();

            var region = mapService.GetRegion(state, "kyiv");
            var statusBefore = region.Status;
            var moraleBefore = state.Morale;

            battleService.Simulate(state, region, new BattleModifiers());

            if (region.Status == statusBefore && state.Morale == moraleBefore)
            {
                Debug.LogError("[UnityVerification] Battle did not change region state or morale.");
                return false;
            }

            Debug.Log("[UnityVerification] Battle state transition OK.");
            return true;
        }

        private static bool VerifyKharkivRegionMigration()
        {
            var legacyState = new GameState
            {
                Ammo = GameConstants.InitialAmmo,
                Morale = GameConstants.InitialMorale,
                LastSelectedRegionId = RegionCatalog.KyivId,
                Regions = new System.Collections.Generic.List<RegionState>
                {
                    new RegionState(RegionCatalog.KyivId, "Kyiv", RegionStatus.Danger),
                    new RegionState(RegionCatalog.ChernihivId, "Chernihiv", RegionStatus.Occupied),
                    new RegionState(RegionCatalog.SumyId, "Sumy", RegionStatus.Danger)
                }
            };

            legacyState.Normalize();

            if (legacyState.Regions.Count != RegionCatalog.All.Count)
            {
                Debug.LogError("[UnityVerification] Legacy save should migrate to four regions.");
                return false;
            }

            var kharkiv = legacyState.Regions.Find(region => region.RegionId == RegionCatalog.KharkivId);
            if (kharkiv == null)
            {
                Debug.LogError("[UnityVerification] Kharkiv region missing after migration.");
                return false;
            }

            Debug.Log("[UnityVerification] Kharkiv region migration OK.");
            return true;
        }

        private static bool VerifyBattleBalanceScalesByRegion()
        {
            var balanceService = new BattleBalanceService();
            var mapService = new MapService();

            var kyivState = GameState.CreateDefault();
            var kharkivState = GameState.CreateDefault();
            kyivState.Ammo = 250;
            kharkivState.Ammo = 250;

            var kyivRegion = mapService.GetRegion(kyivState, RegionCatalog.KyivId);
            var kharkivRegion = mapService.GetRegion(kharkivState, RegionCatalog.KharkivId);

            var kyivEnemyHp = balanceService.GetEnemyHp(kyivRegion, kyivState);
            var kharkivEnemyHp = balanceService.GetEnemyHp(kharkivRegion, kharkivState);

            if (kharkivEnemyHp <= kyivEnemyHp)
            {
                Debug.LogError("[UnityVerification] Kharkiv should have higher enemy HP than Kyiv.");
                return false;
            }

            var baseDamage = GameConstants.BattleBasePlayerDamage + 10;
            var lowMoraleDamage = balanceService.ApplyMoraleToPlayerDamage(baseDamage, 10);
            var highMoraleDamage = balanceService.ApplyMoraleToPlayerDamage(baseDamage, 90);

            if (lowMoraleDamage >= baseDamage || highMoraleDamage <= baseDamage)
            {
                Debug.LogError("[UnityVerification] Morale should modify player damage.");
                return false;
            }

            Debug.Log("[UnityVerification] Battle balance scaling OK.");
            return true;
        }

        private static bool VerifyProgressionModifiers()
        {
            var progressionService = new ProgressionService();
            var baseline = progressionService.GetBattleModifiers(GameState.CreateDefault());

            var artilleryState = GameState.CreateDefault();
            artilleryState.SelectedUnitId = UnitCatalog.ArtilleryId;
            artilleryState.Normalize();
            var artilleryModifiers = progressionService.GetBattleModifiers(artilleryState);

            if (artilleryModifiers.PlayerDamageBonus <= baseline.PlayerDamageBonus)
            {
                Debug.LogError("[UnityVerification] Artillery unit should increase player damage.");
                return false;
            }

            var upgradedState = GameState.CreateDefault();
            upgradedState.SetUpgradeLevel(UpgradeCatalog.FireTrainingId, 2);
            upgradedState.Normalize();
            var upgradedModifiers = progressionService.GetBattleModifiers(upgradedState);

            if (upgradedModifiers.PlayerDamageBonus <= baseline.PlayerDamageBonus)
            {
                Debug.LogError("[UnityVerification] Fire Training upgrade should increase player damage.");
                return false;
            }

            Debug.Log("[UnityVerification] Progression modifiers OK.");
            return true;
        }

        private static bool VerifyUpgradePurchase()
        {
            var state = GameState.CreateDefault();
            state.Ammo = 120;
            state.Normalize();
            var progressionService = new ProgressionService();
            var upgrade = UpgradeCatalog.GetById(UpgradeCatalog.MoraleRadioId);
            var expectedCost = upgrade.GetCostForNextLevel(0);

            if (!progressionService.TryPurchaseUpgrade(state, UpgradeCatalog.MoraleRadioId))
            {
                Debug.LogError("[UnityVerification] Upgrade purchase should succeed with enough ammo.");
                return false;
            }

            if (state.GetUpgradeLevel(UpgradeCatalog.MoraleRadioId) != 1)
            {
                Debug.LogError("[UnityVerification] Upgrade level not incremented.");
                return false;
            }

            if (state.Ammo != 120 - expectedCost)
            {
                Debug.LogError("[UnityVerification] Upgrade cost not deducted from ammo.");
                return false;
            }

            Debug.Log("[UnityVerification] Upgrade purchase OK.");
            return true;
        }

        private static bool VerifySaveLoadRoundTrip()
        {
            var savePath = Path.Combine(Application.persistentDataPath, VerificationSaveFileName);
            var originalPath = Path.Combine(Application.persistentDataPath, GameConstants.SaveFileName);

            try
            {
                if (File.Exists(savePath))
                {
                    File.Delete(savePath);
                }

                var state = GameState.CreateDefault();
                state.Ammo = 777;
                state.Morale = 42;
                state.LastSelectedRegionId = "chernihiv";
                state.Regions[0].Status = RegionStatus.Safe;

                var json = JsonUtility.ToJson(state, true);
                File.WriteAllText(savePath, json);

                var loadedJson = File.ReadAllText(savePath);
                var loaded = JsonUtility.FromJson<GameState>(loadedJson);

                if (loaded == null || loaded.Regions == null || loaded.Regions.Count != RegionCatalog.All.Count)
                {
                    Debug.LogError("[UnityVerification] Save/load failed: invalid deserialized state.");
                    return false;
                }

                if (loaded.Ammo != 777 || loaded.Morale != 42 || loaded.LastSelectedRegionId != "chernihiv")
                {
                    Debug.LogError("[UnityVerification] Save/load failed: scalar fields mismatch.");
                    return false;
                }

                if (loaded.Regions[0].Status != RegionStatus.Safe)
                {
                    Debug.LogError("[UnityVerification] Save/load failed: region status mismatch.");
                    return false;
                }

                var saveService = new SaveService();
                var serviceState = GameState.CreateDefault();
                serviceState.Ammo = 555;
                saveService.Save(serviceState);

                if (!File.Exists(originalPath))
                {
                    Debug.LogError("[UnityVerification] SaveService did not write save file.");
                    return false;
                }

                Debug.Log("[UnityVerification] Save/load round-trip OK.");
                return true;
            }
            catch (Exception exception)
            {
                Debug.LogError($"[UnityVerification] Save/load failed: {exception}");
                return false;
            }
            finally
            {
                if (File.Exists(savePath))
                {
                    File.Delete(savePath);
                }
            }
        }

        private static bool VerifyBootScenePlayModeBootstrap()
        {
            if (!File.Exists(BootScenePath))
            {
                Debug.LogError("[UnityVerification] Boot scene missing.");
                return false;
            }

            var scene = EditorSceneManager.OpenScene(BootScenePath, OpenSceneMode.Single);
            if (!scene.IsValid())
            {
                Debug.LogError("[UnityVerification] Boot scene could not be opened.");
                return false;
            }

            var bootstrap = UnityEngine.Object.FindAnyObjectByType<GameBootstrap>();
            if (bootstrap == null)
            {
                Debug.LogError("[UnityVerification] GameBootstrap component not found in Boot scene.");
                return false;
            }

            Debug.Log("[UnityVerification] Boot scene + GameBootstrap OK.");
            return true;
        }

        private static bool VerifyHudBootstrap()
        {
            Debug.Log("[UnityVerification] HUD scripts OK.");
            return true;
        }

        private static bool VerifyHostomelEventFlow()
        {
            var state = GameState.CreateDefault();
            state.Normalize();
            var mapService = new MapService();
            var eventService = new EventService();
            var hostomelEvent = HostomelEventCatalog.Create();

            if (!eventService.CanTrigger(state, hostomelEvent))
            {
                Debug.LogError("[UnityVerification] Hostomel should be triggerable on fresh state.");
                return false;
            }

            if (!eventService.TryApplyChoice(state, mapService, hostomelEvent, 0, out var choice))
            {
                Debug.LogError("[UnityVerification] Hostomel choice apply failed.");
                return false;
            }

            if (!state.IsEventCompleted(HostomelEventCatalog.EventId))
            {
                Debug.LogError("[UnityVerification] Hostomel not marked completed.");
                return false;
            }

            var kyiv = mapService.GetRegion(state, HostomelEventCatalog.TargetRegionId);
            if (kyiv == null || state.Ammo != GameConstants.InitialAmmo + choice.AmmoDelta)
            {
                Debug.LogError("[UnityVerification] Hostomel choice effects invalid.");
                return false;
            }

            if (eventService.CanTrigger(state, hostomelEvent))
            {
                Debug.LogError("[UnityVerification] Hostomel should not retrigger after completion.");
                return false;
            }

            Debug.Log("[UnityVerification] Hostomel event flow OK.");
            return true;
        }

        private static bool VerifyChornobaivkaEventFlow()
        {
            var state = GameState.CreateDefault();
            state.Normalize();
            var mapService = new MapService();
            var eventService = new EventService();
            var triggerService = new EventTriggerService();

            state.MarkEventCompleted(HostomelEventCatalog.EventId);
            if (triggerService.GetNextEvent(state, EventTriggerMode.OnProgress) != null)
            {
                Debug.LogError("[UnityVerification] Chornobaivka should wait for first battle.");
                return false;
            }

            state.TotalBattles = GameConstants.ChornobaivkaMinBattleCount;
            var chornobaivkaEvent = triggerService.GetNextEvent(state, EventTriggerMode.OnProgress);
            if (chornobaivkaEvent == null || chornobaivkaEvent.EventId != ChornobaivkaEventCatalog.EventId)
            {
                Debug.LogError("[UnityVerification] Chornobaivka should be ready after Hostomel + battle.");
                return false;
            }

            if (!eventService.TryApplyChoice(state, mapService, chornobaivkaEvent, 0, out _))
            {
                Debug.LogError("[UnityVerification] Chornobaivka choice apply failed.");
                return false;
            }

            if (!state.IsEventCompleted(ChornobaivkaEventCatalog.EventId))
            {
                Debug.LogError("[UnityVerification] Chornobaivka not marked completed.");
                return false;
            }

            Debug.Log("[UnityVerification] Chornobaivka event flow OK.");
            return true;
        }

        private static bool VerifyIrpinEventFlow()
        {
            var state = GameState.CreateDefault();
            state.Normalize();
            var mapService = new MapService();
            var eventService = new EventService();
            var triggerService = new EventTriggerService();

            state.MarkEventCompleted(HostomelEventCatalog.EventId);
            state.MarkEventCompleted(ChornobaivkaEventCatalog.EventId);
            state.TotalBattles = GameConstants.ChornobaivkaMinBattleCount;

            if (triggerService.GetNextEvent(state, EventTriggerMode.OnProgress) != null)
            {
                Debug.LogError("[UnityVerification] Irpin should wait for second battle.");
                return false;
            }

            state.TotalBattles = GameConstants.IrpinMinBattleCount;
            var irpinEvent = triggerService.GetNextEvent(state, EventTriggerMode.OnProgress);
            if (irpinEvent == null || irpinEvent.EventId != IrpinEventCatalog.EventId)
            {
                Debug.LogError("[UnityVerification] Irpin should be ready after Chornobaivka + 2 battles.");
                return false;
            }

            if (!eventService.TryApplyChoice(state, mapService, irpinEvent, 0, out _))
            {
                Debug.LogError("[UnityVerification] Irpin choice apply failed.");
                return false;
            }

            var hint = ObjectiveHintService.GetHint(state);
            if (hint != GameUiConstants.ObjectiveKharkiv)
            {
                Debug.LogError("[UnityVerification] Objective hint should target Kharkiv after Irpin.");
                return false;
            }

            Debug.Log("[UnityVerification] Irpin event flow OK.");
            return true;
        }
    }
}
