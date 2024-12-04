using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Venera.Graphics;
using static Cosmos.HAL.BlockDevice.ATA_PIO;

namespace Venera.Shell.Programs
{
    internal class RunChromat : BuiltIn
    {
        public override string Name => "chromat";

        public override string Description => "Run Venera's experimental graphical environment.";

        public override CommandDescription ArgumentDescription => new();

        protected override ExitCode Execute()
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Write("[!!!] ");
            Console.ForegroundColor = ConsoleColor.White;
            WriteLine("Chromat is an experimental graphical environment. You can't exit unless you reboot.\n");

            while (true)
            {
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Write("\nType [y]es to continue anyway, or [n]o to quit: ");
                ConsoleKeyInfo key = Console.ReadKey();
                Console.ForegroundColor = ConsoleColor.White;
                WriteLine();

                if (key.KeyChar == 'n')
                {
                    return ExitCode.Success;
                }
                else if (key.KeyChar == 'y')
                {
                    break;
                }
            }

            Chromat chromat = new Chromat();
            chromat.Loop();

            // Will never execute
            return ExitCode.Success;
        }
    }
}
