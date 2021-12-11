namespace Supercell.Laser.Server.Protocol.Command.Server.Avatar
{
    using Supercell.Laser.Server.Network;

    internal class LogicChangeNameCommand : LogicCommand
    {
        internal LogicChangeNameCommand(Connection connection) : base(connection)
        {
            this.Type = Enums.Command.ChangeName;
        }

        internal override void Encode()
        {
            Stream.WriteString(Connection.Avatar.Name);
            Stream.WriteVInt(0);
        }
    }
}
