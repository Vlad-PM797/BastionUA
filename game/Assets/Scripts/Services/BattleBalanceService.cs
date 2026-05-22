using BastionUA.Core;
using UnityEngine;

namespace BastionUA.Services
{
    public sealed class BattleBalanceService
    {
        public int GetEnemyHp(RegionState region, GameState state)
        {
            var definition = RegionCatalog.GetById(region.RegionId);
            var regionBonus = definition?.EnemyHpBonus ?? 0;
            var occupiedBonus = region.Status == RegionStatus.Occupied
                ? GameConstants.BattleOccupiedEnemyHpBonus
                : 0;
            var progressionBonus = GetProgressionHpBonus(state);

            return GameConstants.BattleBaseEnemyHp + regionBonus + occupiedBonus + progressionBonus;
        }

        public int GetEnemyDamage(RegionState region, BattleModifiers modifiers)
        {
            var safeModifiers = modifiers ?? new BattleModifiers();
            var definition = RegionCatalog.GetById(region.RegionId);
            var regionBonus = definition?.EnemyDamageBonus ?? 0;
            var occupiedBonus = region.Status == RegionStatus.Occupied
                ? GameConstants.BattleOccupiedEnemyDamageBonus
                : 0;

            var damage = GameConstants.BattleBaseEnemyDamage +
                         regionBonus +
                         occupiedBonus -
                         safeModifiers.EnemyDamageReduction;

            return Mathf.Max(GameConstants.BattleMinEnemyDamage, damage);
        }

        public int ApplyMoraleToPlayerDamage(int playerDamage, int morale)
        {
            if (morale < GameConstants.BattleLowMoraleThreshold)
            {
                return Mathf.Max(
                    1,
                    playerDamage * GameConstants.BattleLowMoraleDamagePercent / 100);
            }

            if (morale >= GameConstants.BattleHighMoraleThreshold)
            {
                return playerDamage + GameConstants.BattleHighMoraleDamageBonus;
            }

            return playerDamage;
        }

        private static int GetProgressionHpBonus(GameState state)
        {
            if (state == null || state.TotalBattles <= 0)
            {
                return 0;
            }

            var steps = state.TotalBattles / GameConstants.BattleProgressionBattlesStep;
            return Mathf.Min(
                GameConstants.BattleProgressionHpCap,
                steps * GameConstants.BattleProgressionHpPerStep);
        }
    }
}
