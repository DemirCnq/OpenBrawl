namespace Supercell.Laser.Server.Protocol.Messages.Client.Home
{
    using Supercell.Laser.Server.Network;
    using Supercell.Laser.Server.Protocol.Messages.Server.Home;
    using Supercell.Laser.Titan.DataStream;

    internal class GetLeaderboardMessage : PiranhaMessage
    {
        internal GetLeaderboardMessage(Connection connection, ByteStream stream) : base(connection, stream)
        {
        }

        internal override void Handle()
        {
            new LeaderboardMessage(Connection).Send();
        }
    }
}
