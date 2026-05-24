using BastionUA.Core;
using UnityEngine;

namespace BastionUA.Services
{
    public sealed class ProgressionService
    {
        public BattleModifiers GetBattleModifiers(GameState state)
        {
            var modifiers = new BattleModifiers();
            if (state == null)
            {
                return modifiers;
            }

            var prestigeService = new PrestigeService();
            modifiers.PlayerDamageBonus += prestigeService.GetDamageBonus(state);

            var unit = UnitCatalog.GetById(state.SelectedUnitId) ?? UnitCatalog.GetDefault();
            modifiers.PlayerDamageBonus += unit.DamageBonus;
            modifiers.MoraleBonusOnVictory += unit.MoraleBonusOnVictory;
            modifiers.AmmoBudgetReduction += unit.AmmoBudgetReduction;
            modifiers.EnemyDamageReduction += unit.EnemyDamageReduction;

            foreach (var upgradeDefinition in UpgradeCatalog.All)
            {
                var level = state.GetUpgradeLevel(upgradeDefinition.UpgradeId);
                modifiers.PlayerDamageBonus += upgradeDefinition.DamagePerLevel * level;
                modifiers.MoraleBonusOnVictory += upgradeDefinition.MoralePerLevel * level;
                modifiers.AmmoBudgetReduction += upgradeDefinition.AmmoBudgetReductionPerLevel * level;
            }

            return modifiers;
        }

        public bool TrySelectUnit(GameState state, string unitId)
        {
            if (state == null)
            {
                return false;
            }

            var unit = UnitCatalog.GetById(unitId);
            if (unit == null)
            {
                Debug.LogWarning($"[ProgressionService] Unknown unit: {unitId}");
                return false;
            }

            state.SelectedUnitId = unitId;
            Debug.Log($"[ProgressionService] Unit selected: {unit.DisplayName}");
            return true;
        }

        public bool TryPurchaseUpgrade(GameState state, string upgradeId)
        {
            if (state == null)
            {
                return false;
            }

            var upgrade = UpgradeCatalog.GetById(upgradeId);
            if (upgrade == null)
            {
                Debug.LogWarning($"[ProgressionService] Unknown upgrade: {upgradeId}");
                return false;
            }

            var currentLevel = state.GetUpgradeLevel(upgradeId);
            if (currentLevel >= upgrade.MaxLevel)
            {
                Debug.Log($"[ProgressionService] Upgrade maxed: {upgrade.DisplayName}");
                return false;
            }

            var cost = upgrade.GetCostForNextLevel(currentLevel);
            if (state.Ammo < cost)
            {
                Debug.Log($"[ProgressionService] Not enough ammo for {upgrade.DisplayName}. Need {cost}, have {state.Ammo}");
                return false;
            }

            state.Ammo -= cost;
            state.SetUpgradeLevel(upgradeId, currentLevel + 1);
            Debug.Log(
                $"[ProgressionService] Upgrade purchased: {upgrade.DisplayName} L{currentLevel + 1}/{upgrade.MaxLevel}, " +
                $"cost={cost}, ammo={state.Ammo}");
            return true;
        }
    }
}
