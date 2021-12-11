namespace Supercell.Laser.Server.Core
{
    using Supercell.Laser.Server.Database;
    using Supercell.Laser.Server.Files;
    using Supercell.Laser.Server.Logic;
    using Supercell.Laser.Server.Logic.Slots;
    using Supercell.Laser.Server.Network;
    using Supercell.Laser.Server.Network.Udp;
    using Supercell.Laser.Server.Protocol;
    using Supercell.Laser.Titan.Logic.Enums;
    using Supercell.Laser.Titan.Logic.Math;
    using System;

    internal static class Loader
    {
        internal static LogicRandom Random;

        internal static bool Initialized
        {
            get;
            private set;
        }

        internal static void Init()
        {
            if (Loader.Initialized)
            {
                return;
            }

            Loader.Random = new LogicRandom();

            MessageManager.Init();

            Fingerprint.Init();
            LogicDataTables.Init();

            if (Settings.Database == DBMS.Mongo)
            {
                Mongo.Init();
            }

            Avatars.Init();
            Events.Init();
            Connections.Init();

            GameMatchmakingManager.Init();

            ServerConnection.Init();
            UdpLaserSocketListener.Init();

            Loader.Initialized = true;
        }
    }
}
