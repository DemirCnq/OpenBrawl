namespace Supercell.Laser.Server.Logic.Slots
{
    using Supercell.Laser.Server.Logic.Slots.Items;
    using Supercell.Laser.Titan.DataStream;
    using Supercell.Laser.Titan.Files.CsvData;
    using Supercell.Laser.Titan.Logic;

    internal static class Events
    {
        internal static LogicArrayList<EventData> ActiveEvents;

        internal static void Encode(ByteStream stream)
        {
            stream.WriteVInt(16);
            for (int i = 1; i <= 16; i++)
            {
                stream.WriteVInt(i);
            }

            stream.WriteVInt(ActiveEvents.Size);
            for (int i = 0;  i < ActiveEvents.Size; i++)
            {
                ActiveEvents[i].Encode(stream);
            }

            stream.WriteVInt(0);
        }

        internal static void Init()
        {
            ActiveEvents = new LogicArrayList<EventData>();
            ActiveEvents.Add(new EventData(1, new LogicData(15, 14))); // 111
        }
    }
}
