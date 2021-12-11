using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Supercell.Laser.Server.Files.CsvReader;

namespace Supercell.Laser.Server.Files
{
    public partial class LogicDataTables
    {
        public static readonly List<string> Gamefiles = new List<string>();
        public static Gamefiles Tables;

        public static void Init()
        {
            Gamefiles.Add("Gamefiles/csv_logic/projectiles.csv");
            Gamefiles.Add("Gamefiles/csv_logic/locations.csv");
            Gamefiles.Add("Gamefiles/csv_logic/characters.csv");
            Gamefiles.Add("Gamefiles/csv_logic/skills.csv");
            Gamefiles.Add("Gamefiles/csv_logic/cards.csv");
            Gamefiles.Add("Gamefiles/csv_logic/area_effects.csv");
            Gamefiles.Add("Gamefiles/csv_logic/items.csv");
            Gamefiles.Add("Gamefiles/csv_logic/skins.csv");

            Tables = new Gamefiles();

            Tables.Initialize(new Table(Gamefiles[0]), Files.Projectiles);
            Tables.Initialize(new Table(Gamefiles[1]), Files.Locations);
            Tables.Initialize(new Table(Gamefiles[2]), Files.Characters);
            Tables.Initialize(new Table(Gamefiles[3]), Files.Skills);
            Tables.Initialize(new Table(Gamefiles[4]), Files.Cards);
            Tables.Initialize(new Table(Gamefiles[5]), Files.AreaEffects);
            Tables.Initialize(new Table(Gamefiles[6]), Files.Items);
            Tables.Initialize(new Table(Gamefiles[7]), Files.Skins);

            Console.WriteLine($"{Gamefiles.Count} Gamefiles loaded.");
        }
    }
}