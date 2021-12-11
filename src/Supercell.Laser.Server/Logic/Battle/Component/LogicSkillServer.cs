namespace Supercell.Laser.Server.Logic.Battle.Component
{
    using Supercell.Laser.Server.DataStream;
    using Supercell.Laser.Server.Logic.Battle.Objects;
    using Supercell.Laser.Titan.Logic.Math;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    internal class LogicSkillServer
    {
        internal LogicSkillServer()
        {
            Charge = 3000;
        }

        internal void Start()
        {
            MaxCharge = 1000 * OwnerCharacter.WeaponSkill.MaxCharge;
            Charge = MaxCharge;
        }

        internal int MaxCharge;
        internal int Charge;
        internal LogicCharacterServer OwnerCharacter;

        internal int AttackTick;
        internal int Activated;
        internal int State;

        internal bool ConsumeCharge()
        {
            if (State == 6) return false;

            if (Charge >= 1000)
            {
                Charge -= 1000;
                return true;
            }
            return false;
        }

        internal bool Activate()
        {
            bool result = ConsumeCharge();

            if (!result) return result;

            AttackTick = OwnerCharacter.BattleModeServer.TicksGone;
            Activated = 1;
            State = 6;

            return result;
        }

        internal void Tick()
        {
            if (State == 0)
            {
                if (Charge < MaxCharge)
                {
                    if (OwnerCharacter.WeaponSkill.RechargeTime != 0)
                    {
                        Charge = LogicMath.Min(MaxCharge, Charge + 1000 / (OwnerCharacter.WeaponSkill.RechargeTime * 20 / 1000));
                    }
                }
            }

            if (State == 6)
            {
                if (OwnerCharacter.BattleModeServer.TicksGone - AttackTick >= OwnerCharacter.WeaponSkill.ActiveTime * 20 / 1000)
                {
                    Activated = 0;
                    State = 0;
                }
            }
        }

        internal void Encode(BitStream stream)
        {
            stream.WritePositiveVIntMax255OftenZero(Activated);
            stream.WritePositiveInt(0, 1);
            stream.WritePositiveVIntMax255OftenZero(State);
            stream.WritePositiveInt(Charge, 12);
            stream.WritePositiveInt(1, 1);
            stream.WritePositiveInt(0, 1);
            stream.WritePositiveInt(1, 1);
        }
    }
}
