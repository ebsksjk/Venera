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

        public override CommandDescription ArgumentDescription => new()
        {
            Arguments = [
                new(
                    valueName: "directory_name",
                    description: "Name of new directory",
                    type: typeof(string),
                    argsPosition: 0,
                    required: true
                ),
            ]
        };

        protected override ExitCode Execute()
        {
            string cwd = Kernel.GlobalEnvironment.GetFirst(DefaultEnvironments.CurrentWorkingDirectory);

            string path = (string)GetArgument(0);
            if (path == null)
            {
                return ExitCode.Usage;
            }

            foreach (string item in Args)
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
