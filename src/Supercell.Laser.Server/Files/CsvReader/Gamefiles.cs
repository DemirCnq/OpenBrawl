using System;
using System.Collections.Generic;
using Supercell.Laser.Server.Files.CsvHelpers;
using static Supercell.Laser.Server.Files.LogicDataTables;

namespace Supercell.Laser.Server.Files.CsvReader
{
    public class Gamefiles : IDisposable
    {
        private readonly Dictionary<LogicDataTables.Files,DataTable> _dataTables = new Dictionary<LogicDataTables.Files, DataTable>();

        public Gamefiles()
        {
            if (LogicDataTables.Gamefiles.Count <= 0) return;

            /*for (var i = 0; i < LogicDataTables.Gamefiles.Count; i++)
                _dataTables.Add(new DataTable());*/
        }

        public void Dispose()
        {
            _dataTables.Clear();
        }

        public DataTable Get(LogicDataTables.Files index)
        {
            return _dataTables[index];
        }

        public DataTable Get(int index)
        {
            return _dataTables[(LogicDataTables.Files)index];
        }

        public void Initialize(Table table, LogicDataTables.Files index)
        {
            _dataTables.Add(index, new DataTable(table, index));
        }
    }
}