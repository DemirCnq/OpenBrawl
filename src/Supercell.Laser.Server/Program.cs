namespace Supercell.Laser.Server
{
    using Supercell.Laser.Server.Core;
    using Supercell.Laser.Titan.Core.Consoles;
    using Supercell.Laser.Titan.Helpers;
    using System;
    using System.Reflection;

    internal class Program
    {
        private const int Width = 120;
        private const int Height = 32;

        /// <summary>
        /// Defines the entry point of the application.
        /// </summary>
        private static void Main()
        {
            Natives.SetupConsole();

            Console.Title = $"{Assembly.GetExecutingAssembly().GetName().Name} | {DateTime.Now.Year} ©";

            Console.SetOut(new Prefixed());

            //Console.SetWindowSize(Program.Width, Program.Height);
            //Console.SetBufferSize(Program.Width, Program.Height);

            Console.ForegroundColor = ConsoleColor.Cyan;

            Console.Write(@"
  ____                     _    _____ _                 
 |  _ \                   | |  / ____| |                
 | |_) |_ __ __ ___      _| | | (___ | |_ __ _ _ __ ___ 
 |  _ <| '__/ _` \ \ /\ / / |  \___ \| __/ _` | '__/ __|
 | |_) | | | (_| |\ V  V /| |  ____) | || (_| | |  \__ \
 |____/|_|  \__,_| \_/\_/ |_| |_____/ \__\__,_|_|  |___/
  " + Environment.NewLine);

            Console.ForegroundColor = ConsoleColor.White;

            Console.WriteLine("Starting..." + Environment.NewLine);

            Loader.Init();

            Console.WriteLine();

            //Parser.Init();
        }
    }
}
