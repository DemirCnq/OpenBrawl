namespace Supercell.Laser.Server.Network
{
    using System;
    using System.Linq;
    using System.Net;
    using System.Net.Sockets;
    using System.Reflection;

    using Supercell.Laser.Titan.Helpers;
    using Supercell.Laser.Titan.Logic.Enums;

    using Supercell.Laser.Server.Logic.Avatar;
    using Supercell.Laser.Server.Protocol;

    internal class Connection
    {
        internal readonly Socket Socket;
        internal Token Token;

        internal readonly Messaging Messaging;
        
        internal LogicClientAvatar Avatar;

        internal State State = State.Disconnected;

        internal long UdpSessionId;

        internal bool InMatchmaking;

        internal string Nonce;

        // LogicDefines
        internal string MACAddress;
        internal string DeviceModel;
        // ------------

        internal int MessageTick = 1;

        /// <summary>
        /// Initializes a new instance of the <see cref="Connection"/> class.
        /// </summary>
        internal Connection(Socket socket)
        {
            this.Socket    = socket;
            this.Messaging = new Messaging(this);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Connection"/> class.
        /// </summary>
        internal Connection(Socket socket, Token token) : this(socket)
        {
            this.Token = token;
        }

        /// <summary>
        /// Gets the end point of this connection.
        /// </summary>
        internal IPEndPoint EndPoint
        {
            get
            {
                return (IPEndPoint)this.Socket.RemoteEndPoint;
            }
        }

        /// <summary>
        /// Gets a value indicating whether this <see cref="Connection"/> is connected.
        /// </summary>
        internal bool IsConnected
        {
            get
            {
                if (this.Socket.Connected)
                {
                    try
                    {
                        if (!this.Socket.Poll(1000, SelectMode.SelectRead) || this.Socket.Available != 0)
                        {
                            return true;
                        }
                    }
                    catch (Exception)
                    {
                        return false;
                    }
                }

                return false;
            }
        }

        /// <summary>
        /// Shows the values.
        /// </summary>
        internal void ShowValues()
        {
            foreach (FieldInfo field in this.GetType().GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance).Where(field => field != null))
            {
                Debugger.Info(field.Name.Pad() + " : " + (!string.IsNullOrEmpty(field.Name) ? (field.GetValue(this) != null ? field.GetValue(this).ToString() : "(null)") : "(null)").Pad(40));
            }
        }
    }
}