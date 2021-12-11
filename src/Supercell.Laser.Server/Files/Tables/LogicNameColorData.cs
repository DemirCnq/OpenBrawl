namespace Supercell.Laser.Server.Files.Tables
{
using Supercell.Laser.Server.Files.CsvReader;
using Supercell.Laser.Server.Files.CsvHelpers;
    public class LogicNameColorData : CsvData
    {
        public LogicNameColorData(Row row, DataTable datatable) : base(row, datatable)
        {
            LoadData(this, GetType(), row);
        }

        public string Name { get; set; }

        public string ColorCode { get; set; }

        public string Gradient { get; set; }

        public int RequiredExpLevel { get; set; }

        public int RequiredTotalTrophies { get; set; }

        public int RequiredSeasonPoints { get; set; }

        public string RequiredHero { get; set; }

        public int SortOrder { get; set; }

        public string ColorGradient { get; set; }
    }
}
