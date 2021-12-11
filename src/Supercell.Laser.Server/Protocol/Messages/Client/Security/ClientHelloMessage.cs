namespace Supercell.Laser.Server.Protocol.Messages.Client.Security
{
    using Supercell.Laser.Server.Core;
    using Supercell.Laser.Server.Network;
    using Supercell.Laser.Server.Protocol.Enums;
    using Supercell.Laser.Server.Protocol.Messages.Server.Account;
    using Supercell.Laser.Server.Protocol.Messages.Server.Security;
    using Supercell.Laser.Titan.DataStream;
    using Supercell.Laser.Titan.Logic.Enums;

    internal class ClientHelloMessage : PiranhaMessage
    {
        /// <summary>
        /// The service node for this message.
        /// </summary>
        internal override ServiceNode Node
        {
            get
            {
                return ServiceNode.Account;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ClientHelloMessage"/> class.
        /// </summary>
        public ClientHelloMessage(Connection connection, ByteStream stream) : base(connection, stream)
        {
            this.Connection.State = State.Handshake;
        }

        internal override void Decrypt()
        {

        }

        internal override void Handle()
        {
            Connection.Nonce = Loader.Random.GenerateRandomString(32);
            Connection.Messaging.SetEncrypters();
            new ServerHelloMessage(this.Connection).Send();

            /*new LoginFailedMessage(Connection)
            {
                Code = ErrorCode.SERVER_MAINTENANCE
            }.Send();*/
        }
    }
}
