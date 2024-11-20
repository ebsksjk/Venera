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
            Arguments = new[]
            {
                new CommandArgument
                {
                    ShortForm = 'o',
                    LongForm = "output",
                    Description = "Specify the output file path",
                    ValueName = "output_path",
                    Type = typeof(string)
                },
                new CommandArgument
                {
                    ShortForm = 'v',
                    LongForm = "verbose",
                    Description = "Enable verbose logging",
                    Type = typeof(bool)
                },
                new CommandArgument
                {
                    ArgsPosition = 0,
                    Description = "Input file to process",
                    ValueName = "input_file",
                    Type = typeof(string)
                }
            }
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
