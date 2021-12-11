namespace Supercell.Laser.Server.Network.Udp
{
    using Supercell.Laser.Server.Protocol.Messages;
    using Supercell.Laser.Titan.DataStream;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    internal class UdpSingleMessagePacket
    {
        internal PiranhaMessage message;
        internal UdpLaserSocket Socket;

        internal ByteStream Stream = new ByteStream(15);

        internal void Encode()
        {
            message.Encode();

            Stream.WriteLong(Socket.SessionId);
            Stream.WriteShort(0);

            byte[] payload = message.Stream.ToArray();

            Stream.WriteVInt((short)message.Type);
            Stream.WriteVInt(message.Stream.Offset);
            Stream.Write(payload, message.Stream.Offset);
        }
    }
}
