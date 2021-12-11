namespace Supercell.Laser.Server.Logic.Slots
{
    using Newtonsoft.Json;
    using Supercell.Laser.Server.Logic.Slots.Items;
    using Supercell.Laser.Titan.Files.CsvData;

    internal class LogicResources
    {
        public const int Count = 1;

        [JsonProperty] internal LogicDataSlot Gold;

        public LogicResources()
        {
            Gold = new LogicDataSlot(new LogicData(5, 8));
        }
    }
}
