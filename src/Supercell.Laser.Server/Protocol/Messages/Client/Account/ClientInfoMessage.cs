namespace Supercell.Laser.Server.Protocol.Messages.Client.Account
{
    using Supercell.Laser.Server.Network;
    using Supercell.Laser.Server.Protocol.Enums;
    using Supercell.Laser.Server.Protocol.Messages.Server.Account;
    using Supercell.Laser.Titan.DataStream;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    internal class ClientInfoMessage : PiranhaMessage
    {
        internal override ServiceNode Node 
        {
            get
            {
                return ServiceNode.Account;
            }
        }

        internal ClientInfoMessage(Connection connection, ByteStream stream) : base(connection, stream)
        {
        }

        internal override void Handle()
        {
            new UdpConnectionInfoMessage(this.Connection).Send();
        }
    }
}
