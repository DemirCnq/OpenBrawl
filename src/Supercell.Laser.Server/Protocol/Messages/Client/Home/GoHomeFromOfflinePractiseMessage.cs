namespace Supercell.Laser.Server.Protocol.Messages.Client.Home
{
    using Supercell.Laser.Server.Network;
    using Supercell.Laser.Server.Protocol.Messages.Server.Home;
    using Supercell.Laser.Titan.DataStream;

    internal class GoHomeFromOfflinePractiseMessage : PiranhaMessage
    {
        internal GoHomeFromOfflinePractiseMessage(Connection connection, ByteStream stream) : base(connection, stream)
        {
            this.Connection.State = Titan.Logic.Enums.State.Home;
        }

        internal override void Handle()
        {
            new OwnHomeDataMessage(this.Connection).Send();
        }
    }
}
