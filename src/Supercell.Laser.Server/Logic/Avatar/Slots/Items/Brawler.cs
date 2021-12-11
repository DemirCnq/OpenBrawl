namespace Supercell.Laser.Server.Logic.Avatar.Slots.Items
{
    using Newtonsoft.Json;
    using Supercell.Laser.Titan.DataStream;
    using Supercell.Laser.Titan.Files.CsvData;

    public class Brawler
    {
        [JsonProperty] internal readonly LogicData CardData;
        [JsonProperty] internal readonly LogicData CharData;

        [JsonProperty] internal int Trophies;

        [JsonProperty] internal int PowerLevel;
        [JsonProperty] internal int PowerPoints;
        [JsonProperty] internal bool Unlocked;
        [JsonProperty] internal bool Disabled;

        internal Brawler(int charId, int cardId)
        {
            CardData = new LogicData(23, cardId);
            CharData = new LogicData(16, charId);
        }

        [JsonConstructor] internal Brawler()
        {
        }

        internal void EncodeUnlockDataSlot(ByteStream stream)
        {
            stream.WriteDataReference(CardData);
            stream.WriteBoolean(Unlocked);
        }

        internal void EncodeTrophiesDataSlot(ByteStream stream)
        {
            stream.WriteDataReference(CharData);
            stream.WriteVInt(Trophies);
        }
    }
}
