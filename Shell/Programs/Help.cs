using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Venera.Shell.Programs
{
    public class Help : BuiltIn
    {
        public override string Name => "help";

        public override string Description => "Prints this list.";

        public override CommandDescription ArgumentDescription => new();

        protected override ExitCode Execute()
        {
            int longestName = "Command".Length;

            foreach (BuiltIn builtIn in Sokolsh.AvailableBuiltIns)
            {
                if (builtIn.Name.Length > longestName)
                {
                    longestName = builtIn.Name.Length;
                }
            }

            // Apply some padding
            longestName += 2;

            int descriptionSize = Console.WindowWidth - longestName;
            Console.Write("Command".Pad(longestName) + "Description".Pad(descriptionSize));

            for (int i = 0; i < Console.WindowWidth; i++)
            {
                Console.Write("-");
            }

            foreach (BuiltIn builtIn in Sokolsh.AvailableBuiltIns)
            {
                Console.Write(builtIn.Name.Pad(longestName) + builtIn.Description.Pad(descriptionSize));
            }

            // Special exit command
            Console.Write("exit".Pad(longestName) + "Exits the current shell.".Pad(descriptionSize));

            return ExitCode.Success;
        }
    }
}
