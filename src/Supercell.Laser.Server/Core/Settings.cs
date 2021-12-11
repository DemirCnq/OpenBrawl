namespace Supercell.Laser.Server.Core
{
    using Supercell.Laser.Titan.Logic.Enums;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    internal static class Settings
    {
        /// <summary>
        /// Whether we should save/find player in <see cref="Mongo"/> or a local file.
        /// </summary>
        internal const DBMS Database = DBMS.Mongo;

        public const string UdpIp = "192.168.0.108"; // Your ip here.

        /// <summary>
        /// Gets the maintenance time.
        /// </summary>
        internal static DateTime MaintenanceTime
        {
            get
            {
                return DateTime.UtcNow.AddMinutes(-1);
            }
        }

        /// <summary>
        /// Array of IP Address authorized to log in to the server even if it's in maintenance/updating/local.
        /// </summary>
        internal static readonly string[] AuthorizedIP =
        {
        };
    }
}
