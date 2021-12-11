namespace Supercell.Laser.Server.Protocol.Messages.Server.Account
{
    using Supercell.Laser.Server.Network;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    internal class KeepAliveServerMessage : PiranhaMessage
    {
        internal KeepAliveServerMessage(Connection connection) : base(connection)
        {
            Type = Enums.Message.KeepAliveServer;
        }
    }
}
