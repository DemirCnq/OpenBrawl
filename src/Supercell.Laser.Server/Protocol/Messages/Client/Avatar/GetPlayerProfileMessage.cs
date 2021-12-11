namespace Supercell.Laser.Server.Protocol.Messages.Client.Avatar
{
    using Supercell.Laser.Server.Network;
    using Supercell.Laser.Server.Protocol.Messages.Server.Avatar;
    using Supercell.Laser.Titan.DataStream;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    internal class GetPlayerProfileMessage : PiranhaMessage
    {
        internal GetPlayerProfileMessage(Connection connection, ByteStream stream) : base(connection, stream)
        {
        }

        internal int HighId;
        internal int LowId;

        internal override void Decode()
        {
            HighId = Stream.ReadInt();
            LowId = Stream.ReadInt();
        }

        internal override void Handle()
        {
            new PlayerProfileMessage(Connection, HighId, LowId).Send();
        }
    }
}
