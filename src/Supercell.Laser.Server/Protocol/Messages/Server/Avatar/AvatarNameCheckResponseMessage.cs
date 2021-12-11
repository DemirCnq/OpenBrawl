namespace Supercell.Laser.Server.Protocol.Messages.Server.Avatar
{
    using Supercell.Laser.Server.Network;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    internal class AvatarNameCheckResponseMessage : PiranhaMessage
    {
        internal AvatarNameCheckResponseMessage(Connection connection) : base(connection)
        {
            Type = Enums.Message.AvatarNameCheckResponse;
        }

        internal override void Encode()
        {
            Stream.WriteVInt(0);
            Stream.WriteInt(0);
            Stream.WriteString(Connection.Avatar.Name);
        }
    }
}
