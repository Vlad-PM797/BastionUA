using System.Collections.Generic;
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
        public int RoundsFought;
        public string RegionDisplayName;
        public RegionStatus RegionStatusBefore;
        public RegionStatus RegionStatusAfter;
        public List<string> CombatLog = new List<string>();
    }

    public sealed class BattleService
    {
        private readonly BattleBalanceService _balanceService = new BattleBalanceService();

        public static int ResolveAmmoBudget(GameState state, BattleModifiers modifiers)
        {
            var safeModifiers = modifiers ?? new BattleModifiers();
            var ammoBudget = Mathf.Min(state.Ammo, GameConstants.BattleMaxAmmoBudget);
            return Mathf.Max(
                GameConstants.BattleMinAmmoBudget,
                ammoBudget - safeModifiers.AmmoBudgetReduction);
        }

        public static int ResolvePlayerDamage(GameState state, int ammoBudget, BattleModifiers modifiers)
        {
            var safeModifiers = modifiers ?? new BattleModifiers();
            var balanceService = new BattleBalanceService();
            var playerDamage = GameConstants.BattleBasePlayerDamage +
                               (ammoBudget / GameConstants.BattleAmmoDamageDivisor) +
                               safeModifiers.PlayerDamageBonus;
            return balanceService.ApplyMoraleToPlayerDamage(playerDamage, state.Morale);
        }

        public BattleResult Simulate(GameState state, RegionState region, BattleModifiers modifiers)
        {
            var safeModifiers = modifiers ?? new BattleModifiers();
            var enemyHp = _balanceService.GetEnemyHp(region, state);
            var result = new BattleResult
            {
                IsVictory = false,
                AmmoSpent = 0,
                PlayerHpRemaining = GameConstants.BattleBasePlayerHp,
                EnemyHpRemaining = enemyHp,
                RegionDisplayName = region.DisplayName,
                RegionStatusBefore = region.Status,
                RegionStatusAfter = region.Status
            };

            var ammoBudget = ResolveAmmoBudget(state, safeModifiers);
            var playerDamage = ResolvePlayerDamage(state, ammoBudget, safeModifiers);
            var enemyDamage = _balanceService.GetEnemyDamage(region, safeModifiers);

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

                if (result.CombatLog.Count < GameConstants.BattleLogMaxLines)
                {
                    result.CombatLog.Add(string.Format(
                        GameUiConstants.BattleLogRoundFormat,
                        rounds,
                        playerDamage,
                        enemyDamage,
                        result.PlayerHpRemaining,
                        result.EnemyHpRemaining));
                }
            }

            result.RoundsFought = rounds;
            if (rounds > GameConstants.BattleLogMaxLines)
            {
                result.CombatLog.Add(string.Format(
                    GameUiConstants.BattleLogTruncatedFormat,
                    rounds - GameConstants.BattleLogMaxLines));
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
                $"EnemyHP={enemyHp}, AmmoSpent={result.AmmoSpent}, PlayerHP={result.PlayerHpRemaining}, " +
                $"Morale={state.Morale}, RegionStatus={region.Status}");

            return result;
        }
    }
}
