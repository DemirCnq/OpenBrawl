namespace Supercell.Laser.Server.Protocol.Messages.Client.Alliance
{
    using Supercell.Laser.Server.Files;
    using Supercell.Laser.Server.Logic;
    using Supercell.Laser.Server.Logic.Slots;
    using Supercell.Laser.Server.Network;
    using Supercell.Laser.Server.Protocol.Messages.Server.Alliance;
    using Supercell.Laser.Titan.DataStream;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    internal class ChatToAllianceStreamMessage : PiranhaMessage
    {
        internal ChatToAllianceStreamMessage(Connection connection, ByteStream stream) : base(connection, stream)
        {
        }

        internal string Message;

        internal override void Decode()
        {
            Message = Stream.ReadString();
        }

        internal override void Handle()
        {
            new AllianceStreamEntryMessage(Connection)
            {
                SenderId = Connection.Avatar.Identifier,
                SenderName = Connection.Avatar.Name,
                Message = Message
            }.Send();

            string response = null;

            if (Message == "/help")
            {
                response = "Commands:\n/help - shows this message\n/status - shows server status\n";
            }
            else if (Message == "/status")
            {
                response = $"Server status:\nBuild: alpha 0.1.1\nVersion: {LogicVersion.VersionString}\nSHA:{Fingerprint.Sha}\nUsed memory:{System.Diagnostics.Process.GetCurrentProcess().WorkingSet64 / (1024 * 1024)} MB\nAccounts Registered: {Avatars.Count}\nPlayers online: {Connections.Count}";
            }

            if (response != null)
            {
                new AllianceStreamEntryMessage(Connection)
                {
                    SenderId = new Titan.Logic.Math.LogicLong(2),
                    SenderName = "PekkaWorld",
                    Message = response
                }.Send();
            }
        }
    }
}
