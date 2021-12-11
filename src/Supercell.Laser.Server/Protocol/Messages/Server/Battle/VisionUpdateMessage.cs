namespace Supercell.Laser.Server.Protocol.Messages.Server.Battle
{
    using Supercell.Laser.Server.DataStream;
    using Supercell.Laser.Server.Network;
    using Supercell.Laser.Server.Protocol.Enums;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    internal class VisionUpdateMessage : PiranhaMessage
    {
        /// <summary>
        /// The service node for this message.
        /// </summary>
        internal override ServiceNode Node
        {
            get
            {
                return ServiceNode.Battle;
            }
        }

        internal int HandledInput;


        internal VisionUpdateMessage() : base(null)
        {
            Type = Message.VisionUpdate;
        }

        internal VisionUpdateMessage(Connection connection) : base(connection)
        {
            Type = Message.VisionUpdate;
        }

        internal int TicksGone;
        internal int ViewersCount;
        internal BitStream VisionBitStream;

        internal override void Encode()
        {
            Stream.WriteVInt(TicksGone);
            Stream.WriteVInt(HandledInput);
            Stream.WriteVInt(0);
            Stream.WriteVInt(ViewersCount);

            Stream.WriteBoolean(false);

            byte[] data = VisionBitStream.GetStreamData();

            Stream.WriteInt(data.Length);
            Stream.Write(data, data.Length);
        }
    }
}
