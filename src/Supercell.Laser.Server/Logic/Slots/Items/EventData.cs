namespace Supercell.Laser.Server.Logic.Slots.Items
{
    using Supercell.Laser.Titan.DataStream;
    using Supercell.Laser.Titan.Files.CsvData;

    internal class EventData
    {
        internal LogicData Location;
        internal int Index;

        internal EventData(int index, LogicData location)
        {
            Index = index;
            Location = location;
        }

        internal void Encode(ByteStream stream)
        {
            stream.WriteVInt(0);
            stream.WriteVInt(Index);
            stream.WriteVInt(0);
            stream.WriteVInt(75555);
            stream.WriteVInt(0); // Tokens

            stream.WriteDataReference(Location);

            stream.WriteVInt(3);

            stream.WriteString(null);

            stream.WriteVInt(0);
            stream.WriteVInt(0);
            stream.WriteVInt(3);

            stream.WriteVInt(0); // Modifiers

            stream.WriteVInt(0);
            stream.WriteVInt(0);
        }
    }
}
