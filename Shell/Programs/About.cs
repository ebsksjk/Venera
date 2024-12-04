using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Venera.Shell.Programs
{
    public class About : BuiltIn
    {
        public override string Name => "about";

        public override string Description => "Display information about this operating system and hardware.";

        public override CommandDescription ArgumentDescription => new();

        protected override ExitCode Execute()
        {
            WriteLine($"{Kernel.OS_NAME} {Kernel.OS_VERSION}");

            return ExitCode.Success;
        }
    }
}
