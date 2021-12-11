namespace Supercell.Laser.Server.Files.Tables
{
using Supercell.Laser.Server.Files.CsvReader;
using Supercell.Laser.Server.Files.CsvHelpers;
    public class LogicPinData : CsvData
    {
        public LogicPinData(Row row, DataTable datatable) : base(row, datatable)
        {
            LoadData(this, GetType(), row);
        }

        public string Name { get; set; }

        public int PinType { get; set; }

        public int Rarity { get; set; }

        public int Index { get; set; }

        public int Bonus { get; set; }

        public int CraftCost { get; set; }
    }
}
