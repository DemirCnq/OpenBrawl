namespace Supercell.Laser.Server.Files.Tables
{
using Supercell.Laser.Server.Files.CsvReader;
using Supercell.Laser.Server.Files.CsvHelpers;
    public class LogicGlobalData : CsvData
    {
        public LogicGlobalData(Row row, DataTable datatable) : base(row, datatable)
        {
            LoadData(this, GetType(), row);
        }

        public string Name { get; set; }

        public int NumberValue { get; set; }

        public bool BooleanValue { get; set; }

        public string TextValue { get; set; }

        public string StringArray { get; set; }

        public int NumberArray { get; set; }
    }
}
