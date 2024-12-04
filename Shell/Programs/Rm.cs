using System;
using System.IO;

namespace Venera.Shell.Programs
{
    internal class Rm : BuiltIn
    {
        public override string Name => "rm";

        public override string Description => "Delete a file from disk.";

        public override CommandDescription ArgumentDescription => new()
        {
            Arguments = [
                new(
                    valueName: "file",
                    description: "File to wipe out of existence.",
                    type: typeof(string),
                    required: true,
                    argsPosition: 0
                ),
            ]
        };

        protected override ExitCode Execute()
        {
            string file = ((string)GetArgument(0)).AbsoluteOrRelativePath();
            if (!File.Exists(file))
            {
                Console.WriteLine($"rm: File {file} does not exist");
                return ExitCode.Error;
            }

            File.Delete(file);

            return ExitCode.Success;
        }
    }
}
