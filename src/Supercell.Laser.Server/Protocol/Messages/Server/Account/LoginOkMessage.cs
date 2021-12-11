namespace Supercell.Laser.Server.Protocol.Messages.Server.Account
{
    using Supercell.Laser.Server.Logic;
    using Supercell.Laser.Server.Network;
    using Supercell.Laser.Server.Protocol.Enums;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    internal class LoginOkMessage : PiranhaMessage
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
        /// Initializes a new instance of the <see cref="OutOfSyncMessage"/> class.
        /// </summary>
        public LoginOkMessage(Connection connection) : base(connection)
        {
            this.Type = Message.LoginOk;
        }

        internal override void Encode()
        {
            Stream.WriteLogicLong(Connection.Avatar.Identifier);
            Stream.WriteLogicLong(Connection.Avatar.Identifier);

            Stream.WriteString(Connection.Avatar.Token);
            Stream.WriteString(null);
            Stream.WriteString(null);

            Stream.WriteInt(LogicVersion.Major);
            Stream.WriteInt(LogicVersion.Build);
            Stream.WriteInt(LogicVersion.Minor);

            Stream.WriteString(LogicVersion.ServerType);

            Stream.WriteInt(0);
            Stream.WriteInt(0);
            Stream.WriteInt(0);

            Stream.WriteString(null);
            Stream.WriteString(null);
            Stream.WriteString(null);

            Stream.WriteInt(0);

            Stream.WriteString(null);
            Stream.WriteString("RU");
            Stream.WriteString(null);

            Stream.WriteInt(1);

            Stream.WriteString(null);
            Stream.WriteString(null);
            Stream.WriteString(null);

            Stream.WriteInt(0);
            Stream.WriteString(null);
            Stream.WriteInt(1);
        }
    }
}
