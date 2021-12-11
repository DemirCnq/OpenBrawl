namespace Supercell.Laser.Server.Protocol.Messages.Server.Matchmaking
{
    using Supercell.Laser.Server.Network;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    internal class MatchMakingStatusMessage : PiranhaMessage
    {
        internal MatchMakingStatusMessage(Connection connection) : base(connection)
        {
            Type = Enums.Message.MatchMakingStatus;
        }

        internal int Timer;
        internal int PlayersFound;
        internal int MaxPlayers;

        internal bool UseTimer;

        internal override void Encode()
        {
            Stream.WriteInt(Timer);
            Stream.WriteInt(PlayersFound);
            Stream.WriteInt(MaxPlayers);
            Stream.WriteInt(0);
            Stream.WriteInt(0);

            Stream.WriteBoolean(UseTimer);
        }
    }
}
