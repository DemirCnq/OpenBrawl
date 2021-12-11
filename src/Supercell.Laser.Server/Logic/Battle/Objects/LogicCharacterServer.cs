namespace Supercell.Laser.Server.Logic.Battle.Objects
{
    using Supercell.Laser.Server.DataStream;
    using Supercell.Laser.Server.Files;
    using Supercell.Laser.Server.Files.Tables;
    using Supercell.Laser.Server.Logic.Battle.Component;
    using Supercell.Laser.Titan.Files.CsvData;
    using Supercell.Laser.Titan.Logic.Math;

    internal class LogicCharacterServer : LogicGameObjectServer
    {
        internal LogicCharacterServer(LogicData data) : base(data)
        {
            Visiblity = 10;
            LogicSkillServer = new LogicSkillServer();
            Destination = new LogicVector2();

            CharacterData = LogicDataTables.Tables.Get(LogicDataTables.Files.Characters).GetDataWithInstanceId<LogicCharacterData>(data.InstanceId);

            WeaponSkill = LogicDataTables.Tables.Get(LogicDataTables.Files.Skills).GetData<LogicSkillData>(CharacterData.WeaponSkill);
            UltimateSkill = LogicDataTables.Tables.Get(LogicDataTables.Files.Skills).GetData<LogicSkillData>(CharacterData.UltimateSkill);

            MaxHitpoints = CharacterData.Hitpoints;
            Hitpoints = MaxHitpoints;

            if (IsHero())
            {
                LogicSkillServer.OwnerCharacter = this;
                LogicSkillServer.Start();
            }
        }

        internal int Hitpoints;
        internal int MaxHitpoints;

        internal float HitpointsMultiplier = 1f;

        internal int ShootingAngle;
        internal int LastAttackTick;
        internal LogicVector2 IndirectShootPoint;

        internal LogicSkillServer LogicSkillServer;
        internal LogicVector2 Destination;

        internal LogicCharacterData CharacterData;
        internal LogicSkillData WeaponSkill;
        internal LogicSkillData UltimateSkill;

        internal LogicSkillData ActiveSkill;

        internal bool ExecutingCharge;
        internal int StartChargeTick;

        internal int ItemsCollected;

        internal float DamageMultiplier = 1f;

        internal int LastPoisonCloudDamageTick;

        internal int LastTimeDamageCaused;
        internal int LastHealTick;

        internal override void Tick()
        {
            base.Tick();

            this.TickPoison();
            if (IsHero())
            {
                TickAntiTeaming();
                LogicSkillServer.Tick();
                this.TickHeals();
                this.TickRapidFire();
                this.TickCharge();
                this.HandleMoveAndAttack();
            }
        }

        internal bool IsPlayerControlRemoved = true;

        internal void TickAntiTeaming()
        {
            ;
        }

        internal void TickHeals()
        {
            if (Hitpoints < MaxHitpoints)
            {
                if (BattleModeServer.TicksGone - LastTimeDamageCaused >= 50 && BattleModeServer.TicksGone - LastHealTick >= 20)
                {
                    LastHealTick = BattleModeServer.TicksGone;
                    Hitpoints = LogicMath.Min(MaxHitpoints, Hitpoints + (MaxHitpoints / 7));
                }
            }
        }

        internal void TickPoison()
        {
            if (BattleModeServer.TicksGone - LastPoisonCloudDamageTick >= 20)
            {
                GetTile(out int xTile, out int yTile);

                if (BattleModeServer.IsTileOnPoisonArea(xTile, yTile))
                {
                    LastPoisonCloudDamageTick = BattleModeServer.TicksGone;
                    CauseDamage(this, 1000);
                }
            }
        }

        internal void CauseDamage(LogicCharacterServer damageDealer, int damage)
        {
            if (Hitpoints > 0)
            {
                LastTimeDamageCaused = BattleModeServer.TicksGone;
                Hitpoints = LogicMath.Max(0, Hitpoints - damage);

                bool dead = Hitpoints == 0;
                if (dead)
                {
                    LogicGameObjectManager.RemoveGameObject(this);
                    Died();
                    if (IsHero())
                    {
                        BattleModeServer.PlayerDied(this, damageDealer);
                    }
                }
            }
        }

        internal void ItemPickedUp(LogicItemServer item)
        {
            if (item.Data.InstanceId == 9)
            {
                ItemsCollected++;
                if (BattleModeServer.GameMode == Enums.GameModeVariation.Showdown)
                {
                    HitpointsMultiplier += 0.1f;
                    int newHp = (int)(HitpointsMultiplier * CharacterData.Hitpoints);
                    int deltaHp = newHp - MaxHitpoints;
                    MaxHitpoints = newHp;
                    Hitpoints += deltaHp;
                    DamageMultiplier += 0.25f;
                }
            }
        }

        internal void Died()
        {
            if (Data.InstanceId == 51)
            {
                LogicItemServer item = new LogicItemServer(new LogicData(18, 9));
                item.Vector2 = Vector2.Clone();
                LogicGameObjectManager.AddGameObject(item);
            }
        }

        internal bool IsHero()
        {
            return Data.InstanceId < 40;
        }

        internal void Attack(int x, int y, bool autoaim)
        {
            if (Data.InstanceId == 26) return;
            if (!LogicSkillServer.Activate()) return;
            
            int angle = !autoaim ? Vector2.GetAngleBetween(x, y) : Vector2.GetAngleBetween(x - Vector2.X, y - Vector2.Y);

            if (WeaponSkill.BehaviorType == "Attack") 
            {
                LogicProjectileData projectile = LogicDataTables.Tables.Get(LogicDataTables.Files.Projectiles).GetData<LogicProjectileData>(WeaponSkill.Projectile);

                if (projectile != null)
                {
                    if (projectile.Indirect)
                    {
                        angle = Vector2.GetAngleBetween(x - Vector2.X, y - Vector2.Y);
                    }
                    StartRapidFire(angle, x, y, WeaponSkill);
                }

                if (Data.InstanceId == 34)
                {
                    LogicAreaEffectData effectData = LogicDataTables.Tables.Get(LogicDataTables.Files.AreaEffects).GetData<LogicAreaEffectData>(WeaponSkill.AreaEffectObject);

                    LogicAreaEffectServer effect = new LogicAreaEffectServer(new LogicData(17, effectData.GetInstanceId()));
                    effect.Vector2 = Vector2.Clone();
                    LogicGameObjectManager.AddGameObject(effect);
                }
            }
            else if (WeaponSkill.BehaviorType == "Charge")
            {
                StartCharge(angle, x, y, WeaponSkill);
            }
        }

        private void StartCharge(int angle, int x, int y, LogicSkillData skill)
        {
            ExecutingCharge = true;
            Angle = angle;

            StartChargeTick = BattleModeServer.TicksGone;
            ActiveSkill = skill;
        }

        private void TickCharge()
        {
            if (!ExecutingCharge) return;

            double speed = (float)ActiveSkill.ChargeSpeed / 80000f;

            if (ActiveSkill.ChargeType == 4)
            {
                int VelocityX = 0;
                int VelocityY = 0;

                if (VelocityX == 0 && VelocityY == 0)
                {
                    VelocityX += (int)(speed * (float)LogicMath.Cos((int)(Angle - angleCorrection)));
                    VelocityY += (int)(speed * (float)LogicMath.Sin((int)(Angle - angleCorrection)));
                }

                for (int i = 0; i < 8; i++)
                {
                    Vector2.X += VelocityX;
                    Vector2.Y += VelocityY;

                    GetTile(out int xTile, out int yTile);
                    if (BattleModeServer.TileMap.GetTile(xTile, yTile).DestroysProjectile())
                    {
                        ExecutingCharge = false;
                        break;
                    }

                    if (BattleModeServer.TileMap.GetTile(xTile + 1, yTile).DestroysProjectile())
                    {
                        if (BattleModeServer.TileMap.GetTile(xTile + 1, yTile).Position.GetDistance(Vector2) < 222)
                        {
                            ExecutingCharge = false;
                            break;
                        }
                    }

                    if (BattleModeServer.TileMap.GetTile(xTile, yTile + 1).DestroysProjectile())
                    {
                        if (BattleModeServer.TileMap.GetTile(xTile, yTile + 1).Position.GetDistance(Vector2) < 222)
                        {
                            ExecutingCharge = false;
                            break;
                        }
                    }

                    if (BattleModeServer.TileMap.GetTile(xTile - 1, yTile).DestroysProjectile())
                    {
                        if (BattleModeServer.TileMap.GetTile(xTile - 1, yTile).Position.GetDistance(Vector2) < 222)
                        {
                            ExecutingCharge = false;
                            break;
                        }
                    }

                    if (BattleModeServer.TileMap.GetTile(xTile, yTile - 1).DestroysProjectile())
                    {
                        if (BattleModeServer.TileMap.GetTile(xTile, yTile - 1).Position.GetDistance(Vector2) < 222)
                        {
                            ExecutingCharge = false;
                            break;
                        }
                    }

                    if (BattleModeServer.TileMap.GetTile(xTile + 1, yTile + 1).DestroysProjectile())
                    {
                        if (BattleModeServer.TileMap.GetTile(xTile + 1, yTile + 1).Position.GetDistance(Vector2) < 222)
                        {
                            ExecutingCharge = false;
                            break;
                        }
                    }

                    if (BattleModeServer.TileMap.GetTile(xTile - 1, yTile - 1).DestroysProjectile())
                    {
                        if (BattleModeServer.TileMap.GetTile(xTile - 1, yTile - 1).Position.GetDistance(Vector2) < 222)
                        {
                            ExecutingCharge = false;
                            break;
                        }
                    }
                }

                foreach (LogicGameObjectServer gameObj in LogicGameObjectManager.GetGameObjects())
                {
                    if (gameObj != null)
                    {
                        if (gameObj.GetType() == typeof(LogicCharacterServer))
                        {
                            if (this != gameObj)
                            {
                                if (Vector2.GetDistance(gameObj.Vector2) <= 80)
                                {
                                    var character = (LogicCharacterServer)gameObj;

                                    character.CauseDamage(this, ActiveSkill.Damage);
                                }
                            }
                        }
                    }
                }

                Destination.Set(X, Y);

                if (BattleModeServer.TicksGone - StartChargeTick > ActiveSkill.CastingRange / 2)
                {
                    ExecutingCharge = false;
                }
            }
        }

        private void StartRapidFire(int angle, int x, int y, LogicSkillData skill)
        {
            ShootingAngle = angle;
            IndirectShootPoint = new LogicVector2(x, y);
            ActiveSkill = skill;
            LastAttackTick = 0;

            if (!skill.ExecuteFirstAttackImmediately) LastAttackTick = BattleModeServer.TicksGone;
        }

        private void TickRapidFire()
        {
            if (ActiveSkill == null) return;
            if (LogicSkillServer.Activated == 1)
            {
                if (BattleModeServer.TicksGone > LogicSkillServer.AttackTick + 1)
                {
                    if (BattleModeServer.TicksGone - LastAttackTick > ActiveSkill.MsBetweenAttacks * 20 / 1000)
                    {
                        LastAttackTick = BattleModeServer.TicksGone;

                        LogicProjectileData projectileData = LogicDataTables.Tables.Get(LogicDataTables.Files.Projectiles).GetData<LogicProjectileData>(ActiveSkill.Projectile);

                        if (projectileData != null)
                        {
                            int guns = ActiveSkill.TwoGuns ? 2 : 1;

                            int[] directions = new int[ActiveSkill.NumBulletsInOneAttack];
                            if (ActiveSkill.Spread != 0)
                            {
                                int direction = (-ActiveSkill.Spread / 2) / 2;
                                for (int i = 0; i < ActiveSkill.NumBulletsInOneAttack; i++)
                                {
                                    directions[i] = direction;
                                    direction += (ActiveSkill.Spread / 2) / ActiveSkill.NumBulletsInOneAttack;
                                }
                            }
                            else
                            {
                                int direction = -2 * (ActiveSkill.NumBulletsInOneAttack / 2);
                                for (int i = 0; i < ActiveSkill.NumBulletsInOneAttack; i++)
                                {
                                    directions[i] = direction;
                                    direction += 4;
                                }
                            }

                            for (int j = 0; j < 1; j++)
                            {
                                if (true)
                                {
                                    for (int i = 0; i < ActiveSkill.NumBulletsInOneAttack; i++)
                                    {
                                        var projectile = new LogicProjectileServer(new LogicData(6, projectileData.GetInstanceId()));

                                        if (projectileData.Indirect)
                                        {
                                            projectile.IndirectPosition = IndirectShootPoint;
                                        }

                                        projectile.Vector2 = Vector2.Clone();
                                        projectile.Angle = ActiveSkill.NumBulletsInOneAttack > 1 ? ShootingAngle + directions[i] : ShootingAngle;
                                        projectile.ObjectIdx = ObjectIdx;
                                        projectile.Damage = (int)(ActiveSkill.Damage * DamageMultiplier);
                                        projectile.CastingTime = CalculateFlyTicksByRange(ActiveSkill.CastingRange, projectileData.Indirect);
                                        projectile.Z = 400;
                                        projectile.OwnerCharacter = this;

                                        BattleModeServer.GameObjectManagerServer.AddGameObject(projectile);
                                    }
                                }
                            }
                        }

                        ShootingAngle += ActiveSkill.Spread / 10;
                    }
                }
            }
        }

        private static LogicVector2 Dude(LogicVector2 vector, int angle)
        {
            int VelocityX = 0;
            int VelocityY = 0;

            float speed = 0.2f;

            if (VelocityX == 0 && VelocityY == 0)
            {
                VelocityX -= (int)(speed * (float)LogicMath.Cos((int)(angle - angleCorrection)));
                VelocityY -= (int)(speed * (float)LogicMath.Sin((int)(angle - angleCorrection)));
            }

            for (int i = 0; i < 5; i++)
            {
                vector.X += VelocityX;
                vector.Y += VelocityY;
            }

            return vector;
        }

        private int CalculateFlyTicksByRange(int range, bool indirect)
        {
            if (!indirect)
                return range / 2;
            else
                return range;
        }

        internal void HandleMoveAndAttack()
        {
            if (ExecutingCharge) return;

            if (Z > 0)
            {
                Z = LogicMath.Max(Z - 160, 0);
            }
            if (Vector2.GetDistance(Destination) != 0)
            {
                int deltaX = 0;
                int deltaY = 0;

                int movespeed = 30;

                if (Destination.X - Vector2.X != 0)
                {
                    bool block = false;

                    if (Destination.X - Vector2.X > 0) deltaX += LogicMath.Min(movespeed, Destination.X - Vector2.X);
                    else deltaX += LogicMath.Max(-movespeed, Destination.X - Vector2.X);

                    if (!block)
                    {
                        Vector2.X += deltaX;
                    }
                    Angle = Vector2.GetAngleBetween(Destination.X - Vector2.X, Destination.Y - Vector2.Y);
                }
                if (Destination.Y - Vector2.Y != 0)
                {
                    bool block = false;

                    if (Destination.Y - Vector2.Y > 0) deltaY += LogicMath.Min(movespeed, Destination.Y - Vector2.Y);
                    else deltaY += LogicMath.Max(-movespeed, Destination.Y - Vector2.Y);
                    if (!block)
                    {
                        Vector2.Y += deltaY;
                    }
                    Angle = Vector2.GetAngleBetween(Destination.X - Vector2.X, Destination.Y - Vector2.Y);
                }
            }

            foreach (LogicGameObjectServer gameObj in LogicGameObjectManager.GetGameObjects())
            {
                if (Vector2.GetDistance(gameObj.Vector2) <= 300)
                {
                    if (gameObj.GetType() == typeof(LogicItemServer))
                    {
                        var item = (LogicItemServer)gameObj;

                        if (!item.Magnet)
                        {
                            item.PickUp(this);
                        }
                    }
                }
            }
        }

        internal int Visiblity;

        internal void MoveTo(int x, int y)
        {
            Destination.Set(x, y);
        }

        internal override void SetPosition(int x, int y, int z)
        {
            base.SetPosition(x, y, z);
            Destination = new LogicVector2(x, y);
        }

        internal override void SetPositionByTile(int xTile, int yTile)
        {
            base.SetPositionByTile(xTile, yTile);
            Destination = new LogicVector2(X, Y);
        }

        internal override void Encode(BitStream stream, int idx = -1)
        {
            base.Encode(stream, Data.InstanceId == 51 ? 170 : IsPlayerControlRemoved ? 16 + ObjectIdx : idx);

            if (Data.InstanceId != 51)
            {
                int AttackState = 0;
                if (LogicSkillServer.State == 6) AttackState = 2;
                if (ExecutingCharge) AttackState = 1;

                bool Moving = Destination.X - X != 0 || Destination.Y - Y != 0;

                stream.WritePositiveInt(Visiblity, 4);
                if (!IsPlayerControlRemoved)
                {
                    stream.WritePositiveInt(0, 1);
                    stream.WritePositiveInt(0, 1);
                }
                else
                {
                    stream.WritePositiveInt(Angle, 9);
                    stream.WritePositiveInt(Angle, 9);
                }
                stream.WritePositiveInt(AttackState, 3); // unk 2 attacking related?
                stream.WritePositiveInt(0, 1); // Rage
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
                stream.WritePositiveVInt(Hitpoints, 4);
                stream.WritePositiveVInt(MaxHitpoints, 4);
                stream.WritePositiveVIntMax255OftenZero(ItemsCollected);
                stream.WritePositiveInt(1, 1);
                stream.WritePositiveInt(0, 1);
                stream.WriteBoolean(ExecutingCharge);
                if (ExecutingCharge)
                {
                    stream.WriteBoolean(ExecutingCharge);
                    stream.WriteBoolean(false); // shield
                    stream.WritePositiveInt(0, 1); // stun?
                    stream.WritePositiveInt(0, 1); // red
                    stream.WritePositiveInt(0, 1); // ??
                    stream.WritePositiveInt(0, 1); // aiming ulti
                    stream.WritePositiveInt(0, 1); // yellow shield
                    stream.WritePositiveInt(0, 1);
                }
                if (!IsPlayerControlRemoved)
                {
                    stream.WritePositiveInt(0, 1);
                }
                if (Data.InstanceId == 22) stream.WritePositiveInt(0, 1); // tick ebalo vipalo
                if (Data.InstanceId == 29) stream.WriteInt(0, 2);
                if (Data.InstanceId == 38) stream.WritePositiveInt(0, 2);
                if (!IsPlayerControlRemoved)
                {
                    stream.WritePositiveInt(0, 4);
                }
                if (Data.InstanceId == 26)
                {
                    stream.WritePositiveInt(0, 1);
                    stream.WritePositiveVInt(40, 3);
                }
                if (ExecutingCharge)
                {
                    stream.WritePositiveInt(2, 8);
                    stream.WritePositiveInt(4, 4);
                }
                stream.WritePositiveInt(0, 2);
                stream.WritePositiveInt(0, 1);
                stream.WritePositiveInt(1, 9); // 0
                if (!IsPlayerControlRemoved)
                {
                    stream.WritePositiveInt(0, 1);
                    stream.WritePositiveInt(0, 1);
                }
                stream.WritePositiveInt(0, 5);
                LogicSkillServer.Encode(stream);
            }
            else if (Data.InstanceId == 51)
            {
                stream.WritePositiveInt(10, 4);
                stream.WritePositiveInt(0, 3);
                stream.WritePositiveInt(1, 1);
                stream.WritePositiveInt(1, 1);
                stream.WritePositiveInt(0, 1);
                stream.WritePositiveInt(0, 1);
                stream.WritePositiveInt(0, 2);
                stream.WritePositiveVInt(Hitpoints, 4);
                stream.WritePositiveVInt(MaxHitpoints, 4);
                stream.WritePositiveInt(0, 2);
                stream.WritePositiveInt(0, 1);
                stream.WritePositiveInt(0, 9);
                stream.WritePositiveInt(0, 5);
            }
        }
    }
}
