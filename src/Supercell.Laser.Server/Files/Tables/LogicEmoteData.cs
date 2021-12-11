namespace Supercell.Laser.Server.Files.Tables
{
using Supercell.Laser.Server.Files.CsvReader;
using Supercell.Laser.Server.Files.CsvHelpers;
    public class LogicEmoteData : CsvData
    {
        public LogicEmoteData(Row row, DataTable datatable) : base(row, datatable)
        {
            LoadData(this, GetType(), row);
        }

        public string Name { get; set; }

        public bool Disabled { get; set; }

        public string FileName { get; set; }

        public string ExportName { get; set; }

        public string Character { get; set; }

        public string Skin { get; set; }

        public bool IsPicto { get; set; }

        public int BattleCategory { get; set; }

        public int Rarity { get; set; }

        public int EmoteType { get; set; }

        public bool LockedForChronos { get; set; }

        public int BundleCode { get; set; }

        public bool IsDefaultBattleEmote { get; set; }
    }
}
