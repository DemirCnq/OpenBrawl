namespace Supercell.Laser.Server.Logic.Slots.Items
{
    using Newtonsoft.Json;
    using Supercell.Laser.Titan.DataStream;

    internal class BrawlPassSeasonData
    {
        [JsonProperty] internal int Season;
        [JsonProperty] internal int Tokens;

        internal void Encode(ByteStream stream)
        {
            stream.WriteVInt(1);
            {
                stream.WriteVInt(2);
                stream.WriteVInt(0);
                stream.WriteVInt(1);
                stream.WriteVInt(0);
                stream.WriteVInt(0);
            }
        }
    }
}
