namespace Supercell.Laser.Server.Network.Udp
{
    using Supercell.Laser.Server.Logic.Battle.Mode;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Text;
    using System.Threading.Tasks;

    internal class UdpLaserSocket
    {
        internal EndPoint EndPoint;
        internal long SessionId;

        internal BattleMode BattleMode;

        internal Connection TcpConnection;

        internal bool Alive;

        internal UdpLaserSocket(EndPoint ep, long s)
        {
            EndPoint = ep;
            SessionId = s;
            Alive = true;
        }

        internal UdpLaserSocket(long s, Connection connection)
        {
            TcpConnection = connection;
            SessionId = s;
            Alive = true;
        }

        internal void Send(UdpSingleMessagePacket packet)
        {
            if (Alive)
            {
                packet.Encode();

                byte[] data = packet.Stream.ToArray();
                UdpLaserSocketListener.Send(data, packet.Stream.Offset, EndPoint);
            }
            else
            {
                Console.WriteLine("UdpLaserSocket.Send called when socket is dead.");
            }
        }
    }
}
