using System;

namespace Venera.Shell.Programs
{
    public class Pwd : BuiltIn
    {
        public override string Name => "pwd";

        public override string Description => "Print working directory";

        public override ExitCode Execute(string[] args)
        {
            string cwd = Kernel.GlobalEnvironment.GetFirst(DefaultEnvironments.CurrentWorkingDirectory);
            Console.WriteLine(cwd);

            return ExitCode.Success;
        }
    }
}
