namespace Supercell.Laser.Server.Protocol.Messages.Client.Alliance
{
    using Supercell.Laser.Server.Network;
    using Supercell.Laser.Server.Protocol.Messages.Server.Alliance;
    using Supercell.Laser.Titan.DataStream;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    internal class AskForAllianceDataMessage : PiranhaMessage
    {
        internal AskForAllianceDataMessage(Connection connection, ByteStream stream) : base(connection, stream)
        {
        }

        internal override void Handle()
        {
            new AllianceDataMessage(Connection).Send();
        }
    }
}
