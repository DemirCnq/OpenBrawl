namespace Supercell.Laser.Server.Logic.Battle.Objects
{
    using Supercell.Laser.Server.Core;
    using Supercell.Laser.Server.DataStream;
    using Supercell.Laser.Server.Files;
    using Supercell.Laser.Server.Files.Tables;
    using Supercell.Laser.Titan.Files.CsvData;
    using Supercell.Laser.Titan.Logic.Math;

    internal class LogicItemServer : LogicGameObjectServer
    {
        internal LogicItemServer(LogicData data) : base(data)
        {
            ItemData = LogicDataTables.Tables.Get(LogicDataTables.Files.Items).GetDataWithInstanceId<LogicItemData>(data.InstanceId);
            if (ItemData.CanBePickedUp)
            {
                Angle = Loader.Random.Rand(0, 360);
                Drop = true;
            }
        }

        internal LogicItemData ItemData;

        internal bool Drop;
        internal int TicksDropping;
        internal const int DropTicks = 10;

        internal bool Magnet;
        internal int TicksMagnetting;
        internal const int MagnetTicks = 4;

        internal int SpawnTick;

        internal const int OrbSpawnerCapacity = 29;
        internal int OrbsSpawned;

        internal int OrbSpawnerValue;
        internal LogicCharacterServer PickedBy;

        internal override void Tick()
        {
            if (SpawnTick == 0) SpawnTick = BattleModeServer.TicksGone;

            if (Data.InstanceId == 5)
            {
                if (OrbsSpawned < OrbSpawnerCapacity)
                {
                    OrbSpawnerValue++;
                    if (OrbSpawnerValue == 140)
                    {
                        OrbsSpawned++;
                        OrbSpawnerValue = 0;
                        LogicItemServer point = new LogicItemServer(new LogicData(18, 3));
                        point.SetPosition(Vector2.X, Vector2.Y, 0);
                        LogicGameObjectManager.AddGameObject(point);
                    }
                }
            }

            if (Data.InstanceId == 25)
            {
                if (BattleModeServer.TicksGone - SpawnTick >= ItemData.Value2 * 20 / 1000)
                {
                    Trigger();
                    LogicGameObjectManager.RemoveGameObject(this);
                }
            }

            if (TicksDropping < DropTicks && Drop)
            {
                TickDrop();
            }

            if (Magnet)
            {
                TickMagnet();
            }
        }

        internal void TickMagnet()
        {
            TicksMagnetting++;

            int VelocityX = 0;
            int VelocityY = 0;

            if (VelocityX == 0 && VelocityY == 0)
            {
                VelocityX += (int)(0.04f * (float)LogicMath.Cos((int)(Angle - angleCorrection)));
                VelocityY += (int)(0.04f * (float)LogicMath.Sin((int)(Angle - angleCorrection)));
            }

            Vector2.X += VelocityX;
            Vector2.Y += VelocityY;

            Z += 70;

            if (TicksMagnetting >= MagnetTicks)
            {
                PickedBy.ItemPickedUp(this);
                LogicGameObjectManager.RemoveGameObject(this);
                return;
            }
        }

        internal void PickUp(LogicCharacterServer pickedBy)
        {
            if (ItemData.CanBePickedUp)
            {
                PickedBy = pickedBy;
                Angle = Vector2.GetAngleBetweenPositions(pickedBy.X, pickedBy.Y);
                Magnet = true;
            }
        }

        private void Trigger()
        {
            if (ItemData.TriggerAreaEffect != null)
            {
                LogicAreaEffectData effect = LogicDataTables.Tables.Get(LogicDataTables.Files.AreaEffects).GetData<LogicAreaEffectData>(ItemData.TriggerAreaEffect);

                LogicAreaEffectServer effectServer = new LogicAreaEffectServer(new LogicData(17, effect.GetInstanceId()));
                effectServer.Vector2 = Vector2.Clone();
                LogicGameObjectManager.AddGameObject(effectServer);
            }
        }

        private void TickDrop()
        {
            TicksDropping++;

            int VelocityX = 0;
            int VelocityY = 0;

            if (VelocityX == 0 && VelocityY == 0)
            {
                VelocityX += (int)(30f * (float)System.Math.Cos((int)(Angle - angleCorrection)));
                VelocityY += (int)(30f * (float)System.Math.Sin((int)(Angle - angleCorrection)));
            }

            Vector2.X += VelocityX;
            Vector2.Y += VelocityY;

            if (TicksDropping <= DropTicks / 2)
            {
                Z += 80;
            }
            else
            {
                Z -= 80;
            }
        }

        internal override void Encode(BitStream stream, int idx = -1)
        {
            base.Encode(stream, 102);
            stream.WritePositiveInt(10, 4);

            if (Data.InstanceId == 5)
            {
                //stream.WritePositiveInt(OrbSpawnerValue > 70 ? 10 : 0, 14);
                //stream.WritePositiveInt(OrbSpawnerValue > 70 ? 30 : 0, 14);
                stream.WritePositiveInt(170, 14);
                stream.WritePositiveInt(30, 14);
            }

            if (Data.InstanceId == 25)
            {
                stream.WritePositiveInt(7, 6);
                stream.WritePositiveInt(0, 6);
            }
        }
    }
}
