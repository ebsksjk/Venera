using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Venera.Shell.Programs
{
    internal class Mkdir : BuiltIn
    {
        public override string Name => "mkdir";

        public override string Description => "Make directory";

        public override ExitCode Execute(string[] args)
        {
            string cwd = Kernel.GlobalEnvironment.GetFirst(DefaultEnvironments.CurrentWorkingDirectory);

            if (args.Length == 0)
            {
                Console.WriteLine($"{Name}: Missing argument");

                return ExitCode.Error;
            }

            foreach (string item in args)
            {
                try
                {
                    Kernel.FileSystem.CreateDirectory($@"{cwd}\{item}");
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Error: {e.Message}");
                    return ExitCode.Error;
                }
            }

            return ExitCode.Success;
        }
    }
}
