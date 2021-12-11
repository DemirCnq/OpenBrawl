namespace Supercell.Laser.Server.Protocol.Messages.Client.Account
{
    using Supercell.Laser.Server.Core;
    using Supercell.Laser.Server.Helpers;
    using Supercell.Laser.Server.Logic.Slots;
    using Supercell.Laser.Server.Network;
    using Supercell.Laser.Server.Protocol.Enums;
    using Supercell.Laser.Server.Protocol.Messages.Server.Account;
    using Supercell.Laser.Server.Protocol.Messages.Server.Alliance;
    using Supercell.Laser.Server.Protocol.Messages.Server.Home;
    using Supercell.Laser.Server.Protocol.Messages.Server.Security;
    using Supercell.Laser.Titan.DataStream;
    using Supercell.Laser.Titan.Library.Cryptography.RC4;
    using Supercell.Laser.Titan.Logic.Enums;
    using Supercell.Laser.Titan.Logic.Math;
    using System;
    using System.Threading;

    internal class LoginMessage : PiranhaMessage
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
        /// Initializes a new instance of the <see cref="LoginMessage"/> class.
        /// </summary>
        public LoginMessage(Connection connection, ByteStream stream) : base(connection, stream)
        {
        }

        private byte[] Key;
        private LogicLong AvatarID;
        private string Token;

        internal override void Decode()
        {
            if (this.Connection.State == State.HandshakeSuccess)
            {
                this.AvatarID = this.Stream.ReadLogicLong();
                this.Token = this.Stream.ReadString();

                Console.WriteLine(AvatarID);
            }
        }

        internal override void Handle()
        {
            if (this.Connection.State != State.HandshakeSuccess)
            {
                new LoginFailedMessage(Connection)
                {
                    Code = ErrorCode.CLIENT_VERSION,
                    Reason = "New version of PekkaWorld Brawl is available!",
                    UpdateUrl = "https://drive.google.com/file/d/1gh5gbuHn0mJSBXxRPMb6QqFZaQOO4ZmN/view?usp=sharing"
                }.Send();
                return;
            }

            if (this.AvatarID.Low == 0)
            {
                this.Connection.Avatar = Avatars.Create(this.Connection);
                this.Connection.Avatar.Reset();
                this.Login();
            }
            else if (this.AvatarID.Low > 0)
            {
                this.Connection.Avatar = Avatars.Get(this.Connection, this.AvatarID);

                if (this.Connection.Avatar == null)
                {
                    new LoginFailedMessage(Connection)
                    {
                        Code = ErrorCode.CUSTOM_MESSAGE,
                        Reason = "We couldn't find your account in our systems or your token is invalid."
                    }.Send();
                    return;
                }

                if (this.Connection.Avatar.Token != Token)
                {
                    new LoginFailedMessage(Connection)
                    {
                        Code = ErrorCode.CUSTOM_MESSAGE,
                        Reason = "We couldn't find your account in our systems or your token is invalid."
                    }.Send();
                    return;
                }

                this.Login();
            }
        }

        private void Login()
        {
            if (this.Connection == null)
            {
                this.Connection.Avatar.Connection = this.Connection;
            }

            this.Connection.State = State.LoggedIn;
            new LoginOkMessage(this.Connection).Send();

            this.Connection.State = State.Home;
            new OwnHomeDataMessage(this.Connection).Send();

            new MyAllianceMessage(this.Connection).Send();
        }
    }
}
