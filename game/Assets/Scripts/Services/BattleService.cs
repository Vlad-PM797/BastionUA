using BastionUA.Core;
using UnityEngine;

namespace BastionUA.Services
{
    public sealed class BattleResult
    {
        public bool IsVictory;
        public int AmmoSpent;
        public int PlayerHpRemaining;
        public int EnemyHpRemaining;
        public string RegionDisplayName;
        public RegionStatus RegionStatusBefore;
        public RegionStatus RegionStatusAfter;
    }

    public sealed class BattleService
    {
        public BattleResult Simulate(GameState state, RegionState region, BattleModifiers modifiers)
        {
            var safeModifiers = modifiers ?? new BattleModifiers();
            var result = new BattleResult
            {
                IsVictory = false,
                AmmoSpent = 0,
                PlayerHpRemaining = GameConstants.BattleBasePlayerHp,
                EnemyHpRemaining = GameConstants.BattleBaseEnemyHp,
                RegionDisplayName = region.DisplayName,
                RegionStatusBefore = region.Status,
                RegionStatusAfter = region.Status
            };

            var ammoBudget = Mathf.Min(state.Ammo, GameConstants.BattleMaxAmmoBudget);
            ammoBudget = Mathf.Max(
                GameConstants.BattleMinAmmoBudget,
                ammoBudget - safeModifiers.AmmoBudgetReduction);

            var playerDamage = GameConstants.BattleBasePlayerDamage +
                               (ammoBudget / GameConstants.BattleAmmoDamageDivisor) +
                               safeModifiers.PlayerDamageBonus;

            var enemyDamage = GameConstants.BattleBaseEnemyDamage +
                              (region.Status == RegionStatus.Occupied ? 4 : 0) -
                              safeModifiers.EnemyDamageReduction;
            enemyDamage = Mathf.Max(1, enemyDamage);

            var rounds = 0;
            while (result.PlayerHpRemaining > 0 &&
                   result.EnemyHpRemaining > 0 &&
                   rounds < GameConstants.BattleMaxRounds)
            {
                result.EnemyHpRemaining -= playerDamage;
                if (result.EnemyHpRemaining <= 0)
                {
                    break;
                }

                result.PlayerHpRemaining -= enemyDamage;
                rounds++;
            }

            result.IsVictory = result.EnemyHpRemaining <= 0 && result.PlayerHpRemaining > 0;
            result.AmmoSpent = ammoBudget;
            state.Ammo = Mathf.Max(0, state.Ammo - ammoBudget);

            if (result.IsVictory)
            {
                state.Morale += GameConstants.BattleVictoryMoraleGain + safeModifiers.MoraleBonusOnVictory;
                region.Status = region.Status == RegionStatus.Occupied ? RegionStatus.Danger : RegionStatus.Safe;
            }
            else
            {
                state.Morale = Mathf.Max(0, state.Morale - GameConstants.BattleDefeatMoraleLoss);
                region.Status = RegionStatus.Occupied;
            }

            result.RegionStatusAfter = region.Status;

            Debug.Log(
                $"[BattleService] Region={region.DisplayName}, Victory={result.IsVictory}, " +
                $"AmmoSpent={result.AmmoSpent}, PlayerHP={result.PlayerHpRemaining}, EnemyHP={result.EnemyHpRemaining}, " +
                $"Morale={state.Morale}, RegionStatus={region.Status}");

            return result;
        }
    }
}
