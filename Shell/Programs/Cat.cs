using System;
using System.IO;

namespace Venera.Shell.Programs
{
    internal class Cat : BuiltIn
    {
        public override string Name => "cat";

        public override string Description => "Output plaintext from files";

        public override CommandDescription ArgumentDescription => new()
        {
            Arguments = [
                new CommandArgument
                (
                    valueName: "file_paths",
                    description: "Specify the input files to read.",
                    type: typeof(string[]),
                    argsPosition: -1,
                    required: true
                )
            ]
        };

        protected override ExitCode Execute()
        {
            foreach (string filename in (string[])GetArgument(-1))
            {
                string path = Args[0].AbsoluteOrRelativePath();

                try
                {
                    Console.WriteLine(File.ReadAllText(path));
                }
                catch (Exception)
                {
                    Console.WriteLine($"Sokolsh: cat: File {path} does not exist");
                    return ExitCode.Error;
                }
            }

            return ExitCode.Success;
        }
    }
}
