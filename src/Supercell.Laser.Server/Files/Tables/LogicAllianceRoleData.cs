namespace Supercell.Laser.Server.Files.Tables
{
using Supercell.Laser.Server.Files.CsvReader;
using Supercell.Laser.Server.Files.CsvHelpers;
    public class LogicAllianceRoleData : CsvData
    {
        public LogicAllianceRoleData(Row row, DataTable datatable) : base(row, datatable)
        {
            LoadData(this, GetType(), row);
        }

        public string Name { get; set; }

        public int Level { get; set; }

        public string TID { get; set; }

        public bool CanInvite { get; set; }

        public bool CanSendMail { get; set; }

        public bool CanChangeAllianceSettings { get; set; }

        public bool CanAcceptJoinRequest { get; set; }

        public bool CanKick { get; set; }

        public bool CanBePromotedToLeader { get; set; }

        public int PromoteSkill { get; set; }
    }
}
