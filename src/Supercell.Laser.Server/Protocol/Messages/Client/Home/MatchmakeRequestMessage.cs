namespace Supercell.Laser.Server.Protocol.Messages.Client.Home
{
    using Supercell.Laser.Server.Logic;
    using Supercell.Laser.Server.Logic.Slots;
    using Supercell.Laser.Server.Network;
    using Supercell.Laser.Server.Network.Udp;
    using Supercell.Laser.Server.Protocol.Enums;
    using Supercell.Laser.Server.Protocol.Messages.Server.Account;
    using Supercell.Laser.Server.Protocol.Messages.Server.Battle;
    using Supercell.Laser.Server.Protocol.Messages.Server.Matchmaking;
    using Supercell.Laser.Titan.DataStream;

    internal class MatchmakeRequestMessage : PiranhaMessage
    {
        /// <summary>
        /// The service node for this message.
        /// </summary>
        internal override ServiceNode Node
        {
            get
            {
                return ServiceNode.Home;
            }
        }

        internal MatchmakeRequestMessage(Connection connection, ByteStream stream) : base(connection, stream)
        {
        }

        internal int SelectedCharacter;

        internal override void Decode()
        {
            Stream.ReadVInt();
            SelectedCharacter = Stream.ReadVInt();
        }

        internal override void Handle()
        {
            if (Check())
            {
                MatchMakingStatusMessage status = new MatchMakingStatusMessage(Connection);
                status.PlayersFound = 1;
                status.MaxPlayers = 4;
                status.Send();

                Connection.Avatar.SelectedBrawler = new Titan.Files.CsvData.LogicData(16, SelectedCharacter);

                GameMatchmakingManager.Enqueue(Connection);
            }
            else
            {
                MatchMakingStatusMessage status = new MatchMakingStatusMessage(Connection);
                status.PlayersFound = 0;
                status.MaxPlayers = 1;
                status.Send();

                new LoginFailedMessage(Connection)
                {
                    Code = ErrorCode.CUSTOM_MESSAGE,
                    Reason = "Unable to start matchmaking."
                }.Send();
            }
        }

        internal bool Check()
        {
            if (SelectedCharacter < 0 || Connection.Avatar.Brawlers.Count < SelectedCharacter) return false;

            var brawler = Connection.Avatar.Brawlers[SelectedCharacter];

            return brawler.Unlocked && !brawler.Disabled;
        }
    }
}
