namespace Supercell.Laser.Server.Logic.Slots.Items
{
    using Newtonsoft.Json;
    using Supercell.Laser.Titan.DataStream;
    using Supercell.Laser.Titan.Files.CsvData;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    internal class LogicDataSlot
    {
        [JsonProperty] internal LogicData Data;
        [JsonProperty] internal int Count { get; private set; }

        internal LogicDataSlot(LogicData data) : this(data, 0)
        {
        }

        internal LogicDataSlot(LogicData data, int count)
        {
            Data = data;
            Count = count;
        }

        internal bool Use(int amount)
        {
            bool result = Count >= amount;
            if (result)
            {
                Count -= amount;
            }
            return result;
        }

        internal void Add(int count)
        {
            Count += count;
        }

        internal void Encode(ByteStream stream)
        {
            stream.WriteDataReference(Data);
            stream.WriteVInt(Count);
        }
    }
}
