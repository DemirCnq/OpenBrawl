using System;
using System.Collections.Generic;
using Supercell.Laser.Server.Files.CsvHelpers;
using Supercell.Laser.Server.Files.CsvReader;
using Supercell.Laser.Server.Files.Tables;

namespace Supercell.Laser.Server.Files
{
    public partial class LogicDataTables
    {
        public enum Files
        {
            Projectiles = 6,
            Locations = 15,
            Characters = 16,
            AreaEffects = 17,
            Items = 18,
            Skills = 20,
            Cards = 23,
            Skins = 29
        }

        public static Dictionary<Files, Type> DataTypes = new Dictionary<Files, Type>();

        static LogicDataTables()
        {
            DataTypes.Add(Files.Projectiles, typeof(LogicProjectileData));
            DataTypes.Add(Files.Locations, typeof(LogicLocationData));
            DataTypes.Add(Files.Characters, typeof(LogicCharacterData));
            DataTypes.Add(Files.Skills, typeof(LogicSkillData));
            DataTypes.Add(Files.Cards, typeof(LogicCardData));
            DataTypes.Add(Files.AreaEffects, typeof(LogicAreaEffectData));
            DataTypes.Add(Files.Items, typeof(LogicItemData));
            DataTypes.Add(Files.Skins, typeof(LogicSkinData));
        }

        public static CsvData Create(Files file, Row row, DataTable dataTable)
        {
            if (DataTypes.ContainsKey(file)) return Activator.CreateInstance(DataTypes[file], row, dataTable) as CsvData;

            return null;
        }
    }
}