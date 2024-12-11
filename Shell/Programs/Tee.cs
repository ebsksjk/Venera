using System;
using System.IO;
using System.Text;

namespace Venera.Shell.Programs
{
    internal class Tee : BuiltIn
    {
        public override string Name => "tee";

        public override string Description => "Write stdin into file.";

        public override CommandDescription ArgumentDescription => new()
        {
            Arguments = [
                new CommandArgument
                (
                    valueName: "apppend",
                    description: "Set to append data, not to override file.",
                    type: typeof(bool),
                    shortForm: 'a',
                    longForm: "append"
                ),
                new CommandArgument
                (
                    valueName: "file_path",
                    description: "Target file to write into.",
                    type: typeof(string),
                    argsPosition: 0,
                    required: true
                ),
            ]
        };

        protected override ExitCode Execute()
        {
            string path = ((string)GetArgument(0)).AbsoluteOrRelativePath();
            bool append = (bool)GetArgument("a");

            if (!path.isAccessible())
            {
                WriteLine("tee: Path is not accessible.");
                return ExitCode.Error;
            }

            try
            {
                byte[] content = [];

                if (append)
                {
                    content = File.ReadAllBytes(path);
                }

                File.WriteAllBytes(path, [.. content, .. Stdin]);

                Console.WriteLine(Encoding.ASCII.GetString(Stdin));
            }
            catch (Exception e)
            {
                Console.WriteLine($"tee: Error occured: {e.ToString()}");
                return ExitCode.Error;
            }

            return ExitCode.Success;
        }
    }
}
