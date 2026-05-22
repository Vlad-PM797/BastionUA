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
    }

    public sealed class BattleService
    {
        public BattleResult Simulate(GameState state, RegionState region)
        {
            var result = new BattleResult
            {
                IsVictory = false,
                AmmoSpent = 0,
                PlayerHpRemaining = GameConstants.BattleBasePlayerHp,
                EnemyHpRemaining = GameConstants.BattleBaseEnemyHp
            };

            var ammoBudget = Mathf.Min(state.Ammo, 50);
            var playerDamage = GameConstants.BattleBasePlayerDamage + (ammoBudget / 5);
            var enemyDamage = GameConstants.BattleBaseEnemyDamage + (region.Status == RegionStatus.Occupied ? 4 : 0);

            var rounds = 0;
            while (result.PlayerHpRemaining > 0 && result.EnemyHpRemaining > 0 && rounds < 20)
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
                state.Morale += 5;
                region.Status = region.Status == RegionStatus.Occupied ? RegionStatus.Danger : RegionStatus.Safe;
            }
            else
            {
                state.Morale = Mathf.Max(0, state.Morale - 3);
                region.Status = RegionStatus.Occupied;
            }

            Debug.Log(
                $"[BattleService] Region={region.DisplayName}, Victory={result.IsVictory}, " +
                $"AmmoSpent={result.AmmoSpent}, PlayerHP={result.PlayerHpRemaining}, EnemyHP={result.EnemyHpRemaining}, " +
                $"Morale={state.Morale}, RegionStatus={region.Status}");

            return result;
        }
    }
}
