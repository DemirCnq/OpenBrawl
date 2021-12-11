namespace Supercell.Laser.Server.Logic.Battle.Input
{
    using Supercell.Laser.Server.DataStream;
    using System;

    internal class ClientInput
    {
        internal InputType Identifier;

        internal int X;
        internal int Y;

        internal int PinId;

        internal long SessionId;

        internal int Counter;
        internal bool AutoAim;

        internal void Decode(BitStream stream)
        {
            Counter = stream.ReadPositiveInt(15);

            Identifier = (InputType)stream.ReadPositiveInt(4);

            X = stream.ReadInt(15);
            Y = stream.ReadInt(15);

            AutoAim = stream.ReadBoolean();

            if ((int)Identifier == 9)
            {
                PinId = stream.ReadPositiveInt(3);
            }

            if (AutoAim)
            {
                bool dude = stream.ReadBoolean();
                if (dude) stream.ReadPositiveInt(14);
            }

            //Console.WriteLine("Input: " + Identifier);
        }

        internal enum InputType : int
        {
            Attack = 0,
            Move = 2,
            UsePin = 9,
        }
    }
}
