namespace Supercell.Laser.Server.Logic
{
    using Supercell.Laser.Server.Logic.Slots;
    using Supercell.Laser.Server.Network;
    using Supercell.Laser.Server.Network.Udp;
    using Supercell.Laser.Server.Protocol.Messages;
    using Supercell.Laser.Server.Protocol.Messages.Server.Battle;
    using Supercell.Laser.Server.Protocol.Messages.Server.Matchmaking;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;

    internal static class GameMatchmakingManager
    {
        private static List<Connection> ToAdd;
        private static List<Connection> Queue;
        private static Thread Thread;

        private const int MaxPlayers = 4;

        internal static void Init()
        {
            Queue = new List<Connection>();
            ToAdd = new List<Connection>();
            Thread = new Thread(GameMatchmakingManager.Update);
            Thread.Start();
        }

        internal static void Enqueue(Connection connection)
        {
            connection.InMatchmaking = true;
            ToAdd.Add(connection);
        }

        private static void Update()
        {
            while (true)
            {
                List<Connection> ToRemove = new List<Connection>();

                foreach (Connection connection in ToAdd)
                {
                    if (!Queue.Contains(connection)) Queue.Add(connection);
                }
                ToAdd.Clear();

                foreach (Connection connection in Queue)
                {
                    if (!connection.IsConnected) ToRemove.Add(connection);
                    else if (!connection.InMatchmaking) ToRemove.Add(connection);
                }

                foreach (Connection connection in ToRemove)
                {
                    Queue.Remove(connection);
                }
                ToRemove.Clear();

                if (Queue.Count >= MaxPlayers)
                {
                    StartGame();
                }

                foreach (Connection connection in Queue)
                {
                    MatchMakingStatusMessage status = new MatchMakingStatusMessage(connection);
                    status.PlayersFound = Queue.Count;
                    status.MaxPlayers = MaxPlayers;
                    status.Send();
                }

                Thread.Sleep(250);
            }
        }

        private static void StartGame()
        {
            List<Connection> connections = new List<Connection>();

            for (int i = 0; i < MaxPlayers; i++)
            {
                connections.Add(Queue[0]);
                Queue.RemoveAt(0);
            }

            UdpLaserSocketListener.CreateBattleSession(connections);
        }
    }
}
