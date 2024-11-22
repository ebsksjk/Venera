using System;
using System.IO;

namespace Venera.Shell.Programs
{
    internal class Cat : BuiltIn
    {
        public override string Name => "cat";

        public override string Description => "output plaintext from files";

        public override CommandDescription ArgumentDescription => new()
        {
            UsageText = "myapp [options] <input-file>",
            Arguments = [
                new CommandArgument
                (
                    valueName: "output_path",
                    description: "Specify the output file path",
                    type: typeof(string),
                    shortForm: 'o',
                    longForm: "output"
                )
            ]
        };

        protected override ExitCode Execute()
        {
            if (Args.Length == 0)
            {
                Console.WriteLine($"Sokolsh: cat: No Argument provided");
                return ExitCode.Error;
            }

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

            return ExitCode.Success;
        }
    }
}
