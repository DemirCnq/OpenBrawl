namespace Supercell.Laser.Server.Logic.Game
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    internal static class LogicGamePlayUtil
    {
        private static readonly int[] SoloBattleRoyaleTrophies = new int[] { 8, 5, 2, -2};

        internal static int GetTrophiesRewardBattleRoyale(int rank, int currentTrophies)
        {
            return SoloBattleRoyaleTrophies[rank - 1];
        }
    }
}
