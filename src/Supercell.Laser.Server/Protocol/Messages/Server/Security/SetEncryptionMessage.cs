namespace Supercell.Laser.Server.Protocol.Messages.Server.Security
{
    using Supercell.Laser.Server.Network;
    using Supercell.Laser.Server.Protocol.Enums;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    internal class SetEncryptionMessage : PiranhaMessage
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
        /// Initializes a new instance of the <see cref="SetEncryptionMessage"/> class.
        /// </summary>
        public SetEncryptionMessage(Connection connection) : base(connection)
        {
            this.Type = Message.SetEncryption;
        }

        internal string Nonce;

        internal override void Encode()
        {
            Stream.Write(Encoding.UTF8.GetBytes(Nonce));
        }
    }
}
