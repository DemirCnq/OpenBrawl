namespace Supercell.Laser.Server.Protocol.Messages.Client.Account
{
    using Supercell.Laser.Server.Network;
    using Supercell.Laser.Server.Protocol.Messages.Server.Account;
    using Supercell.Laser.Titan.DataStream;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    internal class KeepAliveMessage : PiranhaMessage
    {
        internal KeepAliveMessage(Connection connection, ByteStream stream) : base(connection, stream)
        {

        }

        internal override void Handle()
        {
            new KeepAliveServerMessage(Connection).Send();
        }
    }
}
