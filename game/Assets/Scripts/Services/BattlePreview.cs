using BastionUA.Core;

namespace BastionUA.Services
{
    public sealed class BattlePreview
    {
        public string RegionDisplayName;
        public RegionStatus RegionStatus;
        public int AmmoCost;
        public int CurrentAmmo;
        public bool CanAfford;
        public int EnemyHp;
        public int PlayerDamagePerRound;
        public int EnemyDamagePerRound;
        public int EstimatedRoundsToWin;
    }
}
