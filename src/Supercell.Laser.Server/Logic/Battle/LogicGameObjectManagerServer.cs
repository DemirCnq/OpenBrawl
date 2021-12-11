namespace Supercell.Laser.Server.Logic.Battle
{
    using Supercell.Laser.Server.DataStream;
    using Supercell.Laser.Server.Logic.Battle.Objects;
    using Supercell.Laser.Server.Logic.Battle.Tiles;
    using Supercell.Laser.Titan.Logic;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    internal class LogicGameObjectManagerServer
    {
        internal LogicBattleModeServer BattleModeServer;

        private LogicArrayList<LogicGameObjectServer> LogicGameObjects;

        private Queue<LogicGameObjectServer> ToAdd;
        private Queue<LogicGameObjectServer> ToRemove;

        internal int ObjectReferenceSeed;

        internal LogicGameObjectManagerServer()
        {
            LogicGameObjects = new LogicArrayList<LogicGameObjectServer>();
            ToAdd = new Queue<LogicGameObjectServer>();
            ToRemove = new Queue<LogicGameObjectServer>();
        }

        internal LogicArrayList<LogicGameObjectServer> GetGameObjects()
        {
            return LogicGameObjects;
        }

        internal void AddGameObject(LogicGameObjectServer gameObject)
        {
            gameObject.ObjectReference = ObjectReferenceSeed;
            ObjectReferenceSeed++;
            gameObject.AttachLogicGameObjectManager(this);
            ToAdd.Enqueue(gameObject);
        }

        internal void RemoveGameObject(LogicGameObjectServer gameObject)
        {
            ToRemove.Enqueue(gameObject);
        }

        internal void Tick()
        {
            while (ToAdd.Count > 0)
            {
                LogicGameObjectServer obj = ToAdd.Dequeue();
                if (obj != null)
                {
                    LogicGameObjects.Add(obj);
                }
            }

            while (ToRemove.Count > 0)
            {
                LogicGameObjectServer obj = ToRemove.Dequeue();
                if (obj != null)
                {
                    LogicGameObjects.Remove(obj);
                }
            }

            foreach (LogicGameObjectServer gameObject in LogicGameObjects)
            {
                gameObject.Tick();
            }
        }

        internal void Encode(BitStream stream, int idx)
        {
            stream.WritePositiveInt(1000000 + idx, 21); // а кто.
            stream.WritePositiveInt(0, 1);
            stream.WriteInt(BattleModeServer.RoundState, 4); // round state. -1 - round not finished yet. 0 - lost. 1 - win. 2 - draw
            stream.WritePositiveInt(0, 1);
            stream.WritePositiveInt(1, 1);
            stream.WritePositiveInt(0, 1);
            stream.WritePositiveInt(0, 1);

            stream.WritePositiveInt(0, 6); // 9
            stream.WritePositiveInt(0, 6);
            stream.WritePositiveInt(0, 6); // 39
            stream.WritePositiveInt(0, 6); // 19

            stream.WritePositiveInt(0, 1);
            stream.WritePositiveInt(1, 1);
            stream.WritePositiveInt(1, 1);
            stream.WritePositiveInt(1, 1);

            for (int i = 0; i < BattleModeServer.Players.Size; i++)
            {
                stream.WriteBoolean(BattleModeServer.Players[i].UltiCharge >= 4000);
                stream.WritePositiveInt(0, 4);
                stream.WritePositiveInt(0, 1);
                if (i == idx)
                {
                    stream.WriteBoolean(BattleModeServer.Players[i].UsePin);
                    if (BattleModeServer.Players[i].UsePin)
                    {
                        stream.WriteInt(BattleModeServer.Players[i].PinIndex, 3);
                        stream.WritePositiveInt(BattleModeServer.Players[i].PinMaxTick, 14);
                    }
                    stream.WritePositiveInt(BattleModeServer.Players[i].UltiCharge, 12);
                    stream.WritePositiveInt(0, 1);
                }
                stream.WritePositiveInt(0, 1);
                stream.WritePositiveInt(1, 1);
            }

            stream.WritePositiveInt(BattleModeServer.PlayersAlive, 4); // players left

            for (int i = 0; i < BattleModeServer.Players.Size; i++)
            {
                stream.WritePositiveInt(0, 1);
                stream.WritePositiveInt(0, 1);
            }

            stream.WritePositiveInt(LogicGameObjects.Size, 8); // GameObjects Count

            for (int i = 0; i < LogicGameObjects.Size; i++)
            {
                stream.WritePositiveInt(LogicGameObjects[i].Data.ClassId, 5);
                stream.WritePositiveInt(LogicGameObjects[i].Data.InstanceId, 9);
            }

            for (int i = 0; i < LogicGameObjects.Size; i++)
            {
                stream.WritePositiveInt(LogicGameObjects[i].ObjectReference, 14);
            }

            for (int i = 0; i < LogicGameObjects.Size; i++)
            {
                if (LogicGameObjects[i].GetType() == typeof(LogicCharacterServer))
                {
                    var chara = (LogicCharacterServer)LogicGameObjects[i];
                    if (chara.ObjectReference == idx)
                    {
                        chara.IsPlayerControlRemoved = false; // бля))
                    }
                }
                LogicGameObjects[i].Encode(stream);
                if (LogicGameObjects[i].GetType() == typeof(LogicCharacterServer))
                {
                    var chara = (LogicCharacterServer)LogicGameObjects[i];
                    if (chara.ObjectReference == idx)
                    {
                        chara.IsPlayerControlRemoved = true;
                    }
                }
            }

            /*stream.WritePositiveInt(16, 5);
            stream.WritePositiveInt(39, 9);
            stream.WritePositiveInt(16, 5);
            stream.WritePositiveInt(7, 9);
            stream.WritePositiveInt(16, 5);
            stream.WritePositiveInt(8, 9);
            stream.WritePositiveInt(16, 5);
            stream.WritePositiveInt(51, 9);
            stream.WritePositiveInt(16, 5);
            stream.WritePositiveInt(51, 9);
            stream.WritePositiveInt(6, 5);
            stream.WritePositiveInt(261, 9);
            stream.WritePositiveInt(6, 5);
            stream.WritePositiveInt(72, 9);
            stream.WritePositiveInt(6, 5);
            stream.WritePositiveInt(73, 9);
            stream.WritePositiveInt(6, 5);
            stream.WritePositiveInt(261, 9);*/

            /*stream.WritePositiveInt(0, 14);
            stream.WritePositiveInt(1, 14);
            stream.WritePositiveInt(2, 14);
            stream.WritePositiveInt(14, 14);
            stream.WritePositiveInt(28, 14);
            stream.WritePositiveInt(49, 14);
            stream.WritePositiveInt(50, 14);
            stream.WritePositiveInt(55, 14);
            stream.WritePositiveInt(56, 14);*/

            /*stream.WritePositiveVInt(7217, 4);
            stream.WritePositiveVInt(2305, 4);
            stream.WritePositiveVInt(0, 3);
            stream.WritePositiveVInt(0, 4);
            stream.WritePositiveInt(10, 4);
            stream.WritePositiveInt(0, 1);
            stream.WritePositiveInt(0, 1);
            stream.WritePositiveInt(2, 3);
            stream.WritePositiveInt(0, 1);
            stream.WriteInt(0, 6);
            stream.WritePositiveInt(0, 1);
            stream.WritePositiveInt(0, 1);
            stream.WritePositiveInt(0, 1);
            stream.WritePositiveInt(0, 1);
            stream.WritePositiveInt(1, 1);
            stream.WritePositiveInt(1, 1);
            stream.WritePositiveInt(0, 1);
            stream.WritePositiveInt(0, 1);
            stream.WritePositiveInt(0, 2);
            stream.WritePositiveVInt(3200, 4);
            stream.WritePositiveVInt(3200, 4);
            stream.WritePositiveInt(1, 1);
            stream.WritePositiveInt(1, 1);
            stream.WritePositiveInt(0, 1);
            stream.WritePositiveInt(0, 1);
            stream.WritePositiveInt(0, 1);
            stream.WritePositiveInt(0, 4);
            stream.WritePositiveInt(0, 2);
            stream.WritePositiveInt(0, 1);
            stream.WritePositiveInt(0, 9);
            stream.WritePositiveInt(0, 1);
            stream.WritePositiveInt(0, 1);
            stream.WritePositiveInt(0, 5);
            stream.WritePositiveInt(0, 1);
            stream.WritePositiveVInt(1, 3);
            stream.WritePositiveInt(0, 1);
            stream.WritePositiveInt(0, 1);
            stream.WritePositiveVInt(6, 3);
            stream.WritePositiveInt(1108, 12);
            stream.WritePositiveInt(1, 1);
            stream.WritePositiveInt(0, 1);
            stream.WritePositiveInt(1, 1);
            stream.WritePositiveVInt(9098, 4);
            stream.WritePositiveVInt(714, 4);
            stream.WritePositiveVInt(17, 3);
            stream.WritePositiveVInt(0, 4);
            stream.WritePositiveInt(10, 4);
            stream.WritePositiveInt(130, 9);
            stream.WritePositiveInt(130, 9);
            stream.WritePositiveInt(1, 3);
            stream.WritePositiveInt(0, 1);
            stream.WriteInt(63, 6);
            stream.WritePositiveInt(0, 1);
            stream.WritePositiveInt(0, 1);
            stream.WritePositiveInt(0, 1);
            stream.WritePositiveInt(0, 1);
            stream.WritePositiveInt(1, 1);
            stream.WritePositiveInt(1, 1);
            stream.WritePositiveInt(0, 1);
            stream.WritePositiveInt(0, 1);
            stream.WritePositiveInt(0, 2);
            stream.WritePositiveVInt(3200, 4);
            stream.WritePositiveVInt(3200, 4);
            stream.WritePositiveInt(1, 1);
            stream.WritePositiveInt(1, 1);
            stream.WritePositiveInt(0, 1);
            stream.WritePositiveInt(0, 1);
            stream.WritePositiveInt(0, 2);
            stream.WritePositiveInt(0, 1);
            stream.WritePositiveInt(0, 9);
            stream.WritePositiveInt(0, 5);
            stream.WritePositiveInt(1, 1);
            stream.WritePositiveInt(0, 1);
            stream.WritePositiveInt(1, 1);
            stream.WritePositiveInt(2027, 12);
            stream.WritePositiveInt(1, 1);
            stream.WritePositiveInt(0, 1);
            stream.WritePositiveInt(1, 1);
            stream.WritePositiveVInt(2891, 4);
            stream.WritePositiveVInt(3886, 4);
            stream.WritePositiveVInt(34, 3);
            stream.WritePositiveVInt(0, 4);
            stream.WritePositiveInt(3, 4);
            stream.WritePositiveInt(332, 9);
            stream.WritePositiveInt(332, 9);
            stream.WritePositiveInt(1, 3);
            stream.WritePositiveInt(0, 1);
            stream.WriteInt(63, 6);
            stream.WritePositiveInt(0, 1);
            stream.WritePositiveInt(0, 1);
            stream.WritePositiveInt(0, 1);
            stream.WritePositiveInt(0, 1);
            stream.WritePositiveInt(1, 1);
            stream.WritePositiveInt(1, 1);
            stream.WritePositiveInt(0, 1);
            stream.WritePositiveInt(0, 1);
            stream.WritePositiveInt(0, 2);
            stream.WritePositiveVInt(4000, 4);
            stream.WritePositiveVInt(4000, 4);
            stream.WritePositiveInt(1, 1);
            stream.WritePositiveInt(1, 1);
            stream.WritePositiveInt(0, 1);
            stream.WritePositiveInt(0, 1);
            stream.WritePositiveInt(0, 2);
            stream.WritePositiveInt(0, 1);
            stream.WritePositiveInt(0, 9);
            stream.WritePositiveInt(0, 5);
            stream.WritePositiveInt(1, 1);
            stream.WritePositiveInt(0, 1);
            stream.WritePositiveInt(1, 1);
            stream.WritePositiveInt(3000, 12);
            stream.WritePositiveInt(1, 1);
            stream.WritePositiveInt(0, 1);
            stream.WritePositiveInt(1, 1);
            stream.WritePositiveVInt(8550, 4);
            stream.WritePositiveVInt(1050, 4);
            stream.WritePositiveVInt(170, 3);
            stream.WritePositiveVInt(0, 4);
            stream.WritePositiveInt(10, 4);
            stream.WritePositiveInt(0, 3);
            stream.WritePositiveInt(1, 1);
            stream.WritePositiveInt(1, 1);
            stream.WritePositiveInt(0, 1);
            stream.WritePositiveInt(0, 1);
            stream.WritePositiveInt(0, 2);
            stream.WritePositiveVInt(3660, 4);
            stream.WritePositiveVInt(4500, 4);
            stream.WritePositiveInt(0, 2);
            stream.WritePositiveInt(0, 1);
            stream.WritePositiveInt(140, 9);
            stream.WritePositiveInt(0, 5);
            stream.WritePositiveVInt(9150, 4);
            stream.WritePositiveVInt(4350, 4);
            stream.WritePositiveVInt(170, 3);
            stream.WritePositiveVInt(0, 4);
            stream.WritePositiveInt(10, 4);
            stream.WritePositiveInt(0, 3);
            stream.WritePositiveInt(1, 1);
            stream.WritePositiveInt(1, 1);
            stream.WritePositiveInt(0, 1);
            stream.WritePositiveInt(0, 1);
            stream.WritePositiveInt(0, 2);
            stream.WritePositiveVInt(4500, 4);
            stream.WritePositiveVInt(4500, 4);
            stream.WritePositiveInt(0, 2);
            stream.WritePositiveInt(0, 1);
            stream.WritePositiveInt(0, 9);
            stream.WritePositiveInt(0, 5);
            stream.WritePositiveVInt(9817, 4);
            stream.WritePositiveVInt(2217, 4);
            stream.WritePositiveVInt(0, 3);
            stream.WritePositiveVInt(400, 4);
            stream.WritePositiveInt(1, 3);
            stream.WritePositiveInt(0, 1);
            stream.WritePositiveInt(1000, 10);
            stream.WritePositiveInt(0, 1);
            stream.WritePositiveVInt(8766, 4);
            stream.WritePositiveVInt(848, 4);
            stream.WritePositiveVInt(17, 3);
            stream.WritePositiveVInt(350, 4);
            stream.WritePositiveInt(3, 3);
            stream.WritePositiveInt(0, 1);
            stream.WritePositiveInt(220, 10);
            stream.WritePositiveInt(146, 9);
            stream.WritePositiveInt(0, 1);
            stream.WritePositiveVInt(8488, 4);
            stream.WritePositiveVInt(1113, 4);
            stream.WritePositiveVInt(17, 3);
            stream.WritePositiveVInt(350, 4);
            stream.WritePositiveInt(0, 3);
            stream.WritePositiveInt(0, 1);
            stream.WritePositiveInt(190, 10);
            stream.WritePositiveInt(136, 9);
            stream.WritePositiveInt(0, 1);
            stream.WritePositiveVInt(7691, 4);
            stream.WritePositiveVInt(2282, 4);
            stream.WritePositiveVInt(0, 3);
            stream.WritePositiveVInt(400, 4);
            stream.WritePositiveInt(0, 3);
            stream.WritePositiveInt(0, 1);
            stream.WritePositiveInt(249, 10);
            stream.WritePositiveInt(0, 1);*/
        }
    }
}
