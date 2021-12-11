namespace Supercell.Laser.Server.Logic.Battle.Objects
{
    using Supercell.Laser.Server.DataStream;
    using Supercell.Laser.Titan.Files.CsvData;
    using Supercell.Laser.Titan.Logic.Math;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    internal class LogicGameObjectServer
    {
        internal const float angleCorrection = (float)(System.Math.PI * 90 / 180.0);

        internal LogicGameObjectServer(LogicData data)
        {
            Data = data;
            Vector2 = new LogicVector2();
        }

        internal LogicData Data;
        internal LogicVector2 Vector2;

        internal int X => Vector2.X;
        internal int Y => Vector2.Y;
        internal int Z;

        internal int ObjectReference;

        internal int ObjectIdx;

        internal LogicGameObjectManagerServer LogicGameObjectManager;

        internal int Angle;

        internal LogicBattleModeServer BattleModeServer
        {
            get
            {
                if (LogicGameObjectManager != null)
                {
                    return LogicGameObjectManager.BattleModeServer;
                }
                return null;
            }
        }

        internal virtual void Tick()
        {
            ;
        }

        internal virtual void Encode(BitStream stream, int idx = -1)
        {
            if (idx == -1)
            {
                idx = ObjectIdx;
            }
            stream.WritePositiveVInt(X, 4);
            stream.WritePositiveVInt(Y, 4);
            stream.WritePositiveVInt(idx, 3);
            stream.WritePositiveVInt(Z, 4);
        }

        internal virtual void SetPosition(int x, int y, int z)
        {
            Vector2.X = x;
            Vector2.Y = y;
            Z = z;
        }

        internal virtual void SetPositionByTile(int xTile, int yTile)
        {
            Vector2.X = 150 + 300 * xTile;
            Vector2.Y = 50 + 300 * yTile;
        }

        internal void GetTile(out int xTile, out int yTile)
        {
            xTile = ((Vector2.X - 150) / 300);
            yTile = ((Vector2.Y - 50) / 300);
        }

        internal static void GetTile(int x, int y, out int xTile, out int yTile)
        {
            xTile = ((x - 150) / 300);
            yTile = ((y - 50) / 300);
        }

        internal void AttachLogicGameObjectManager(LogicGameObjectManagerServer manager)
        {
            LogicGameObjectManager = manager;
        }
    }
}
