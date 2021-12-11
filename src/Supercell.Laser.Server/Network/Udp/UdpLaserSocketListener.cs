namespace Supercell.Laser.Server.Network.Udp
{
    using Supercell.Laser.Server.Logic.Battle;
    using Supercell.Laser.Server.Logic.Battle.Input;
    using Supercell.Laser.Server.Logic.Battle.Mode;
    using Supercell.Laser.Server.Logic.Slots;
    using Supercell.Laser.Server.Protocol.Messages;
    using Supercell.Laser.Server.Protocol.Messages.Client.Battle;
    using Supercell.Laser.Server.Protocol.Messages.Server.Battle;
    using Supercell.Laser.Titan.DataStream;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Net.Sockets;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;

    internal static class UdpLaserSocketListener
    {
        internal const int Port = 1337;

        private static long SessionSeed;
        private static Socket Socket;
        private static Thread Thread;

        private static Dictionary<long, UdpLaserSocket> Sessions;
        private static List<BattleMode> BattleModes;

        internal static UdpLaserSocket GetSocketById(long sessionId)
        {
            if (Sessions.ContainsKey(sessionId))
            {
                return Sessions[sessionId];
            }
            return null;
        }

        internal static void Init()
        {
            SessionSeed = 1;
            Socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

            BattleModes = new List<BattleMode>();
            Sessions = new Dictionary<long, UdpLaserSocket>();

            //Socket.ReceiveTimeout = 150;
            Socket.Bind(new IPEndPoint(IPAddress.Any, Port));
            Thread = new Thread(UdpLaserSocketListener.Update);
            Thread.Start();

            Console.WriteLine("Battle server listening on port " + Port);
        }

        /// <summary>
        /// Creates new battle session for signle player battle
        /// </summary>
        internal static void CreateBattleSession(Connection connection)
        {
            connection.UdpSessionId = SessionSeed;
            SessionSeed++;
            UdpLaserSocket socket = new UdpLaserSocket(connection.UdpSessionId, connection);
            Sessions.Add(connection.UdpSessionId, socket);

            BattleMode battleMode = new BattleMode();
            battleMode.Sockets.Add(socket);
            battleMode.Players.Add(connection.UdpSessionId, new LogicPlayer(connection.UdpSessionId));
            BattleModes.Add(battleMode);

            socket.BattleMode = battleMode;

            battleMode.InitSinglePlayer();
        }

        /// <summary>
        /// Creates new battle session for multiplayer battle
        /// </summary>
        internal static void CreateBattleSession(List<Connection> connections)
        {
            BattleMode battleMode = new BattleMode();

            foreach (Connection connection in connections)
            {
                connection.UdpSessionId = SessionSeed;
                SessionSeed++;
                UdpLaserSocket socket = new UdpLaserSocket(connection.UdpSessionId, connection);
                Sessions.Add(connection.UdpSessionId, socket);

                socket.BattleMode = battleMode;
                battleMode.Sockets.Add(socket);
                battleMode.Players.Add(connection.UdpSessionId, new LogicPlayer(connection.UdpSessionId)
                {
                    Avatar = connection.Avatar,
                    SelectedCharacter = connection.Avatar.SelectedBrawler
                });
            }

            foreach (Connection connection in connections)
            {
                StartLoadingMessage message = new StartLoadingMessage(connection);
                message.Location = Events.ActiveEvents[0].Location;
                message.Players = battleMode.Players.Values.ToList();
                message.Send();
            }

            BattleModes.Add(battleMode);

            battleMode.InitMultiPlayer();
        }

        private static void Update()
        {
            EndPoint remote = new IPEndPoint(IPAddress.Any, 0);
            while (true)
            {
                byte[] ReceiveBuffer = new byte[1400];
                try
                {
                    int n = Socket.ReceiveFrom(ReceiveBuffer, ref remote);
                    OnReceive(ReceiveBuffer, remote, n);
                }
                catch (SocketException)
                {

                }
            }
        }

        internal static void Send(byte[] data,int length, EndPoint endPoint)
        {
            Socket.SendTo(data, 0, length, SocketFlags.None, endPoint);
        }

        private static async void ReceiveAsync(byte[] ReceiveBuffer, EndPoint remote, int n)
        {
            await Task.Run(() =>
            {
                OnReceive(ReceiveBuffer, remote, n);
            });
        }

        private static void OnReceive(byte[] ReceiveBuffer, EndPoint remote, int n)
        {
            if (n > 0)
            {
                ByteStream stream = new ByteStream(ReceiveBuffer);

                long sessionId = stream.ReadLong();
                                 stream.ReadShort();


                int messageType = stream.ReadVInt();
                int messageEncodingLength = stream.ReadVInt();
                byte[] messageBytes = stream.ReadBytes(messageEncodingLength);

                ByteStream messageStream = new ByteStream(messageBytes);

                if (Sessions.ContainsKey(sessionId))
                {
                    if (messageType == 10555)
                    {
                        if (Sessions[sessionId].EndPoint == null) Sessions[sessionId].EndPoint = remote;
                        ClientInputMessage message = new ClientInputMessage(messageStream);
                        message.Stream = messageStream;
                        message.Decode();

                        foreach (ClientInput input in message.Inputs)
                        {
                            input.SessionId = sessionId;
                            Sessions[sessionId].BattleMode.AddInput(input);
                        }
                    }
                    else
                    {
                        Console.WriteLine("UdpLaserSocketListener.OnReceive - non-udp message in UDP packet! Type: " + messageType);
                        return;
                    }
                }
                else
                {
                    Console.WriteLine("UdpLaserSocketListener.OnReceive - unknown session!");
                }
            }
        }
    }
}
