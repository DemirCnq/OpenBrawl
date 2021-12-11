namespace Supercell.Laser.Server.Protocol.Messages.Server.Account
{
    using Supercell.Laser.Server.Core;
    using Supercell.Laser.Server.Network;
    using Supercell.Laser.Server.Network.Udp;
    using Supercell.Laser.Server.Protocol.Enums;

    internal class UdpConnectionInfoMessage : PiranhaMessage
    {
        internal UdpConnectionInfoMessage(Connection connection) : base(connection)
        {
            Type = Message.UdpConnectionInfo;
        }

        internal override void Encode()
        {
            Stream.WriteVInt(UdpLaserSocketListener.Port);

            Stream.WriteString(Settings.UdpIp);

            Stream.WriteInt(10);
            Stream.WriteLong(Connection.UdpSessionId);
            Stream.WriteShort(0);

            Stream.WriteInt(0);
        }
    }
}
