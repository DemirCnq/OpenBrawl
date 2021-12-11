namespace Supercell.Laser.Server.Protocol.Messages.Server.Home
{
    using Supercell.Laser.Server.Network;
    using Supercell.Laser.Server.Protocol.Enums;

    internal class OwnHomeDataMessage : PiranhaMessage
    {
        internal OwnHomeDataMessage(Connection connection) : base(connection)
        {
            this.Type = Message.OwnHomeData;
        }

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

        internal override void Encode()
        {
            Connection.Avatar.Home.Encode(Stream);
            Connection.Avatar.Encode(Stream);

            Stream.WriteVInt(10);
        }

        internal override void Handle()
        {
            Connection.Avatar.TokensReward = 0;
            Connection.Avatar.TrophiesReward = 0;
        }
    }
}
