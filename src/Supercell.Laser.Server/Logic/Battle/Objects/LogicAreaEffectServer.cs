namespace Supercell.Laser.Server.Logic.Battle.Objects
{
    using Supercell.Laser.Server.DataStream;
    using Supercell.Laser.Server.Files;
    using Supercell.Laser.Server.Files.Tables;
    using Supercell.Laser.Titan.Files.CsvData;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    internal class LogicAreaEffectServer : LogicGameObjectServer
    {
        internal LogicAreaEffectServer(LogicData data) : base(data)
        {
            EffectData = LogicDataTables.Tables.Get(LogicDataTables.Files.AreaEffects).GetDataWithInstanceId<LogicAreaEffectData>(data.InstanceId);
            AlreadyDamaged = new List<LogicCharacterServer>();
        }

        internal int StartTick;
        internal LogicAreaEffectData EffectData;

        internal LogicCharacterServer OwnerCharacter;

        internal int Damage;
        internal List<LogicCharacterServer> AlreadyDamaged;

        internal override void Tick()
        {
            base.Tick();

            if (StartTick == 0) StartTick = BattleModeServer.TicksGone;

            if (BattleModeServer.TicksGone - StartTick > EffectData.TimeMs * 20 / 1000)
            {
                this.Destroy();
            }

            if (EffectData.Type == "Dot")
            {
                foreach (LogicGameObjectServer gameObj in LogicGameObjectManager.GetGameObjects())
                {
                    if (gameObj.GetType() == typeof(LogicCharacterServer))
                    {
                        var character = (LogicCharacterServer)gameObj;
                        if (character != OwnerCharacter)
                        {
                            if (!AlreadyDamaged.Contains(character))
                            {
                                if (character.Vector2.GetDistance(Vector2) <= EffectData.Radius)
                                {
                                    AlreadyDamaged.Add(character);
                                    character.CauseDamage(OwnerCharacter, Damage);
                                }
                            }
                        }
                    }
                }
            }
        }

        internal void Destroy()
        {
            if (EffectData.Type == "BulletExplosion")
            {
                int count = EffectData.CustomValue;

                LogicItemData item = LogicDataTables.Tables.Get(LogicDataTables.Files.Items).GetData<LogicItemData>(EffectData.BulletExplosionItem);

                if (item != null)
                {
                    for (int i = 0; i < count; i++)
                    {
                        LogicItemServer bullet = new LogicItemServer(new LogicData(18, item.GetInstanceId()));
                        bullet.Vector2 = Vector2.Clone();
                        LogicGameObjectManager.AddGameObject(bullet);
                    }
                }
            }

            if (EffectData.Type == "Damage")
            {
                foreach (LogicGameObjectServer gameObj in LogicGameObjectManager.GetGameObjects())
                {
                    if (gameObj.GetType() == typeof(LogicCharacterServer))
                    {
                        var character = (LogicCharacterServer)gameObj;
                        if (character != OwnerCharacter)
                        {
                            if (!AlreadyDamaged.Contains(character))
                            {
                                if (character.Vector2.GetDistance(Vector2) <= EffectData.Radius)
                                {
                                    AlreadyDamaged.Add(character);
                                    character.CauseDamage(OwnerCharacter, Damage);
                                }
                            }
                        }
                    }
                }
            }

            LogicGameObjectManager.RemoveGameObject(this);
        }

        internal override void Encode(BitStream stream, int idx = -1)
        {
            base.Encode(stream, idx);
            stream.WritePositiveInt(3, 4);
            stream.WritePositiveInt(0, 7);
        }
    }
}
