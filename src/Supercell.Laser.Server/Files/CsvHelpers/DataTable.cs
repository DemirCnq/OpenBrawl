using System.Collections.Generic;
using Supercell.Laser.Server.Files.CsvReader;

namespace Supercell.Laser.Server.Files.CsvHelpers
{
    public class DataTable
    {
        public List<CsvData> Datas;
        public LogicDataTables.Files Index;

        public DataTable()
        {
            Datas = new List<CsvData>();
        }

        public DataTable(Table table, LogicDataTables.Files index)
        {
            Index = index;
            Datas = new List<CsvData>();

            for (var i = 0; i < table.GetRowCount(); i += 2)
            {
                var row = table.GetRowAt(i);
                var data = LogicDataTables.Create(Index, row, this);
                Datas.Add(data);
            }
        }

        public int Count()
        {
            return Datas?.Count ?? 0;
        }

        public List<CsvData> GetDatas()
        {
            return Datas;
        }

        public CsvData GetDataWithId(int id)
        {
            return Datas[GlobalId.GetInstanceId(id)];
        }

        public T GetDataWithId<T>(int id) where T : CsvData
        {
            return Datas[GlobalId.GetInstanceId(id)] as T;
        }

        public T GetDataWithInstanceId<T>(int id) where T : CsvData
        {
            if (Datas.Count < id) return null;

            return Datas[id] as T;
        }

        public T GetData<T>(string name) where T : CsvData
        {
            return Datas.Find(data => data.GetName() == name) as T;
        }

        public int GetIndex()
        {
            return (int) Index;
        }
    }
}