namespace Supercell.Laser.Server.Files
{
    using Supercell.Laser.Server.Logic;
    using Supercell.Laser.Titan.Helpers;
    using Supercell.Laser.Titan.Logic.Json;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using System.Threading.Tasks;

    public static class Fingerprint
    {
        public static string Json;
        public static string Sha;
        public static string[] Version;

        public static bool Custom;

        /// <summary>
        /// Gets a value indicating whether this <see cref="Fingerprint"/> is initialized.
        /// </summary>
        internal static bool Initialized
        {
            get;
            private set;
        }

        /// <summary>
        /// Initializes the <see cref="Fingerprint"/> class.
        /// </summary>
        public static void Init()
        {
            if (Fingerprint.Initialized)
            {
                return;
            }

            try
            {
                FileInfo file = new FileInfo($@"{Path.GetDirectoryName(Assembly.GetEntryAssembly().Location)}\Gamefiles\fingerprint.json");

                if (file.Exists)
                {
                    Fingerprint.Json = file.ReadAllText();

                    LogicJSONObject json = LogicJSONParser.ParseObject(Fingerprint.Json);
                    Fingerprint.Sha = json.GetJsonString("sha").GetStringValue();
                    Fingerprint.Version = json.GetJsonString("version").GetStringValue().Split('.');

                    Console.WriteLine("Loaded fingerprint.json v" + LogicVersion.VersionString);
                }
                else
                {
                    Debugger.Error("The Fingerprint cannot be loaded, the file does not exist.");
                }
            }
            catch (Exception exception)
            {
                Debugger.Error($"{exception.GetType().Name} while parsing the fingerprint.");
            }

            Fingerprint.Initialized = true;
        }
    }
}
