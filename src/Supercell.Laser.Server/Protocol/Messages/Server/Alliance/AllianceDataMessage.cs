namespace Supercell.Laser.Server.Protocol.Messages.Server.Alliance
{
    using Supercell.Laser.Server.Network;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    internal class AllianceDataMessage : PiranhaMessage
    {
        internal AllianceDataMessage(Connection connection) : base(connection)
        {
            Type = Enums.Message.AllianceData;
        }

        internal override void Encode()
        {
            Stream.WriteVInt(2);

            Stream.WriteLong(1337);
            Stream.WriteString("PW Brawl");

            Stream.WriteVInt(8);
            Stream.WriteVInt(0);

            Stream.WriteVInt(1);
            Stream.WriteVInt(1);

            Stream.WriteVInt(7777);
            Stream.WriteVInt(1337);

            Stream.WriteVInt(0);
            Stream.WriteString("RU");
            Stream.WriteVInt(0);
            Stream.WriteVInt(0);

            Stream.WriteString("Server made by XeonDev#1710");

            Stream.WriteVInt(0);
        }
    }
}
