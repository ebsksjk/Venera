using System;

namespace Venera.Shell.Programs
{
    internal class Man : BuiltIn
    {
        public override string Name => "man";

        public override string Description => "Display manual for every command";

        public override CommandDescription ArgumentDescription => new()
        {
            Arguments = [
                new(
                    valueName: "builtin_name",
                    description: "Name of the program need the manual for.",
                    type: typeof(string),
                    required: true,
                    argsPosition: 0
                )
            ]
        };

        protected override ExitCode Execute()
        {
            string programName = (string)GetArgument(0);

            if (!Kernel.SokolshInstance.FindBuiltIn(programName.ToLower(), out BuiltIn program))
            {
                WriteLine($"No program found with this name.");
                return ExitCode.Error;
            }

            WriteLine(program.GenerateUsage());

            return ExitCode.Success;
        }
    }
}
