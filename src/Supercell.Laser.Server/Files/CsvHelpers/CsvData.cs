using System;
using Supercell.Laser.Server.Files.CsvReader;

namespace Supercell.Laser.Server.Files.CsvHelpers
{
    public class CsvData
    {
        private int _dataType;
        private int _id;
        protected DataTable DataTable;
        protected Row Row;

        public CsvData(Row row, DataTable dataTable)
        {
            Row = row;
            DataTable = dataTable;
        }

        public void LoadData(CsvData data, Type type, Row row, int dataType = 0)
        {
            _dataType = dataType;
            _id = GlobalId.CreateGlobalId(_dataType, DataTable.Count());
            Row = row;
            Row.LoadData(data);
        }

        public int GetDataType()
        {
            return _dataType;
        }

        public int GetGlobalId()
        {
            return _id;
        }

        public int GetInstanceId()
        {
            return GlobalId.GetInstanceId(_id);
        }

        public string GetName()
        {
            return Row.GetName();
        }
    }
}