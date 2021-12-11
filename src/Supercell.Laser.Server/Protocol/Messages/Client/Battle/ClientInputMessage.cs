namespace Supercell.Laser.Server.Protocol.Messages.Client.Battle
{
    using Supercell.Laser.Server.DataStream;
    using Supercell.Laser.Server.Logic.Battle.Input;
    using Supercell.Laser.Server.Network;
    using Supercell.Laser.Titan.DataStream;
    using Supercell.Laser.Titan.Logic;

    internal class ClientInputMessage : PiranhaMessage
    {
        internal ClientInputMessage(Connection connection, ByteStream stream) : base(connection, stream)
        {
        }

        internal ClientInputMessage(ByteStream stream) : base(null, stream)
        {
        }

        internal LogicArrayList<ClientInput> Inputs;

        internal override void Decode()
        {
            Inputs = new LogicArrayList<ClientInput>();

            BitStream stream = new BitStream(Stream.ReadAllBytes());

            stream.ReadPositiveInt(14);
            stream.ReadPositiveInt(10);

            stream.ReadPositiveInt(13);

            stream.ReadPositiveInt(10);
            stream.ReadPositiveInt(10);

            int count = stream.ReadPositiveInt(5);

            for (int i = 0; i < count; i++)
            {
                ClientInput input = new ClientInput();
                input.Decode(stream);
                Inputs.Add(input);
            }
        }
    }
}
