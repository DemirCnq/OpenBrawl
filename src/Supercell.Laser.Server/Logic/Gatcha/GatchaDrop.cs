namespace Supercell.Laser.Server.Logic.Gatcha
{
    using Supercell.Laser.Server.Logic.Avatar;
    using Supercell.Laser.Titan.DataStream;
    using Supercell.Laser.Titan.Files.CsvData;

    internal class GatchaDrop
    {
        internal int DropId;
        internal LogicData Data;
        internal int Count;

        internal void Encode(ByteStream stream)
        {
            stream.WriteVInt(Count);
            stream.WriteDataReference(Data);
            stream.WriteVInt(DropId);

            stream.WriteVInt(0);
            stream.WriteVInt(0);
            stream.WriteVInt(0);
            stream.WriteVInt(0);
        }

        internal void Apply(LogicClientAvatar avatar)
        {
            if (DropId == 1)
            {
                avatar.Brawlers[Data.InstanceId].Unlocked = true;
            }
        }
    }
}
