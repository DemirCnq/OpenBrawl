namespace Supercell.Laser.Server.Protocol.Messages.Client.Avatar
{
    using Supercell.Laser.Server.Helpers;
    using Supercell.Laser.Server.Network;
    using Supercell.Laser.Server.Protocol.Command.Server.Avatar;
    using Supercell.Laser.Server.Protocol.Messages.Server.Home;
    using Supercell.Laser.Titan.DataStream;

    internal class ChangeNameMessage : PiranhaMessage
    {
        internal ChangeNameMessage(Connection connection, ByteStream stream) : base(connection, stream)
        {
        }

        internal string Name;

        internal override void Decode()
        {
            this.Name = Stream.ReadString();
        }

        internal override void Handle()
        {
            this.Connection.Avatar.Name = this.Name;
            this.Connection.Avatar.NameSet = true;

            new AvailableServerCommandMessage(this.Connection, new LogicChangeNameCommand(this.Connection)).Send();

            this.Connection.Avatar.Save();
        }
    }
}
