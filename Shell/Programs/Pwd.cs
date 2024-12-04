using System;

namespace Venera.Shell.Programs
{
    public class Pwd : BuiltIn
    {
        public override string Name => "pwd";

        public override string Description => "Print working directory";

        public override CommandDescription ArgumentDescription => new();

        protected override ExitCode Execute()
        {
            string cwd = Kernel.GlobalEnvironment.GetFirst(DefaultEnvironments.CurrentWorkingDirectory);
            WriteLine(cwd);

            return ExitCode.Success;
        }
    }
}
