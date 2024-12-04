using System.Text;

namespace Venera.Shell.Programs
{
    public class ReadStdin : BuiltIn
    {
        public override string Name => "read_stdin";

        public override string Description => "Prints out the passed stdin using pipes. For tests only.";

        public override CommandDescription ArgumentDescription => new();

        protected override ExitCode Execute()
        {
            if (Stdin == null)
            {
                WriteLine("No stdin provided");
                return ExitCode.Error;
            }

            WriteLine($"Stdin contains {Stdin.Length} bytes:");

            Write(Encoding.ASCII.GetString(Stdin));

            return ExitCode.Success;
        }
    }
}
