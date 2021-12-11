namespace Supercell.Laser.Server.Files.Tables
{
using Supercell.Laser.Server.Files.CsvReader;
using Supercell.Laser.Server.Files.CsvHelpers;
    public class LogicSkinsraritData : CsvData
    {
        public LogicSkinsraritData(Row row, DataTable datatable) : base(row, datatable)
        {
            LoadData(this, GetType(), row);
        }

        public string Name { get; set; }

        public int Price { get; set; }

        public int Rarity { get; set; }
    }
}
