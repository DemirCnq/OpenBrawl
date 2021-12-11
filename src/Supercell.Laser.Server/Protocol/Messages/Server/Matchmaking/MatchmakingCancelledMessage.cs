namespace Supercell.Laser.Server.Protocol.Messages.Server.Matchmaking
{
    using Supercell.Laser.Server.Network;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    internal class MatchmakingCancelledMessage : PiranhaMessage
    {
        internal MatchmakingCancelledMessage(Connection connection) : base(connection)
        {
            Type = Enums.Message.MatchMakingCancelled;
        }
    }
}
