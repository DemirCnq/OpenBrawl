namespace Supercell.Laser.Server.Protocol.Messages.Client.Home
{
    using Supercell.Laser.Server.Network;
    using Supercell.Laser.Server.Protocol.Messages.Server.Matchmaking;
    using Supercell.Laser.Titan.DataStream;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    internal class CancelMatchmakingMessage : PiranhaMessage
    {
        internal CancelMatchmakingMessage(Connection connection, ByteStream stream) : base(connection, stream)
        {

        }

        internal override void Handle()
        {
            this.Connection.InMatchmaking = false;
            new MatchmakingCancelledMessage(Connection).Send();
        }
    }
}
