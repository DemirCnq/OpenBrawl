namespace Supercell.Laser.Server.Files.Tables
{
using Supercell.Laser.Server.Files.CsvReader;
using Supercell.Laser.Server.Files.CsvHelpers;
    public class LogicMapData : CsvData
    {
        public LogicMapData(Row row, DataTable datatable) : base(row, datatable)
        {
            LoadData(this, GetType(), row);
        }

        public string CodeName { get; set; }

        public string Group { get; set; }

        public string Data { get; set; }

        public bool ConstructFromBlocks { get; set; }
    }
}
