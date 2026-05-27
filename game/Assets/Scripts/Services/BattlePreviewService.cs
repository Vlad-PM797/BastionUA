using System.Collections.Generic;
using BastionUA.Core;
using UnityEngine;

namespace BastionUA.Services
{
    public sealed class BattlePreviewService
    {
        private readonly BattleBalanceService _balanceService = new BattleBalanceService();

        public BattlePreview BuildPreview(GameState state, RegionState region, BattleModifiers modifiers)
        {
            var safeModifiers = modifiers ?? new BattleModifiers();
            var ammoCost = BattleService.ResolveAmmoBudget(state, safeModifiers);
            var playerDamage = BattleService.ResolvePlayerDamage(state, ammoCost, safeModifiers);
            var enemyHp = _balanceService.GetEnemyHp(region, state);
            var enemyDamage = _balanceService.GetEnemyDamage(region, safeModifiers);
            var estimatedRounds = playerDamage > 0
                ? Mathf.CeilToInt(enemyHp / (float)playerDamage)
                : GameConstants.BattleMaxRounds;

            return new BattlePreview
            {
                RegionDisplayName = region.DisplayName,
                RegionStatus = region.Status,
                AmmoCost = ammoCost,
                CurrentAmmo = state.Ammo,
                CanAfford = state.Ammo >= GameConstants.BattleMinAmmoBudget,
                EnemyHp = enemyHp,
                PlayerDamagePerRound = playerDamage,
                EnemyDamagePerRound = enemyDamage,
                EstimatedRoundsToWin = estimatedRounds
            };
        }
    }
}
