namespace Supercell.Laser.Server.Protocol
{
    using Supercell.Laser.Server.Network;
    using Supercell.Laser.Server.Protocol.Enums;
    using Supercell.Laser.Server.Protocol.Messages;
    using Supercell.Laser.Titan.Core;
    using Supercell.Laser.Titan.DataStream;
    using Supercell.Laser.Titan.Library.Cryptography.RC4;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    internal class Messaging
    {
        internal readonly Connection Connection;

        internal RC4Encrypter Crypto;

        /// <summary>
        /// Initializes a new instance of the <see cref="Messaging"/> class.
        /// </summary>
        internal Messaging(Connection connection)
        {
            this.Connection = connection;
        }

        /// <summary>
        /// Gets a value indicating whether <see cref="Messaging.Connection"/> is connected.
        /// </summary>
        internal bool IsConnected
        {
            get
            {
                return this.Connection.IsConnected;
            }
        }

        /// <summary>
        /// Sets the encrypters.
        /// </summary>
        internal void SetEncrypters()
        {
            this.Crypto = new RC4Encrypter(LogicLaserMessageFactory.RC4Key, Connection.Nonce);
        }

        /// <summary>
        /// Sets the encrypters.
        /// </summary>
        internal void SetEncrypters(string nonce)
        {
            this.Crypto = new RC4Encrypter(LogicLaserMessageFactory.RC4Key, nonce);
        }

        /// <summary>
        /// Processes the specified buffer.
        /// </summary>
        internal void OnReceive(byte[] buffer)
        {
            if (buffer != null && this.IsConnected)
            {
                if (buffer.Length >= 7 && buffer.Length <= Constants.BufferSize)
                {
                    Messaging.ReadHeader(buffer, out int id, out int length, out int version);

                    if ((id - 10000 < 10000) && (buffer.Length - 7 >= length))
                    {
                        using (ByteStream stream = new ByteStream(buffer.Skip(7).ToArray()))
                        {
                            PiranhaMessage message = LogicLaserMessageFactory.CreateMessageByType(id, this.Connection, stream);
                            if (message != null)
                            {
                                message.Type = (Message)id;
                                message.Length = length;
                                message.Version = (short)version;

                                MessageManager.Enqueue(message);
                            }
                            else
                            {
                                message = new PiranhaMessage(Connection, stream);
                                message.Length = length;
                                Console.WriteLine("Unknown Message: " + id);
                            }

                            if (!this.Connection.Token.Aborting)
                            {
                                this.Connection.Token.Packet.RemoveRange(0, length + 7);

                                if (buffer.Length - 7 - length >= 7)
                                {
                                    this.OnReceive(stream.ReadBytes(buffer.Length - 7 - length));
                                }
                            }
                        }
                    }
                    else
                    {
                        Debugger.Error("The received buffer length is inferior the header length.");

                        this.Connection.Token.Packet.Clear();
                    }
                }
                else
                {
                    //ServerConnection.Disconnect(this.Connection.Token.Args);
                }
            }
        }

        /// <summary>
        /// Reads the message header.
        /// </summary>
        private static void ReadHeader(IReadOnlyList<byte> buffer, out int id, out int length, out int version)
        {
            id = buffer[0] << 8 | buffer[1];
            length = buffer[2] << 16 | buffer[3] << 8 | buffer[4];
            version = buffer[5] << 8 | buffer[6];
        }

        /// <summary>
        /// Sends the specified <see cref="PiranhaMessage"/>.
        /// </summary>
        internal void Send(PiranhaMessage message) => MessageManager.Enqueue(message);
    }
}
