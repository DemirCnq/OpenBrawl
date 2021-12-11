namespace Supercell.Laser.Server.Logic.Battle.Objects
{
    using Supercell.Laser.Server.DataStream;
    using Supercell.Laser.Server.Files;
    using Supercell.Laser.Server.Files.Tables;
    using Supercell.Laser.Titan.Files.CsvData;
    using Supercell.Laser.Titan.Logic.Math;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    internal class LogicProjectileServer : LogicGameObjectServer
    {
        internal LogicProjectileServer(LogicData data) : base(data)
        {
            ProjectileData = LogicDataTables.Tables.Get(LogicDataTables.Files.Projectiles).GetDataWithInstanceId<LogicProjectileData>(data.InstanceId);
            AlreadyDamaged = new List<LogicCharacterServer>();
        }

        internal LogicVector2 IndirectPosition;
        internal LogicProjectileData ProjectileData;
        internal int StartTick;
        internal int CastingTime;
        internal int Damage;

        internal List<LogicCharacterServer> AlreadyDamaged;

        internal bool Calculated;

        internal int VelocityX, VelocityY;

        internal LogicCharacterServer OwnerCharacter;

        private int CalculateIndirectCastingTime()
        {
            int x = Vector2.X;
            int y = Vector2.Y;
            int tick = 0;
            Z = 0;
            bool f = true;
            while (f && tick < 100)
            {
                tick++;
                for (int i = 0; i < 4; i++)
                {
                    int VelocityX = 0;
                    int VelocityY = 0;

                    float speed = 0.22f / 4;

                    if (VelocityX == 0 && VelocityY == 0)
                    {
                        VelocityX += (int)(speed * (float)LogicMath.Cos((int)(Angle - angleCorrection)));
                        VelocityY += (int)(speed * (float)LogicMath.Sin((int)(Angle - angleCorrection)));
                    }

                    x += VelocityX;
                    y += VelocityY;

                    if (IndirectPosition != null)
                    {
                        if (new LogicVector2(x, y).GetDistance(IndirectPosition) <= 350)
                        {
                            f = false;
                            break;
                        }
                    }
                    else
                    {
                        f = false;
                        break;
                    }
                }
            }
            Console.WriteLine(tick);
            return tick;
        }

        internal override void Tick()
        {
            base.Tick();

            if (StartTick == 0) StartTick = BattleModeServer.TicksGone;
            if (!Calculated && ProjectileData.Indirect)
            {
                Calculated = true;
                CastingTime = CalculateIndirectCastingTime();
            }

            if (BattleModeServer.TicksGone - StartTick < CastingTime)
            {
                //if (ProjectileData.Indirect) speed = 0.12f;

                if (ProjectileData.TriggerWithDelayMs != 0)
                {
                    if (BattleModeServer.TicksGone - StartTick >= ProjectileData.TriggerWithDelayMs * 20 / 1000)
                    {
                        this.Destroy();
                    }
                }

                int VelocityX = 0;
                int VelocityY = 0;

                float speed = 0.22f / 4;

                if (VelocityX == 0 && VelocityY == 0)
                {
                    VelocityX += (int)(speed * (float)LogicMath.Cos((int)(Angle - angleCorrection)));
                    VelocityY += (int)(speed * (float)LogicMath.Sin((int)(Angle - angleCorrection)));
                }

                if (ProjectileData.Indirect)
                {
                    if (BattleModeServer.TicksGone - StartTick < CastingTime / 2)
                    {
                        Z += 120;
                    }
                    else
                    {
                        Z -= 120;
                    }
                }

                for (int i = 0; i < 4; i++)
                {
                    Vector2.X += VelocityX;
                    Vector2.Y += VelocityY;

                    GetTile(out int xTile, out int yTile);
                    if (BattleModeServer.TileMap.GetTile(xTile, yTile).DestroysProjectile() && !ProjectileData.Indirect && !ProjectileData.PassesEnvironment)
                    {
                        if (ProjectileData.IsBoomerang) MaxRangeReached();
                        this.Destroy();
                        break;
                    }

                    if (Vector2.X < 0 || Vector2.Y < 0 || Vector2.X > 150 + 300 * 61 || Vector2.Y > 50 + 300 * 61)
                    {
                        if (ProjectileData.IsBoomerang) MaxRangeReached();
                        this.Destroy();
                        break;
                    }

                    if (!ProjectileData.Indirect) 
                    {
                        foreach (LogicGameObjectServer gameObj in LogicGameObjectManager.GetGameObjects())
                        {
                            if (gameObj != null)
                            {
                                if (gameObj.GetType() == typeof(LogicCharacterServer))
                                {
                                    if (OwnerCharacter != gameObj)
                                    {
                                        if (Vector2.GetDistance(gameObj.Vector2) <= 222)
                                        {
                                            var character = (LogicCharacterServer)gameObj;

                                            if (!AlreadyDamaged.Contains(character))
                                            {
                                                character.CauseDamage(OwnerCharacter, Damage);
                                                AlreadyDamaged.Add(character);

                                                if (!ProjectileData.PiercesCharacters)
                                                {
                                                    LogicGameObjectManager.RemoveGameObject(this);
                                                    return;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }

                    if (Data.InstanceId == 195)
                    {
                        if (OwnerCharacter.Vector2.GetDistance(Vector2) <= 222)
                        {
                            OwnerCharacter.LogicSkillServer.Charge = 1000;
                            this.Destroy();
                            return;
                        }
                    }

                    if (ProjectileData.Indirect)
                    {
                        if (IndirectPosition != null)
                        {
                            if (Vector2.GetDistance(IndirectPosition) <= 250)
                            {
                                MaxRangeReached();
                                this.Destroy();

                                break;
                            }
                        }
                        else
                        {
                            this.Destroy();
                            break;
                        }
                    }
                }

            }
            else
            {
                MaxRangeReached();
                this.Destroy();
            }
        }

        internal void MaxRangeReached()
        {
            if (ProjectileData.IsBoomerang)
            {
                if (Data.InstanceId == 195)
                {
                    OwnerCharacter.LogicSkillServer.Charge = 1000;
                    return;
                }

                LogicProjectileData data = LogicDataTables.Tables.Get(LogicDataTables.Files.Projectiles).GetData<LogicProjectileData>(ProjectileData.ChainBullet);

                
                LogicProjectileServer projectile = new LogicProjectileServer(new LogicData(6, data.GetInstanceId()));
                projectile.Vector2 = Vector2.Clone();
                projectile.Angle = Vector2.GetAngleBetweenPositions(OwnerCharacter.X, OwnerCharacter.Y);
                projectile.ObjectIdx = ObjectIdx;
                projectile.Damage = Damage;
                projectile.CastingTime = CastingTime;
                projectile.Z = 400;
                projectile.OwnerCharacter = OwnerCharacter;

                BattleModeServer.GameObjectManagerServer.AddGameObject(projectile);
            }

            if (ProjectileData.ChainBullet != null)
            {
                int[] directions = new int[ProjectileData.ChainBullets];
                if (ProjectileData.ChainSpread != 0)
                {
                    int direction = (-ProjectileData.ChainSpread / 2) / 2;
                    for (int i = 0; i < ProjectileData.ChainBullets; i++)
                    {
                        directions[i] = direction;
                        direction += (ProjectileData.ChainSpread / 2) / ProjectileData.ChainBullets;
                    }
                }
                else
                {
                    int direction = -2 * (ProjectileData.ChainBullets / 2);
                    for (int i = 0; i < ProjectileData.ChainBullets; i++)
                    {
                        directions[i] = direction;
                        direction += 4;
                    }
                }

                for (int i = 0; i < ProjectileData.ChainBullets; i++)
                {
                    var projectile = new LogicProjectileServer(new LogicData(6, LogicDataTables.Tables.Get(LogicDataTables.Files.Projectiles).GetData<LogicProjectileData>(ProjectileData.ChainBullet).GetInstanceId()));

                    projectile.Vector2 = Vector2.Clone();
                    projectile.Angle = ProjectileData.ChainBullets > 1 ? Angle + directions[i] : Angle;
                    projectile.ObjectIdx = ObjectIdx;
                    projectile.Damage = Damage;
                    projectile.CastingTime = ProjectileData.ChainTravelDistance / 2;
                    projectile.Z = 400;
                    projectile.OwnerCharacter = OwnerCharacter;

                    BattleModeServer.GameObjectManagerServer.AddGameObject(projectile);
                }
            }
        }

        internal void Destroy()
        {
            if (ProjectileData.SpawnAreaEffectObject != null)
            {
                LogicAreaEffectServer effect = new LogicAreaEffectServer(new LogicData(17, LogicDataTables.Tables.Get(LogicDataTables.Files.AreaEffects).GetData<LogicAreaEffectData>(ProjectileData.SpawnAreaEffectObject).GetInstanceId()));
                effect.Vector2 = Vector2.Clone();
                effect.Damage = Damage;
                effect.OwnerCharacter = OwnerCharacter;
                LogicGameObjectManager.AddGameObject(effect);
            }

            LogicGameObjectManager.RemoveGameObject(this);
        }

        internal override void Encode(BitStream stream, int idx = -1)
        {
            base.Encode(stream, idx);

            stream.WritePositiveInt(0, 3);
            stream.WriteBoolean(false);
            if (Data.InstanceId == 88)
            {
                stream.WritePositiveVInt(230, 4);
            }
            stream.WritePositiveInt(Data.InstanceId == 88 ? 329 : 0, 10);  // ?
            if (ProjectileData.IsBouncing)
            {
                stream.WritePositiveInt(0, 1);
                stream.WritePositiveInt(1000, 10);
            }
            if (ProjectileData.Rendering != "DoNotRotateClip") stream.WritePositiveInt(Angle, 9);
            stream.WritePositiveInt(0, 1);
        }
    }
}
