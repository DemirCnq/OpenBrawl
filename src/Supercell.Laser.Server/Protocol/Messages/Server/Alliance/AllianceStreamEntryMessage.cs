namespace Supercell.Laser.Server.Protocol.Messages.Server.Alliance
{
    using Supercell.Laser.Server.Network;
    using Supercell.Laser.Titan.DataStream;
    using Supercell.Laser.Titan.Logic.Math;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    internal class AllianceStreamEntryMessage : PiranhaMessage
    {
        internal AllianceStreamEntryMessage(Connection connection) : base(connection)
        {
            Type = Enums.Message.AllianceStreamEntry;
        }

        internal string Message;
        internal string SenderName;
        internal LogicLong SenderId;

        internal override void Encode()
        {
            Stream.WriteVInt(2);
            Stream.WriteVInt(0);
            Stream.WriteVInt(Connection.MessageTick);
            Connection.MessageTick++;

            Stream.EncodeLogicLong(SenderId);
            Stream.WriteString(SenderName);
            Stream.WriteVInt(0);
            Stream.WriteVInt(0);
            Stream.WriteVInt(0);

            Stream.WriteString(Message);
        }
    }
}
