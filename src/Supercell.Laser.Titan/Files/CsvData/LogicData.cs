namespace Supercell.Laser.Titan.Files.CsvData
{
    using Newtonsoft.Json;

    public class LogicData : IData
    {
        [JsonProperty] public int ClassId;
        [JsonProperty] public int InstanceId;
        public int GlobalId => ClassId * 1000000 + InstanceId;

        public string Name { get; set; }

        public LogicData(int classId, int instanceId)
        {
            ClassId = classId;
            InstanceId = instanceId;
        }
    }
}
