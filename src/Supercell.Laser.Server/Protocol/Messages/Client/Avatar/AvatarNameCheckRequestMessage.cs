namespace Supercell.Laser.Server.Protocol.Messages.Client.Avatar
{
    using Supercell.Laser.Server.Network;
    using Supercell.Laser.Server.Protocol.Messages.Server.Avatar;
    using Supercell.Laser.Titan.DataStream;

    internal class AvatarNameCheckRequestMessage : PiranhaMessage
    {
        internal AvatarNameCheckRequestMessage(Connection connection, ByteStream stream) : base(connection, stream)
        {
        }

        internal string NewName;

        internal override void Decode()
        {
            NewName = Stream.ReadString();
        }

        internal override void Handle()
        {
            Connection.Avatar.Name = NewName;
            new AvatarNameCheckResponseMessage(Connection).Send();
        }
    }
}
